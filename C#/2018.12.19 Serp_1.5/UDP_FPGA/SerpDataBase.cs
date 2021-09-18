using MySql.Data.MySqlClient;

namespace UDP_FPGA
{
    class SerpDataBase
    {
        private MySqlConnection dBase;
        private MySqlCommand command = new MySqlCommand();
        public SerpDataBase(string connectionStr, int deviceCount)
        {
            dBase = new MySqlConnection(connectionStr);
            dBase.Open();
            Clear("serp");
            for (int i = 0; i < deviceCount; i++)
                AddLine(i, 0, 0, 0);
        }

        public void UpdateGPSData( int ID, float latitude, float longitude)
        {
            command.CommandText = "UPDATE serp SET latitude = " + (latitude.ToString()).Replace(',', '.') + ", longitude = " + (longitude.ToString()).Replace(',', '.') + " WHERE number = " + (ID+1) + ";";
            command.Connection = dBase;
            command.ExecuteNonQuery();
        }

        public void UpdateRotatorData(int ID, float azimuth)
        {
            command.CommandText = "UPDATE serp SET azimuth = " + (azimuth.ToString()).Replace(',', '.') + " WHERE number = " + (ID+1) + ";";
            command.Connection = dBase;
            command.ExecuteNonQuery();
        }

        public void AddLine(int index, float latitude, float longitude, float azimuth)
        {
            command.CommandText = "INSERT INTO serp (number, latitude, longitude, azimuth) VALUES(" + index + ", " + latitude + ", " + longitude + ", " + azimuth + ");";
            command.Connection = dBase;
            command.ExecuteNonQuery();
        }

        public void Clear(string tableName)
        {
            command.CommandText = "TRUNCATE " + tableName + ";";
            command.Connection = dBase;
            command.ExecuteNonQuery();
        }

        public void Close()
        {
            dBase.Close();
        }

    }
}
