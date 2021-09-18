using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSerialazer;

namespace UsbTest
{
    public class MeasDataComplex : MeasData
    {
        public alglib.complex[, ,] complex;
        public double[, ,] amplitude;
        public double[, ,] phase;
        //
        public List<double>[][] calibrationCorners;
        public List<double>[][] calibrationAmpDiff;
        //
        public MeasDataComplex()
        {

        }

        public MeasDataComplex(MeasData measData)  // no extremum parsing
        {
            int freqsCount = measData.freqs.Length;
            int raysCount = measData.rays.Length;
            int cornersCount = measData.corners.Length;

            complex = new alglib.complex[raysCount, freqsCount, cornersCount];
            complexRe = new double[raysCount, freqsCount, cornersCount];
            complexIm = new double[raysCount, freqsCount, cornersCount];
            amplitude = new double[raysCount, freqsCount, cornersCount];
            phase = new double[raysCount, freqsCount, cornersCount];
            rays = new int[raysCount];
            freqs = new double[freqsCount];
            corners = new double[cornersCount];
            description = measData.description;
            for (int i = 0; i < raysCount; i++)
            {
                rays[i] = measData.rays[i];
                for (int j = 0; j < freqsCount; j++)
                {
                    for (int k = 0; k < cornersCount; k++)
                    {
                        complexRe[i, j, k] = measData.complexRe[i, j, k];
                        complexIm[i, j, k] = measData.complexIm[i, j, k];
                        complex[i, j, k] = new alglib.complex(measData.complexRe[i, j, k], measData.complexIm[i, j, k]);
                        amplitude[i, j, k] = complex[i, j, k].Abs;
                        phase[i, j, k] = complex[i, j, k].Arg;
                    }
                }
            }
            for (int i = 0; i < measData.freqs.Length; i++)
            {
                freqs[i] = measData.freqs[i];
            }
            for (int i = 0; i < measData.corners.Length; i++)
            {
                corners[i] = measData.corners[i];
            }
        }

        public MeasDataComplex(MeasData measData, int quarter1corner) // full measDataComplex with extremum parsing
        {
            int freqsCount = measData.freqs.Length;
            int raysCount = measData.rays.Length;
            int cornersCount = measData.corners.Length;

            complex = new alglib.complex[raysCount, freqsCount, cornersCount];
            complexRe = new double[raysCount, freqsCount, cornersCount];
            complexIm = new double[raysCount, freqsCount, cornersCount];
            amplitude = new double[raysCount, freqsCount, cornersCount];
            phase = new double[raysCount, freqsCount, cornersCount];
            rays = new int[raysCount];
            freqs = new double[freqsCount];
            corners = new double[cornersCount];
            description = measData.description;
            for (int i = 0; i < raysCount; i++)
            {
                rays[i] = measData.rays[i];
                for (int j = 0; j < freqsCount; j++)
                {
                    for (int k = 0; k < cornersCount; k++)
                    {
                        complexRe[i, j, k] = measData.complexRe[i, j, k];
                        complexIm[i, j, k] = measData.complexIm[i, j, k];
                        complex[i, j, k] = new alglib.complex(measData.complexRe[i, j, k], measData.complexIm[i, j, k]);
                        amplitude[i, j, k] = complex[i, j, k].Abs;
                        phase[i, j, k] = complex[i, j, k].Arg;
                    }
                }
            }
            for (int i = 0; i < measData.freqs.Length; i++)
            {
                freqs[i] = measData.freqs[i];
            }
            for (int i = 0; i < measData.corners.Length; i++)
            {
                corners[i] = measData.corners[i];
            }

            calibrationCorners = new List<double>[freqsCount][];
            calibrationAmpDiff = new List<double>[freqsCount][];

            if (cornersCount>=8) // makes sense only if corners count is >= 8
            {
            #region Calculate Calibration Data

            for (int freqNum = 0; freqNum < freqsCount; freqNum++)
            {
                calibrationCorners[freqNum] = new List<double>[4];
                calibrationAmpDiff[freqNum] = new List<double>[4];

                calibrationCorners[freqNum][0] = new List<double>();
                calibrationCorners[freqNum][1] = new List<double>();
                calibrationCorners[freqNum][2] = new List<double>();
                calibrationCorners[freqNum][3] = new List<double>();


                calibrationAmpDiff[freqNum][0] = new List<double>();
                calibrationAmpDiff[freqNum][1] = new List<double>();
                calibrationAmpDiff[freqNum][2] = new List<double>();
                calibrationAmpDiff[freqNum][3] = new List<double>();


                ////test
                //cornersCount = 36;
                //double[] ampDifference = new double[cornersCount];
                //corners = new double[cornersCount];
                //for (int i = 0; i < cornersCount; i++)
                //{
                //    corners[i] = i * 360 / cornersCount;
                //    ampDifference[i] = Math.Abs(Math.Cos((corners[i] + 4 * 360 / cornersCount) * Math.PI / 180)) - Math.Abs(Math.Sin((corners[i] + 4 * 360 / cornersCount) * Math.PI / 180));
                //}
                ////test

                double[] ampDifference = new double[corners.Length];
                int[] extremumPoints = new int[4];
                int partsCount = extremumPoints.Length;
                double maxValue = double.MinValue;
                double minValue = double.MaxValue;
                for (int cornerNum = 0; cornerNum < cornersCount; cornerNum++)
                {
                    ampDifference[cornerNum] = (20 * Math.Log10(complex[0, freqNum, cornerNum].Abs) - 20 * Math.Log10(complex[1, freqNum, cornerNum].Abs));

                    if (ampDifference[cornerNum] > maxValue)  // amp difference in dB at single freq
                    {
                        maxValue = ampDifference[cornerNum];
                        extremumPoints[0] = cornerNum;  // first MAX
                    }

                    if (ampDifference[cornerNum] < minValue)
                    {
                        minValue = ampDifference[cornerNum];
                        extremumPoints[1] = cornerNum;  // first MIN
                    }
                }
                int shiftedCornerNum;
                int max1Index = extremumPoints[0] + cornersCount / 2;
                int min1Index = extremumPoints[1] + cornersCount / 2;
                maxValue = minValue;
                for (int cornerNum = max1Index - (int)(cornersCount / 4); cornerNum < max1Index + (int)(cornersCount / 4); cornerNum++)
                {
                    shiftedCornerNum = cornerNum;
                    if (cornerNum >= cornersCount)
                        shiftedCornerNum = cornerNum - cornersCount;
                    if (cornerNum < 0)
                        shiftedCornerNum = cornerNum + cornersCount;
                    if (ampDifference[shiftedCornerNum] > maxValue)
                    {
                        maxValue = ampDifference[shiftedCornerNum];
                        extremumPoints[2] = cornerNum;                           // second MAX
                    }
                }
                minValue = maxValue;
                for (int cornerNum = min1Index - (int)(cornersCount / 4); cornerNum < min1Index + (int)(cornersCount / 4); cornerNum++)
                {
                    shiftedCornerNum = cornerNum;
                    if (cornerNum >= cornersCount)
                        shiftedCornerNum = cornerNum - cornersCount;
                    if (cornerNum < 0)
                        shiftedCornerNum = cornerNum + cornersCount;
                    if (ampDifference[shiftedCornerNum] < minValue)
                    {
                        minValue = ampDifference[shiftedCornerNum];
                        extremumPoints[3] = cornerNum;                           // second MIN
                    }
                }
                // sort extremums
                for (int extremumNum = 0; extremumNum < partsCount; extremumNum++)
                {
                    while (extremumPoints[extremumNum] > cornersCount)
                        extremumPoints[extremumNum] -= cornersCount;
                }
                Array.Sort(extremumPoints);
                // find I quarter
                int quarter1partNum = 3;
                for (int extremumNum = 0; extremumNum < partsCount - 1; extremumNum++)
                {
                    for (int cornerNum = extremumPoints[extremumNum]; cornerNum < extremumPoints[extremumNum + 1]; cornerNum++)
                    {
                        if ((quarter1corner>corners[cornerNum]) && (quarter1corner<=corners[cornerNum+1]))
                                quarter1partNum = extremumNum;
                    }
                }
                // divide array to monotonous parts and sort using quarter1partNum
                int  startCornerNum, stopCornerNum,partNum, shiftedPartNum, index;
                for (int i = quarter1partNum; i < partsCount + quarter1partNum; i++)
                {
                    partNum = i;
                    shiftedPartNum = partNum - quarter1partNum;
                    if (partNum >= partsCount)
                        partNum = partNum - partsCount;
                    startCornerNum = extremumPoints[partNum];
                    if (partNum + 1 >= partsCount)
                        stopCornerNum = extremumPoints[partNum - partsCount + 1] + cornersCount;
                    else
                        stopCornerNum = extremumPoints[partNum + 1];
                    for (int cornerNum = startCornerNum; cornerNum <= stopCornerNum; cornerNum++)
                    {
                        if (cornerNum >= cornersCount)
                        {
                            index = cornerNum - cornersCount;
                            calibrationCorners[freqNum][shiftedPartNum].Add(corners[index] + 360);
                        }
                        else
                        {
                            index = cornerNum;
                            calibrationCorners[freqNum][shiftedPartNum].Add(corners[index]);
                        }
                        calibrationAmpDiff[freqNum][shiftedPartNum].Add(ampDifference[index]);
                    }
                }
            }
            #endregion Calculate Calibration Data
            }
        }
    }

    class MeasDataParser
    {
        /// <summary>
        /// Рассчитывает массив амплитуд луча на указанной частоте
        /// </summary>
        /// <param name="measData"></param>
        /// <param name="rayNum"></param>
        /// <param name="freqNum"></param>
        /// <returns></returns>
        static public double[] GetCornerArrayAmpl(MeasDataComplex measData, int rayNum, int freqNum)
        {
            double[] cornerArrayAmpl = new double[measData.corners.Length];
            for (int i = 0; i < measData.corners.Length; i++)
            {
                cornerArrayAmpl[i] = measData.complex[rayNum, freqNum, i].Abs;
            }
            return cornerArrayAmpl;
        }

        static public double[] GetCornerArrayAmpl(MeasData measData, int rayNum, int freqNum)
        {
            double[] cornerArrayAmpl = new double[measData.corners.Length];
            double Re, Im;
            for (int i = 0; i < measData.corners.Length; i++)
            {
                Re = measData.complexRe[rayNum, freqNum, i];
                Im = measData.complexIm[rayNum, freqNum, i];
                cornerArrayAmpl[i] = Math.Sqrt(Re * Re + Im * Im);
            }
            return cornerArrayAmpl;
        }

        /// <summary>
        //// Рассчитывает массив разностей фаз между каналами rayNum и опорным refChannel для всех направлений на заданной частоте
        /// </summary>
        /// <param name="measData"></param>
        /// <param name="refChannel"></param>
        /// <param name="rayNum"></param>
        /// <param name="freqNum"></param>
        /// <returns></returns>
        static public double[] GetCornerArrayPhase(MeasDataComplex measData, int refChannel, int rayNum, int freqNum)
        {
            double[] cornerArrayPhaseShift = new double[measData.corners.Length];

            for (int i = 0; i < measData.corners.Length; i++)
            {
                cornerArrayPhaseShift[i] = 57.2958 * (measData.complex[rayNum, freqNum, i].Arg - measData.complex[refChannel, freqNum, i].Arg);
                if (cornerArrayPhaseShift[i] < 0)
                    cornerArrayPhaseShift[i] += 360;
            }
            return cornerArrayPhaseShift;
        }

        /// <summary>
        /// Линейная аппроксимация вещественных чисел
        /// </summary>
        /// <param name="NumA"></param>
        /// <param name="A"></param>
        /// <param name="NumB"></param>
        /// <param name="B"></param>
        /// <param name="NumC"></param>
        /// <returns></returns>
        static public double LinApprox(double NumA, double A, double NumB, double B, double NumC)
        {
            double k = (NumC - NumA) / (NumB - NumA);
            double C = (A + (B - A) * k);
            return C;
        }

        /// <summary>
        /// Поиск номера частоты по значению
        /// </summary>
        /// <param name="measData"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static double FindFreqNumByFreq(MeasDataComplex measData, double freq)
        {
            double freqNum = -1;
            if (freq <= measData.freqs[0]) freqNum = 0;
            if (freq >= measData.freqs[measData.freqs.Length - 1]) freqNum = measData.freqs.Length - 1;
            if ((freq > measData.freqs[0]) && (freq < measData.freqs[measData.freqs.Length - 1]))
            {
                for (int i = 0; i < measData.freqs.Length; i++)
                {
                    if (freq == measData.freqs[i])
                    {
                        freqNum = i;
                        i = measData.freqs.Length - 1;
                    }
                    else
                        if (freq < measData.freqs[i])
                        {
                            freqNum = LinApprox(measData.freqs[i - 1], i - 1, measData.freqs[i], i, freq);
                            i = measData.freqs.Length - 1;
                        }
                }
            }
            return freqNum;
        }

        /// <summary>
        /// Поиск номера угла по значению
        /// </summary>
        /// <param name="measData"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static double FindAngleNumByAngle(MeasDataComplex measData, double angle)
        {
            double angleNum = -1;
            while (angle < 0)
                angle += 360;
            while (angle > 360)
                angle -= 360;

            if ((angle >= measData.corners[0]) && (angle <= measData.corners[measData.corners.Length - 1]))
            {
                for (int i = 0; i < measData.corners.Length; i++)
                {
                    if (angle == measData.corners[i])
                    {
                        angleNum = i;
                        i = measData.corners.Length - 1;
                    }
                    else
                        if (angle < measData.corners[i])
                        {
                            angleNum = LinApprox(measData.corners[i - 1], i - 1, measData.corners[i], i, angle);
                            i = measData.corners.Length;
                        }
                }
            }
            else
                angleNum = LinApprox(measData.corners[measData.corners.Length - 1], measData.corners.Length - 1, measData.corners[0] + 360, measData.corners.Length, angle);
            return angleNum;
        }

        /// <summary>
        /// Возвращает частоту по заданному номеру частоты
        /// </summary>
        /// <param name="measData"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static double FindFreqByFreqNum(MeasDataComplex measData, double freqNum)
        {
            double targetFreq = -1;
            if ((freqNum <= measData.freqs.Length - 1) && (freqNum >= 0))
                if ((int)freqNum == freqNum)
                    targetFreq = measData.freqs[(int)freqNum];
                else
                    targetFreq = LinApprox((int)freqNum, measData.freqs[(int)freqNum], (int)freqNum + 1, measData.freqs[(int)freqNum + 1], freqNum);
            return targetFreq;
        }

        /// <summary>
        /// Возвращает угол  по заданному номеру угла
        /// </summary>
        /// <param name="measData"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static double FindAngleByAngleNum(MeasDataComplex measData, double angleNum)
        {
            double targetAngle;
            while (angleNum > measData.corners.Length)
                angleNum -= measData.corners.Length;
            while (angleNum < 0)
                angleNum += measData.corners.Length;
            if ((int)angleNum == angleNum)
                targetAngle = measData.corners[(int)angleNum];
            else
            {
                if ((int)angleNum == measData.corners.Length - 1)
                    targetAngle = LinApprox((int)angleNum, measData.corners[(int)angleNum], (int)angleNum + 1, measData.corners[0] + 360, angleNum);
                else
                    targetAngle = LinApprox((int)angleNum, measData.corners[(int)angleNum], (int)angleNum + 1, measData.corners[(int)angleNum + 1], angleNum);
            }
            if (targetAngle >= 360)
                targetAngle -= 360;
            return targetAngle;
        }

        /// <summary>
        /// Возвращает комплексный КП заданного луча при заданных угле и частоте с аппроксимацией по углам и частотам
        /// </summary>
        /// <param name="measData"></param>
        /// <param name="ray"></param>
        /// <param name="freqNum"></param>
        /// <param name="cornerNum"></param>
        /// <returns></returns>
        public static alglib.complex GetPoint(MeasDataComplex measData, int ray, double freqNum, double cornerNum)
        {
            while (cornerNum < 0)
                cornerNum += measData.corners.Length - 1;
            while (cornerNum >= measData.corners.Length)
                cornerNum -= measData.corners.Length - 1;

            alglib.complex p1, p2, targetPoint;
            targetPoint = new alglib.complex(0, 0);
            if ((freqNum > measData.freqs.Length) || (ray > measData.rays.Length))
                return targetPoint;

            bool c1 = ((int)freqNum == freqNum);
            bool c2 = ((int)cornerNum == cornerNum);
            bool c3 = (cornerNum < measData.corners.Length - 1);
            if (c1 && c2)
                targetPoint = measData.complex[ray, (int)freqNum, (int)cornerNum];
            else
            {
                if (!c1 && !c2)
                {
                    p1 = LinApprox((int)freqNum, measData.complex[ray, (int)freqNum, (int)cornerNum], (int)freqNum + 1, measData.complex[ray, (int)freqNum + 1, (int)cornerNum], freqNum);

                    if (c3)
                    {
                        p2 = LinApprox((int)freqNum, measData.complex[ray, (int)freqNum, (int)cornerNum + 1], (int)freqNum + 1, measData.complex[ray, (int)freqNum + 1, (int)cornerNum + 1], freqNum);
                        targetPoint = LinApprox((int)cornerNum, p1, (int)cornerNum + 1, p2, cornerNum);
                    }
                    else
                    {
                        p2 = LinApprox((int)freqNum, measData.complex[ray, (int)freqNum, 0], (int)freqNum + 1, measData.complex[ray, (int)freqNum + 1, 0], freqNum);
                        targetPoint = LinApprox((int)cornerNum, p1, (int)cornerNum + 1, p2, cornerNum);
                    }
                }
                if (c1 && !c2)
                    if (c3)
                    {
                        targetPoint = LinApprox((int)cornerNum, measData.complex[ray, (int)freqNum, (int)cornerNum], (int)cornerNum + 1, measData.complex[ray, (int)freqNum, (int)cornerNum + 1], cornerNum);
                    }
                    else
                    {
                        targetPoint = LinApprox((int)cornerNum, measData.complex[ray, (int)freqNum, (int)cornerNum], (int)cornerNum + 1, measData.complex[ray, (int)freqNum, 0], cornerNum);
                    }
                if (!c1 && c2)
                    targetPoint = LinApprox((int)freqNum, measData.complex[ray, (int)freqNum, (int)cornerNum], (int)freqNum + 1, measData.complex[ray, (int)freqNum + 1, (int)cornerNum], freqNum);
            }
            return targetPoint;
        }

        /// <summary>
        /// Линейная аппроксимация комплексных чисел 
        /// </summary>
        /// <param name="NumA"></param>
        /// <param name="A"></param>
        /// <param name="NumB"></param>
        /// <param name="B"></param>
        /// <param name="NumC"></param>
        /// <returns></returns>
        static public alglib.complex LinApprox(double NumA, alglib.complex A, double NumB, alglib.complex B, double NumC)
        {
            double k = (NumC - NumA) / (NumB - NumA);
            alglib.complex C = new alglib.complex(A.Re + (B.Re - A.Re) * k, A.Im + (B.Im - A.Im) * k);
            return C;
        }

        /// <summary>
        /// Собирает вектор из комплексных КП всех лучей на заданных частоте и угле
        /// </summary>
        /// <param name="measData"></param>
        /// <param name="freqNum"></param>
        /// <param name="cornerNum"></param>
        /// <returns></returns>
        public static alglib.complex[] GetRayVector(MeasDataComplex measData, double freqNum, double cornerNum)
        {
            alglib.complex[] RayVector = new alglib.complex[measData.rays.Length];
            for (int i = 0; i < measData.rays.Length; i++)
                RayVector[i] = GetPoint(measData, i, freqNum, cornerNum);
            return RayVector;
        }

    }
}
