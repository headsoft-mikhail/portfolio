#include "scanner.h"

Scanner::Scanner()
{
    connect(scanTimer, &QTimer::timeout, [this]{
        scanNext();
        emit nextScan();
    });
}

void Scanner::start(float scanCenterAz, float scanCenterEl)
{
    centerAz = scanCenterAz;
    centerEl = scanCenterEl;
    scanTimer->start(timerPeriod);
}

void Scanner::stop()
{
    scanTimer->stop();
    counter = 0;
}

void Scanner::scanNext()
{
    float theta = thetaStart + thetaStep * counter;
    if (theta > thetaStop)
        theta = thetaStop;
    else
        counter++;
    float rho = spiralStep * theta /(2 * M_PI);
    azimuth = rho * cos(theta);
    elevation = rho * sin(theta);
    if (abs(azimuth) < 0.01) azimuth = 0;
    if (abs(elevation) < 0.01) elevation = 0;
	azimuth += centerAz;
    elevation += centerEl;
    if (elevation < minElevation) elevation = minElevation;
    
}


