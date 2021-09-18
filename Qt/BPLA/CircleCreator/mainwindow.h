#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QFile>
#include <QTextStream>
#include <QFileDialog>

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
    void on_pushButton_clicked();

    void on_radioButton_cs_toggled(bool checked);

    void on_checkBox_spdEn_clicked(bool checked);

private:
    Ui::MainWindow *ui;
    QFile logFile;
    QTextStream textStreamLog;
    QVector<QString> latitudes, longitudes;
};
#endif // MAINWINDOW_H
