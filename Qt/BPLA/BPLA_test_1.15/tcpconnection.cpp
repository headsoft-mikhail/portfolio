#include "tcpconnection.h"

tcpconnection::tcpconnection()
{

}

tcpconnection::tcpconnection(QString ip, uint16_t port, QString tag)
{
    this->ip = ip;
    this->port = port;
    this->tag = tag;
    m_connection = new QTcpSocket(this);
    m_tcpReconnectTimer = new QTimer(this);
    m_tcpReconnectTimer->setSingleShot(true);
    m_tcpReconnectTimer->setInterval(tcpReconnect);
    connect(m_tcpReconnectTimer, &QTimer::timeout, this, &tcpconnection::ipconnect);
    connect(m_connection, &QTcpSocket::disconnected, this, &tcpconnection::onDisconnected);
    connect(m_connection, &QTcpSocket::connected, this, &tcpconnection::onConnected);
    connect(m_connection, QOverload<QAbstractSocket::SocketError>::of(&QTcpSocket::error), this, &tcpconnection::onError);
}

void tcpconnection::onError()
{
    qDebug() << QString("error connecting to host [%1]").arg(tag);
    emit message(QString("-----   Error connecting to host [%1]  -----").arg(tag));
    onDisconnected();
}

void tcpconnection::onDisconnected()
{
    qDebug() << QString("disconnected from host  [%1]").arg(tag);
    emit message(QString("-----   Disconnected from host [%1]  -----").arg(tag));
    if (try_reconnect)
    {
        m_tcpReconnectTimer->start();
        qDebug() << QString("reconnect timer started  [%1]").arg(tag);
        emit message(QString("-----   Reconnect timer started [%1]  -----").arg(tag));
    }
    else
    {
        qDebug() << QString("aborted  [%1]").arg(tag);
        emit aborted();
    }
}

void tcpconnection::onConnected()
{
    m_tcpReconnectTimer->stop();
    qDebug() << ip << port << m_connection->localPort() << m_connection->peerPort() << m_connection->peerName();
    qDebug() << QString("connected to host  [%1]").arg(tag);
    emit message(QString("-----   Connection established [%1]  -----").arg(tag));
}

void tcpconnection::ipconnect()
{
    if(m_connection->isOpen())
        m_connection->disconnectFromHost();
    qDebug() << ip << port;
    m_connection->connectToHost(ip, port);

    qDebug() << QString("connecting to host  [%1]...").arg(tag);
    emit message(QString("-----   Connecting to host  [%1]...   -----").arg(tag));
}

void tcpconnection::ipdisconnect()
{
    m_connection->disconnectFromHost();
    m_tcpReconnectTimer->stop();
}
