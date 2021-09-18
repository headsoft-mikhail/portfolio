#include "waypoint.h"

wayPoint::wayPoint()
{

}

wayPoint::wayPoint(const wayPoint &samplePoint)
{
    this->latitude = samplePoint.latitude;
    this->longitude = samplePoint.longitude;
    this->altitude = samplePoint.altitude;
    this->pointInCoordinates.en = samplePoint.pointInCoordinates.en;
    this->pointInCoordinates.latitude = samplePoint.pointInCoordinates.latitude;
    this->pointInCoordinates.longitude = samplePoint.pointInCoordinates.longitude;
    this->courseEn = samplePoint.courseEn;
    this->course = samplePoint.course;
    this->servo.en = samplePoint.servo.en;
    this->servo.angle = samplePoint.servo.angle;
    this->holdTime = samplePoint.holdTime;
}

wayPoint::wayPoint(double latitude,
                 double longitude,
                 float altitude,
                 bool pic_en,
                 double pic_latitude,
                 double pic_longitude,
                 bool courseEn,
                 float course,
                 bool servo_en,
                 float servo_angle,
                 uint32_t holdTime)
{
    this->latitude = latitude;
    this->longitude = longitude;
    this->altitude = altitude;
    this->pointInCoordinates.en = pic_en;
    this->pointInCoordinates.latitude = pic_latitude;
    this->pointInCoordinates.longitude = pic_longitude;
    this->courseEn = courseEn;
    this->course = course;
    this->servo.en = servo_en;
    this->servo.angle = servo_angle;
    this->holdTime = holdTime;
}


wayPoint wayPoint::operator=(const wayPoint &samplePoint)
{
    this->latitude = samplePoint.latitude;
    this->longitude = samplePoint.longitude;
    this->altitude = samplePoint.altitude;
    this->pointInCoordinates.en = samplePoint.pointInCoordinates.en;
    this->pointInCoordinates.latitude = samplePoint.pointInCoordinates.latitude;
    this->pointInCoordinates.longitude = samplePoint.pointInCoordinates.longitude;
    this->courseEn = samplePoint.courseEn;
    this->course = samplePoint.course;
    this->servo.en = samplePoint.servo.en;
    this->servo.angle = samplePoint.servo.angle;
    this->holdTime = samplePoint.holdTime;
    return *this;
}

