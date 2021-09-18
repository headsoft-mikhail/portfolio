/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 5.14.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QLabel>
#include <QtWidgets/QLineEdit>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QRadioButton>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralwidget;
    QPushButton *pushButton;
    QLineEdit *lineEdit_centerLat;
    QLineEdit *lineEdit_centerLon;
    QLabel *label;
    QLabel *label_2;
    QLabel *label_3;
    QLineEdit *lineEdit_radius;
    QLineEdit *lineEdit_stop;
    QLabel *label_4;
    QLineEdit *lineEdit_start;
    QLabel *label_5;
    QLineEdit *lineEdit_step;
    QLabel *label_6;
    QLineEdit *lineEdit_alt;
    QLabel *label_7;
    QRadioButton *radioButton_pic;
    QRadioButton *radioButton_cs;
    QLineEdit *lineEdit_t_alt;
    QLabel *label_8;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName(QString::fromUtf8("MainWindow"));
        MainWindow->resize(372, 293);
        centralwidget = new QWidget(MainWindow);
        centralwidget->setObjectName(QString::fromUtf8("centralwidget"));
        pushButton = new QPushButton(centralwidget);
        pushButton->setObjectName(QString::fromUtf8("pushButton"));
        pushButton->setGeometry(QRect(240, 220, 111, 51));
        lineEdit_centerLat = new QLineEdit(centralwidget);
        lineEdit_centerLat->setObjectName(QString::fromUtf8("lineEdit_centerLat"));
        lineEdit_centerLat->setGeometry(QRect(100, 20, 113, 21));
        lineEdit_centerLon = new QLineEdit(centralwidget);
        lineEdit_centerLon->setObjectName(QString::fromUtf8("lineEdit_centerLon"));
        lineEdit_centerLon->setGeometry(QRect(100, 50, 113, 21));
        label = new QLabel(centralwidget);
        label->setObjectName(QString::fromUtf8("label"));
        label->setGeometry(QRect(20, 20, 81, 16));
        label_2 = new QLabel(centralwidget);
        label_2->setObjectName(QString::fromUtf8("label_2"));
        label_2->setGeometry(QRect(20, 50, 81, 16));
        label_3 = new QLabel(centralwidget);
        label_3->setObjectName(QString::fromUtf8("label_3"));
        label_3->setGeometry(QRect(20, 250, 47, 16));
        lineEdit_radius = new QLineEdit(centralwidget);
        lineEdit_radius->setObjectName(QString::fromUtf8("lineEdit_radius"));
        lineEdit_radius->setGeometry(QRect(100, 250, 113, 21));
        lineEdit_stop = new QLineEdit(centralwidget);
        lineEdit_stop->setObjectName(QString::fromUtf8("lineEdit_stop"));
        lineEdit_stop->setGeometry(QRect(100, 220, 113, 21));
        label_4 = new QLabel(centralwidget);
        label_4->setObjectName(QString::fromUtf8("label_4"));
        label_4->setGeometry(QRect(20, 220, 51, 16));
        lineEdit_start = new QLineEdit(centralwidget);
        lineEdit_start->setObjectName(QString::fromUtf8("lineEdit_start"));
        lineEdit_start->setGeometry(QRect(100, 190, 113, 21));
        label_5 = new QLabel(centralwidget);
        label_5->setObjectName(QString::fromUtf8("label_5"));
        label_5->setGeometry(QRect(20, 190, 51, 20));
        lineEdit_step = new QLineEdit(centralwidget);
        lineEdit_step->setObjectName(QString::fromUtf8("lineEdit_step"));
        lineEdit_step->setGeometry(QRect(100, 160, 113, 21));
        label_6 = new QLabel(centralwidget);
        label_6->setObjectName(QString::fromUtf8("label_6"));
        label_6->setGeometry(QRect(20, 160, 51, 16));
        lineEdit_alt = new QLineEdit(centralwidget);
        lineEdit_alt->setObjectName(QString::fromUtf8("lineEdit_alt"));
        lineEdit_alt->setGeometry(QRect(100, 80, 113, 21));
        label_7 = new QLabel(centralwidget);
        label_7->setObjectName(QString::fromUtf8("label_7"));
        label_7->setGeometry(QRect(20, 80, 47, 16));
        radioButton_pic = new QRadioButton(centralwidget);
        radioButton_pic->setObjectName(QString::fromUtf8("radioButton_pic"));
        radioButton_pic->setGeometry(QRect(240, 20, 121, 19));
        radioButton_pic->setChecked(true);
        radioButton_cs = new QRadioButton(centralwidget);
        radioButton_cs->setObjectName(QString::fromUtf8("radioButton_cs"));
        radioButton_cs->setGeometry(QRect(240, 50, 111, 19));
        lineEdit_t_alt = new QLineEdit(centralwidget);
        lineEdit_t_alt->setObjectName(QString::fromUtf8("lineEdit_t_alt"));
        lineEdit_t_alt->setEnabled(false);
        lineEdit_t_alt->setGeometry(QRect(100, 110, 113, 21));
        label_8 = new QLabel(centralwidget);
        label_8->setObjectName(QString::fromUtf8("label_8"));
        label_8->setGeometry(QRect(20, 110, 71, 20));
        MainWindow->setCentralWidget(centralwidget);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QCoreApplication::translate("MainWindow", "CircleCreator", nullptr));
        pushButton->setText(QCoreApplication::translate("MainWindow", "Generate .CSV", nullptr));
        lineEdit_centerLat->setText(QCoreApplication::translate("MainWindow", "60.415431", nullptr));
        lineEdit_centerLon->setText(QCoreApplication::translate("MainWindow", "30.4677270 ", nullptr));
        label->setText(QCoreApplication::translate("MainWindow", "center latitude", nullptr));
        label_2->setText(QCoreApplication::translate("MainWindow", "center longitude", nullptr));
        label_3->setText(QCoreApplication::translate("MainWindow", "radius", nullptr));
        lineEdit_radius->setText(QCoreApplication::translate("MainWindow", "50", nullptr));
        lineEdit_stop->setText(QCoreApplication::translate("MainWindow", "180", nullptr));
        label_4->setText(QCoreApplication::translate("MainWindow", "stop angle", nullptr));
        lineEdit_start->setText(QCoreApplication::translate("MainWindow", "0", nullptr));
        label_5->setText(QCoreApplication::translate("MainWindow", "start angle", nullptr));
        lineEdit_step->setText(QCoreApplication::translate("MainWindow", "10", nullptr));
        label_6->setText(QCoreApplication::translate("MainWindow", "angle step", nullptr));
        lineEdit_alt->setText(QCoreApplication::translate("MainWindow", "50", nullptr));
        label_7->setText(QCoreApplication::translate("MainWindow", "altitude", nullptr));
        radioButton_pic->setText(QCoreApplication::translate("MainWindow", "PointInCoordinates", nullptr));
        radioButton_cs->setText(QCoreApplication::translate("MainWindow", "Course +  Servo", nullptr));
        lineEdit_t_alt->setText(QCoreApplication::translate("MainWindow", "180", nullptr));
        label_8->setText(QCoreApplication::translate("MainWindow", "target altitude", nullptr));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
