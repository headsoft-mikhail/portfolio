#include "rotator.h"

Rotator::Rotator()
{  
    connect(&telemetry, SIGNAL(receivedAnswer(QString,QByteArray)), this, SLOT(updateTelemetry()));
}

void Rotator::initControlPort(QString port)
{
    control.init(port,  ModBus::PRIMACY::SLAVE);
}


void Rotator::initTelemetryPort(QString port)
{
    telemetry.init(port, ModBus::PRIMACY::MASTER);
}


void Rotator::setCorner(float azimuth, float elevation)
{
    if (control.isOpened)
    {
        QByteArray messageArray;
        messageArray.append(ModBus::float2array(azimuth));
        messageArray.append(ModBus::float2array(elevation));
        targetAzimuth = azimuth;
        targetElevation = elevation;
        writeReg(0x610, messageArray);
    }
}

void Rotator::updateTelemetry()
{
    if (telemetry.rwAnswer.bytesCount >= pAngleDataPos+sizeof(currentElevation))
    {
        currentAzimuth = ModBus::array2float(telemetry.rwAnswer.data.mid(yAngleDataPos, sizeof(currentAzimuth)));
        currentElevation = ModBus::array2float(telemetry.rwAnswer.data.mid(pAngleDataPos, sizeof(currentElevation)));
    }

    if (telemetry.rwAnswer.bytesCount >= pAckDataPos+sizeof(ackPitch))
    {
        ackYaw = ModBus::array2uint16(telemetry.rwAnswer.data.mid(yAckDataPos, sizeof(ackYaw)));
        ackPitch = ModBus::array2uint16(telemetry.rwAnswer.data.mid(pAckDataPos, sizeof(ackPitch)));

        if ((ackYaw==1)&&(ackPitch==1))
            emit rotationFinished();

        if ((ackYaw==0)||(ackPitch==0))
            emit rotationStarted();

        if ((ackYaw==2)||(ackPitch==2))
            emit rotationUnknown();
    }

    if (telemetry.rwAnswer.bytesCount >= pEmergencyDataPos+sizeof(sizeof(uint32_t)) &&
            ((ModBus::array2uint32(telemetry.rwAnswer.data.mid(yEmergencyDataPos, sizeof(uint32_t))) != 0) ||
             (ModBus::array2uint32(telemetry.rwAnswer.data.mid(pEmergencyDataPos, sizeof(uint32_t))) != 0)))
    {
        emit emergency();
    }

    emit telemetryUpdated();
}

void Rotator::readReg(uint16_t startRegNum, uint16_t bytesCount)
{
        if (control.isOpened)
            control.readReg(startRegNum, bytesCount);
}

void Rotator::writeReg(uint16_t startRegNum, QByteArray data)
{
        if (control.isOpened)
            control.writeReg(startRegNum, data);
}
