#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "uav.h"
#include "trackloader.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
	Q_OBJECT

public:
	MainWindow(QWidget *parent = nullptr);
	~MainWindow();
    uav* mk6;
    trackloader trackloader;
    tcpconnection* data_retranslator = new tcpconnection();
        
private slots:
    void on_clearLog_clicked();
    void on_expand_clicked(bool checked);
    void retranslatorAborted();

private:
    Ui::MainWindow *ui;
	void writeToLog(QString);

};
#endif // MAINWINDOW_H
