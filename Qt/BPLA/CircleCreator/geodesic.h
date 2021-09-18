#ifndef GEODESIC_H
#define GEODESIC_H

#define EarthRad 6372795			// радиус Земли в метрах
#define Degrees(x) (x * 180/M_PI)	// радианы -> градусы
#define Radians(x) (x * M_PI/180)	// градусы -> радианы
//#define Radians(x) (x / 57.29577951308232)	// градусы -> радианы

class Geodesic
{
public:
    Geodesic();

    static void SphereDirect(double origin_coordinates[], double direction, double distance, double target_coordinates[]);
    static void SphericToCart(double spheric[], double cart[]);
    static double CartToSpheric(double cart[], double spheric[]);
    static void Rotate(double x[], double angle, int axisNum);

    static float CalculateAzimuth(double lat1, double long1, float lat2, float long2);
    static float CalculateElevation(float distance2, int targetHeight, int ownHeight);
    static float CalculateDistance2(double lat1, double long1, float lat2, float long2);
    static float CalculateDistance3(float distance2, int targetHeight, int ownHeight);
    static float Clamp(float angle);
};

#endif // GEODESIC_H
