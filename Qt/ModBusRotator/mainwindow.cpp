#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    connect(&rotator.control, SIGNAL(askedToRead(QString, QByteArray)), this, SLOT(updateCommandList(QString,QByteArray)));
    connect(&rotator.control, SIGNAL(askedToWrite(QString,QByteArray)), this, SLOT(updateCommandList(QString,QByteArray)));
    connect(&rotator.control, SIGNAL(receivedAnswer(QString,QByteArray)), this, SLOT(updateCommandList(QString,QByteArray)));
    connect(&rotator.control, SIGNAL(error(QString,QByteArray)), this, SLOT(updateCommandList(QString,QByteArray)));

    connect(&rotator.telemetry, SIGNAL(error(QString,QByteArray)), this, SLOT(updateTelemetryList(QString,QByteArray)));
    connect(&rotator.telemetry, SIGNAL(receivedAnswer(QString,QByteArray)), this, SLOT(updateTelemetryList(QString,QByteArray)));

    connect(&rotator, &Rotator::emergency, this,[this]{ ui->label_Status->setText("Status: Emergency"); });
    connect(&rotator, &Rotator::rotationStarted, this,[this]{ ui->label_Status->setText("Status: Rotation"); });
    connect(&rotator, &Rotator::rotationFinished, this,[this]{ ui->label_Status->setText("Status: Stopped"); });
    connect(&rotator, &Rotator::rotationUnknown, this,[this]{ ui->label_Status->setText("Status: Unknown"); });
    connect(&rotator, &Rotator::telemetryUpdated, this,[this]{
        ui->label_Corners->setText(QString("Position:\tA:%1\tE:%2").arg(QString::number(rotator.currentAzimuth, 'f', 1)).arg(QString::number(rotator.currentElevation, 'f', 1)));
        ui->label_Ack->setText(QString("Acknowl.:\tA:%1\tE:%2").arg(QString::number(rotator.ackYaw)).arg(QString::number(rotator.ackPitch)));
    });
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::updateCommandList(QString status, QByteArray data)
{
    ui->listWidget_Control->addItem(status.append(QString(data.toHex(' '))));
    ui->listWidget_Control->scrollToBottom();

    if (ui->listWidget_Control->count() > 500)
        ui->listWidget_Control->takeItem(0);
}

void MainWindow::updateTelemetryList(QString status, QByteArray data)
{
    int receivedCount = data.count();
    if (status == "Received:\t")
        ui->listWidget_Telemetry->addItem(status + QString::number(rotator.telemetry.rwAnswer.parsedBytes) + "[" + QString::number(receivedCount) + "] " + QString(data.toHex(' ')));
    else
        ui->listWidget_Telemetry->addItem(status + "[" + QString::number(rotator.telemetry.rwAnswer.parsedBytes) + "/" + QString::number(receivedCount) + "] " + QString(data.toHex(' ')));

    ui->listWidget_Telemetry->scrollToBottom();
    if (ui->listWidget_Telemetry->count() > 500)
        ui->listWidget_Telemetry->takeItem(0);

    if(isLogWriting)
        textStreamLog << QString(data.toHex()) << "\r\n";
}

void MainWindow::on_pushButton_Log_clicked(bool checked)
{
    isLogWriting = checked;
    if (isLogWriting)
    {
        logFile.setFileName("log.txt");
        if(logFile.open(QIODevice::WriteOnly))
            textStreamLog.setDevice(&logFile);
        else
            ui->pushButton_Log->click();
    }
    else
    {
        if (logFile.isOpen())
            logFile.close();
    }
}

void MainWindow::on_pushButton_ScanPort_clicked()
{
    if (rotator.control.isOpened)
        rotator.control.close();
    if (rotator.telemetry.isOpened)
        rotator.telemetry.close();
    ui->comboBox_ControlPort->clear();
    ui->comboBox_TelemetryPort->clear();
    int count = 0;
    const auto infos = QSerialPortInfo::availablePorts();
    for (const QSerialPortInfo &info : infos)
    {
        ui->comboBox_ControlPort->addItem(info.portName());
        ui->comboBox_TelemetryPort->addItem(info.portName());
        count++;
    }
    if (count > 0)
    {
        ui->comboBox_ControlPort->setEnabled(true);
        ui->comboBox_TelemetryPort->setEnabled(true);
        ui->pushButton_OpenControl->setEnabled(true);
        ui->pushButton_OpenTelemetry->setEnabled(true);
    }
    else
    {
        ui->comboBox_ControlPort->setEnabled(false);
        ui->comboBox_TelemetryPort->setEnabled(false);
        ui->pushButton_OpenControl->setEnabled(false);
        ui->pushButton_OpenTelemetry->setEnabled(false);
    }
}

void MainWindow::on_pushButton_OpenControl_clicked()
{
    rotator.initControlPort(ui->comboBox_ControlPort->currentText());
    ui->pushButton_OpenControl->setEnabled(!rotator.control.isOpened);
    ui->comboBox_ControlPort->setEnabled(!rotator.control.isOpened);
}

void MainWindow::on_pushButton_OpenTelemetry_clicked()
{
    rotator.initTelemetryPort(ui->comboBox_TelemetryPort->currentText());
    ui->pushButton_OpenTelemetry->setEnabled(!rotator.telemetry.isOpened);
    ui->comboBox_TelemetryPort->setEnabled(!rotator.telemetry.isOpened);
}

void MainWindow::on_pushButtonReadReg_clicked()
{
    uint16_t address = ui->spinBox_registerAddress->value();
    uint16_t bytesCount;
    switch (ui->comboBox_valueType->currentIndex())
    {
    case 1:
        bytesCount = 0x02;
        break;
    default:
        bytesCount = 0x04;
        break;
    }
    rotator.readReg(address, bytesCount);
}

void MainWindow::on_pushButton_WriteRegister_clicked()
{
    QByteArray messageArray;
    float float_value;
    uint16_t uint16_value;
    uint32_t uint32_value;
    switch (ui->comboBox_valueType->currentIndex())
    {
    case 0:
        float_value = ui->doubleSpinBox_RegisterValue->value();
        messageArray = ModBus::float2array(float_value);
        break;
    case 1:
        uint16_value = ui->doubleSpinBox_RegisterValue->value();
        messageArray = ModBus::uint162array(uint16_value);
        break;
    case 2:
        uint32_value = ui->doubleSpinBox_RegisterValue->value();
        messageArray = ModBus::int322array(uint32_value);
        break;
    }
    uint16_t address = ui->spinBox_registerAddress->value();
    rotator.writeReg(address, messageArray);
}

void MainWindow::on_pushButton_Move_clicked()
{
    rotator.setCorner(ui->doubleSpinBox_YAW->value(), ui->doubleSpinBox_PITCH->value());
}

void MainWindow::on_pushButton_Stop_clicked()
{
    rotator.setCorner(rotator.currentAzimuth, rotator.currentElevation);
}
