#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

    bool checkCheckSum(QByteArray receivedBuffer,  //filled or overfilled buffer
                               uint8_t message_len,
                               uint8_t postfix_len);
    static const uint16_t crc16table[];
    static const uint16_t INIT_VALUE = 0xFFFF;
    uint16_t get(const uint8_t *data, uint16_t len);
    void calc(uint8_t byte, uint16_t &crc);

private slots:
    void on_pushButton_clicked();

private:
    Ui::MainWindow *ui;
};
#endif // MAINWINDOW_H
