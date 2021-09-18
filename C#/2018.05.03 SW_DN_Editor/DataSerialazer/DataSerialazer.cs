using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataSerialazer
{
    public class DataSerialazer
    {
       static public MeasData Load(string fileName)
        {
            MeasData measData;
            using (FileStream fstream = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                measData = (MeasData)binaryFormatter.Deserialize(fstream);
                fstream.Close();
            }
            return measData;
        }

       static public void Save(MeasData measData, string fileName)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream filestream = new FileStream(fileName + ".sav", FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(filestream, measData);
            }
        }
    }
}
