#ifndef UAV_H
#define UAV_H

#include <QObject>
#include "waypoint.h"
#include "cargo.h"
#include "protovar.h"
#include "tcpconnection.h"
#include "WaypointActionCtrl.pb.h"
#include "TelemetryCtrl.pb.h"
#include <QTimer>
#include <QDebug>

class uav : public QObject
{
    Q_OBJECT
public:
    uav(QString ip, uint main_port, uint cargo_port);
    uint32_t boardId = 924;

    cargo* cargo;
    wayPoint* nextPoint;

    bool pointAccepted = false;
    bool pointCompleted = false;


    struct telemetry
    {
        double latitude;
        double longitude;
        float altitude;
        float course;
        float speed;
        float servo_angle;
    }telemetry;

    void sendNextPoint();
    bool linkCargo();

    void ipconnect();
    void ipdisconnect();
    void try_reconnect(bool try_reconnect);

    void onIncomingData();
    bool parseHeader();
    void parseIncomingData(uint16_t len, uint16_t type);

private:
    QByteArray msgPart;
    tcpconnection* tcp;
    bool enableSender;

public slots:
    void getChildMessage(QString str);
signals:
    void message(QString message);
    void navigationArrived();
    void connectionAborted();
};

#endif // UAV_H
