#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QStyleFactory>
#include <QSerialPortInfo>
#include <math.h>
#include <QToolTip>
#include <QSettings>
#include <QDebug>
#include <QTime>
#include <QDir>
#include <QWhatsThis>
#include <QKeyEvent>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    qDebug() << "start program at " << QTime::currentTime().toString("hh:mm:ss.zzz");
    /////////////////////////////////////
    ///
    ///
    /// form settings
    ///
    ///
    /////////////////////////////////////
    QStringList styles = QStyleFactory::keys();
    foreach (QString styleName, styles)
    {
        if (QString::compare(styleName, "Fusion", Qt::CaseInsensitive) == 0)
            QApplication::setStyle(QStyleFactory::create("Fusion"));
    }
    setWindowIcon(QIcon("images/antenna.png"));
    setWindowTitle("Программа управления БЛУ-0320 v" + QString(VERSION_STRING));
    this->setGeometry(0,0,800,600);

    if (!QDir("logs").exists())
        QDir().mkdir("logs");
    if (!QDir("logs\\txt").exists())
        QDir().mkdir("logs\\txt");
    if (!QDir("logs\\json").exists())
        QDir().mkdir("logs\\json");

    /////////////////////////////////////
    ///
    ///
    /// graphic
    ///
    ///
    /////////////////////////////////////
    graphicsForm = new Graphics();
    plotTimer->start(100);

    connect(&locator, &Locator::packetsQueueError, this, [this]{ShowStatusMessage("Порядок пакетов!");});

    connect(ui->pushButton_graph, &QAbstractButton::clicked, graphicsForm, [this]{
        if (graphicsForm->isHidden())
            graphicsForm->show();
        else
            graphicsForm->hide();
    });

    connect(plotTimer, &QTimer::timeout, [this]{ plot = true; });

    connect(&locator, &Locator::dataReceived, [this]{
        if (sendTimerResult)
        {
            emit dataTimerElapsed(receiveGraphTime.elapsed());
            sendTimerResult = false;
        }
        if (!graphicsForm->isHidden() && plot)
        {
            emit plotData(locator.Distances_m,
                          locator.Amplitudes_dB,
                          locator.Noises_dB,
                          locator.headerData.TSSec,
                          locator.headerData.TSNSec,
                          locator.headerData.DataType,
                          locator.headerData.ScaleCode,
                          locator.headerData.MTIWidth,
                          locator.headerData.WorkMode,
                          locator.headerData.DistResolution,
                          locator.headerData.DoplerResolution,
                          locator.headerData.ProcThreshold,
                          locator.headerData.DistOffset,
                          locator.headerData.TRxFreq,
                          locator.headerData.MaxAmp,
                          locator.headerData.YawAngle,
                          locator.headerData.PitchAngle,
                          locator.headerData.PitchPosReady,
                          locator.headerData.YawPosReady,
                          locator.headerData.MaxItems
                          );
            plot = false;
        }
    });

    connect(graphicsForm, SIGNAL(getDebug()), &locator, SLOT(enableDebug()));

    connect(this, SIGNAL(dataTimerElapsed(int)), graphicsForm, SLOT(receiveTimerElapsed(int)));

    //typedef QVector<double> myQVectorDataType;
    //qRegisterMetaType<myQVectorDataType>("myQVectorDataType");

    connect(this, SIGNAL(plotData(QVector<double>,
                                  QVector<double>,
                                  QVector<double>,
                                  unsigned int,
                                  unsigned int,
                                  quint8,
                                  quint8,
                                  quint8,
                                  quint8,
                                  float,
                                  float,
                                  float,
                                  unsigned short,
                                  unsigned int,
                                  float,
                                  float,
                                  float,
                                  unsigned short,
                                  unsigned short,
                                  unsigned short)),
            graphicsForm, SLOT(plot(QVector<double>,
                                    QVector<double>,
                                    QVector<double>,
                                    unsigned int,
                                    unsigned int,
                                    quint8,
                                    quint8,
                                    quint8,
                                    quint8,
                                    float,
                                    float,
                                    float,
                                    unsigned short,
                                    unsigned int,
                                    float,
                                    float,
                                    float,
                                    unsigned short,
                                    unsigned short,
                                    unsigned short)));

    connect(this, SIGNAL(setRecordFilters(double, double, double)), graphicsForm, SLOT(receiveRecordFilters(double, double, double)));


    /////////////////////////////////////
    ///
    ///
    /// table
    ///
    ///
    /////////////////////////////////////


    item0->setText("-");
    item1->setText("-");
    item2->setText("-");
    item3->setText("-");
    item4->setText("-");
    item5->setText("-");
    item6->setText("-");
    item7->setText("-");
    ui->tableWidget_TargetInfo->setItem(0,0,item0);
    ui->tableWidget_TargetInfo->setItem(0,1,item1);
    ui->tableWidget_TargetInfo->setItem(0,2,item2);
    ui->tableWidget_TargetInfo->setItem(0,3,item3);
    ui->tableWidget_TargetInfo->setItem(0,4,item4);
    ui->tableWidget_TargetInfo->setItem(0,5,item5);
    ui->tableWidget_TargetInfo->setItem(0,6,item6);
    ui->tableWidget_TargetInfo->setItem(0,7,item7);

    connect(&locator, &Locator::dataReceived, [this]{
        if (locator.isMaxExists)
        {
            item0->setTextColor("black");
            item1->setTextColor("black");
            item2->setTextColor("black");
            item3->setTextColor("black");
            item4->setTextColor("black");
            item5->setTextColor("black");
            item6->setTextColor("black");
            item7->setTextColor("black");
            item0->setText(QString::number(locator.maxDistance_m, 'f', 1));
            item1->setText(QString::number(locator.currentAzimuth, 'f', 1));
            item2->setText(QString::number(locator.currentElevation, 'f', 1));
            item3->setText(QString::number(locator.maxDopler_mps, 'f', 1));
            item4->setText(QString::number(locator.maxAmplitude_dB, 'f', 1));
            item5->setText(QString::number(locator.maxNoise_shifted_dB, 'f', 1));
            item6->setText(QString::number(locator.maxSNR_shifted_dB, 'f', 1));
            item7->setText(QString::number(locator.maxReachableDistance_km, 'f', 1));
        }
        else
        {
            item0->setTextColor("red");
            item1->setTextColor("red");
            item2->setTextColor("red");
            item3->setTextColor("red");
            item4->setTextColor("red");
            item5->setTextColor("red");
            item6->setTextColor("red");
            item7->setTextColor("red");
        }
    });

    /////////////////////////////////////
    ///
    ///
    /// record
    ///
    ///
    /////////////////////////////////////

    connect(ui->doubleSpinBox_minSNR_1, QOverload<double>::of(&QDoubleSpinBox::valueChanged), [this]{
        ui->doubleSpinBox_minSNR->setValue(ui->doubleSpinBox_minSNR_1->value());
    });

    connect(ui->doubleSpinBox_minSNR, QOverload<double>::of(&QDoubleSpinBox::valueChanged), [this]{
        ui->doubleSpinBox_minSNR_1->setValue(ui->doubleSpinBox_minSNR->value());
    });

    connect(ui->doubleSpinBox_minSNR, QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, &MainWindow::recordFiltersChanged);
    connect(ui->spinBox_minDist, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::recordFiltersChanged);
    connect(ui->spinBox_maxDist, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::recordFiltersChanged);
    connect(ui->doubleSpinBox_minDopler, QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, &MainWindow::recordFiltersChanged);
    connect(ui->doubleSpinBox_maxDopler, QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, &MainWindow::recordFiltersChanged);
    connect(ui->pushButton_SDC,  QOverload<bool>::of(&QAbstractButton::clicked), this, [this]{
        if (ui->pushButton_SDC->isChecked())
            ui->pushButton_SDC->setStyleSheet("QPushButton{background:#46A367;}");
        else
            ui->pushButton_SDC->setStyleSheet("QPushButton{background:#F7F7F7;}");
        recordFiltersChanged();
    });

    connect(this, &MainWindow::scaleLengthReceived, &MainWindow::setLocatorVariables);
    connect(ui->doubleSpinBox_ECS_BPLA, QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, &MainWindow::setLocatorVariables);
    connect(ui->doubleSpinBox_NoiseLevelShift, QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, &MainWindow::setLocatorVariables);
    connect(ui->comboBox_Power_1, QOverload<int>::of(&QComboBox::currentIndexChanged), this, &MainWindow::setLocatorVariables);

    connect(ui->pushButton_WriteLog,QOverload<bool>::of(&QAbstractButton::clicked), this, [this]{
        if(ui->pushButton_WriteLog->isChecked())
        {
            ui->pushButton_WriteLog->setStyleSheet("QPushButton{background:#46A367;}");
            locator.startLog("Log.txt");
        }
        else
        {
            ui->pushButton_WriteLog->setStyleSheet("QPushButton{background:#F7F7F7;}");
            locator.stopLog();
        }
    });

    recordFiltersChanged();
    setLocatorVariables();

    connect(graphicsForm, SIGNAL(saveJsonSignal(int)), this, SLOT(jsonRecord(int)));

    /////////////////////////////////////
    ///
    ///
    /// rotator
    ///
    ///
    /////////////////////////////////////
    ui->label_RotationProgress->setPixmap(blackDot->scaled(18,18));
    ui->label_RotationProgress_1->setPixmap(blackDot->scaled(28,28));

    connect(&locator,&Locator::rotationStarted,this,[this]{
        qDebug() << "rotationStarted" << QTime::currentTime().toString("hh:mm:ss.zzz");
        ui->label_RotationProgress_1->setPixmap(blackDot->scaled(28,28));
        ui->label_RotationProgress->setPixmap(blackDot->scaled(18,18));
    });

    connect(&locator,&Locator::rotationFinished,this,[this]{
        qDebug() << "rotationFinished" << QTime::currentTime().toString("hh:mm:ss.zzz");
        ui->label_RotationProgress_1->setPixmap(greenDot->scaled(28,28));
        ui->label_RotationProgress->setPixmap(greenDot->scaled(18,18));
    });

    connect(&rli, &Rli::rotatorAnswerReceived, this, [this]
    {
        QString text = ">> ";
        if (rli.rotatorCommand.ModBusRW == 0x03)
        {
            text.append(QString::number(rli.rotatorCommand.ModBusBytesRead)
                        + " bytes read: register "
                        + QString::number(rli.rotatorCommand.RegAddr, 16)
                        + "*, value: ");
            switch (rli.rotatorCommand.DataType)
            {
            case 0:
                text.append(QString::number(rli.rotatorCommand.RegData_float));
                break;
            case 1:
                text.append(QString::number(rli.rotatorCommand.RegData_uint16));
                break;
            case 2:
                text.append(QString::number(rli.rotatorCommand.RegData_uint32));
                break;
            }
        }
        if (rli.rotatorCommand.ModBusRW == 0x10)
        {
            text.append(QString::number(rli.rotatorCommand.ModBusRegistersWritten)
                        + " reg. written from "
                        + QString::number(rli.rotatorCommand.ModbusRegAddr, 16));
        }
        WriteRotatorCommandLog(text);
    });

    connect(ui->pushButton_RotatorSet_1, &QAbstractButton::clicked, this, [this]{
        ui->pushButton_ApplyRliMode_1->clicked();
    });

    connect(ui->pushButton_RW, &QAbstractButton::clicked, this, [this]{
        if (ui->pushButton_RW->isChecked())
        {
            ui->pushButton_RW->setText("W");
            ui->doubleSpinBox_RotatorDataValue->setEnabled(true);
            rli.rotatorCommand.ReadReg = false;
        }
        else
        {
            ui->pushButton_RW->setText("R");
            ui->doubleSpinBox_RotatorDataValue->setEnabled(false);
            rli.rotatorCommand.ReadReg = true;
        }
    });

    ui->pushButton_RW->clicked(true);

    connect(ui->comboBox_RotatorDataType, QOverload<int>::of(&QComboBox::currentIndexChanged), this, [this]
    {
        switch (ui->comboBox_RotatorDataType->currentIndex())
        {
        case 0:
        case 3:
            ui->doubleSpinBox_RotatorDataValue->setDecimals(2);
            break;
        case 1:
        case 2:
            ui->doubleSpinBox_RotatorDataValue->setDecimals(0);
            break;
        }
    });

    connect(ui->pushButton_SendRotatorCommand, &QAbstractButton::clicked, this, [this]{
        rli.rotatorCommand.RegAddr = ui->spinBox_RotatorRegisterAddress->value();
        rli.rotatorCommand.ReadReg = !ui->pushButton_RW->isChecked();
        rli.rotatorCommand.DataType = ui->comboBox_RotatorDataType->currentIndex();
        switch (rli.rotatorCommand.DataType)
        {
        case 0:
            rli.rotatorCommand.RegData_float = ui->doubleSpinBox_RotatorDataValue->value();
            break;
        case 1:
            rli.rotatorCommand.RegData_uint16 = ui->doubleSpinBox_RotatorDataValue->value();
            break;
        case 2:
            rli.rotatorCommand.RegData_uint32 = ui->doubleSpinBox_RotatorDataValue->value();
            break;
        }
        rli.sendRotatorCommand();
        QString text = "<< addr " + QString::number(rli.rotatorCommand.RegAddr, 16) + "  size " + QString::number(rli.rotatorCommand.RegSize) + "  val ";
        switch (rli.rotatorCommand.DataType)
        {
        case 0:
            text.append(QString::number(rli.rotatorCommand.RegData_float));
            break;
        case 1:
            text.append(QString::number(rli.rotatorCommand.RegData_uint16));
            break;
        case 2:
            text.append(QString::number(rli.rotatorCommand.RegData_uint32));
            break;
        }
        WriteRotatorCommandLog(text);
    });

    connect(ui->pushButton_GetRotatorTelemetry, &QAbstractButton::clicked, this, [this]{
        rli.addTask(tasks_enum::GetRotatorTelemetry);
    });

    connect(&rli, &Rli::rotatorTelemetryReceived, this, [this]{
        WriteRotatorTelemetryLog(QString("Y %1\tP %2\tACKY %3\tACKP %4\tLAT %5\tLON %6\tALT %7\tMAZ %8")
                                 .arg(QString::number(rli.rotatorTelemetry.Position0, 'f', 2))
                                 .arg(QString::number(rli.rotatorTelemetry.Position1, 'f', 2))
                                 .arg(rli.rotatorTelemetry.SetPointAcknowledge0)
                                 .arg(rli.rotatorTelemetry.SetPointAcknowledge1)
                                 .arg(QString::number(rli.rotatorTelemetry.Latitude, 'f', 6))
                                 .arg(QString::number(rli.rotatorTelemetry.Longitude, 'f', 6))
                                 .arg(QString::number(rli.rotatorTelemetry.Altitude, 'f', 1))
                                 .arg(QString::number(rli.rotatorTelemetry.MagAzimuth, 'f', 2)));
        if (ui->pushButton_translateTelemetry->isChecked())
        {
            QByteArray data;
            QDataStream ds(&data, QIODevice::WriteOnly);
            ds.setByteOrder(QDataStream::LittleEndian);
            ds.setFloatingPointPrecision(QDataStream::SinglePrecision);

            ds << rli.rotatorTelemetry.Position0
               << rli.rotatorTelemetry.Position1
               << rli.rotatorTelemetry.Latitude
               << rli.rotatorTelemetry.Longitude
               << rli.rotatorTelemetry.Altitude
               << rli.rotatorTelemetry.MagAzimuth;

            int bytesWritten = data_retranslator->m_connection->write(data);
        }
    });

    connect(ui->pushButton_translateTelemetry, &QPushButton::clicked, this, [this]{
        if (ui->pushButton_translateTelemetry->isChecked())
        {
            data_retranslator = new tcpconnection(ui->lineEdit_translateTelemetry_IP->text(), ui->lineEdit_translateTelemetry_Port->text().toInt(), "retranslator");
            connect(data_retranslator, &tcpconnection::message, this, &MainWindow::WriteRotatorTelemetryLog);
            data_retranslator->ipconnect();
        }
        else
        {
            data_retranslator->ipdisconnect();
            disconnect(data_retranslator, &tcpconnection::message, this, &MainWindow::WriteRotatorTelemetryLog);
        }
    });

    /////////////////////////////////////
    ///
    ///
    /// amplifier
    ///
    ///
    /////////////////////////////////////

    connect(ui->pushButton_SendAmplifierCommand, &QAbstractButton::clicked, this, [this]{
        switch(ui->comboBox_AmplifierData->currentIndex()){
        case 0: rli.amplifierCommand.Command = 0xAA;
            break;
        case 1: rli.amplifierCommand.Command = 0xC0;
            break;
        case 2: rli.amplifierCommand.Command = 0xC3;
            break;
        case 3: rli.amplifierCommand.Command = 0xC6;
            break;
        case 4: rli.amplifierCommand.Command = 0xB1;
            break;
        case 5: rli.amplifierCommand.Command = 0x00;
            break;
        }
        WriteAmplifierCommandLog("<< " + QString::number(rli.amplifierCommand.Command,16));
        rli.sendAmplifierCommand();
    });

    connect(&rli, &Rli::amplifierAnswerReceived, this, [this]
    {
        WriteAmplifierCommandLog(">> addr " + QString::number(rli.amplifierCommand.Addr,16) +
                                 "  status " + QString::number(rli.amplifierCommand.Status,16) +
                                 "  data " + rli.amplifierCommand.Data.toHex());
    });

    connect(ui->pushButton_GetAmplifierTelemetry, &QAbstractButton::clicked, this, [this]{
        rli.addTask(tasks_enum::GetAmplifierTelemetry);
    });

    connect(&rli, &Rli::amplifierTelemetryReceived, this, [this]{
        WriteAmplifierTelemetryLog("addr " + QString::number(rli.amplifierTelemetry.Addr,16) +
                                   "\tstatus " + QString::number(rli.amplifierTelemetry.Status,16) +
                                   "\terrorCount " + QString::number(rli.amplifierTelemetry.ErrorCount,16) +
                                   "\terrors " + rli.amplifierTelemetry.Errors.toHex());
    });

    /////////////////////////////////////
    ///
    ///
    /// RLI control
    ///
    ///
    /////////////////////////////////////
    connect(ui->spinBox_ADCGain, QOverload<int>::of(&QSpinBox::valueChanged), [this]{
        if(!isGetAdcGain)
        {
            rli.ADCGain = ui->spinBox_ADCGain->value();
            rli.addTask(tasks_enum::SetAdcGain);
        }
    });

    connect(ui->spinBox_DACAtt, QOverload<int>::of(&QSpinBox::valueChanged), [this]{
        if(!isGetDacAtt)
        {
            rli.DACAtt = ui->spinBox_DACAtt->value();
            rli.addTask(tasks_enum::SetDacAtt);
        }
    });

    connect(ui->doubleSpinBox_ProcThreshold, QOverload<double>::of(&QDoubleSpinBox::valueChanged), [this]{
        if(!isGetMti)
        {
            rli.ProcThreshold = ui->doubleSpinBox_ProcThreshold->value();
            rli.addTask(tasks_enum::SetProcThreshold);
        }
    });

    connect(ui->doubleSpinBox_MTIVal, QOverload<double>::of(&QDoubleSpinBox::valueChanged), [this]{
        if(!isGetMti)
        {
            rli.MTIVal = ui->doubleSpinBox_MTIVal->value();
            rli.addTask(tasks_enum::SetMti);
        }
    });

    connect(ui->spinBox_DistOffset, QOverload<int>::of(&QSpinBox::valueChanged), [this]{
        if(!isGetDistOffset)
        {
            rli.DistOffset = ui->spinBox_DistOffset->value();
            rli.addTask(tasks_enum::SetDistOffset);
        }
    });

    connect(ui->comboBox_NumPeaks, QOverload<int>::of(&QComboBox::currentIndexChanged), [this]{
        if(!isGetNumPeaks)
        {
            rli.NumPeaks = ui->comboBox_NumPeaks->currentIndex();
            rli.addTask(tasks_enum::SetNumPeaks);
        }
    });

    connect(&rli, &Rli::DistOffsetUpdated, [this]{
        isGetDistOffset = true;
        ui->spinBox_DistOffset->setValue(rli.DistOffset);
        isGetDistOffset = false;
    });

    connect(&rli, &Rli::ProcThresholdUpdated, [this]{
        isGetProcThreshold = true;
        ui->doubleSpinBox_ProcThreshold->setValue(rli.ProcThreshold);
        isGetProcThreshold = false;
    });

    connect(&rli, &Rli::NumPeaksUpdated, [this]{
        isGetNumPeaks = true;
        ui->comboBox_NumPeaks->setCurrentIndex(rli.NumPeaks);
        isGetNumPeaks = false;
    });

    connect(&rli, &Rli::scaleTableUpdated, [this]{
        ui->comboBox_Scales->clear();
        ui->comboBox_Scales_1->clear();
        for (int i=0;i<rli.scales.size();++i)
        {
            ui->comboBox_Scales->addItem(QString::number(rli.scales[i].ScaleLen));
            ui->comboBox_Scales_1->addItem(QString::number(rli.scales[i].ScaleLen));
        }
        SetScalesToolTip(ui->comboBox_Scales->currentIndex());
    });

    connect(ui->comboBox_Scales, QOverload<int>::of(&QComboBox::currentIndexChanged), [this]{
        SetScalesToolTip(ui->comboBox_Scales->currentIndex());
    });

    connect(&rli, &Rli::statusUpdated, [this]{
        ui->labelFpgaTemperature->setText(QString("Температура FPGA: %1°").arg(rli.state.TempFPGA1));
        ui->labelFmcTemperature->setText(QString("Температура FMC: %1°").arg(rli.state.TempFMC));
    });

    connect(&rli, &Rli::modeCtrlUpdated, [this]{

        ui->comboBox_WorkMode->setCurrentIndex(rli.modeCtrl.WorkMode);
        ui->comboBox_Scales->setCurrentIndex(rli.modeCtrl.ScaleCode);
        ui->spinBox_ZondCount->setValue(rli.modeCtrl.ZondNum);
        ui->doubleSpinBox_Elevation->setValue(rli.modeCtrl.TargetPitch);
        ui->doubleSpinBox_Azimuth->setValue(rli.modeCtrl.TargetYaw);

        if (ui->comboBox_Power->count() > rli.modeCtrl.PwrLevel) ui->comboBox_Power->setCurrentIndex(rli.modeCtrl.PwrLevel);
        ui->spinBox_Frequency->setValue(rli.modeCtrl.TRxFreq);
        ui->spinBox_ExtEnableTX->setValue(rli.modeCtrl.ExtEnableTX);

        ui->comboBox_WorkMode_1->setCurrentIndex(rli.modeCtrl.WorkMode);
        ui->comboBox_Scales_1->setCurrentIndex(rli.modeCtrl.ScaleCode);
        if (ui->comboBox_Power_1->count() > rli.modeCtrl.PwrLevel) ui->comboBox_Power_1->setCurrentIndex(rli.modeCtrl.PwrLevel);
        ui->doubleSpinBox_Azimuth_1->setValue(rli.modeCtrl.TargetYaw);
        ui->doubleSpinBox_Elevation_1->setValue(rli.modeCtrl.TargetPitch);

        locator.setCurrentAngles(rli.modeCtrl.TargetYaw, rli.modeCtrl.TargetPitch);

        emit scaleLengthReceived();
    });

    connect(&rli, &Rli::ADCGainUpdated, [this]{
        isGetAdcGain = true;
        ui->spinBox_ADCGain->setValue(rli.ADCGain);
        isGetAdcGain = false;
    });

    connect(&rli, &Rli::multicastAddressUpdated, [this]{
        quint8 a1 = ((rli.GroupAddr>>24) & 0xFF);
        quint8 a2 = ((rli.GroupAddr>>16) & 0xFF);
        quint8 a3 = ((rli.GroupAddr>>8) & 0xFF);
        quint8 a4 = ((rli.GroupAddr>>0) & 0xFF);
        ui->labelMulticastAddress->setText(QString("IP адрес: %1.%2.%3.%4:%5").arg(a1).arg(a2).arg(a3).arg(a4).arg(rli.DstPort));
    });

    connect(&rli, &Rli::DACAttUpdated, [this]{
        isGetDacAtt = true;
        ui->spinBox_DACAtt->setValue(rli.DACAtt);
        isGetDacAtt = false;
    });

    connect(&rli, &Rli::ProcThresholdUpdated, [this]{
        ui->doubleSpinBox_ProcThreshold->setValue(rli.ProcThreshold);
    });

    connect(&rli, &Rli::MTIValUpdated, [this]{
        isGetMti = true;
        ui->doubleSpinBox_MTIVal->setValue(rli.MTIVal);
        isGetMti = false;
    });

    connect(&rli, &Rli::DistOffsetUpdated, [this]{
        ui->spinBox_DistOffset->setValue(rli.DistOffset);
    });

    connect(&rli, &Rli::adjustUpdated, [this]{
        isGetAdjust = true;
        ui->spinBox_DACDelay->setValue(rli.adjust.DACDelay);
        ui->spinBox_RefDelay->setValue(rli.adjust.RefDelay);
        ui->spinBox_TxDelay->setValue(rli.adjust.TxDelay);
        ui->spinBox_TxLen->setValue(rli.adjust.TxLen);
        ui->spinBox_RxDelay->setValue(rli.adjust.RxDelay);
        ui->spinBox_RxLen->setValue(rli.adjust.RxLen);
        isGetAdjust = false;
    });

    connect(&rli, &Rli::viewModeUpdated, [this]{
        isGetViewMode = true;
        ui->spinBox_OffsetView->setValue(rli.viewMode.OffsetView);
        ui->spinBox_BlankSize->setValue(rli.viewMode.BlankSize);
        ui->spinBox_RejectSize->setValue(rli.viewMode.RejectSize);
        ui->spinBox_ViewMode->setValue(rli.viewMode.Mode);
        isGetViewMode = false;

        ShowStatusMessage("Параметры считаны");
    });

    connect(ui->spinBox_DACDelay, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setAdjust);
    connect(ui->spinBox_RefDelay, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setAdjust);
    connect(ui->spinBox_TxDelay, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setAdjust);
    connect(ui->spinBox_TxLen, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setAdjust);
    connect(ui->spinBox_RxDelay, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setAdjust);
    connect(ui->spinBox_RxLen, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setAdjust);

    connect(&rli, &Rli::acknowledgeReceived, this, [this]{
    if (rli.acknowledge.Result > 0)
        ShowStatusMessage(QString("%1: код %2.%3").arg(QString::number(rli.acknowledge.ReplyMsgID, 16)).arg(rli.acknowledge.Result).arg(rli.acknowledge.ErrorInfo));
    });

    connect(&pingProcess, SIGNAL(finished(int, QProcess::ExitStatus)), this, SLOT(onPingEnded()));

    connect(ui->pushButton_ping, &QAbstractButton::clicked, this, [this]{pingProcess.start("ping 192.168.12.5");});

    connect(&rli, &Rli::deviceReboot, this, [this]{
        rli.close();
        ui->pushButton_ApplyRliMode->setEnabled(false);
        ui->pushButton_ApplyRliMode_1->setEnabled(false);
        ui->pushButton_Scan->setChecked(false);
        ui->pushButton_Scan->setEnabled(false);
        ui->pushButton_RestoreDefaultsRli->setEnabled(false);
        ui->pushButton_UpdateFromRli->setEnabled(false);
        ui->pushButton_RotatorSet_1->setEnabled(false);
        ui->pushButton_SDC->setEnabled(false);
        ui->pushButton_Reboot->setEnabled(false);
        ui->comboBox_RebootPage->setEnabled(false);
        ui->pushButton_OpenCloseRli->setText(QStringLiteral("Подключить"));
    });

    connect(ui->pushButton_OpenCloseRli, &QAbstractButton::clicked, this, [this]{
        if(rli.isOpen())
        {
            rli.close();
            ui->pushButton_OpenCloseRli->setText(QStringLiteral("Подключить"));
        }
        else
        {
            rli.open();
            ui->pushButton_OpenCloseRli->setText(QStringLiteral("Отключить"));

        }
        ui->pushButton_ApplyRliMode->setEnabled(rli.isOpen());
        ui->pushButton_ApplyRliMode_1->setEnabled(rli.isOpen());
        ui->pushButton_Scan->setEnabled(rli.isOpen());
        ui->pushButton_RestoreDefaultsRli->setEnabled(rli.isOpen());
        ui->pushButton_UpdateFromRli->setEnabled(rli.isOpen());
        ui->pushButton_RotatorSet_1->setEnabled(rli.isOpen());
        ui->pushButton_SDC->setEnabled(rli.isOpen());
        ui->pushButton_Reboot->setEnabled(rli.isOpen());
        ui->comboBox_RebootPage->setEnabled(rli.isOpen());
    });

    connect(ui->pushButton_ApplyRliMode, &QAbstractButton::clicked,this, [this]{
        if (ui->comboBox_WorkMode->currentIndex() != 0)
        {
            receiveGraphTime.restart();
            sendTimerResult = true;
        }
        rli.modeCtrl.WorkMode = ui->comboBox_WorkMode->currentIndex();
        if (ui->comboBox_Scales->count() > 0)
            rli.modeCtrl.ScaleCode = ui->comboBox_Scales->currentIndex();
        else
            rli.modeCtrl.ScaleCode = 0;
        rli.modeCtrl.ZondNum = ui->spinBox_ZondCount->value();
        rli.modeCtrl.PwrLevel = ui->comboBox_Power->currentIndex();
        rli.modeCtrl.TRxFreq = ui->spinBox_Frequency->value();
        rli.modeCtrl.ExtEnableTX = ui->spinBox_ExtEnableTX->value();

        rli.modeCtrl.TargetYaw = Locator::clamp(ui->doubleSpinBox_Azimuth->value());
        //rli.modeCtrl.TargetPitch = ui->doubleSpinBox_Elevation->value();
        if (ui->doubleSpinBox_Elevation->value() >= 45)
            rli.modeCtrl.TargetPitch = 45;
        else
            rli.modeCtrl.TargetPitch = ui->doubleSpinBox_Elevation->value();
        if (rli.modeCtrl.WorkMode > 0)
            locator.setTargetAngles(rli.modeCtrl.TargetYaw, rli.modeCtrl.TargetPitch);

        rli.addTask(tasks_enum::SetModeCtrl);
    });

    connect(ui->pushButton_ApplyRliMode_1, &QAbstractButton::clicked, this, [this]{
        rli.modeCtrl.WorkMode = ui->comboBox_WorkMode_1->currentIndex();
        if (ui->comboBox_Scales_1->count() > 0)
            rli.modeCtrl.ScaleCode = ui->comboBox_Scales_1->currentIndex();
        else
            rli.modeCtrl.ScaleCode = 0;
        rli.modeCtrl.ZondNum = ui->spinBox_ZondCount->value();
        rli.modeCtrl.PwrLevel = ui->comboBox_Power_1->currentIndex();
        rli.modeCtrl.TRxFreq = ui->spinBox_Frequency->value();
        rli.modeCtrl.ExtEnableTX = ui->spinBox_ExtEnableTX->value();

        rli.modeCtrl.TargetYaw = Locator::clamp(ui->doubleSpinBox_Azimuth_1->value());
        //rli.modeCtrl.TargetPitch = ui->doubleSpinBox_Elevation_1->value();
        if (ui->doubleSpinBox_Elevation_1->value() >= 45)
            rli.modeCtrl.TargetPitch = 45;
        else
            rli.modeCtrl.TargetPitch = ui->doubleSpinBox_Elevation_1->value();
        if (rli.modeCtrl.WorkMode > 0)
            locator.setTargetAngles(rli.modeCtrl.TargetYaw, rli.modeCtrl.TargetPitch);

        rli.addTask(tasks_enum::SetModeCtrl);
    });

    connect(ui->pushButton_Reboot, &QAbstractButton::clicked, this, [this]{
       rli.reboot_page = ui->comboBox_RebootPage->currentIndex();
       rli.addTask(tasks_enum::Reboot);
    });

    connect(ui->spinBox_OffsetView, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setViewMode);
    connect(ui->spinBox_BlankSize, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setViewMode);
    connect(ui->spinBox_RejectSize, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setViewMode);
    connect(ui->spinBox_ViewMode, QOverload<int>::of(&QSpinBox::valueChanged), this, &MainWindow::setViewMode);

    connect(ui->pushButton_UpdateFromRli, &QAbstractButton::clicked,this,[this]{
        rli.addTask(tasks_enum::GetScaleTable);
        rli.addTask(tasks_enum::GetMulticastAddress);
        rli.addTask(tasks_enum::GetDistOffset);
        rli.addTask(tasks_enum::GetProcThreshold);
        rli.addTask(tasks_enum::GetModeCtrl);
        rli.addTask(tasks_enum::GetMti);
        rli.addTask(tasks_enum::GetDacAtt);
        rli.addTask(tasks_enum::GetAdcGain);
        rli.addTask(tasks_enum::GetNumPeaks);
        rli.addTask(tasks_enum::GetStatus);
        rli.addTask(tasks_enum::GetAdjust);
        rli.addTask(tasks_enum::GetViewMode);
    });

    connect(ui->pushButton_RestoreDefaultsRli, &QAbstractButton::clicked, this, [this]{
        RestoreDefaultsRli();
        ui->pushButton_ApplyRliMode->clicked();
        recordFiltersChanged();
        setLocatorVariables();
    });

    connect(ui->pushButton_RestoreDefaultsRli_1, &QAbstractButton::clicked, this, [this]{
        if (!rli.isOpen())
            ui->pushButton_OpenCloseRli->clicked();

        if (ui->pushButton_RestoreDefaultsRli_1->isChecked())
        {
            ui->pushButton_RestoreDefaultsRli_1->setStyleSheet("QPushButton{background:#C4C4C4;}");
            ui->pushButton_UpdateFromRli->clicked();
            ShowStatusMessage("Чтение параметров");
        }
        else
        {
            ui->pushButton_RestoreDefaultsRli_1->setStyleSheet("QPushButton{background:#F7F7F7;}");
            RestoreDefaultsRli();
            ui->pushButton_ApplyRliMode_1->clicked();
            recordFiltersChanged();
            setLocatorVariables();
            ShowStatusMessage("Установка параметров");
        }
    });

    connect(&rli, &Rli::rliTimeoutEvent, this, [this]{
        ShowStatusMessage("Ожидание");
        qDebug() << "No answer from device";
    });

    ui->pushButton_OpenCloseRli->clicked();
    ui->pushButton_UpdateFromRli->clicked();

    connect(&scanner, &Scanner::nextScan, this, [this]{
        ui->doubleSpinBox_Azimuth->setValue(scanner.azimuth);
        ui->doubleSpinBox_Elevation->setValue(scanner.elevation);
        ui->pushButton_ApplyRliMode->click();
    });
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::closeEvent(QCloseEvent *event)
 {
    QApplication::closeAllWindows();
 }

void MainWindow::GetINISettings(QString filename)
{
    QSettings ini(filename, QSettings::IniFormat);

    ini.beginGroup("CHEATING");
    ui->doubleSpinBox_NoiseLevelShift->setValue(ini.value("NoiseLevelShift", 28).toDouble());
    int cheating_NoisePointsShift = ini.value("NoisePointsShift", 50).toInt();
    ini.endGroup();

    ini.beginGroup("LOCATOR");
    ui->doubleSpinBox_ECS_BPLA->setValue(ini.value("ECSd", 0.28).toDouble());
    locator.setCalculationCoefficients(cheating_NoisePointsShift,
                                       ini.value("Pmax", 100).toDouble(),
                                       ini.value("S", 20.4).toDouble(),
                                       ini.value("Tmax", 4).toDouble(),
                                       ini.value("ECS", 3).toDouble());
    ini.endGroup();

    ini.beginGroup("RLI");
    ui->spinBox_ADCGain->setValue(ini.value("ADCGain", 0).toInt());
    ui->spinBox_DACAtt->setValue(ini.value("DACAtt", 0).toInt());
    ui->doubleSpinBox_ProcThreshold->setValue(ini.value("ProcThreshold", 0).toDouble());
    ui->doubleSpinBox_MTIVal->setValue(ini.value("MTIVal", 0).toDouble());
    ui->spinBox_DistOffset->setValue(ini.value("DistOffset", 0).toInt());
    ui->comboBox_NumPeaks->setCurrentIndex(ini.value("NumPeaks", 0).toInt());

    ui->spinBox_DACDelay->setValue(ini.value("DACDelay", 0).toInt());
    ui->spinBox_RefDelay->setValue(ini.value("RefDelay", 0).toInt());
    ui->spinBox_TxDelay->setValue(ini.value("TxDelay", 0).toInt());
    ui->spinBox_TxLen->setValue(ini.value("TxLen", 0).toInt());
    ui->spinBox_RxDelay->setValue(ini.value("RxDelay", 0).toInt());
    ui->spinBox_RxLen->setValue(ini.value("RxLen", 0).toInt());

    ui->spinBox_OffsetView->setValue(ini.value("OffsetView", 12).toInt());
    ui->spinBox_BlankSize->setValue(ini.value("BlankSize", 0).toInt());
    ui->spinBox_RejectSize->setValue(ini.value("RejectSize", 570).toInt());
    ui->spinBox_ViewMode->setValue(ini.value("ViewMode", 1).toInt());

    ui->spinBox_Frequency->setValue(ini.value("Frequency", 3350000).toInt());
    ui->spinBox_ZondCount->setValue(ini.value("ZondCount", 3).toInt());
    ui->spinBox_ExtEnableTX->setValue(ini.value("ExtEnableTX", 4).toInt());
    ui->doubleSpinBox_Elevation->setValue(ini.value("Elevation", 0).toDouble());
    ui->doubleSpinBox_Azimuth->setValue(ini.value("Azimuth", 85).toDouble());
    ini.endGroup();

    ini.beginGroup("FILTERING");
    ui->doubleSpinBox_minSNR->setValue(ini.value("minSNR", 13.2).toDouble());
    ui->spinBox_minDist->setValue(ini.value("minDist", 1000).toInt());
    ui->spinBox_maxDist->setValue(ini.value("maxDist", 1300).toInt());
    ui->doubleSpinBox_minDopler->setValue(ini.value("minDopler", 2).toDouble());
    ui->doubleSpinBox_maxDopler->setValue(ini.value("maxDopler", 15).toDouble());
    ini.endGroup();

    ini.beginGroup("RETRANSLATOR");
    ui->lineEdit_translateTelemetry_IP->setText(ini.value("ip", "192.168.1.112").toString());
    ui->lineEdit_translateTelemetry_Port->setText(ini.value("port", "32179").toString());
    ini.endGroup();

    ini.beginGroup("SCANNER");
    scanner.timerPeriod = ini.value("scanTimer", "2500").toInt();
    scanner.minElevation = ini.value("minElevation", "0").toInt();
    scanner.spiralStep = ini.value("spiralStep", "1").toDouble();


}

void MainWindow::RestoreDefaultsRli()
{
    qDebug() << "Restore default settings";
    GetINISettings(iniFile);
}

void MainWindow::setAdjust()
{
    if(!isGetAdjust)
    {
        rli.adjust.DACDelay = ui->spinBox_DACDelay->value();
        rli.adjust.RefDelay = ui->spinBox_RefDelay->value();
        rli.adjust.TxDelay = ui->spinBox_TxDelay->value();
        rli.adjust.TxLen = ui->spinBox_TxLen->value();
        rli.adjust.RxDelay = ui->spinBox_RxDelay->value();
        rli.adjust.RxLen = ui->spinBox_RxLen->value();
        rli.addTask(tasks_enum::SetAdjust);
    }
}

void MainWindow::setViewMode()
{
    if(!isGetViewMode)
    {
        rli.viewMode.OffsetView = ui->spinBox_OffsetView->value();
        rli.viewMode.BlankSize = ui->spinBox_BlankSize->value();
        rli.viewMode.RejectSize = ui->spinBox_RejectSize->value();
        rli.viewMode.Mode = ui->spinBox_ViewMode->value();
        rli.addTask(tasks_enum::SetViewMode);
    }
}

void MainWindow::recordFiltersChanged()
{
    emit setRecordFilters(ui->spinBox_minDist->value(),
                          ui->spinBox_maxDist->value(),
                          ui->doubleSpinBox_minSNR->value() + locator.noiseLevelShift);
    locator.setTresholds(ui->spinBox_minDist->value(),
                         ui->spinBox_maxDist->value(),
                         ui->doubleSpinBox_minDopler->value(),
                         ui->pushButton_SDC->isChecked(),
                         ui->doubleSpinBox_maxDopler->value(),
                         ui->doubleSpinBox_minSNR->value());
}

void MainWindow::setLocatorVariables()
{
    float T_;
    if ((rli.scales.count()>0) && (rli.scales.count()>=rli.modeCtrl.ScaleCode))
        T_ = rli.scales[rli.modeCtrl.ScaleCode].ScaleLen;
    else
        T_ = 0.02;
    locator.setVariables(ui->doubleSpinBox_NoiseLevelShift->value(), ui->comboBox_Power_1->currentIndex(), T_, ui->doubleSpinBox_ECS_BPLA->value());
    emit setRecordFilters(ui->spinBox_minDist->value(),
                          ui->spinBox_maxDist->value(),
                          ui->doubleSpinBox_minSNR->value() + locator.noiseLevelShift);

    QVector<double> Amplitudes_dB;
    QVector<double> Noises_dB;
    QVector<double> Distances_m;
//    for (int i=0;i<2000;i++)
//    {
//        Amplitudes_dB.append(i*sin(i/12)/4 + ui->doubleSpinBox_NoiseLevelShift->value()) ;
//        Noises_dB.append(i*cos(i/24)/5 - ui->doubleSpinBox_NoiseLevelShift->value());
//        Distances_m.append(6*i);
//    }
//    emit plotData(Distances_m, Amplitudes_dB, Noises_dB,
//                  1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17);
}


void MainWindow::ShowStatusMessage(QString message)
{
    QToolTip::showText(this->mapToGlobal(QPoint(-3, 564)), message);
}

void MainWindow::SetScalesToolTip(int scaleNum)
{
    if (rli.scales.size() > scaleNum)
        ui->comboBox_Scales->setToolTip(QString("Макс. доплер для СДЦ %1 \n"
                                                "Макс. смещение нулевой дальности %2")
                                        .arg(QString::number(rli.scales[scaleNum].ScaleMaxMTI, 'f', 2))
                                        .arg(QString::number(rli.scales[scaleNum].ScaleMaxDistOffs, 'f', 2)));
}

void MainWindow::WriteRotatorCommandLog(QString text)
{
    ui->listWidget_rotatorLog->addItem(text);
    ui->listWidget_rotatorLog->scrollToBottom();
}

void MainWindow::WriteRotatorTelemetryLog(QString text)
{
    ui->listWidget_rotatorTelemetryLog->addItem(text);
    ui->listWidget_rotatorTelemetryLog->scrollToBottom();
}

void MainWindow::WriteAmplifierCommandLog(QString text)
{
    ui->listWidget_amplifierLog->addItem(text);
    ui->listWidget_amplifierLog->scrollToBottom();
}

void MainWindow::WriteAmplifierTelemetryLog(QString text)
{
    ui->listWidget_amplifierTelemetryLog->addItem(text);
    ui->listWidget_amplifierTelemetryLog->scrollToBottom();
}

void MainWindow::onPingEnded()
{
    QByteArray output = pingProcess.readAllStandardOutput();
    if (!output.isEmpty())
    {
        if (-1 != QString(output).indexOf("ttl", 0, Qt::CaseInsensitive))
        {
            qDebug() << "Ping received";
            ShowStatusMessage("Ответ получен");
        }
        else
        {
            qDebug() << "No ping";
            ShowStatusMessage("Устройство недоступно");
        }
    }
}

void MainWindow::savePointToJson()
{
    qDebug() << "Measure saved to JSON" << QTime::currentTime().toString("hh:mm:ss.zzz");
    jsonSaver.addDataArray(locator.Distances_m, locator.Amplitudes_dB, locator.Noises_dB, locator.Doplers_mps);
    if (jsonSaver.pointCounter==jsonSaver.maxPoints)
    {
        disconnect(&locator, SIGNAL(dataReceived()), this, SLOT(savePointToJson()));
        jsonSaver.saveFile();
        graphicsForm->enableSaveJsonButton();
    }
}

void MainWindow::jsonRecord(int count)
{
    jsonSaver.clear(count);
    jsonSaver.addParameter("TSSec", QString::number(locator.headerData.TSSec));
    jsonSaver.addParameter("TSNSec", QString::number(locator.headerData.TSNSec));
    jsonSaver.addParameter("DataType", QString::number(locator.headerData.DataType));
    jsonSaver.addParameter("ScaleCode", QString::number(locator.headerData.ScaleCode));
    jsonSaver.addParameter("MTIWidth", QString::number(locator.headerData.MTIWidth));
    jsonSaver.addParameter("MTIWidth", QString::number(locator.headerData.MTIWidth));
    jsonSaver.addParameter("WorkMode", QString::number(locator.headerData.WorkMode));
    jsonSaver.addParameter("DistResolution", QString::number(locator.headerData.DistResolution));
    jsonSaver.addParameter("DoplerResolution", QString::number(locator.headerData.DoplerResolution));
    jsonSaver.addParameter("ProcThreshold", QString::number(locator.headerData.ProcThreshold));
    jsonSaver.addParameter("DistOffset", QString::number(locator.headerData.DistOffset));
    jsonSaver.addParameter("TRxFreq", QString::number(locator.headerData.TRxFreq));
    jsonSaver.addParameter("MaxAmp", QString::number(locator.headerData.MaxAmp));
    jsonSaver.addParameter("YawAngle", QString::number(locator.headerData.YawAngle));
    jsonSaver.addParameter("PitchAngle", QString::number(locator.headerData.PitchAngle));
    jsonSaver.addParameter("YawAngle", QString::number(locator.headerData.YawAngle));
    jsonSaver.addParameter("PitchPosReady", QString::number(locator.headerData.PitchPosReady));
    jsonSaver.addParameter("YawPosReady", QString::number(locator.headerData.YawPosReady));
    jsonSaver.addParameter("MaxItems", QString::number(locator.headerData.MaxItems));

    jsonSaver.addParameter("Scale", ui->comboBox_Scales->currentText());
    jsonSaver.addParameter("ADCGain", QString::number(rli.ADCGain));
    jsonSaver.addParameter("DACAtt", QString::number(rli.DACAtt));
    jsonSaver.addParameter("ProcThreshold", QString::number(rli.ProcThreshold));
    jsonSaver.addParameter("MTIVal", QString::number(rli.MTIVal));
    jsonSaver.addParameter("DistOffset", QString::number(rli.DistOffset));
    jsonSaver.addParameter("NumPeaks", QString::number(rli.NumPeaks));
    jsonSaver.addParameter("DACDelay", QString::number(rli.adjust.DACDelay));
    jsonSaver.addParameter("RefDelay", QString::number(rli.adjust.RefDelay));
    jsonSaver.addParameter("TXDelay", QString::number(rli.adjust.TxDelay));
    jsonSaver.addParameter("TXLen", QString::number(rli.adjust.TxLen));
    jsonSaver.addParameter("RXDelay", QString::number(rli.adjust.RxDelay));
    jsonSaver.addParameter("RXLen", QString::number(rli.adjust.RxLen));
    jsonSaver.addParameter("OffsetView", QString::number(rli.viewMode.OffsetView));
    jsonSaver.addParameter("BlankSize", QString::number(rli.viewMode.BlankSize));
    jsonSaver.addParameter("RejectSize", QString::number(rli.viewMode.RejectSize));
    jsonSaver.addParameter("ViewMode", QString::number(rli.viewMode.Mode));
    jsonSaver.addParameter("Frequency", QString::number(rli.modeCtrl.TRxFreq));
    jsonSaver.addParameter("Longitude", QString::number(rli.rotatorTelemetry.Longitude));
    jsonSaver.addParameter("Latitude", QString::number(rli.rotatorTelemetry.Latitude));
    jsonSaver.addParameter("Altitude", QString::number(rli.rotatorTelemetry.Altitude));

    connect(&locator, SIGNAL(dataReceived()), this, SLOT(savePointToJson()));
}

void MainWindow::keyPressEvent(QKeyEvent *event){
    event->accept();
    if (event->key() == Qt::Key_F1)
            QWhatsThis::enterWhatsThisMode();
}

void MainWindow::on_pushButton_Scan_clicked(bool checked)
{
    if (checked)
        scanner.start(ui->doubleSpinBox_Azimuth->value(), ui->doubleSpinBox_Elevation->value());
    else
        scanner.stop();
}
