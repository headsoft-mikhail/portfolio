#ifndef SCANNER_H
#define SCANNER_H

#include <QObject>
#include <QTimer>
#include <QtMath>


class Scanner : public QObject
{
    Q_OBJECT

public:
    Scanner();
    QTimer *scanTimer = new QTimer();

    void start(float scanCenterAz, float scanCenterEl);
    void stop();
    void scanNext();
    float azimuth = 0;
    float elevation = 0;
    int timerPeriod = 2500;
    int minElevation = 0;
    float spiralStep = 1;

private:
    bool timerOn = false;
    float thetaStep = M_PI_4;
    float thetaStart = 2*M_PI;
    float thetaStop = 12*M_PI;
    int counter = 0;
    float centerAz = 0;
    float centerEl = 0;
signals:
    void nextScan();
};

#endif // SCANNER_H
