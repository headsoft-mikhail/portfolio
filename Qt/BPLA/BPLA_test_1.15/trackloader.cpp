#include "trackloader.h"

trackloader::trackloader()
{
    pointsSendTimer = new QTimer(this);
}

void trackloader::load(QString filename)
{
        QFile file(filename);
        if ( !file.open(QFile::ReadOnly | QFile::Text) )
        {
            qDebug() << "File not exist";
            emit message("File not exists");
            return;
        }
        else
        {
            // Создаём поток для извлечения данных из файла
            QTextStream in(&file);
            // Создаем список для сохранения данных
            QVector<QStringList> loadedData;
            // Считываем данные до конца файла
            while (!in.atEnd())
            {
                QStringList lineList = in.readLine().replace(", ", ";").split(";");
                qDebug() << lineList;
                loadedData.append(lineList);
            }
            file.close();
            loadedData.removeAt(0);

            track.clear();
            QStringList strLine;
            QString strValue;
            wayPoint trackPoint;
            foreach (strLine, loadedData)
            {
                if (strLine.count() == 11)
                {
                    trackPoint.latitude = strLine[0].toDouble();
                    trackPoint.longitude = strLine[1].toDouble();
                    trackPoint.altitude = strLine[2].toFloat();
                    trackPoint.pointInCoordinates.en = (strLine[3] == "1");
                    trackPoint.pointInCoordinates.latitude = strLine[4].toDouble();
                    trackPoint.pointInCoordinates.longitude = strLine[5].toDouble();
                    trackPoint.courseEn = (strLine[6] == "1");
                    trackPoint.course = strLine[7].toFloat();
                    trackPoint.servo.en = (strLine[8] == "1");
                    trackPoint.servo.angle = strLine[9].toFloat();
                    trackPoint.holdTime = strLine[10].toUInt();
                    track.append(trackPoint);
                }
                else
                {
                    emit message(("Track point import error"));
                    return;
                }
            }
            emit message((QString::number(track.count()) + " track points loaded"));
            return;
        }
}

void trackloader::deleteAcceptedPoint()
{
    track.removeFirst();
}

int trackloader::pointsLeft()
{
    if ((track.count()==0) && (!finish_declared))
    {
        finish_declared = true;
        emit message("<<<  All track points sent");
        qDebug() << "all track points sent";
    }
    return track.count();
}

void trackloader::start(bool en)
{
    if (en)
    {
        finish_declared = false;
        pointsSendTimer->start(500);
        emit message("<<<  Track sending started");
        qDebug() << "track sending started";
    }
    else
    {
        pointsSendTimer->stop();
        emit message("<<<  Track sending finished");
        qDebug() << "track sending finished";
    }
}

wayPoint trackloader::next()
{
    wayPoint nextPoint = wayPoint(track.first());
    return nextPoint;
}
