#ifndef CARGO_H
#define CARGO_H

#include <QObject>
#include <QDebug>
#include <QtNetwork/QTcpSocket>
#include <QTimer>
#include "tcpconnection.h"

class cargo : public QObject
{
    Q_OBJECT
public:
    cargo(QString ip, uint port);
    tcpconnection* tcp;

    struct genParams
    {
        uint16_t period;
        uint8_t periodNum;
        uint16_t duration;
        uint8_t durationNum;
        uint64_t frequency;
        uint8_t frequencyNum;
        bool isPreset = true;
    }genParams;

    void sendMessage(int8_t commandID, int8_t value);
    void turnOn(uint16_t period, uint16_t duration, uint16_t frequency);
    void turnOff();
    void setPeriod(uint16_t period);
    void setDuration(uint16_t duration);
    void setFrequency(uint64_t frequency);
    uint8_t period2num(uint16_t period);
    uint8_t duration2num(uint16_t duration);
    uint8_t frequency2num(uint64_t frequency);
    void ipconnect();
    void ipdisconnect();
    void onIncomingData();
private:
    QString ip;
    uint32_t port;
public slots:
    void getChildMessage(QString str);
signals:
    void message(QString str);



};

#endif // CARGO_H
