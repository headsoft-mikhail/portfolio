using System;

namespace DataSerialazer
{
    [Serializable]
    public class MeasData
    {
        public int[] rays;
        public double[] corners;
        public double[] freqs;

        public double[, ,] complexRe;
        public double[, ,] complexIm;

        public string description;

        public MeasData(int raysNum, int freqsNum, int cornerNum)
        {
            rays = new int[raysNum];
            corners = new double[cornerNum];
            freqs = new double[freqsNum];
            complexRe = new double[raysNum, freqsNum, cornerNum];
            complexIm = new double[raysNum, freqsNum, cornerNum];
        }

        public MeasData()
        {
        }

    }
}
