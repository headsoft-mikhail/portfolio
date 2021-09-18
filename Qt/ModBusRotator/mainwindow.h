#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QSerialPort>
#include <QSerialPortInfo>
#include <rotator.h>
#include <QFile>
#include <QObject>
#include <QTextStream>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();


private slots:
    void updateCommandList(QString status, QByteArray data);
    void updateTelemetryList(QString status, QByteArray data);

    void on_pushButton_ScanPort_clicked();
    void on_pushButton_OpenControl_clicked();
    void on_pushButton_OpenTelemetry_clicked();

    void on_pushButtonReadReg_clicked();
    void on_pushButton_WriteRegister_clicked();

    void on_pushButton_Move_clicked();

    void on_pushButton_Stop_clicked();

    void on_pushButton_Log_clicked(bool checked);

private:
    Ui::MainWindow *ui;
    Rotator rotator;

    QFile logFile;
    QTextStream textStreamLog;
    bool isLogWriting = false;

};
#endif // MAINWINDOW_H
