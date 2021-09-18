#include "locator.h"
#include <math.h>
#include <QDataStream>
#include <QDateTime>

Locator::Locator(QObject *parent) : QObject(parent)
{
    ethConn.init();
    startTimer(5);
}

Locator::~Locator()
{
    ethConn.close();
}

void Locator::startRecord()
{
    recordList.clear();
    isRecording = true;
}

void Locator::stopRecord()
{
    isRecording = false;
}

void Locator::saveRecord(QString filename)
{
    if(!recordList.size()) return;
    QFile filesave(QDateTime::currentDateTime().toString("yyyy.MM.dd_hh-mm-ss_")+filename);
    filesave.open(QIODevice::WriteOnly);
    QDataStream ds(&filesave);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);

    uint32_t NumBlks = recordList.size();
    ds << NumBlks;
    for(uint32_t i=0;i<NumBlks;++i)
    {
        ds << i;
        ds << recordList[i];
    }
    filesave.close();
    recordList.clear();
}

bool Locator::startLog(QString filename)
{
    if(isLogWriting) return false;
    logFile.setFileName("logs\\txt\\" + QDateTime::currentDateTime().toString("yyyy.MM.dd_hh-mm-ss_")+filename);
    if(!logFile.open(QIODevice::WriteOnly)) return false;
    textStreamLog.setDevice(&logFile);
    isLogWriting = true;
    return true;
}

void Locator::stopLog()
{
    isLogWriting = false;
    logFile.close();
}

void Locator::timerEvent(QTimerEvent *)
{
    QByteArray currentData;
    while(ethConn.receive(currentData))
    {

        if(currentData.size()<2)
        {
            return;
        }
        unsigned short FragIdx = (currentData[0]&0xFF) | (((unsigned short)currentData[1]&0xFF)<<8);
        bool errorNum = (FragIdx != FragIdxToReceive) && (FragIdx != 0);
        if(errorNum)
        {
            FragIdxToReceive = 0;
            rliMessage.clear();
            emit packetsQueueError();
            qDebug() << "packetsQueueError";
        }
        else
        {
            if(FragIdx==0) FragIdxToReceive=1;
            else ++FragIdxToReceive;
            rliMessage.append(currentData.mid(2));

            if ((rliMessage.count() >= 12) && (checkZeroEnding(rliMessage.right(12))))
            {
                if (debugLocatorData)
                    qDebug() << "Locator data received" << QTime::currentTime().toString("hh:mm:ss.zzz");
                debugLocatorData = wantDebugLocator;
                wantDebugLocator = false;

                if(parse(rliMessage))
                {
                    emit dataReceived();
                    if(isRecording) recordList.append(rliMessage);
                    rliMessage.clear();
                }
            }
        }
    }
}

bool Locator::parse(QByteArray &qbRliResult)
{
    QDataStream ds(&qbRliResult,QIODevice::ReadOnly);
    ds.setByteOrder(QDataStream::LittleEndian);
    ds.setFloatingPointPrecision(QDataStream::SinglePrecision);
    ds >>
            headerData.TSSec >>
            headerData.TSNSec >>
            headerData.DataType >>
            headerData.ScaleCode >>
            headerData.MTIWidth >>
            headerData.WorkMode >>
            headerData.Rsv1 >>
            headerData.DistResolution >>
            headerData.DoplerResolution >>
            headerData.ProcThreshold >>
            headerData.DistOffset >>
            headerData.Rsv2 >>
            headerData.TRxFreq >>
            headerData.MaxAmp >>
            headerData.YawAngle >>
            headerData.PitchAngle >>
            headerData.YawPosReady >>
            headerData.PitchPosReady >>
            headerData.Rsv4 >>
            headerData.Rsv5 >>
            headerData.Rsv6 >>
            headerData.MaxItems;
    int dataCount = (qbRliResult.size() - ds.device()->pos())/12 - 1;

    // rotator status
    setCurrentAngles(headerData.YawAngle, headerData.PitchAngle);
    if (isRotating && (headerData.YawPosReady > 0) && (headerData.PitchPosReady > 0))
    {
        isRotating = false;
        emit rotationFinished();
    }

    // get amplitudes, find max
    Amplitudes_dB.resize(dataCount);
    Doplers_mps.resize(dataCount);
    Noises_dB.resize(dataCount);
    Distances_m.resize(dataCount);

    bool dataValid = true;

    unsigned short distance;
    short dopler;
    float amplitude;
    float noise;

    isMaxExists = false;
    maxAmplitude_dB = std::numeric_limits<float>::min();

    for(int i=0; i<dataCount;++i)
    {
        ds >> distance >> dopler >> amplitude >> noise;
        if ((debugLocatorData) && (i % 40 == 0))
            qDebug() << QString::number(distance) + "   "
                        + QString::number(dopler) + "   "
                        + QString::number(amplitude, 'f', 1) + "  (" + QString::number(20*log10(amplitude), 'f', 1) + " dB)   "
                        + QString::number(noise, 'f', 1) + "  (" + QString::number(20*log10(noise) - noiseLevelShift, 'f', 1) + " dB)";
        if(distance < dataCount)
        {
            Distances_m[distance] = headerData.DistResolution * distance;
            Amplitudes_dB[distance] = 20*log10(amplitude);
            Doplers_mps[distance] = headerData.DoplerResolution * dopler;
            Noises_dB[distance] = 20*log10(noise) - noiseLevelShift;

            if ((Distances_m[distance] >= minDistanceTreshold) &&  (Distances_m[distance] <= maxDistanceTreshold))
            {
                if ((qAbs(Doplers_mps[distance]) >= minDoplerTreshold) && (qAbs(Doplers_mps[distance]) <= maxDoplerTreshold))
                {
                    if (maxAmplitude_dB < Amplitudes_dB[distance])
                    {
                        maxAmplitude_dB = Amplitudes_dB[distance];
                        if ((maxAmplitude_dB - Noises_dB[distance]) >= (snrTreshold + noiseLevelShift))
                        {
                            maxDistance = distance;
                            maxDistance_m = Distances_m[distance];
                            maxDopler_mps = Doplers_mps[distance];
                            isMaxExists = true;
                        }
                    }
                }
            }
        }
        else
        {
            dataValid = false;
            qDebug() << "Data not valid: distance >= dataCount";
        }
    }
    if (debugLocatorData)
        qDebug() << "______________________________________________________";


    if(isMaxExists)
    {
        // calculate table data
        maxNoise_shifted_dB = Noises_dB[qMin(maxDistance + noisePointsShift, dataCount)];
        maxSNR_shifted_dB = maxAmplitude_dB - maxNoise_shifted_dB;
        maxReachableDistance_km = pow(((Pmax*ECS*pow(10, maxSNR_shifted_dB/10)*Tmax)/(P*ECSd*S*T)), 0.25) * maxDistance_m/1000;

        // log
        if(isLogWriting)
        {
            textStreamLog << QString("%1 %2 %3 %4 %5 %6 %7\r")
                             .arg(QDateTime::currentDateTime().toString("HHmmss.zzz"))
                             .arg(maxDistance_m)
                             .arg(maxDopler_mps)
                             .arg(maxAmplitude_dB)
                             .arg(maxNoise_shifted_dB)
                             .arg(maxSNR_shifted_dB)
                             .arg(maxReachableDistance_km);
        }
    }

    return dataValid;
}

bool Locator::checkZeroEnding(QByteArray formularEnd)
{
    QDataStream ds(&formularEnd,QIODevice::ReadOnly);
    unsigned short distance;
    short dopler;
    float amplitude;
    float noise;
    ds  >> distance >> dopler >> amplitude >> noise;
    return ((distance == 0) && (dopler == 0) && (amplitude == 0) && (noise == 0));
}

void Locator::setTresholds(short minDist_, short maxDist_, double minDopler_, bool enSDC, double maxDopler_, double minSNR_)
{
    minDistanceTreshold = minDist_;
    maxDistanceTreshold = maxDist_;
    snrTreshold = minSNR_;
    if(enSDC)
        minDoplerTreshold = minDopler_;
    else
        minDoplerTreshold = 0;
    maxDoplerTreshold = maxDopler_;
}

void Locator::setCalculationCoefficients(int noisePointsShift_, double Pmax_, double S_, double Tmax_, double ECS_)
{
    noisePointsShift = noisePointsShift_;
    Pmax = Pmax_;
    S = S_;
    Tmax = Tmax_;
    ECS = ECS_;
}

void Locator::setVariables(double noiseLevelShift_, int powerLevel_, double T_, double ECSd_)
{
    noiseLevelShift = noiseLevelShift_;
    T=T_;
    ECSd=ECSd_;
    switch(powerLevel_)
    {
    case 0:
        P=10;
        //noiseLevelShift = noiseLevelShift_ + 6;
        break;
    case 1:
        P=5;
        //noiseLevelShift = noiseLevelShift_ + 3;
        break;
    case 2:
        P=2.5;
        //noiseLevelShift = noiseLevelShift_;
        break;
    }
}

float Locator::clamp(float angle)
{
    while(angle>180)
        angle-=360;
    while (angle<-180)
        angle+=360;
    return angle;
}

float Locator::lesserArc(float angle_1, float angle_2)
{
    float arc = qAbs(angle_1 - angle_2);
    while(arc>180)
        arc-=360;
    return qAbs(arc);
}

void Locator::setCurrentAngles(float yaw, float pitch)
{
    currentAzimuth = yaw;
    currentElevation = pitch;
}

void Locator::setTargetAngles(float yaw, float pitch)
{
    targetAzimuth = yaw;
    targetElevation = pitch;
    isRotating = true;
    emit rotationStarted();
}

void Locator::enableDebug()
{
    wantDebugLocator = true;
}
