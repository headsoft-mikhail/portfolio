#ifndef GRAPHICS_H
#define GRAPHICS_H

#include <QDialog>
#include "locator.h"

namespace Ui {
class Graphics;
}

class Graphics : public QDialog
{
    Q_OBJECT

public:
    explicit Graphics(QWidget *parent = nullptr);
    ~Graphics();
    double minDist, maxDist, minSNR;
    void enableSaveJsonButton();

private:
    Ui::Graphics *ui;
    double minAmp_bound, maxAmp_bound, minNoise_bound, maxNoise_bound, minSNR_bound, maxSNR_bound, minDist_bound, maxDist_bound;


public slots:
    void plot(QVector<double> distances,
              QVector<double> amplitudes,
              QVector<double> noises,
              unsigned int TSSec,
              unsigned int TSNSec,
              quint8 DataType,
              quint8 ScaleCode,
              quint8 MTIWidth,
              quint8 WorkMode,
              float DistResolution,
              float DoplerResolution,
              float ProcThreshold,
              unsigned short DistOffset,
              unsigned int TRxFreq,
              float MaxAmp,
              float YawAngle,
              float PitchAngle,
              unsigned short PitchPosReady,
              unsigned short YawPosReady,
              unsigned short MaxItems);
    void receiveRecordFilters(double minDist_, double maxDist_, double minSNR_);
    void receiveTimerElapsed(int time);
signals:
    void saveJsonSignal(int count);
    void getDebug();
private slots:
    void on_pushButton_Debug_clicked();
};

#endif // GRAPHICS_H
