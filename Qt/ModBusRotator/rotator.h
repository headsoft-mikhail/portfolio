#ifndef ROTATOR_H
#define ROTATOR_H

#include <modbus.h>

class Rotator: public QObject
{
    Q_OBJECT
public:
    Rotator();
    ModBus control, telemetry;

    void initControlPort(QString port);
    void initTelemetryPort(QString port);

    void setCorner(float azimuth, float elevation);

    void readReg(uint16_t startRegNum, uint16_t bytesCount);
    void writeReg(uint16_t startRegNum, QByteArray data);

    float targetAzimuth=0, targetElevation=0, currentAzimuth=0, currentElevation=0;
    int ackYaw=2, ackPitch=2;
private:
    int yAngleDataPos=0, pAngleDataPos=4, yAckDataPos=48, pAckDataPos=50, yEmergencyDataPos=52, pEmergencyDataPos=56;

signals:
    void rotationStarted();
    void rotationFinished();
    void rotationUnknown();
    void telemetryUpdated();
    void emergency();
private slots:
    void updateTelemetry();

};

#endif // ROTATOR_H
