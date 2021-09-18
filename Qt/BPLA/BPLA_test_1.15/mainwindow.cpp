#include "mainwindow.h"
#include "./ui_mainwindow.h"
#include <QTime>
#include <QStyleFactory>
#include <QFileDialog>

// ----- Some constants and defines -----

MainWindow::MainWindow(QWidget *parent)
	: QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
	ui->setupUi(this);
    this->resize(508, 407);;
    // Form Style
    QStringList styles = QStyleFactory::keys();
    foreach (QString styleName, styles)
    {
        if (QString::compare(styleName, "Fusion", Qt::CaseInsensitive) == 0)
            QApplication::setStyle(QStyleFactory::create("Fusion"));
    }
    setWindowFlags(Qt::Dialog | Qt::MSWindowsFixedSizeDialogHint);

    mk6 = new uav("127.0.0.1", 32180, 64300);

    // connection to UAV
    connect(ui->connectToHost, &QPushButton::clicked, this, [this]{
        if(ui->connectToHost->isChecked())
            mk6->ipconnect();
        else
        {
            mk6->ipdisconnect();
            ui->connectCargo->setChecked(false);
            if (ui->sendTrack->isChecked()) ui->sendTrack->click();
        }

    });
    connect(ui->checkBox_Reconnect, &QAbstractButton::clicked, mk6, &uav::try_reconnect);
    connect(mk6, &uav::connectionAborted, this, [this]{
        if (!ui->checkBox_Reconnect->isChecked())
                ui->connectToHost->setChecked(false);
    });
    // send single point
    connect(ui->sendCtrlMessage, &QPushButton::clicked, this, [this]{
        mk6->nextPoint = new wayPoint(ui->moveToPointRequestLatitude->text().toDouble(),
                ui->moveToPointRequestLongitude->text().toDouble(),
                ui->moveToPointRequestAltitude->text().toFloat(),
                ui->groupBox_pointInCoordinates->isChecked(),
                ui->pointInCoordinatesLatitude->text().toDouble(),
                ui->pointInCoordinatesLongitude->text().toDouble(),
                ui->groupBox_course->isChecked(),
                ui->moveToPointRequestCourse->text().toFloat(),
                ui->groupBox_servo->isChecked(),
                ui->servoAngle->text().toFloat(),
                ui->holdPointActionTime->text().toInt()
                );
        mk6->sendNextPoint();
    });
    // send track
    connect(ui->loadTrack, &QPushButton::clicked, this, [this]{
        if (ui->sendTrack->isChecked()) ui->sendTrack->click();
        QString path = QFileDialog::getOpenFileName(0, QObject::tr("Select track file"));
        writeToLog(path);
        trackloader.load(path);
        ui->sendTrack->setText("Send track\n[" + QString::number(trackloader.pointsLeft()) + "]");
        ui->sendTrack->setEnabled(trackloader.pointsLeft() > 0);
        ui->previewTrack->setEnabled(trackloader.pointsLeft() > 0);
    });
    connect(ui->sendTrack, &QPushButton::clicked, this, [this]{
        trackloader.start(ui->sendTrack->isChecked());
        mk6->pointCompleted = true;
    });
    connect(trackloader.pointsSendTimer, &QTimer::timeout, this, [this]() {
        if (mk6->pointAccepted)
        {
            trackloader.deleteAcceptedPoint();
            mk6->pointAccepted = false;
            ui->sendTrack->setText("Send track\n[" + QString::number(trackloader.pointsLeft()) + "]");
            ui->sendTrack->setEnabled(trackloader.pointsLeft() > 0);
            ui->sendTrack->setChecked(trackloader.pointsLeft() > 0);
        }
        if (mk6->pointCompleted)
        {
            if (trackloader.pointsLeft() > 0)
            {
                mk6->nextPoint = new wayPoint(trackloader.next());
                mk6->sendNextPoint();
            }
            else
            {
                writeToLog("<<<  Track flight finished");
                qDebug() << "Track flight finished";
                ui->sendTrack->clicked(false);
            }
        }
    });
    connect(ui->previewTrack, &QPushButton::clicked, this, [this]{
        QVector<wayPoint> track_preview;
        for (int i=0; i<trackloader.track.count(); i++)
            track_preview.append(trackloader.track[i]);
        while (track_preview.count()>0)
        {
            mk6->nextPoint = new wayPoint(track_preview.first());
            mk6->sendNextPoint();
            track_preview.removeFirst();
        }
    });

    // cargo
    connect(ui->connectCargo, &QPushButton::clicked, this, [this]{ui->connectCargo->setChecked(mk6->linkCargo());});
    connect(ui->pushButton_SetCargoPeriod, &QPushButton::clicked, this, [this]{mk6->cargo->setPeriod(ui->spinBox_cargoPeriod->value());});
    connect(ui->pushButton_SetCargoDuration, &QPushButton::clicked, this, [this]{mk6->cargo->setDuration(ui->spinBox_cargoDuration->value());});
    connect(ui->pushButton_SetCargoFrequency, &QPushButton::clicked, this,  [this]{mk6->cargo->setFrequency(ui->spinBox_CargoFrequency->value());});
    connect(ui->pushButton_CargoGeneratorON, &QPushButton::clicked, this, [this]{mk6->cargo->turnOn(ui->spinBox_cargoPeriod->value(), ui->spinBox_cargoDuration->value(), ui->spinBox_CargoFrequency->value());});
    connect(ui->pushButton_CargoGeneratorOFF, &QPushButton::clicked, this, [this]{mk6->cargo->turnOff();});

    // message to LOG
    connect(mk6, &uav::message, this, &MainWindow::writeToLog);
    connect(&trackloader, &trackloader::message, this, &MainWindow::writeToLog);

    // retranslator
    connect(ui->retranslator_enable, &QPushButton::clicked, this, [this]{
        if (ui->retranslator_enable->isChecked())
        {
            data_retranslator = new tcpconnection(ui->retranslator_ip->text(), ui->retranslator_port->value(), "retranslator");
            connect(data_retranslator, &tcpconnection::message, this, &MainWindow::writeToLog);
            connect(data_retranslator, &tcpconnection::aborted, this, &MainWindow::retranslatorAborted);
            data_retranslator->try_reconnect = ui->checkBox_Reconnect_retranslator->isChecked();
            data_retranslator->ipconnect();
        }
        else
        {
            data_retranslator->ipdisconnect();
            disconnect(data_retranslator, &tcpconnection::message, this, &MainWindow::writeToLog);
            disconnect(data_retranslator, &tcpconnection::aborted, this, &MainWindow::retranslatorAborted);
        }
    });
    connect(ui->checkBox_Reconnect_retranslator, &QCheckBox::clicked, this, [this]{data_retranslator->try_reconnect = ui->checkBox_Reconnect_retranslator->isChecked();});
    connect(mk6, &uav::navigationArrived, this, [this]{
        if (ui->naviLog->isChecked())
        {
            writeToLog( QString(">>>  POS[%1 %2 %3] S:%4 A:%5 E:%6").
                        arg(mk6->telemetry.latitude, 0, 'g', 8).
                        arg(mk6->telemetry.longitude, 0, 'g', 8).
                        arg(mk6->telemetry.altitude, 0, 'f', 2).
                        arg(mk6->telemetry.speed, 0, 'f', 0).
                        arg(mk6->telemetry.course, 0, 'f', 0).
                        arg(mk6->telemetry.servo_angle, 0, 'f', 0));
        }
        if (ui->retranslator_enable->isChecked())
        {
            QByteArray data;
            QDataStream ds(&data, QIODevice::WriteOnly);
            ds.setByteOrder(QDataStream::LittleEndian);
            ds.setFloatingPointPrecision(QDataStream::SinglePrecision);

            ds << mk6->telemetry.latitude
               << mk6->telemetry.longitude
               << mk6->telemetry.altitude
               << mk6->telemetry.speed
               << mk6->telemetry.course
               << mk6->telemetry.servo_angle
               << mk6->cargo->genParams.frequency
               << mk6->cargo->genParams.period
               << mk6->cargo->genParams.duration
               << mk6->cargo->genParams.isPreset;
            int bytesWritten = data_retranslator->m_connection->write(data);
            //qDebug() << bytesWritten << "   " << data.toHex();
        }
    });

}

MainWindow::~MainWindow()
{
	delete ui;
}

// LOG
void MainWindow::writeToLog(QString txt)
{
    while (txt.length()<55)
        txt.append(" ");
    txt.append(QTime::currentTime().toString("hh:mm:ss.zzz"));
    ui->log->append(txt);
}

void MainWindow::on_clearLog_clicked()
{
    ui->log->setText("");
}

void MainWindow::retranslatorAborted()
{
    ui->retranslator_enable->setChecked(false);
}

void MainWindow::on_expand_clicked(bool checked)
{
    if (checked)
        this->resize(690, 407);
    else
        this->resize(508, 407);
}
