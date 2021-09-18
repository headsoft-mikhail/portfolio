#include "ethconncontrol.h"


EthConnControl::EthConnControl()
{
}

void EthConnControl::init()
{
    if (FAILED (WSAStartup (MAKEWORD( 1, 1 ), &ws) ) )
    {
        int error = WSAGetLastError();
        qDebug() << error;
        return;
    }

    if (INVALID_SOCKET == (controlSocket = socket (AF_INET, SOCK_DGRAM, IPPROTO_UDP) ) )
    {
        int error = WSAGetLastError();
        qDebug() << error;
        return;
    }

    controlAddress.sin_family = AF_INET;
    controlAddress.sin_addr.s_addr = inet_addr("192.168.12.5");
    controlAddress.sin_port = htons(8051);

    DWORD nonBlocking = 1;
    if ( ioctlsocket( controlSocket, FIONBIO, &nonBlocking ) != 0 )
    {
        printf( "failed to set non-blocking socket\n" );
    }
}

void EthConnControl::close()
{
    closesocket(controlSocket);
    WSACleanup();
}

void EthConnControl::send(QByteArray &send)
{
    sendto(controlSocket, send.data(), send.size(), 0, reinterpret_cast<const sockaddr*>(&controlAddress), sizeof(controlAddress));
}

bool EthConnControl::receive(QByteArray &receive)
{
    receive.resize(1600);
    int count = recv( controlSocket, receive.data(), receive.size(), 0);
    receive.resize(count);
    return count>0;
}
