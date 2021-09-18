#include "uav.h"

uav::uav(QString ip, uint main_port, uint cargo_port)
{
    tcp = new tcpconnection(ip, main_port, "mk6");
    nextPoint = new wayPoint();
    cargo = new class cargo(ip, cargo_port);
    connect(tcp->m_connection, &QTcpSocket::readyRead, this, [this]{onIncomingData();});
    connect(cargo, SIGNAL(message(QString)), this, SLOT(getChildMessage(QString)));
    connect(tcp, &tcpconnection::message, this, &uav::getChildMessage);
    connect(tcp, &tcpconnection::aborted, this, [this]{emit connectionAborted();});
}

void uav::try_reconnect(bool try_reconnect)
{
    tcp->try_reconnect = try_reconnect;
    cargo->tcp->try_reconnect = try_reconnect;
}

void uav::ipconnect()
{
    tcp->ipconnect();
    cargo->ipconnect();
}

void uav::ipdisconnect()
{
    tcp->ipdisconnect();
    cargo->ipdisconnect();
}

void uav::sendNextPoint()
{
    //Get data from UI
    WAYPOINT_ACTION_CTRL::Ctrl proto;
    proto.set_boardid(boardId);
    WAYPOINT_ACTION_CTRL::MoveToPointRequest* moveToPointRequest = proto.mutable_request();
    moveToPointRequest->set_latitude(nextPoint->latitude);
    moveToPointRequest->set_longitude(nextPoint->longitude);
    moveToPointRequest->set_altitude(nextPoint->altitude);

    if (nextPoint->courseEn)
        moveToPointRequest->set_course(nextPoint->course);
    // ACTIONS
    // Action for HoldPoint
    WAYPOINT_ACTION_CTRL::ActionInPoint* action = moveToPointRequest->add_actions();
    action->set_type(WAYPOINT_ACTION_CTRL::HoldPoint);
    action->set_actiontime(nextPoint->holdTime);

    // Action for PointInCoordinates
    if (nextPoint->pointInCoordinates.en)
    {
        WAYPOINT_ACTION_CTRL::ActionInPoint* action = moveToPointRequest->add_actions();
        action->set_type(WAYPOINT_ACTION_CTRL::PointInCoordinate);
        action->mutable_pointincoordprop()->set_latitude(nextPoint->pointInCoordinates.latitude);
        action->mutable_pointincoordprop()->set_longitude(nextPoint->pointInCoordinates.longitude);
        qDebug() << nextPoint->pointInCoordinates.latitude << nextPoint->pointInCoordinates.longitude;
    }
    // Action for Servo
    if (nextPoint->servo.en)
    {
        WAYPOINT_ACTION_CTRL::ActionInPoint* action = moveToPointRequest->add_actions();
        action->set_type(WAYPOINT_ACTION_CTRL::Servo);
        action->mutable_servoprop()->set_targetangle(nextPoint->servo.angle);
    }

    // Generate message with proto
    QByteArray protoByteArray;
    protoByteArray.resize(proto.ByteSize());
    proto.SerializeToArray(protoByteArray.data(), protoByteArray.size());
    GcsExngHeader header = GCS_EXNG_HEADER_INIT;
    header.type = 4;
    header.len = protoByteArray.size();
    QByteArray protoMsg(reinterpret_cast<const char*>(&header), sizeof(header));
    protoMsg.append(protoByteArray);
    // Send message
    qint64 writeSize = tcp->m_connection->write(protoMsg);
    pointAccepted = false;
    pointCompleted = false;
    qDebug() << "proto message sent [" << writeSize << "]";
    qDebug() << protoMsg.toHex();
    emit message(QString("<<<  %1 bytes sent to UAV").arg(writeSize));
    QString holdTimeStr = "∞";
    if (nextPoint->holdTime != 0)
        holdTimeStr = QString::number(nextPoint->holdTime);
    QString pointActions = "<<<  ACTIONS: Hold " + holdTimeStr;
    if (nextPoint->pointInCoordinates.en)
        pointActions += ", PIC";
    if (nextPoint->courseEn)
        pointActions += ", Course " + QString::number(nextPoint->course);
    if (nextPoint->servo.en)
        pointActions += ", Servo " + QString::number(nextPoint->servo.angle);
    emit message(pointActions);
}

bool uav::linkCargo()
{
    QByteArray cargoMsg;
    cargoMsg.append("user link command b924d2");
    auto writeSize = cargo->tcp->m_connection->write(cargoMsg);
    qDebug() << "connecting to cargo [" << writeSize << "]";
    if (writeSize == -1)
        emit message(QString("-----  Error connecting cargo [%1]  -----").arg(writeSize));
    else if (writeSize == 24)
    {
        cargo->turnOff();
        emit message(QString("-----  Connected to cagro [%1]  -----").arg(writeSize));
        return true;
    }
    else
        emit message("-----  Connecting cargo unknown  -----");
    return false;
}

void uav::onIncomingData()
{
    msgPart.append(tcp->m_connection->readAll());

    //qDebug() << "proto bytes received [" <<msgPart.count() << "]";

    // Parse headers, preambule and take Proto
    const GcsExngHeader* header;
    while(msgPart.count() > 0)
    {
        if (!parseHeader()) return;
        header = reinterpret_cast<const GcsExngHeader*>(msgPart.data());
        //qDebug() << "proto bytes parsed [" << header->len << "]";
        parseIncomingData(header->len, header->type);
        msgPart.remove(0, header->len + sizeof(GcsExngHeader));
    }
    msgPart.clear();
}

bool uav::parseHeader()
{
    // Message is shorter than header
    if (static_cast<size_t>(msgPart.size()) < sizeof(GcsExngHeader))
    {
        emit message("!!!!!   ERROR. Bad message header received");
        return false;
    }
    // Message is not full
    if ((reinterpret_cast<const GcsExngHeader*>(msgPart.data()))->len + sizeof(GcsExngHeader) > static_cast<size_t>(msgPart.size()))
    {
        emit message("Part of a message received");
        return false;
    }
    // Message is longer than expected
    if ((reinterpret_cast<const GcsExngHeader*>(msgPart.data()))->len + sizeof(GcsExngHeader) < static_cast<size_t>(msgPart.size()))
    {
        emit message("Bundle of messages received");
    }
    return true;
}

void uav::parseIncomingData(uint16_t len, uint16_t type)
{
    QByteArray msg;
    msg.append(msgPart.mid(0, len + sizeof(GcsExngHeader)));
    const QByteArray protoData = msg.mid(sizeof(GcsExngHeader));

    // Telemetry
    if (type == static_cast<uint16_t>(CTRL_MSG_TYPE::CTRL_MSG_TELEMETRY))
    {
        TELEMETRY_CTRL::Ctrl proto;
        if(!proto.ParseFromArray(protoData.data(), protoData.size()))
        {
            emit message("!!!!!   ERROR parse proto");
        }
        if(proto.msgtype() == TELEMETRY_CTRL::CtrlMsgType_Telemetry)
        {
            for (int i = 0; i < proto.telemetrylist().list_size() ; ++i)
            {

                if (proto.telemetrylist().list(i).has_gpsnavigation())
                {
                    telemetry.latitude = proto.telemetrylist().list(i).gpsnavigation().latitude();
                    telemetry.longitude = proto.telemetrylist().list(i).gpsnavigation().longitude();
                    telemetry.altitude = proto.telemetrylist().list(i).gpsnavigation().altitude();
                    telemetry.speed = proto.telemetrylist().list(i).gps_speed();
                    //telemetry.course = proto.telemetrylist().list(i).gps_course();
                    telemetry.course = proto.telemetrylist().list(i).mag_course();
                    telemetry.servo_angle = proto.telemetrylist().list(i).servo_angle();
                }
            }
            emit navigationArrived();
        }
    }

    // Ctrl
    if (type == static_cast<uint16_t>(CTRL_MSG_TYPE::CTRL_MSG_WAYPOINT_ACTION))
    {
        WAYPOINT_ACTION_CTRL::Ctrl proto;

        // Error
        if (!proto.ParseFromArray(protoData.data(), protoData.size()))
        {
            emit message("!!!!!   ERROR parse proto");
            return;
        }

        // MoveToPointRequest_Result
        if (proto.has_requestresult())
        {
            if (proto.requestresult().result() == WAYPOINT_ACTION_CTRL::MoveToPointRequestResult_Result_Result_Accepted)
            {
                emit message(">>>  Command ACCEPTED");
                pointAccepted = true;
            }
            if (proto.requestresult().result() == WAYPOINT_ACTION_CTRL::MoveToPointRequestResult_Result_Result_Rejected)
            {
                emit message(">>>  Command REJECTED");
                pointAccepted = false;
            }
            if (proto.requestresult().result() == WAYPOINT_ACTION_CTRL::MoveToPointRequestResult_Result_Result_Undefined)
            {
                emit message(">>>  Command RESULT UNDEFINED");
            }
        }

        // ActionInPoint_Result
        if (proto.has_actionresult())
        {
            // Action finished
            if(proto.actionresult().result() == WAYPOINT_ACTION_CTRL::ActionInPointResult_Result_ActionFinished)
            {
                QString id = "Id=";
                if (proto.actionresult().has_actionid())
                    id.append(QString::number(proto.actionresult().actionid()));
                else
                    id.append("??");

                QString res = "Unknown";
                if (proto.actionresult().has_success())
                    res = QString("%1").arg(proto.actionresult().success() ? QString("OK") : QString("ERROR"));

                emit message(QString(">>>  Action finished: %1, %2").arg(res).arg(id));
            }
            // All actions finished
            if (proto.actionresult().result() == WAYPOINT_ACTION_CTRL::ActionInPointResult_Result_AllActionsFinished)
            {
                QString res = "Unknown";
                if (proto.actionresult().has_success())
                    res = QString("%1").arg(proto.actionresult().success() ? QString("OK") : QString("ERROR"));
                emit message(QString(">>>  All actions finished: %1").arg(res));
                pointCompleted = true;
            }
        }
    }
}

void uav::getChildMessage(QString str)
{
    emit message(str);
}

