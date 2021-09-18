#ifndef ETHCONNDATAGRAM_H
#define ETHCONNDATAGRAM_H

#include <ws2tcpip.h>
#include <QBuffer>
#include <QDebug>
#include <QTimer>

class EthConnDatagram
{
public:
    EthConnDatagram();
    void init();
    void close();
    void send(QByteArray send);
    bool receive(QByteArray &receive);
private:
    WSADATA ws;
    SOCKET datagramSocket;
    sockaddr_in datagramAddress;
    QTimer timer;
};

#endif // ETHCONNDATAGRAM_H
