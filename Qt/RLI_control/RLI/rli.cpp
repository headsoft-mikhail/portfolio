#include "rli.h"

#include <QDataStream>
#include <QMap>
#include <QTimerEvent>
#include <QDateTime>

Rli::Rli(QObject *parent) : QObject(parent),
    parseMap{
{0x34, &Rli::parse34},
{0xA0, &Rli::parseA0},
{0xA1, &Rli::parseA1},
{0xC0, &Rli::parseC0},
{0xC1, &Rli::parseC1},
{0xC2, &Rli::parseC2},
{0xC3, &Rli::parseC3},
{0xC4, &Rli::parseC4},
{0xC5, &Rli::parseC5},
{0xC6, &Rli::parseC6},
//{0xCF, &Rli::parseCF},
{0xD0, &Rli::parseD0},
{0xD2, &Rli::parseD2},
{0xD4, &Rli::parseD4},
{0xF0, &Rli::parseF0},
{0xF1, &Rli::parseF1},
{0xF8, &Rli::parseF8},
{0xF9, &Rli::parseF9},},

    taskSet{
{ {tasks_enum::GetAdcGain}, {0xA0, &Rli::adcGainRequest} },
{ {tasks_enum::SetAdcGain}, {0x34, &Rli::setAdcGain} },

{ {tasks_enum::GetDacAtt}, {0xA1, &Rli::dacAttRequest} },
{ {tasks_enum::SetDacAtt}, {0x34, &Rli::setDacAtt} },

{ {tasks_enum::GetModeCtrl}, {0xC0, &Rli::modeCtrlRequest} },
{ {tasks_enum::SetModeCtrl}, {0x34, &Rli::setModeCtrl} },

{ {tasks_enum::GetProcThreshold}, {0xC1, &Rli::procThresholdRequest} },
{ {tasks_enum::SetProcThreshold}, {0x34, &Rli::setProcThreshold} },

{ {tasks_enum::GetMti}, {0xC2, &Rli::mtiRequest} },
{ {tasks_enum::SetMti}, {0x34, &Rli::setMti} },

{ {tasks_enum::GetDistOffset}, {0xC3, &Rli::distOffsetRequest} },
{ {tasks_enum::SetDistOffset}, {0x34, &Rli::setDistOffset} },

{ {tasks_enum::GetNumPeaks}, {0xC4, &Rli::numPeaksRequest} },
{ {tasks_enum::SetNumPeaks}, {0x34, &Rli::setNumPeaks} },

{ {tasks_enum::GetAdjust}, {0xC5, &Rli::adjustRequest} },
{ {tasks_enum::SetAdjust}, {0x34, &Rli::setAdjust} },

{ {tasks_enum::GetViewMode}, {0xC6, &Rli::viewModeRequest} },
{ {tasks_enum::SetViewMode}, {0x34, &Rli::setViewMode} },

{ {tasks_enum::GetTimeSecsSinceEpoh}, {0xCF, &Rli::timeSecsSinceEpohRequest} },

{ {tasks_enum::Reboot}, {0xCE, &Rli::reboot} },

{ {tasks_enum::GetStatus}, {0xD0, &Rli::statusRequest} },

{ {tasks_enum::GetScaleTable}, {0xD2, &Rli::scaleTableRequest} },

{ {tasks_enum::GetMulticastAddress}, {0xD4, &Rli::multicastAddressRequest} },

{ {tasks_enum::SendToRotator}, {0xF0, &Rli::sendRotatorCommand} },

{ {tasks_enum::SendToAmplifier}, {0xF1, &Rli::sendAmplifierCommand} },

{ {tasks_enum::GetRotatorTelemetry}, {0xF8, &Rli::rotatorTelemetryRequest} },

{ {tasks_enum::GetAmplifierTelemetry}, {0xF9, &Rli::amplifierTelemetryRequest} }
        }

{
    QMap<uint8_t,QString> ackErrs =
    {{0x00, "errNoErrors" },
     {0x01, "errNotImplemented" },
     {0x02, "errWrongSize" },
     {0x03, "errInvalidParam" },
     {0x04, "errBusy" },
     {0x05, "errDeviceNotReply" },
     {0x06, "errDeviceReplyWrongCmd" },
     {0x07, "errDeviceReplyRejectCmd" },
     {0x08, "errUnknownError" }};
}

void Rli::addTask(tasks_enum taskType)
{
    taskQueue.enqueue(taskSet.value(taskType));
//    if(!taskQueue.isEmpty())
        taskQueueUpdate();
}

void Rli::taskQueueUpdate()
{
    if(taskQueue.isEmpty()) return;

    task_t &task = taskQueue.head();

    if(task.isTimeout)
    {
        taskQueue.clear();
        killTimer(queueTaskTimerId);
        queueTaskTimerId = 0;
        emit rliTimeoutEvent();
        //error
    }
    else
    {
        if(task.isTaskLaunched)
        {
            if(task.resultId == MsgID)
            {
                killTimer(queueTaskTimerId);
                taskQueue.dequeue();

                if(taskQueue.isEmpty())
                {
                    queueTaskTimerId = 0;
                    return;
                }
                else
                {
                    task_t &taskNext = taskQueue.head();
                    (this->*taskNext.taskFunc)();
                    taskNext.isTaskLaunched = true;
                    queueTaskTimerId = startTimer(100);
                }
            }
        }
        else
        {
            (this->*task.taskFunc)();
            task.isTaskLaunched = true;
            queueTaskTimerId = startTimer(1000);
        }
    }
}

void Rli::transferMessage(QByteArray message)
{
    ethConn->send(message);
}

void Rli::setAdcGain()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xA0;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds << ADCGain;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set ADCGain sent:" << mes.toHex(' ');
}

void Rli::setDacAtt()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xA1;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds << DACAtt;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set DACAtt sent:" << mes.toHex(' ');
}

void Rli::setProcThreshold()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xC1;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds << ProcThreshold;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set ProcTreshhold sent:" << mes.toHex(' ');
}

void Rli::setMti()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xC2;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds << MTIVal;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set MTI sent:" << mes.toHex(' ');
}

void Rli::setDistOffset()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xC3;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds << DistOffset;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set DistOffset sent:" << mes.toHex(' ');
}

void Rli::setNumPeaks()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xC4;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds << NumPeaks;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set NumPeaks sent:" << mes.toHex(' ');
}

void Rli::sendRotatorCommand()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    uint8_t ReqFlag = 0x00;
    if (rotatorCommand.ReadReg)  //read register
        ReqFlag = 0x10;
    uint8_t MsgID = 0xF0;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize << rotatorCommand.RegAddr;
    rotatorCommand.RegSize = 0x01;

    switch (rotatorCommand.DataType)
    {
    case 0:
        ds.setFloatingPointPrecision(QDataStream::SinglePrecision);
        ds << rotatorCommand.RegSize << rotatorCommand.RegData_float;
        break;
    case 1:
        rotatorCommand.RegSize = 0x00;
        ds << rotatorCommand.RegSize << rotatorCommand.RegData_uint16;
        break;
    case 2:
        ds  << rotatorCommand.RegSize << rotatorCommand.RegData_uint32;
        break;
    }

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Rotator command sent:" << mes.toHex(' ');
}

void Rli::sendAmplifierCommand()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xF1;
    uint16_t DataSize = 0x00;
    ds << ReqFlag << MsgID << DataSize << amplifierCommand.Command;
    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Amplifier command sent:" << mes.toHex(' ');
}


Rli::~Rli()
{
    close();
}

void Rli::request(uint8_t MsgID)
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);

    uint8_t ReqFlag = 0x10;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ethConn->send(mes);
    qDebug() << mes.toHex(' ');
}

void Rli::open()
{
    ethConn = new EthConnControl();
    ethConn->init();
    timerIdReceive = startTimer(50);
}

bool Rli::isOpen()
{
    return ethConn != nullptr;
}

void Rli::close()
{
    if(timerIdReceive) killTimer(timerIdReceive);
    timerIdReceive = 0;
    if(ethConn != nullptr)
    {
        ethConn->close();
        delete ethConn;
    }
    ethConn = nullptr;
}

void Rli::setModeCtrl()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xC0;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds
            << modeCtrl.WorkMode
            << modeCtrl.ScaleCode
            << modeCtrl.ZondNum
            << modeCtrl.TargetYaw
            << modeCtrl.TargetPitch
            << modeCtrl.PwrLevel
            << modeCtrl.ExtEnableTX
            << modeCtrl.Rsv
            << modeCtrl.TRxFreq;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set ModeCTRL sent:" << mes.toHex(' ');
}

void Rli::setAdjust()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xC5;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds
            << adjust.DACDelay
            << adjust.RefDelay
            << adjust.TxDelay
            << adjust.TxLen
            << adjust.RxDelay
            << adjust.RxLen;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set Adjust sent:" << mes.toHex(' ');
}

void Rli::setViewMode()
{
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xC6;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize;

    ds
            << viewMode.BlankSize
            << viewMode.RejectSize
            << viewMode.OffsetView
            << viewMode.Mode;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Set ViewMode sent:" << mes.toHex(' ');
}

void Rli::reboot(){
    QByteArray mes;
    QDataStream ds(&mes, QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);

    uint8_t ReqFlag = 0x00;
    uint8_t MsgID = 0xCE;
    uint16_t DataSize = 0x00;

    ds << ReqFlag << MsgID << DataSize << reboot_page;

    ds.device()->seek(2);
    DataSize = mes.size()-4;
    ds << DataSize;

    ethConn->send(mes);
    qDebug() << "Reboot from" << reboot_page << ":" << mes.toHex(' ');
}


void Rli::adcGainRequest()
{
    qDebug() << "ADCGain request";
    request(0xA0);
}

void Rli::dacAttRequest()
{
    qDebug() << "DACAtt request";
    request(0xA1);
}

void Rli::modeCtrlRequest()
{
    qDebug() << "ModeCTRL request";
    request(0xC0);
}

void Rli::procThresholdRequest()
{
    qDebug() << "ProcThreshold request";
    request(0xC1);
}

void Rli::mtiRequest()
{
    qDebug() << "MTIVal request";
    request(0xC2);
}

void Rli::distOffsetRequest()
{
    qDebug() << "DistOffset request";
    request(0xC3);
}

void Rli::numPeaksRequest()
{
    qDebug() << "NumPeaks request";
    request(0xC4);
}

void Rli::adjustRequest()
{
    qDebug() << "Adjust request";
    request(0xC5);
}

void Rli::viewModeRequest()
{
    qDebug() << "ViewMode request";
    request(0xC6);
}

void Rli::timeSecsSinceEpohRequest()
{
    qDebug() << "TimeSecsSinceEpoh request";
    request(0xCF);
}

void Rli::statusRequest()
{
    qDebug() << "Status request";
    request(0xD0);
}

void Rli::scaleTableRequest()
{
    qDebug() << "Scale table request";
    request(0xD2);
}

void Rli::multicastAddressRequest()
{
    qDebug() << "Multicast address request";
    request(0xD4);
}

void Rli::rotatorTelemetryRequest()
{
    qDebug() << "Rotator telemetry request";
    request(0xF8);
}

void Rli::amplifierTelemetryRequest()
{
    qDebug() << "Amplifier telemetry request";
    request(0xF9);
}


void Rli::parse34(QDataStream &ds)
{
    uint8_t Rsv;
    ds
            >> acknowledge.ReplyMsgID
            >> Rsv
            >> acknowledge.Result
            >> acknowledge.ErrorInfo;
    emit acknowledgeReceived();
    qDebug() << "RLI Acknowledge received:"
             << QString::number(acknowledge.ReplyMsgID, 16)
             << QString::number(acknowledge.Result, 16)
             << QString::number(acknowledge.ErrorInfo, 16);
    if (acknowledge.ReplyMsgID == 0xCE)
        emit deviceReboot();
}

void Rli::parseA0(QDataStream &ds)
{
    ds >> ADCGain;
    qDebug() << "ADC Gain received";
    emit ADCGainUpdated();
}

void Rli::parseA1(QDataStream &ds)
{
    ds >> DACAtt;
    qDebug() << "ADC Gain received";
    emit DACAttUpdated();
}

void Rli::parseC0(QDataStream &ds)
{
    ds
            >> modeCtrl.WorkMode
            >> modeCtrl.ScaleCode
            >> modeCtrl.ZondNum
            >> modeCtrl.TargetYaw
            >> modeCtrl.TargetPitch
            >> modeCtrl.PwrLevel
            >> modeCtrl.ExtEnableTX
            >> modeCtrl.Rsv
            >> modeCtrl.TRxFreq;
    qDebug() << "ModeCTRL received";
    emit modeCtrlUpdated();
}

void Rli::parseC1(QDataStream &ds)
{
    ds >> ProcThreshold;
    qDebug() << "ProcThreshold received";
    emit ProcThresholdUpdated();
}

void Rli::parseC2(QDataStream &ds)
{
    ds >> MTIVal;
    qDebug() << "MTIVal received";
    emit MTIValUpdated();
}

void Rli::parseC3(QDataStream &ds)
{
    ds >> DistOffset;
    qDebug() << "DistOffset received";
    emit DistOffsetUpdated();
}

void Rli::parseC4(QDataStream &ds)
{
    ds >> NumPeaks;
    qDebug() << "NumPeaks received";
    emit NumPeaksUpdated();
}

void Rli::parseC5(QDataStream &ds)
{
    ds
            >> adjust.DACDelay
            >> adjust.RefDelay
            >> adjust.TxDelay
            >> adjust.TxLen
            >> adjust.RxDelay
            >> adjust.RxLen;
    qDebug() << "Adjust received";
    emit adjustUpdated();
}

void Rli::parseC6(QDataStream &ds)
{
    ds
            >> viewMode.BlankSize
            >> viewMode.RejectSize
            >> viewMode.OffsetView
            >> viewMode.Mode;
    qDebug() << "ViewMode received";
    emit viewModeUpdated();
}

void Rli::parseD0(QDataStream &ds)
{
    ds
            >> state.TempFPGA1
            >> state.TempFPGA2
            >> state.TempFMC
            >> state.CurWorkMode
            >> state.ErrCodes
            >> state.WorkTime;
    qDebug() << "Status received";
    emit statusUpdated();
}

void Rli::parseD2(QDataStream &ds)
{
    uint8_t len;
    uint16_t Rsv1;
    uint8_t Rsv2;
    ds >> len >> Rsv1 >> Rsv2;
    scales.resize(len);
    for(int i=0;i<len;++i)
        ds >> scales[i].ScaleLen >> scales[i].ScaleMaxMTI >> scales[i].ScaleMaxDistOffs;
    qDebug() << "Scales received";
    emit scaleTableUpdated();
}

void Rli::parseD4(QDataStream &ds)
{
    ds >> GroupAddr >> DstPort;
    qDebug() << "GroupAddr received";
    emit multicastAddressUpdated();
}

void Rli::parseF0(QDataStream &ds)
{
    if (rotatorCommand.ModBusRW == 0x03)
    {
        ds >> rotatorCommand.ModBusBytesRead;
        QByteArray receivedBytes;
        uint8_t byte;
        for (int i = 0; i<rotatorCommand.ModBusBytesRead; i++)
        {
            ds >> byte;
            receivedBytes.append(byte);
        }
        QByteArray restructuredBytes;
        switch(rotatorCommand.ModBusBytesRead)
        {
        case 2:
            restructuredBytes.append(receivedBytes[1]);
            restructuredBytes.append(receivedBytes[0]);
            break;
        case 4:
            restructuredBytes.append(receivedBytes[1]);
            restructuredBytes.append(receivedBytes[0]);
            restructuredBytes.append(receivedBytes[3]);
            restructuredBytes.append(receivedBytes[2]);
            break;
        }
        QDataStream readValueStream(&restructuredBytes, QIODevice::ReadOnly);
        readValueStream.setByteOrder(QDataStream::LittleEndian);
        readValueStream.setFloatingPointPrecision(QDataStream::SinglePrecision);
        switch (rotatorCommand.DataType)
        {
        case 0:
            readValueStream >> rotatorCommand.RegData_float;
            break;
        case 1:
            readValueStream >> rotatorCommand.RegData_uint16;
            break;
        case 2:
            readValueStream >> rotatorCommand.RegData_uint32;
            break;
        }
    }
    if (rotatorCommand.ModBusRW == 0x10)
    {
            ds >> rotatorCommand.ModbusRegAddr >> rotatorCommand.ModBusRegistersWritten;
    }
    qDebug() << "Rotator answer received";
    emit rotatorAnswerReceived();
}

void Rli::parseF1(QDataStream &ds)
{
    ds >> amplifierCommand.Addr >> amplifierCommand.Status;
    uint8_t data;
    amplifierCommand.Data.clear();
    for (int i=0;i<30;i++)
    {
           ds >> data;
           amplifierCommand.Data.append(data);
    }
    qDebug() << "Amplifier answer received";
    emit amplifierAnswerReceived();
}

void Rli::parseF8(QDataStream &ds)
{
        ds
                >> rotatorTelemetry.Position0
                >> rotatorTelemetry.Position1
                >> rotatorTelemetry.Velocity0
                >> rotatorTelemetry.Velocity1
                >> rotatorTelemetry.MotorTemperature0
                >> rotatorTelemetry.MotorTemperature1
                >> rotatorTelemetry.PCBTemperature0
                >> rotatorTelemetry.PCBTemperature1
                >> rotatorTelemetry.VoltageIn0
                >> rotatorTelemetry.VoltageIn1
                >> rotatorTelemetry.CurrentIn0
                >> rotatorTelemetry.CurrentIn1
                >> rotatorTelemetry.SetPointAcknowledge0
                >> rotatorTelemetry.SetPointAcknowledge1
                >> rotatorTelemetry.Emergency0
                >> rotatorTelemetry.Emergency1;
        ds.setFloatingPointPrecision(QDataStream::DoublePrecision);
        ds
                >> rotatorTelemetry.Longitude
                >> rotatorTelemetry.Latitude;
        ds.setFloatingPointPrecision(QDataStream::SinglePrecision);
        ds
                >> rotatorTelemetry.Altitude
                >> rotatorTelemetry.MagAzimuth;
        qDebug() << "Rotator telemetry received";
        emit rotatorTelemetryReceived();
}

void Rli::parseF9(QDataStream &ds)
{
        ds >> amplifierTelemetry.Addr >> amplifierTelemetry.Status >> amplifierTelemetry.ErrorCount;
        uint8_t error;
        amplifierTelemetry.Errors.clear();
        for (int i=0;i<29;i++)
        {
               ds >> error;
               amplifierTelemetry.Errors.append(error);
        }
        qDebug() << "Amplifier telemetry received";
        emit amplifierTelemetryReceived();
}

void Rli::timerEvent(QTimerEvent *event)
{
    if(event->timerId() == timerIdReceive)
    {
        QByteArray data;
        if(ethConn->receive(data))
        {
            qDebug() << data.toHex(' ');
            int8_t ReqFlag;
            //            int8_t MsgID;
            uint16_t DataSize;
            if(data.size() < 4) return;
            QDataStream ds(&data, QIODevice::ReadOnly);
            ds.setByteOrder(QDataStream::LittleEndian);
            ds.setFloatingPointPrecision(QDataStream::SinglePrecision);
            ds >> ReqFlag >> MsgID >> DataSize;
            if(DataSize != data.size()-4) return;
            parseFunc_t f = parseMap.value(MsgID);
            if(f != nullptr) (this->*f)(ds);
            taskQueueUpdate();
        }
    }

    if(event->timerId() == queueTaskTimerId)
    {
        if(taskQueue.isEmpty()) return;
        taskQueue.head().isTimeout = true;
        taskQueueUpdate();
    }
}

