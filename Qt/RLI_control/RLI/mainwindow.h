#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QTableWidgetItem>
#include "locator.h"
#include "rli.h"
#include "graphics.h"
#include <QProcess>
#include "tcpconnection.h"
#include "json.h"
#include "scanner.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

    void RestoreDefaultsRli();
    void GetINISettings(QString filename);
    void WriteRotatorCommandLog(QString text);
    void WriteAmplifierCommandLog(QString text);
    void WriteRotatorTelemetryLog(QString text);
    void WriteAmplifierTelemetryLog(QString text);
    void SetScalesToolTip(int scaleNum);

private:
    Ui::MainWindow *ui;
    Rli rli;
    QProcess pingProcess;
    Locator locator;
    Graphics *graphicsForm;
    QTimer *plotTimer = new QTimer();
    QTime receiveGraphTime;
    bool sendTimerResult = false;
    json jsonSaver;
    tcpconnection* data_retranslator = new tcpconnection();
    Scanner scanner;

    QPixmap *blackDot = new QPixmap("images/blackDot.png");
    QPixmap *greenDot = new QPixmap("images/greenDot.png");

    QTableWidgetItem *item0 = new QTableWidgetItem();
    QTableWidgetItem *item1 = new QTableWidgetItem();
    QTableWidgetItem *item2 = new QTableWidgetItem();
    QTableWidgetItem *item3 = new QTableWidgetItem();
    QTableWidgetItem *item4 = new QTableWidgetItem();
    QTableWidgetItem *item5 = new QTableWidgetItem();
    QTableWidgetItem *item6 = new QTableWidgetItem();
    QTableWidgetItem *item7 = new QTableWidgetItem();

    bool isGetNumPeaks = false;
    bool isGetAdjust = false;
    bool isGetViewMode = false;
    bool isGetDacAtt = false;
    bool isGetAdcGain = false;
    bool isGetMti = false;
    bool isGetProcThreshold = false;
    bool isGetDistOffset = false;

    bool plot = true;

    QString iniFile = "settings.ini";

protected:
    void closeEvent(QCloseEvent *event);
private slots:
    void setAdjust();
    void setViewMode();
    void recordFiltersChanged();
    void setLocatorVariables();

    void onPingEnded();
    void savePointToJson();
    void jsonRecord(int count);
    void ShowStatusMessage(QString message);
    void keyPressEvent(QKeyEvent *event);

    void on_pushButton_Scan_clicked(bool checked);

signals:
    void dataTimerElapsed(int msec);
    void scaleLengthReceived();
    void plotData(QVector<double> distances,
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
    void setRecordFilters(double minDist, double maxDist, double minSNR);
};
#endif // MAINWINDOW_H
