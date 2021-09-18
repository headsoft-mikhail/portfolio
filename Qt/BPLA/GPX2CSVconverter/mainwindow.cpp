#include "mainwindow.h"
#include "ui_mainwindow.h"


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
    QString path = QFileDialog::getOpenFileName(this, tr("Open File"),
                                                QDir::currentPath(),
                                                tr("Track files (*.gpx)"));
    QFile file(path);

    if(file.open(QIODevice::ReadOnly | QIODevice::Text))
    {
        latitudes.clear();
        longitudes.clear();
        int pointsCounter = 0;

        QTextStream ts(&file);
        QString log = "";
        ui->log->setText(log);
        while (!ts.atEnd())
        {
            QString line =ts.readLine();
            if (line.contains("<trkpt"))
            {
                QStringList stringList = line.split("\"");
                latitudes.append(stringList[1]);
                longitudes.append(stringList[3]);
                log.append(stringList[1]).append(" ").append(stringList[3]).append("\n");
                pointsCounter += 1;
            }
        }
        file.close();
        ui->log->setText(log);
        ui->status->setText(QString("%1 points read").arg(pointsCounter));
    }
    else
    {
        ui->status->setText("File opening error");
    }
}

void MainWindow::on_pushButton_Save_clicked()
{
    QString path = QFileDialog::getSaveFileName(this, QObject::tr("Save File"),
                                                QDir::currentPath() + "/new.csv",
                                                tr("Comma separated values (*.csv)"));
    logFile.setFileName(path);
    logFile.open(QIODevice::WriteOnly);
    textStreamLog.setDevice(&logFile);
    textStreamLog << "Latitude;Longitude;Altitude;Send_PIC;PIC_Lat;PIC_Lon;Send_Course;Course;Send_Servo;Servo;HoldTime\r\n";
    for (int i=0; i<latitudes.count(); i++)
    {
        textStreamLog << latitudes.at(i) << ";" << longitudes.at(i) << ";50;";
        for (int j=0;j<=7;j++)
            textStreamLog << "0;";
        textStreamLog <<  "\r\n";
    }
    logFile.close();
}
