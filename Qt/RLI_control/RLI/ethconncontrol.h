#ifndef ETHCONNCONTROL_H
#define ETHCONNCONTROL_H

#include <ws2tcpip.h>
#include <QBuffer>
#include <QDebug>
#include <QTimer>

class EthConnControl
{
public:
    EthConnControl();
    void init();
    void close();
    void send(QByteArray &send);
    bool receive(QByteArray &receive);
private:
    WSADATA ws;
    SOCKET controlSocket;
    sockaddr_in controlAddress;
    QTimer timer;
};

#endif // ETHCONNCONTROL_H
