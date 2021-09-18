#ifndef TCPCONNECTION_H
#define TCPCONNECTION_H

#include <QObject>
#include <QDebug>
#include <QtNetwork/QTcpSocket>
#include <QTimer>

constexpr uint tcpReconnect {5000};

class tcpconnection : public QObject
{
    Q_OBJECT
public:
    tcpconnection();
    tcpconnection(QString ip, uint16_t port, QString tag = "");
    QTcpSocket* m_connection = nullptr;
    bool try_reconnect = false;

    void onError();
    void onDisconnected();
    void onConnected();
    void ipconnect();
    void ipdisconnect();
private:
    QString ip;
    quint16 port;
    QString tag;
    QTimer* m_tcpReconnectTimer = nullptr;
signals:
    void message(QString message);
    void aborted();
};

#endif // TCPCONNECTION_H
