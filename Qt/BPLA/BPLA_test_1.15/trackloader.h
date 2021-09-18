#ifndef TRACKLOADER_H
#define TRACKLOADER_H

#include <QObject>
#include <QFile>
#include <QDebug>
#include <QTimer>
#include "waypoint.h"

class trackloader : public QObject
{
    Q_OBJECT
public:
    trackloader();
    QVector<wayPoint> track;
    void load(QString filename);
    void start(bool en);
    QTimer* pointsSendTimer = nullptr;
    int pointsLeft();
    wayPoint next();
    bool finish_declared = false;
public slots:
    void deleteAcceptedPoint();
signals:
    void message(QString message);
};

#endif // TRACKLOADER_H
