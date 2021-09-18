#ifndef MODBUS_H
#define MODBUS_H

#include <QSerialPort>
#include <QTimer>

#define read_from_master = 0x10;
#define read_from_slave = 0x03;
#define write_to_slave = 0x10;

class ModBus : public QObject
{
    Q_OBJECT
    enum RW
            {
                WRITE = 0x10,
                READ = 0x03
            };

public:
    ModBus();

    enum PRIMACY
            {
                MASTER = true,
                SLAVE = false
            };
    bool primacy;

    QByteArray receivedBuffer;
    bool isOpened = false;
    uint8_t devId = 1;
    int waitTime = 50;

    void writeReg(uint16_t startRegNum, QByteArray data);
    void readReg(uint16_t startRegNum, uint16_t bytesCount);
    void init(QString portName, PRIMACY master_);
    void close();

    struct rwAnswer
    {
        uint8_t devID;
        uint8_t rwStatus;
        uint16_t startRegisterAddress;
        uint16_t registerCount;
        uint8_t bytesCount=0;
        QByteArray data;
        uint8_t parsedBytes;
    }rwAnswer;

    bool checkCheckSum(QByteArray receivedBuffer, uint8_t message_len, uint8_t postfix_len);

private:
    static const uint16_t INIT_VALUE = 0xFFFF;
    static const uint16_t crc16table[];
    QSerialPort *serial;

    int timerID = 0;

    void receiveData();
    bool parse();

    bool shortLengthError, longLengthError, crcError;

public slots:
    static QByteArray int322array(int32_t toSendUint16);
    static int32_t array2uint32(QByteArray receivedArray);
    static QByteArray float2array(float toSendFloat);
    static float array2float(QByteArray receivedArray);
    static QByteArray uint162array(uint16_t toSendUint16);
    static uint16_t array2uint16(QByteArray receivedArray);
private slots:
    uint16_t get(const uint8_t *data, uint16_t len);
    void calc(uint8_t byte, uint16_t &crc);

signals:
    void askedToWrite(QString status, QByteArray qb);
    void askedToRead(QString status, QByteArray qb);

    void receivedAnswer(QString status, QByteArray qb);
    void error(QString status, QByteArray qb);

protected:
    virtual void timerEvent(QTimerEvent *event) override;

};

#endif // MODBUS_H
