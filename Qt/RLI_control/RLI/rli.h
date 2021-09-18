#ifndef RLI_H
#define RLI_H

#include <QObject>
#include <QQueue>
#include "ethconncontrol.h"

enum tasks_enum
{
    GetModeCtrl,
    SetModeCtrl,
    GetAdcGain,
    SetAdcGain,
    GetDacAtt,
    SetDacAtt,
    GetProcThreshold,
    SetProcThreshold,
    GetMti,
    SetMti,
    GetDistOffset,
    SetDistOffset,
    GetNumPeaks,
    SetNumPeaks,
    GetAdjust,
    SetAdjust,
    GetViewMode,
    SetViewMode,
    GetTimeSecsSinceEpoh,
    Reboot,
    GetStatus,
    GetScaleTable,
    GetMulticastAddress,
    SendToRotator,
    SendToAmplifier,
    GetRotatorTelemetry,
    GetAmplifierTelemetry

};

class Rli : public QObject
{
    Q_OBJECT

public:

    typedef void (Rli::*taskFunc_t)();
    struct task_t
    {
        task_t()
        {
            isTaskLaunched = false;
            isTimeout = false;
            resultId = 0;
            taskFunc = nullptr;
        }
        task_t(quint8 res, taskFunc_t fun)
        {
            isTaskLaunched = false;
            isTimeout = false;
            resultId = res;
            taskFunc = fun;
        }
        bool isTaskLaunched;
        bool isTimeout;
        quint8 resultId;
        taskFunc_t taskFunc;
    };

    QQueue<task_t> taskQueue;
    int queueTaskTimerId = 0;

    struct modeCtrl_t
    {
        uint8_t WorkMode;
        uint8_t ScaleCode;
        uint16_t ZondNum;
        float TargetYaw;
        float TargetPitch;
        uint8_t PwrLevel;
        uint8_t ExtEnableTX;
        uint16_t Rsv;
        uint32_t TRxFreq;
    }modeCtrl;

    struct state_t
    {
        uint8_t TempFPGA1;
        uint8_t TempFPGA2;
        uint8_t TempFMC;
        uint8_t CurWorkMode;
        uint32_t ErrCodes;
        uint32_t WorkTime;
    }state;

    struct adjust_t
    {
        uint8_t DACDelay;
        uint8_t RefDelay;
        uint8_t TxDelay;
        uint8_t TxLen;
        uint8_t RxDelay;
        uint8_t RxLen;
    }adjust;

    struct viewMode_t
    {
        uint32_t BlankSize;
        uint32_t RejectSize;
        uint32_t OffsetView;
        uint8_t Mode;
    }viewMode;

    struct acknowledge_t
    {
        uint8_t ReplyMsgID;
        uint8_t Result;
        uint8_t ErrorInfo;
    }acknowledge;

    float DistOffset;

    struct scale_t
    {
        float ScaleLen;
        float ScaleMaxMTI;
        float ScaleMaxDistOffs;
    };
    QVector<struct scale_t> scales;

    struct rotatorCommand
    {
        bool ReadReg = true;
        uint16_t RegAddr;
        int DataType = 0;
        uint8_t RegSize = 0;
        uint16_t RegData_uint16;
        uint32_t RegData_uint32;
        float RegData_float;

        uint8_t ModBusRW;
        uint16_t ModbusRegAddr;
        uint16_t ModBusRegistersWritten;
        uint8_t ModBusBytesRead;
    }rotatorCommand;

    struct rotatorTelemetry
    {
        float Position0;
        float Position1;
        float Velocity0;
        float Velocity1;
        float MotorTemperature0;
        float MotorTemperature1;
        float PCBTemperature0;
        float PCBTemperature1;
        float VoltageIn0;
        float VoltageIn1;
        float CurrentIn0;
        float CurrentIn1;
        unsigned short SetPointAcknowledge0;
        unsigned short SetPointAcknowledge1;
        unsigned int Emergency0;
        unsigned int Emergency1;
        double Longitude;
        double Latitude;
        float Altitude;
		float MagAzimuth;
    }rotatorTelemetry;

    struct amplifierCommand
    {
        uint8_t Addr;
        uint8_t Command;
        uint8_t Status;
        QByteArray Data;
    }amplifierCommand;

    struct amplifierTelemetry
    {
        uint8_t Addr;
        uint8_t Status;
        uint8_t ErrorCount;
        QByteArray Errors;
    }amplifierTelemetry;

    float MTIVal;
    float ProcThreshold;
    uint8_t NumPeaks;
    uint32_t TimeSecsSinceEpoh;
    uint8_t ADCGain;
    uint8_t DACAtt;
    uint8_t reboot_page=0;

    uint32_t GroupAddr;
    uint16_t DstPort;

    uint8_t MsgID;

    explicit Rli(QObject *parent = nullptr);
    ~Rli();
    void setModeCtrl();
    void scaleTableRequest();
    void statusRequest();
    void distOffsetRequest();
    bool isOpen();
    void modeCtrlRequest();
    void mtiRequest();
    void procThresholdRequest();
    void adcGainRequest();
    void numPeaksRequest();
    void dacAttRequest();
    void setAdjust();
    void adjustRequest();
    void setViewMode();
    void viewModeRequest();
    void multicastAddressRequest();
    void sendRotatorCommand();
    void sendAmplifierCommand();
    void rotatorTelemetryRequest();
    void amplifierTelemetryRequest();
    void reboot();

    void open();
    void close();
    void addTask(tasks_enum taskType);
    void addTask(tasks_enum taskType, void *arg);
    void timeSecsSinceEpohRequest();

private:
    int timerIdReceive = 0;
    void request(uint8_t MsgID);

    EthConnControl *ethConn = nullptr;

    typedef void (Rli::*parseFunc_t)(QDataStream &);
    QMap<uint8_t, parseFunc_t> parseMap;

    QMap<tasks_enum,task_t> taskSet;

    void parse34(QDataStream &ds);
    void parseA0(QDataStream &ds);
    void parseA1(QDataStream &ds);
    void parseC0(QDataStream &ds);
    void parseC1(QDataStream &ds);
    void parseC2(QDataStream &ds);
    void parseC3(QDataStream &ds);
    void parseC4(QDataStream &ds);
    void parseC5(QDataStream &ds);
    void parseC6(QDataStream &ds);
//    void parseCF(QDataStream &ds);
    void parseD0(QDataStream &ds);
    void parseD2(QDataStream &ds);
    void parseD4(QDataStream &ds);
    void parseF0(QDataStream &ds);
    void parseF1(QDataStream &ds);
    void parseF8(QDataStream &ds);
    void parseF9(QDataStream &ds);


    void taskQueueUpdate();
    void setAdcGain();
    void setDacAtt();
    void setProcThreshold();
    void setMti();
    void setDistOffset();
    void setNumPeaks();
    void setTimeSecsSinceEpoh();


    void cmdBody(QDataStream &ds);
    void setCommand(void (Rli::*cmd1)(QDataStream &));

    void transferMessage(QByteArray message);

signals:
    void scaleTableUpdated();
    void modeCtrlUpdated();
    void statusUpdated();
    void acknowledgeReceived();
    void ProcThresholdUpdated();
    void MTIValUpdated();
    void DistOffsetUpdated();
    void NumPeaksUpdated();
//    void TimeSecsSinceEpohUpdated();
    void ADCGainUpdated();
    void DACAttUpdated();
    void adjustUpdated();
    void viewModeUpdated();
    void multicastAddressUpdated();
    void rliTimeoutEvent();
    void rotatorAnswerReceived();
    void amplifierAnswerReceived();
    void rotatorTelemetryReceived();
    void amplifierTelemetryReceived();
    void deviceReboot();
    // QObject interface
protected:
    virtual void timerEvent(QTimerEvent *event) override;
};

#endif // RLI_H
