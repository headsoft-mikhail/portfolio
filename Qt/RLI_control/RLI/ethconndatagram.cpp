#include "ethconndatagram.h"


EthConnDatagram::EthConnDatagram()
{
}

void EthConnDatagram::init()
{
    if (FAILED (WSAStartup (MAKEWORD( 1, 1 ), &ws) ) )
    {
        int error = WSAGetLastError();
        qDebug() << error;
        return;
    }

    if (INVALID_SOCKET == (datagramSocket = socket (AF_INET, SOCK_DGRAM, IPPROTO_UDP) ) )
    {
        int error = WSAGetLastError();
        qDebug() << error;
        return;
    }


    datagramAddress.sin_family = AF_INET;
    datagramAddress.sin_addr.s_addr = htonl(INADDR_ANY);
    datagramAddress.sin_port = htons(50005);

    if(bind(datagramSocket, reinterpret_cast<const sockaddr*>(&datagramAddress), sizeof (datagramAddress))<0)
    {
        qDebug() << "bind error";
    }

    int receiveBufferSize = 1000000;
    int receiveBufferSizeLen = sizeof(receiveBufferSize);
    setsockopt(datagramSocket, SOL_SOCKET, SO_RCVBUF, (char*)&receiveBufferSize, receiveBufferSizeLen);

    struct ip_mreq mreq;
    mreq.imr_interface.s_addr = inet_addr("192.168.12.6");
    mreq.imr_multiaddr.s_addr = inet_addr("232.192.0.25");
    setsockopt(datagramSocket, IPPROTO_IP, IP_ADD_MEMBERSHIP, (char *)&mreq, sizeof(mreq));

    DWORD nonBlocking = 1;
    if ( ioctlsocket( datagramSocket, FIONBIO, &nonBlocking ) != 0 )
    {
        printf( "failed to set non-blocking socket\n" );
    }
}

void EthConnDatagram::close()
{
    closesocket(datagramSocket);
    WSACleanup();
}

void EthConnDatagram::send(QByteArray send)
{
    sendto(datagramSocket, send.data(), send.size(), 0, reinterpret_cast<const sockaddr*>(&datagramAddress), sizeof(datagramAddress));
}

bool EthConnDatagram::receive(QByteArray &receive)
{
    receive.resize(1600);
    int count;
    count = recv( datagramSocket, receive.data(), receive.size(), 0);
    receive.resize(count);
    return count>0;
}
