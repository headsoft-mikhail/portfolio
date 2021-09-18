#ifndef LOCATOR_H
#define LOCATOR_H

#include <QObject>
#include "ethconndatagram.h"
#include <QFile>

class Locator : public QObject
{
    Q_OBJECT
public:
    explicit Locator(QObject *parent = nullptr);
    ~Locator();
    QVector<double> Amplitudes_dB;
    QVector<double> Doplers_mps;
    QVector<double> Noises_dB;
    QVector<double> Distances_m;

    int dataReceivedCounter = 0;
    void startRecord();
    void stopRecord();
    void saveRecord(QString filename);

    bool startLog(QString filename);
    void stopLog();

    unsigned short minDistanceTreshold = 0;
    unsigned short maxDistanceTreshold = 0;
    float snrTreshold = 0;
    float minDoplerTreshold = 0;
    float maxDoplerTreshold = 0;

    float maxDistance_m;
    unsigned short maxDistance;
    float maxDopler_mps;
    float maxAmplitude_dB;
    float maxNoise_shifted_dB;
    float maxSNR_shifted_dB;
    float maxReachableDistance_km;
    bool isMaxExists = false;

    float targetAzimuth = 0, targetElevation = 0;
    float currentAzimuth = 0, currentElevation = 0;
    float angleTolerance = 1;

    double Pmax=100, S = 20.4, Tmax = 4, ECS = 3;
    double P=10, T=0.0839, ECSd=0.28;
    double noiseLevelShift = 15;
    int noisePointsShift = 0;

    struct headerData
    {
        unsigned int TSSec;
        unsigned int TSNSec;
        quint8 DataType;
        quint8 ScaleCode;
        quint8 MTIWidth;
        quint8 WorkMode;
        int Rsv1;
        float DistResolution;
        float DoplerResolution;
        float ProcThreshold;
        unsigned short DistOffset;
        short Rsv2;
        unsigned int TRxFreq;
        float MaxAmp;
        float YawAngle;
        float PitchAngle;
        unsigned short PitchPosReady;
        unsigned short YawPosReady;
        int Rsv4;
        int Rsv5;
        short Rsv6;
        unsigned short MaxItems;
    }headerData;
    
private:

    QByteArrayList recordList;
    bool isRecording = false;
    bool isLogWriting = false;
    bool isRotating = false;
    QFile logFile;
    QTextStream textStreamLog;

    EthConnDatagram ethConn;
    QByteArray rliMessage;
    int FragIdxToReceive = 0;

    bool parse(QByteArray &qbRliResult);
    bool checkZeroEnding(QByteArray formularEnd);
    bool debugLocatorData = false;
    bool wantDebugLocator = false;

public slots:
    void setTresholds(short minDist_, short maxDist_, double minDopler_,  bool enSDC, double maxDopler_, double minSNR_);
    void setVariables(double noiseLevelShift_, int powerLevel_, double T_, double ECSd_);
    void setCalculationCoefficients(int noisePointsShift_, double Pmax_, double S_, double Tmax_, double ECS_);
    void setCurrentAngles(float yaw, float pitch);
    void setTargetAngles(float yaw, float pitch);
    static float clamp(float angle);
    static float lesserArc(float angle_1, float angle_2);
    void enableDebug();
signals:
    void dataReceived();
    void rotationFinished();
    void rotationStarted();
    void packetsQueueError();


    // QObject interface
protected:
    virtual void timerEvent(QTimerEvent *) override;
};

#endif // LOCATOR_H
