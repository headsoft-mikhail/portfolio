#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "geodesic.h"
#include <QDebug>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::on_pushButton_clicked()
{

    double radius = ui->lineEdit_radius->text().toDouble();
    double startAngle = ui->lineEdit_start->text().toDouble();
    double stopAngle = ui->lineEdit_stop->text().toDouble();
    double angleStep = ui->lineEdit_step->text().toDouble();
    double altitude = ui->lineEdit_alt->text().toDouble();
    double target_altitude = ui->lineEdit_t_alt->text().toDouble();
    double origin[2], target[2];
    double servo = Geodesic::CalculateElevation(radius, target_altitude, altitude);
    origin[0] = ui->lineEdit_centerLat->text().toDouble();
    origin[1] = ui->lineEdit_centerLon->text().toDouble();
    target[0] = 0;
    target[1] = 0;

    QString path = QFileDialog::getSaveFileName(this, QObject::tr("Save File"),
                                                QDir::currentPath() + "/new.csv",
                                                tr("Comma separated values (*.csv)"));
    logFile.setFileName(path);
    logFile.open(QIODevice::WriteOnly);
    textStreamLog.setDevice(&logFile);
    textStreamLog << "Latitude;Longitude;Altitude;Send_PIC;PIC_Lat;PIC_Lon;Send_Course;Course;Send_Servo;Servo;HoldTime\r\n";

    int angle = startAngle;
    while (angle<=stopAngle)
    {
        Geodesic::SphereDirect(origin, Geodesic::Clamp(angle), radius, target);
        textStreamLog << QString::number(target[0],'f',6) << ";"
                      << QString::number(target[1],'f',6) << ";"
                      << QString::number(altitude) << ";";
        if (ui->radioButton_cs->isChecked())
            textStreamLog << "0;0;0;1;"
                          << QString::number(Geodesic::CalculateAzimuth(origin[0], origin[1], target[0], target[1]), 'f', 1)
                          << ";1;"
                          << QString::number(servo, 'f', 1)
                          << ";0";
        if (ui->radioButton_pic->isChecked())
            textStreamLog << "1;"
                          << QString::number(origin[0],'f',6)
                          << ";"
                          << QString::number(origin[1],'f',6)
                          << ";0;0;0;0;0";
        textStreamLog <<  "\r\n";
        angle += angleStep;
    }
    logFile.close();
}

void MainWindow::on_radioButton_cs_toggled(bool checked)
{
     ui->lineEdit_t_alt->setEnabled(checked);
}
