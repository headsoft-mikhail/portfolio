#include "cargo.h"

cargo::cargo(QString ip, uint port)
{
    tcp = new tcpconnection(ip, port, "cargo");
    connect(tcp->m_connection, &QTcpSocket::readyRead, this, [this]{onIncomingData();});
    connect(tcp, SIGNAL(message(QString)),this, SLOT(getChildMessage(QString)));
}

void cargo::ipconnect()
{
    tcp->ipconnect();
}

void cargo::ipdisconnect()
{
    tcp->ipdisconnect();
}


void cargo::sendMessage(int8_t commandID, int8_t value)
{
    QByteArray cargoMsg;
    cargoMsg.append(commandID);
    cargoMsg.append(value);
    auto writeSize = tcp->m_connection->write(cargoMsg, 2);
    if (writeSize>0)
    {
        qDebug() << "cargo message sent: " << cargoMsg.toHex(' ') << "  [" << writeSize << "]";
        emit message("<<< " + cargoMsg.toHex(' '));
    }
    else
        emit message("-----   Cargo is unavailable  -----");
    return;
}

void cargo::turnOn(uint16_t period, uint16_t duration, uint16_t frequency)
{
    sendMessage(0x81, 0x01); // on amplifier
    sendMessage(0x24, 0x01); // on profile_1
    sendMessage(0x25, 0x04); // select CH4
    setPeriod(period);
    setDuration(duration);
    setFrequency(frequency);
    genParams.isPreset = true;
    qDebug() << "onboard generator switched on";
    emit message("-----   Generator switched on   -----");
    return;
}

void cargo::turnOff()
{
    sendMessage(0x81, 0x10); // off amplifier
    sendMessage(0x24, 0x00); // on profile_off
    sendMessage(0x25, 0x00); // select CH_OFF
    genParams.isPreset = true;
    qDebug() << "onboard generator switched off";
    emit message("-----   Generator switched off   -----");
    return;
}

void cargo::setPeriod(uint16_t period)
{
    genParams.period = period;
    genParams.periodNum = period2num(period);
    genParams.isPreset = false;
    sendMessage(0x21, genParams.periodNum);
}
void cargo::setDuration(uint16_t duration)
{
    genParams.duration = duration;
    genParams.durationNum = duration2num(duration);
    genParams.isPreset = false;
    sendMessage(0x22, genParams.durationNum);
}
void cargo::setFrequency(uint64_t frequency)
{
    genParams.frequency = frequency;
    genParams.frequencyNum = frequency2num(frequency);
    sendMessage(0x23, genParams.frequencyNum);
}

uint8_t cargo::period2num(uint16_t period)
{
    return period;
}
uint8_t cargo::duration2num(uint16_t duration)
{
    return duration;
}
uint8_t cargo::frequency2num(uint64_t frequency)
{
    return (int8_t)((frequency-6800)/100);
}

void cargo::onIncomingData()
{
    QByteArray cargoMsg = tcp->m_connection->readAll();
    emit message(">>> " + cargoMsg.toHex(' '));
    qDebug() << "cargo message received: " << cargoMsg.toHex(' ') << "  [" << cargoMsg.count() << "]";
}

void cargo::getChildMessage(QString str)
{
    emit message(str);
}
