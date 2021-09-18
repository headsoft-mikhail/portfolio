#ifndef JSON_H
#define JSON_H

#include <QJsonObject>
#include <QJsonDocument>
#include <QJsonArray>
#include <QFile>
#include <QDateTime>
#include <QTextStream>

class json
{
public:
    json();
    QJsonObject recordObject;
    QJsonArray measureArray;
    int pointCounter = 0;
    int maxPoints = 20;

    void addParameter(QString parameterName, QString value);
    void addDataArray(QVector<double> distances, QVector<double> amplitudes, QVector<double> noises, QVector<double> doplers);
    void saveFile();
    void clear(int count);
};

#endif // JSON_H
