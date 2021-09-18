namespace UDP_FPGA
{
    public class GPScoordinates
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Altitude { get; set; }
        public GPScoordinates(float lat, float lon, int alt)
        {
            Latitude = lat;
            Longitude = lon;
            Altitude = alt;
        }

        public GPScoordinates()
        {
            Latitude = 0;
            Longitude =0;
            Altitude = 0;
        }
    }
}
