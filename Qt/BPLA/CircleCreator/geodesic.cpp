#include "geodesic.h"
#include <QtMath>

Geodesic::Geodesic()
{

}

/*
* Решение прямой геодезической задачи
*
* Аргументы исходные:
*     origin_coordinates   - {широта, долгота} точки Q1
*     direction            - азимут начального направления
*     distance             - расстояние (сферическое)
*
* Аргументы определяемые:
*     target_coordinates   - {широта, долгота} точки Q2
*/
void Geodesic::SphereDirect(double origin_coordinates[], double direction, double distance, double target_coordinates[])
{
    double pt[2], x[3];

    pt[0] = M_PI_2 - distance / EarthRad;
    pt[1] = M_PI - Radians(direction);
    SphericToCart(pt, x);			// сферические -> декартовы
    Rotate(x, Radians(origin_coordinates[0]) - M_PI_2, 1);	// первое вращение
    Rotate(x, -Radians(origin_coordinates[1]), 2);		// второе вращение
    CartToSpheric(x, target_coordinates);	     		// декартовы -> сферические

    target_coordinates[0] = Degrees(target_coordinates[0]);
    target_coordinates[1] = Degrees(target_coordinates[1]);
}

/*
* Преобразование сферических координат в вектор
*
* Аргументы исходные:
*     shperic - {широта, долгота}
*
* Аргументы определяемые:
*     cart - вектор {x, y, z}
*/
void Geodesic::SphericToCart(double shperic[], double cart[])
{
    double p;

    p = cos(shperic[0]);
    cart[2] = sin(shperic[0]);
    cart[1] = p * sin(shperic[1]);
    cart[0] = p * cos(shperic[1]);

    return;
}

/*
* Вращение вокруг координатной оси
*
* Аргументы:
*     x        - входной/выходной 3-вектор
*     angle    - угол вращения
*     axisNum  - номер координатной оси (0..2)
*/
void Geodesic::Rotate(double x[], double angle, int axisNum)
{
    double c, s, xj;
    int j, k;

    j = (axisNum + 1) % 3;
    k = (axisNum - 1) % 3;
    c = cos(angle);
    s = sin(angle);
    xj = x[j] * c + x[k] * s;
    x[k] = -x[j] * s + x[k] * c;
    x[j] = xj;

    return;
}

/*
* Преобразование вектора в сферические координаты
*
* Аргументы исходные:
*     cart - {x, y, z}
*
* Аргументы определяемые:
*     spheric - {широта, долгота}
*
* Возвращает:
*     длину вектора
*/
double Geodesic::CartToSpheric(double cart[], double spheric[])
{
    double p;

    p = hypot(cart[0], cart[1]);
    spheric[1] = atan2(cart[1], cart[0]);
    spheric[0] = atan2(cart[2], p);

    return hypot(p, spheric[2]);
}

float Geodesic::CalculateAzimuth(double lat1, double long1, float lat2, float long2)
{
    float azimuth = 0;

    //перевод координат в радианы
    lat1 *= M_PI / 180;
    lat2 *= (float)M_PI / 180;
    long1 *= M_PI / 180;
    long2 *= (float)M_PI / 180;

    //косинусы и синусы широт и разницы долгот
    double cl1 = cos(lat1);
    double cl2 = cos(lat2);
    double sl1 = sin(lat1);
    double sl2 = sin(lat2);
    double delta = long2 - long1;
    double cdelta = cos(delta);
    double sdelta = sin(delta);

    //вычисление начального азимута
    double x = (cl1 * sl2) - (sl1 * cl2 * cdelta);
    double y = sdelta * cl2;
    double z = 180 * atan(-y / x) / M_PI;

    if (x < 0)
    {
        z += 180;
    }
    int tmpcl = (z + 180)/360;
    double z2 = (z + 180 - tmpcl * 360) - 180;
    z2 = -z2 * M_PI / 180;
    double anglerad2 = z2 - ((2 * M_PI) * floor((z2 / (2 * M_PI))));
    azimuth = (float)((anglerad2 * 180) / M_PI);
    azimuth = Clamp(azimuth);
    return azimuth;
}

float Geodesic::CalculateElevation(float distance2, int targetHeight, int ownHeight)
{
    double height = targetHeight - ownHeight;
    return (float)(atan(height / distance2) * 180 / M_PI);
}

float Geodesic::CalculateDistance3(float distance2, int targetHeight, int ownHeight)
{
    float dist3d = (float)(sqrt(pow(distance2, 2) + pow(ownHeight - targetHeight, 2)));
    return dist3d; //расстояние между двумя координатами в метрах
}

float Geodesic::CalculateDistance2(double lat1, double long1, float lat2, float long2)
{

    //перевод координат в радианы
    lat1 *= M_PI / 180;
    lat2 *= (float)M_PI / 180;
    long1 *= M_PI / 180;
    long2 *= (float)M_PI / 180;
    //вычисление косинусов и синусов широт и разницы долгот
    double cl1 = cos(lat1);
    double cl2 = cos(lat2);
    double sl1 = sin(lat1);
    double sl2 = sin(lat2);
    double delta = long2 - long1;
    double cdelta = cos(delta);
    double sdelta = sin(delta);
    //вычисления длины большого круга
    double y = sqrt(pow(cl2 * sdelta, 2) + pow(cl1 * sl2 - sl1 * cl2 * cdelta, 2));
    double x = sl1 * sl2 + cl1 * cl2 * cdelta;
    double ad = atan2(y, x);
    float dist2d = (float)round(ad * EarthRad);
    return dist2d; //расстояние между двумя координатами в метрах
}

float Geodesic::Clamp(float angle)
{
    if (angle >= 360)
    {
        angle -= 360;
    }
    if (angle < 0)
    {
        angle += 360;
    }
    return angle;
}
