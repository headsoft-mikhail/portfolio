#ifndef WAYPOINT_H
#define WAYPOINT_H

#include <QObject>

class wayPoint : public QObject
{
    Q_OBJECT
public:
    wayPoint();
    wayPoint(const wayPoint& samplePoint);
    wayPoint(double latitude,
             double longitude,
             float altitude,
             bool pic_en,
             double pic_latitude,
             double pic_longitude,
             bool courseEn,
             float course,
             bool servo_en,
             float servo_angle,
             uint32_t holdTime);

    struct pointIncoordinates
    {
        bool en = false;
        double latitude;
        double longitude;
    };
    struct servo
    {
        bool en = false;
        float angle;
    };
    double latitude;
    double longitude;
    float altitude;
    bool courseEn = false;
    float course;
    bool speedEn = false;
    float speed;
    uint32_t holdTime;
    pointIncoordinates pointInCoordinates;
    servo servo;

    wayPoint operator=(const wayPoint &samplePoint);
};

#endif // WAYPOINT_H
