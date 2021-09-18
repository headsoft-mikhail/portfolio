using System;

namespace UDP_FPGA
{
    class Target
    {
        #region DEFINE
        //public event EventHandler targetFollowingStopped;

        static int predictionInterval = 500;
        public System.Timers.Timer followingTargetTimer;
        private int timeSinceUpdating = 0;

        public byte DeviceID { get; set; }
        public UInt32 ID { get; set; }
        public bool EndFollowTarget { get; set; }
        public byte ErrorType { get; set; } // 0 - ошибок нет;  1 - ошибка ЭВМ; 2 - ошибка ПО; 3 - неверные параметры цели
        public string ErrorMessage { get; set; }

        public GPScoordinates GPS { get; set; }

        public float SpeedNorth { get; set; }
        public float SpeedEast { get; set; }
        public float SpeedAlt { get; set; }

        public float Distance { get; set; }
        public float Azimuth { get; set; }
        public float Elevation { get; set; }

        public float AzimuthNext { get; set; }
        public float ElevationNext { get; set; }

        public Serp.RFchannel CH1 { get; set; }
        public Serp.RFchannel CH2 { get; set; }
        public Serp.RFchannel CH3 { get; set; }
        public Serp.RFchannel CH4 { get; set; }
        public Serp.RFchannel CH5 { get; set; }

        #endregion DEFINE

        #region CONSTRUCTORS

        public Target()
        {
            DeviceID = 0;
            ID = 0;
            EndFollowTarget = true;
            ErrorType = 0;
            ErrorMessage = "";

            GPS = new GPScoordinates(0, 0, 0);
            SpeedNorth = 0;
            SpeedEast = 0;
            SpeedAlt = 0;

            followingTargetTimer = new System.Timers.Timer(predictionInterval);
            followingTargetTimer.Elapsed += FollowingTargetTimer_Elapsed;

        }

        #endregion CONSTRUCTORS

        #region REFRESH_TARGETS

        public bool ReceivedCorrectly(Locator locator)
        {
            if (!TargetErrorCheck(locator))
            {
                Serp.serps[DeviceID].target.Replace(this);
                if (!EndFollowTarget)
                {
                    Serp.serps[DeviceID].target.CoordinatesPrediction(predictionInterval);
                    Serp.serps[DeviceID].target.CalculateTargetParameters(Serp.serps[DeviceID].GPS);

                    if (  (((Serp.serps[DeviceID].target.AzimuthNext >= 270) && (Serp.serps[DeviceID].target.AzimuthNext <=360)) || ((Serp.serps[DeviceID].target.AzimuthNext >= 0) && (Serp.serps[DeviceID].target.AzimuthNext <= 90))) && (Serp.serps[DeviceID].target.ElevationNext >=0)&&(Serp.serps[DeviceID].target.ElevationNext <=40))
                    {
                        Serp.serps[DeviceID].target.followingTargetTimer.Start();
                        System.Console.WriteLine("timer Stopped R serp " + DeviceID.ToString());
                        Serp.serps[DeviceID].SetCorner(AzimuthNext, ElevationNext, false, false);
                    }
                    else
                    {
                        Serp.serps[DeviceID].target.ErrorType = 3;
                        Serp.serps[DeviceID].target.ErrorMessage = "Unreachable rotator angles";
                        locator.TargetingError(Serp.serps[DeviceID].target);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Replace(Target newTarget)
        {
            followingTargetTimer.Stop();
            System.Console.WriteLine("timer Stopped R serp " + DeviceID.ToString());
            timeSinceUpdating = 0;
            DeviceID = newTarget.DeviceID;
            ID = newTarget.ID;
            EndFollowTarget = newTarget.EndFollowTarget;
            ErrorType = newTarget.ErrorType;
            ErrorMessage = newTarget.ErrorMessage;
            GPS = new GPScoordinates(newTarget.GPS.Latitude, newTarget.GPS.Longitude, newTarget.GPS.Altitude);
            SpeedNorth = 0;
            SpeedEast = 0;
            SpeedAlt = 0;

            Distance = 0;
            Azimuth = 0;
            Elevation = 0;
        }

        private bool TargetErrorCheck(Locator locator)
        {
            if (ErrorType == 0)
            {
                if ((GPS.Latitude > 90) || (GPS.Latitude < -90) || (GPS.Longitude > 180) || (GPS.Longitude < -180))
                {
                    ErrorType = 3;
                    ErrorMessage = "Incorrect target coordinates.";
                }

                if ((SpeedAlt > 2000) || (SpeedAlt < -2000) || (SpeedNorth > 2000) || (SpeedNorth < -2000) || (SpeedEast > 2000) || (SpeedEast < -2000))
                {
                    ErrorType = 3;
                    ErrorMessage = "Incorrect target speed.";
                }

                if ((DeviceID >= FormUDP.deviceCount) || (DeviceID < 0))
                {
                    ErrorType = 3;
                    ErrorMessage = "Incorrect device ID";
                }
            }

            if (ErrorType == 0)
            {
                locator.ConfirmTargeting(this);
                return false;
            }
            else
            {
                locator.TargetingError(this);
                return true;
            }
        }

        #endregion REFRESH_TARGETS

        #region PREDICTION

        private void FollowingTargetTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //try
            //{
            System.Console.WriteLine("timerElapsed E serp " + DeviceID.ToString());
            timeSinceUpdating += predictionInterval;
            if (timeSinceUpdating <= 5000)
            {
                CoordinatesPrediction(predictionInterval);
                CalculateTargetParameters(Serp.serps[DeviceID].GPS);
                double deltaAzNext = Math.Abs(Clamp(Serp.serps[DeviceID].AzimuthGeo - Serp.serps[DeviceID].target.AzimuthNext));
                double deltaElNext = Math.Abs(Serp.serps[DeviceID].ElevationGeo - Serp.serps[DeviceID].target.ElevationNext);
                if ((deltaAzNext > 2) || (deltaElNext > 2))
                {
                    Serp.serps[DeviceID].SetCorner(AzimuthNext, ElevationNext, false, false);
                }
            }
            else
            {
                timeSinceUpdating = 0;
                followingTargetTimer.Stop();
                System.Console.WriteLine("timer Stopped E serp " + DeviceID.ToString());
                //if (targetFollowingStopped != null) targetFollowingStopped.Invoke(this, null);
            }
            //}
            //catch
            //{
            //}

        }

        public void CoordinatesPrediction(float time)
        {
            double R = 6372795;
            double L = 2 * Math.PI * R;
            double speedNorthDeg = SpeedNorth * 360.0 / L;
            double speedEastDeg = SpeedEast * 360.0 / (L * Math.Cos(GPS.Latitude * Math.PI / 180));
            GPS.Latitude += (float)speedNorthDeg * time / 1000;
            GPS.Longitude += (float)speedEastDeg * time / 1000;
            GPS.Altitude += (int)(SpeedAlt * time / 1000);
        }

        #endregion PREDICTION

        #region CALCULATIONS

        private void CalculateTargetParameters(GPScoordinates serpGPS)
        {
            Azimuth = AzimuthNext;
            Elevation = ElevationNext;
            float dist2 = CalculateDistance2(serpGPS.Latitude, serpGPS.Longitude, GPS.Latitude, GPS.Longitude);
            Distance = CalculateDistance3(dist2, GPS.Altitude, serpGPS.Altitude);
            AzimuthNext = CalculateAzimuth(serpGPS.Latitude, serpGPS.Longitude, GPS.Latitude, GPS.Longitude);
            ElevationNext = CalculateElevation(dist2, GPS.Altitude, serpGPS.Altitude);
        }

        private float CalculateAzimuth(double lat1, double long1, float lat2, float long2)
        {
            float azimuth = 0;

            //pi - число pi, rad - радиус сферы (Земли)
            //double rad = 6372795;

            //перевод координат в радианы
            lat1 *= Math.PI / 180;
            lat2 *= (float)Math.PI / 180;
            long1 *= Math.PI / 180;
            long2 *= (float)Math.PI / 180;

            //косинусы и синусы широт и разницы долгот
            double cl1 = Math.Cos(lat1);
            double cl2 = Math.Cos(lat2);
            double sl1 = Math.Sin(lat1);
            double sl2 = Math.Sin(lat2);
            double delta = long2 - long1;
            double cdelta = Math.Cos(delta);
            double sdelta = Math.Sin(delta);

            //вычисление начального азимута
            double x = (cl1 * sl2) - (sl1 * cl2 * cdelta);
            double y = sdelta * cl2;
            double z = 180 * Math.Atan(-y / x) / Math.PI;

            if (x < 0)
            {
                z += 180;
            }

            double z2 = (z + 180) % 360 - 180;
            z2 = -z2 * Math.PI / 180;
            double anglerad2 = z2 - ((2 * Math.PI) * Math.Floor((z2 / (2 * Math.PI))));
            azimuth = (float)((anglerad2 * 180) / Math.PI);

            azimuth = Clamp(azimuth);

            return azimuth;
        }

        private float CalculateElevation(float distance2, int targetHeight, int izluchatelHeight)
        {
            double height = targetHeight - izluchatelHeight;
            return (float)(Math.Atan(height / distance2) * 180 / Math.PI);
        }

        private float CalculateDistance3(float distance2, int targetHeight, int izluchatelHeight)
        {
            float dist3d = (float)(Math.Sqrt(Math.Pow(distance2, 2) + Math.Pow(izluchatelHeight - targetHeight, 2)));
            return dist3d; //расстояние между двумя координатами в метрах
        }

        private float CalculateDistance2(double lat1, double long1, float lat2, float long2)
        {
            //радиус Земли
            double R = 6372795;
            //перевод координат в радианы
            lat1 *= Math.PI / 180;
            lat2 *= (float)Math.PI / 180;
            long1 *= Math.PI / 180;
            long2 *= (float)Math.PI / 180;
            //вычисление косинусов и синусов широт и разницы долгот
            double cl1 = Math.Cos(lat1);
            double cl2 = Math.Cos(lat2);
            double sl1 = Math.Sin(lat1);
            double sl2 = Math.Sin(lat2);
            double delta = long2 - long1;
            double cdelta = Math.Cos(delta);
            double sdelta = Math.Sin(delta);
            //вычисления длины большого круга
            double y = Math.Sqrt(Math.Pow(cl2 * sdelta, 2) + Math.Pow(cl1 * sl2 - sl1 * cl2 * cdelta, 2));
            double x = sl1 * sl2 + cl1 * cl2 * cdelta;
            double ad = Math.Atan2(y, x);
            float dist2d = (float)Math.Round(ad * R);
            return dist2d; //расстояние между двумя координатами в метрах
        }

        public static float Clamp(float angle)
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

        #endregion CALCULATIONS
    }

}
