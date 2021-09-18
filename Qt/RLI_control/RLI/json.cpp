#include "json.h"

json::json()
{
    recordObject = QJsonObject();
}

void json::clear(int count)
{
    pointCounter = 0;
    maxPoints = count;
    recordObject = QJsonObject();
}

void json::addParameter(QString parameterName, QString value)
{
    recordObject.insert(parameterName, QJsonValue::fromVariant(value));
}

void json::addDataArray(QVector<double> distances, QVector<double> amplitudes, QVector<double> noises, QVector<double> doplers)
{
    QJsonArray distances_ja, amplitudes_ja, noises_ja, doplers_ja;
    for (int i=0; i<distances.count(); i++)
    {
        distances_ja.append(QString::number(distances[i]));
        amplitudes_ja.append(QString::number(amplitudes[i]));
        noises_ja.append(QString::number(noises[i]));
        doplers_ja.append(QString::number(doplers[i]));
    }
    auto singleMeasure = QJsonObject({
                                         qMakePair(QString("distance"), distances_ja),
                                         qMakePair(QString("amplitude"), amplitudes_ja),
                                         qMakePair(QString("noise"), noises_ja),
                                         qMakePair(QString("dopler"), doplers_ja)
                                    });
    measureArray.append(singleMeasure);
    pointCounter++;
}

void json::saveFile()
{
    recordObject.insert("MeasuresCount", QString::number(measureArray.count()));
    recordObject.insert("Data", measureArray);
    QJsonDocument doc(recordObject);
    QString jsonString = doc.toJson(QJsonDocument::Indented);
    QFile file;
    file.setFileName("logs\\json\\" + QDateTime::currentDateTime().toString("yyyy.MM.dd_hh-mm-ss") + "_data.json");
    file.open(QIODevice::WriteOnly | QIODevice::Text);
    QTextStream stream( &file );
    stream << jsonString;
    file.close();
    while (!measureArray.isEmpty())
        measureArray.pop_back();
}
