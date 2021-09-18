#include "graphics.h"
#include "ui_graphics.h"
#include <QStyleFactory>
#include <Qt>
#include <QTableWidgetItem>

Graphics::Graphics(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Graphics)
{
    ui->setupUi(this);
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
    setWindowIcon(QIcon("images/graph.png"));
    setWindowTitle("Просмотр графика");
    this->setGeometry(820,30,535,600);
    ui->customPlot->setOpenGl(true);
    ui->customPlot->yAxis->setRange(0, 500, Qt::AlignCenter);
    ui->customPlot->xAxis->setRange(-100, 6500, Qt::AlignLeft);
    ui->customPlot->addGraph();
    ui->customPlot->addGraph();
    ui->customPlot->addGraph();
    ui->customPlot->addGraph();

    QPen red;
    red.setColor("red");
    ui->customPlot->graph(1)->setPen(red);
    ui->customPlot->graph(2)->setPen(red);
    ui->customPlot->graph(3)->setPen(red);

    ui->customPlot->setInteractions(QCP::iRangeDrag | QCP::iRangeZoom);

    QStringList table_labels;
    table_labels << tr("TSSec") << tr("TSNSec") << tr("DataType") << tr("ScaleCode")
                 << tr("MTIWidth")  << tr("WorkMode") << tr("DistResolution")
                 << tr("DoplerResolution") << tr("ProcThreshold")  << tr("DistOffset")
                 << tr("TRxFreq") << tr("MaxAmp") << tr("YawAngle") << tr("PitchAngle")
                 << tr("YawPosReady") << tr("PitchPosReady") << tr("MaxItems");
    ui->dataHeader_tableWidget->setVerticalHeaderLabels(table_labels);

    connect(ui->pushButton_Autoscale, &QAbstractButton::clicked, this,[this]{
        QVector<double> minDistX(2, minDist);
        QVector<double> maxDistX(2, maxDist);
        QVector<double> minSNRy(2, minSNR);

        ui->customPlot->graph(1)->setData(minDistX, QVector<double>({ui->customPlot->yAxis->range().lower - 100 ,ui->customPlot->yAxis->range().upper + 100}));
        ui->customPlot->graph(2)->setData(maxDistX, QVector<double>({ui->customPlot->yAxis->range().lower - 100 ,ui->customPlot->yAxis->range().upper + 100}));

        if (ui->radioButtonSnr->isChecked())
            ui->customPlot->graph(3)->setData(QVector<double>({ui->customPlot->xAxis->range().lower - 100 ,ui->customPlot->xAxis->range().upper + 100}), minSNRy);

        ui->customPlot->rescaleAxes();
        ui->customPlot->replot();
    });

    connect(ui->showHeader_checkBox, QOverload<bool>::of(&QPushButton::clicked), this, [this](bool _arg){ui->dataHeader_tableWidget->setVisible(_arg);});

    ui->showHeader_checkBox->click();

    connect(ui->radioButtonAmplitude, &QRadioButton::toggled, this, [this]{
        if (ui->radioButtonAmplitude->isChecked())
        {
            ui->customPlot->xAxis->setRange(minDist_bound, maxDist_bound);
            ui->customPlot->yAxis->setRange(minAmp_bound, maxAmp_bound);
            ui->customPlot->replot();
        }
        else
        {
            if (ui->pushButton_SaveScale->isChecked())
            {
                minDist_bound = ui->customPlot->xAxis->range().lower;
                maxDist_bound = ui->customPlot->xAxis->range().upper;
                minAmp_bound = ui->customPlot->yAxis->range().lower;
                maxAmp_bound = ui->customPlot->yAxis->range().upper;
            }
        }
    });

    connect(ui->radioButtonNoise, &QRadioButton::toggled, this, [this]{
        if (ui->radioButtonNoise->isChecked())
        {
            ui->customPlot->xAxis->setRange(minDist_bound, maxDist_bound);
            ui->customPlot->yAxis->setRange(minNoise_bound, maxNoise_bound);
            ui->customPlot->replot();
        }
        else
        {
            if (ui->pushButton_SaveScale->isChecked())
            {
                minDist_bound = ui->customPlot->xAxis->range().lower;
                maxDist_bound = ui->customPlot->xAxis->range().upper;
                minNoise_bound = ui->customPlot->yAxis->range().lower;
                maxNoise_bound = ui->customPlot->yAxis->range().upper;
            }
        }
    });

    connect(ui->radioButtonSnr, &QRadioButton::toggled, this, [this]{
        if (ui->radioButtonSnr->isChecked())
        {
            ui->customPlot->xAxis->setRange(minDist_bound, maxDist_bound);
            ui->customPlot->yAxis->setRange(minSNR_bound, maxSNR_bound);
            ui->customPlot->replot();
        }
        else
        {
            if (ui->pushButton_SaveScale->isChecked())
            {
                minDist_bound = ui->customPlot->xAxis->range().lower;
                maxDist_bound = ui->customPlot->xAxis->range().upper;
                minSNR_bound = ui->customPlot->yAxis->range().lower;
                maxSNR_bound = ui->customPlot->yAxis->range().upper;
            }
        }
    });

    connect(ui->pushButton_SaveJson, &QAbstractButton::clicked, this,[this]{
        ui->pushButton_SaveJson->setEnabled(false);
        emit saveJsonSignal(ui->spinBox_SaveToJsonCount->value());
    });
}

Graphics::~Graphics()
{
    delete ui;
}

void Graphics::plot(QVector<double> distances,
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
                    unsigned short MaxItems)
{
    QVector<double> minDistX(2,minDist);
    QVector<double> maxDistX(2,maxDist);

    ui->customPlot->graph(1)->setData(minDistX, QVector<double>({ui->customPlot->yAxis->range().lower - 100 ,ui->customPlot->yAxis->range().upper + 100}));
    ui->customPlot->graph(2)->setData(maxDistX, QVector<double>({ui->customPlot->yAxis->range().lower - 100 ,ui->customPlot->yAxis->range().upper + 100}));

    if(ui->radioButtonAmplitude->isChecked())
    {
        ui->customPlot->graph(0)->setData(distances, amplitudes);
        ui->customPlot->graph(3)->setData(QVector<double>(),QVector<double>());
//        qDebug() << "A" << ui->radioButtonAmplitude->isChecked() << amplitudes.count() << *std::max_element(amplitudes.begin(), amplitudes.end()) << *std::min_element(amplitudes.begin(), amplitudes.end());
    }
    else if (ui->radioButtonNoise->isChecked())
    {
        ui->customPlot->graph(0)->setData(distances, noises);
        std::sort(noises.begin(), noises.end());
        ui->customPlot->graph(3)->setData(QVector<double>({distances[0]-100, distances[distances.count()-1] + 100}), QVector<double> (2, noises[(int)(noises.count()/2)]));
//        qDebug() << "N" << ui->radioButtonNoise->isChecked() << noises.count() << *std::max_element(noises.begin(), noises.end()) << *std::min_element(noises.begin(), noises.end());
    }
    else // ui->radioButtonSnr->isChecked())
    {
        QVector<double> snr(distances.size());
        for(int i=0;i<snr.size();++i)
        {
            snr[i] = amplitudes[i] - noises[i];
        }
        ui->customPlot->graph(0)->setData(distances, snr);
        ui->customPlot->graph(3)->setData(QVector<double>({distances[0]-100, distances[distances.count()-1] + 100}),  QVector<double> (2, minSNR));
//        qDebug() << "S" << ui->radioButtonSnr->isChecked() << snr.count() << *std::max_element(snr.begin(), snr.end()) << *std::min_element(snr.begin(), snr.end());
    }
    ui->customPlot->replot();





    ui->dataHeader_tableWidget->setItem(-1,1,new QTableWidgetItem(QString::number(TSSec)));
    ui->dataHeader_tableWidget->setItem(0,1,new QTableWidgetItem(QString::number(TSNSec)));
    ui->dataHeader_tableWidget->setItem(1,1,new QTableWidgetItem(QString::number(DataType)));
    ui->dataHeader_tableWidget->setItem(2,1,new QTableWidgetItem(QString::number(ScaleCode)));
    ui->dataHeader_tableWidget->setItem(3,1,new QTableWidgetItem(QString::number(MTIWidth)));
    ui->dataHeader_tableWidget->setItem(4,1,new QTableWidgetItem(QString::number(WorkMode)));
    ui->dataHeader_tableWidget->setItem(5,1,new QTableWidgetItem(QString::number(DistResolution, 'f', 1)));
    ui->dataHeader_tableWidget->setItem(6,1,new QTableWidgetItem(QString::number(DoplerResolution, 'f', 1)));
    ui->dataHeader_tableWidget->setItem(7,1,new QTableWidgetItem(QString::number(ProcThreshold, 'f', 1)));
    ui->dataHeader_tableWidget->setItem(8,1,new QTableWidgetItem(QString::number(DistOffset)));
    ui->dataHeader_tableWidget->setItem(9,1,new QTableWidgetItem(QString::number(TRxFreq)));
    ui->dataHeader_tableWidget->setItem(10,1,new QTableWidgetItem(QString::number(MaxAmp, 'f', 1)));
    ui->dataHeader_tableWidget->setItem(11,1,new QTableWidgetItem(QString::number(YawAngle, 'f', 1)));
    ui->dataHeader_tableWidget->setItem(12,1,new QTableWidgetItem(QString::number(PitchAngle, 'f', 1)));
    ui->dataHeader_tableWidget->setItem(13,1,new QTableWidgetItem(QString::number(YawPosReady)));
    ui->dataHeader_tableWidget->setItem(14,1,new QTableWidgetItem(QString::number(PitchPosReady)));
    ui->dataHeader_tableWidget->setItem(15,1,new QTableWidgetItem(QString::number(MaxItems)));
}


void Graphics::receiveRecordFilters(double minDist_, double maxDist_, double minSNR_)
{
    minDist = minDist_;
    maxDist = maxDist_;
    minSNR = minSNR_;
}

void Graphics::enableSaveJsonButton()
{
    ui->pushButton_SaveJson->setEnabled(true);
}

void Graphics::receiveTimerElapsed(int time)
{
    ui->label_DataDelay->setText(QString::number(time));
}

void Graphics::on_pushButton_Debug_clicked()
{
    emit getDebug();
}
