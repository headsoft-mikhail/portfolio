#include "modbus.h"
#include <QDataStream>
#include <QTimerEvent>

ModBus::ModBus()
{
}

void ModBus::init(QString portName, PRIMACY master_)
{
    close();
    primacy = master_;
    serial = new QSerialPort(portName);
    serial->setBaudRate(QSerialPort::Baud115200);
    isOpened = serial->open(QIODevice::ReadWrite);
    if (isOpened)
        connect(serial, &QSerialPort::readyRead, this, &ModBus::receiveData);
}

void ModBus::close()
{
    if (isOpened)
    {
        serial->close();
        isOpened = false;
        disconnect(serial, &QSerialPort::readyRead, this, &ModBus::receiveData);
    }
}

void ModBus::receiveData()
{
    if(timerID) killTimer(timerID);
    timerID = startTimer(waitTime);
    receivedBuffer.append(serial->readAll());
}

void ModBus::timerEvent(QTimerEvent *event)
{
    if(timerID)
    {
        killTimer(timerID);
        timerID = 0;
    }
    if ((receivedBuffer.count()>0) && (parse()))
        emit receivedAnswer("Received:\t", receivedBuffer);
    else
    {
        QString status;
        if (shortLengthError)
            status = "LEN_Err:";
        else if (crcError)
            status = "CRC_Err:";
        emit error(status + "\t", receivedBuffer);
    }
    receivedBuffer.clear();
}

bool ModBus::parse()
{
    crcError = false;
    shortLengthError = false;
    longLengthError = false;

    bool parseResult = false;
    uint8_t buffer_length = receivedBuffer.size();
    uint8_t prefix_len = sizeof(rwAnswer.devID) + sizeof(rwAnswer.rwStatus);
    uint8_t postfix_len = sizeof(crc16table[0]);

    shortLengthError = (buffer_length < prefix_len);
    if(!shortLengthError)
    {
        QDataStream ds(&receivedBuffer, QIODevice::ReadOnly);
        rwAnswer.data.clear();
        ds >> rwAnswer.devID >> rwAnswer.rwStatus;
        uint8_t byte;
        switch(rwAnswer.rwStatus)
        {
        case ModBus::READ:
            switch(primacy)
            {
            case PRIMACY::SLAVE: // reading answer from slave
                prefix_len += sizeof(rwAnswer.bytesCount);
                shortLengthError = (buffer_length < prefix_len);
                if (!shortLengthError)
                {
                    ds >> rwAnswer.bytesCount;
                    shortLengthError = (buffer_length < prefix_len + rwAnswer.bytesCount + postfix_len);
                    longLengthError = (buffer_length > prefix_len + rwAnswer.bytesCount + postfix_len);
                    if (longLengthError)
                        receivedBuffer = receivedBuffer.left(prefix_len + rwAnswer.bytesCount + postfix_len);
                    if ((!shortLengthError) && (!shortLengthError))
                    {
                        crcError = !checkCheckSum(receivedBuffer, prefix_len + rwAnswer.bytesCount + postfix_len, postfix_len);
                        if (!crcError)
                        {
                            for (int i=0; i<rwAnswer.bytesCount; i++)
                            {
                                ds >> byte;
                                rwAnswer.data.append(byte);
                            }
                            parseResult = true;
                        }
                    }
                }
                break;
            case PRIMACY::MASTER: // reading request from master
                prefix_len += sizeof(rwAnswer.startRegisterAddress) + sizeof(rwAnswer.registerCount);
                rwAnswer.bytesCount = 0;
                shortLengthError = (buffer_length < prefix_len + rwAnswer.bytesCount + postfix_len);
                longLengthError = (buffer_length > prefix_len + rwAnswer.bytesCount + postfix_len);
                if (longLengthError)
                    receivedBuffer = receivedBuffer.left(prefix_len + rwAnswer.bytesCount + postfix_len);
                if ((!shortLengthError) && (!shortLengthError))
                {
                    crcError = !checkCheckSum(receivedBuffer, prefix_len + rwAnswer.bytesCount + postfix_len, postfix_len);
                    if (!crcError)
                        parseResult = true;
                }
                break;
            }
            break;
        case ModBus::WRITE:
            prefix_len += sizeof(rwAnswer.startRegisterAddress) + sizeof(rwAnswer.registerCount);
            shortLengthError = (buffer_length < prefix_len);
            if (!shortLengthError)
            {
                ds >> rwAnswer.startRegisterAddress >> rwAnswer.registerCount;
                switch(primacy)
                {
                case PRIMACY::SLAVE: //writing answer from slave
                    rwAnswer.bytesCount = 0;
                    shortLengthError = (buffer_length < prefix_len + rwAnswer.bytesCount + postfix_len);
                    longLengthError = (buffer_length > prefix_len + rwAnswer.bytesCount + postfix_len);
                    if (longLengthError)
                        receivedBuffer = receivedBuffer.left(prefix_len + postfix_len);
                    if ((!shortLengthError) && (!shortLengthError))
                    {
                        crcError = !checkCheckSum(receivedBuffer, prefix_len + rwAnswer.bytesCount + postfix_len, postfix_len);
                        if (!crcError)
                            parseResult = true;
                    }
                    break;
                case PRIMACY::MASTER: //writing reqest from master
                    prefix_len += sizeof(rwAnswer.bytesCount);
                    shortLengthError = (buffer_length < prefix_len);
                    if (!shortLengthError)
                    {
                        ds >> rwAnswer.bytesCount;
                        shortLengthError = (buffer_length < prefix_len + rwAnswer.bytesCount + postfix_len);
                        longLengthError = (buffer_length > prefix_len + rwAnswer.bytesCount + postfix_len);
                        if (longLengthError)
                            receivedBuffer = receivedBuffer.left(prefix_len + rwAnswer.bytesCount + postfix_len);
                        if ((!shortLengthError) && (!shortLengthError))
                        {
                            crcError = !checkCheckSum(receivedBuffer, prefix_len + rwAnswer.bytesCount + postfix_len, postfix_len);
                            if (!crcError)
                            {
                                for (int i=0; i<rwAnswer.bytesCount; i++)
                                {
                                    ds >> byte;
                                    rwAnswer.data.append(byte);
                                }
                                parseResult = true;
                            }
                        }
                    }
                    break;
                }
            }
            break;
        }


        if (parseResult)
            rwAnswer.parsedBytes = prefix_len + rwAnswer.bytesCount + postfix_len;
        else
            rwAnswer.parsedBytes = 0;  
    }
    return parseResult;
}

void ModBus::writeReg(uint16_t startRegNum, QByteArray data)
{
    QByteArray qb;
    QDataStream ds(&qb,QIODevice::WriteOnly);
    uint16_t regCount = data.size() >> 1;
    uint8_t byteCount = data.size();
    ds << devId << (uint8_t)ModBus::WRITE << startRegNum << regCount << byteCount;
    for (int i=0;i<byteCount;++i) {
        uint8_t val = data[i];
        ds << val;
    }
    uint16_t crcRes = get(reinterpret_cast<const uint8_t*>(qb.data()),qb.size());
    ds.setByteOrder(QDataStream::LittleEndian);
    ds << crcRes;
    serial->write(qb);
    emit askedToWrite("Writing:\t", qb);
}

void ModBus::readReg(uint16_t startRegNum, uint16_t bytesCount)
{
    rwAnswer.startRegisterAddress = startRegNum;
    QByteArray qb;
    QDataStream ds(&qb,QIODevice::WriteOnly);
    uint16_t regCount = bytesCount >> 1;
    ds << devId << (uint8_t)ModBus::READ << startRegNum << regCount;
    uint16_t crcRes = get(reinterpret_cast<const uint8_t*>(qb.data()),qb.size());
    ds.setByteOrder(QDataStream::LittleEndian);
    ds << crcRes;
    serial->write(qb);
    emit askedToRead("Reading:\t", qb);
}

bool ModBus::checkCheckSum(QByteArray receivedBuffer,  //filled or overfilled buffer
                           uint8_t message_len,
                           uint8_t postfix_len)
{
    QByteArray dataBytes = receivedBuffer.left(message_len);
    uint16_t crcCalculated = get(reinterpret_cast<const uint8_t*>(dataBytes.left(message_len - postfix_len).data()), message_len - postfix_len);
    uint16_t crcReceived = ((dataBytes.right(postfix_len)[1] << 8)&0xFF00) | ((dataBytes.right(postfix_len)[0])&0x00FF);
    return (crcCalculated==crcReceived);
}

const uint16_t ModBus::crc16table[256] =
{
    0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
    0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
    0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
    0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
    0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,
    0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
    0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
    0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
    0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
    0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
    0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
    0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
    0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
    0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
    0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
    0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
    0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
    0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
    0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
    0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
    0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
    0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
    0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
    0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
    0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
    0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
    0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
    0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
    0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
    0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
    0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
    0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040
};


uint16_t ModBus::get(const uint8_t *data, uint16_t len) {
    uint16_t crc = INIT_VALUE;
    while (len--) {
        calc(*data++, crc);
    }
    return crc;
}

void ModBus::calc(uint8_t byte, uint16_t &crc) {
    crc = ((crc >> 8)) ^ crc16table[(crc ^ (byte)) & 0xFF];
}

QByteArray ModBus::float2array(float toSendFloat)
{
    QByteArray data;
    QDataStream ds(&data,QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);
    ds << toSendFloat;
    QByteArray toSend;
    toSend.append(data[1]);
    toSend.append(data[0]);
    toSend.append(data[3]);
    toSend.append(data[2]);
    return toSend;
}

float ModBus::array2float(QByteArray receivedArray)
{
    QByteArray data;
    data.append(receivedArray[1]);
    data.append(receivedArray[0]);
    data.append(receivedArray[3]);
    data.append(receivedArray[2]);
    QDataStream ds(&data,QIODevice::ReadOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);
    float dataFloat;
    ds >> dataFloat;
    return dataFloat;
}

QByteArray ModBus::int322array(int32_t toSendUint32)
{
    QByteArray data;
    QDataStream ds(&data,QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);
    ds << toSendUint32;
    QByteArray toSend;
    toSend.append(data[1]);
    toSend.append(data[0]);
    toSend.append(data[3]);
    toSend.append(data[2]);
    return toSend;
}

int32_t ModBus::array2uint32(QByteArray receivedArray)
{
    QByteArray data;
    data.append(receivedArray[1]);
    data.append(receivedArray[0]);
    data.append(receivedArray[3]);
    data.append(receivedArray[2]);
    QDataStream ds(&data,QIODevice::ReadOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    int32_t dataUint32;
    ds >> dataUint32;
    return dataUint32;
}

QByteArray ModBus::uint162array(uint16_t toSendUint16)
{
    QByteArray data;
    QDataStream ds(&data,QIODevice::WriteOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);
    ds << toSendUint16;
    QByteArray toSend;
    toSend.append(data[1]);
    toSend.append(data[0]);
    return toSend;
}

uint16_t ModBus::array2uint16(QByteArray receivedArray)
{
    QByteArray data;
    data.append(receivedArray[1]);
    data.append(receivedArray[0]);
    QDataStream ds(&data,QIODevice::ReadOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    uint32_t dataUint16;
    ds >> dataUint16;
    return dataUint16;
}
