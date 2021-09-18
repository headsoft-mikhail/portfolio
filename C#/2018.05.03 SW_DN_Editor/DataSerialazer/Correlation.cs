using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DNmeasure
{
    public class Corellation
    {
        static public event EventHandler<PelengFirst> pelengFirstPass;


        #region методы расчета математических функций

        static public alglib.complex Mean(alglib.complex[] X)
        {
            alglib.complex meanX = new alglib.complex(0, 0);
            for (int i = 0; i < X.Length; i++)
                meanX += X[i];
            meanX /= X.Length;
            return meanX;
        }

        static public alglib.complex[] MakeStandard(alglib.complex[] X)
        {
            alglib.complex meanX = Mean(X);
            alglib.complex[] Xstandard = new alglib.complex[X.Length];
            for (int i = 0; i < X.Length; i++)
                Xstandard[i] = new alglib.complex(X[i].Re - meanX.Re, X[i].Im - meanX.Im);
            return Xstandard;
        }

        static public alglib.complex SQRT(alglib.complex X)
        {
            double arg = Math.Atan2(X.Im, X.Re) / 2;
            double abs = Math.Sqrt(X.Abs);
            double re = abs * Math.Cos(arg);
            double im = abs * Math.Sin(arg);
            alglib.complex Y = new alglib.complex(re, im);
            return Y;
        }

        static public alglib.complex STDstandard(alglib.complex[] Xstandard)
        {
            alglib.complex Std = new alglib.complex(0, 0);
            for (int i = 0; i < Xstandard.Length; i++)
                Std += Xstandard[i].Abs * Xstandard[i].Abs;
            Std = SQRT(Std);
            return Std;
        }

        static public alglib.complex CovarianceStandard(alglib.complex[] Xstandard, alglib.complex[] Ystandard)
        {
            alglib.complex Covariance = new alglib.complex(0, 0);
            for (int i = 0; i < Xstandard.Length; i++)
                Covariance += Xstandard[i].GetConjugate() * Ystandard[i];
            return Covariance;
        }

        static public alglib.complex FindCorellation(alglib.complex[] Xstandard, alglib.complex[] Y)
        {
            alglib.complex[] Ystandard = MakeStandard(Y);
            alglib.complex stdX = STDstandard(Xstandard);
            alglib.complex stdY = STDstandard(Ystandard);
            alglib.complex cov = CovarianceStandard(Xstandard, Ystandard);
            alglib.complex CorellationComplex = cov / (stdX * stdY);
            return CorellationComplex;
        }

        private static alglib.complex[] TransposeVector(alglib.complex[,] Xcolumn)
        {
            alglib.complex[] Xrow = new alglib.complex[Xcolumn.GetLength(0)];
            for (int i = 0; i < Xcolumn.GetLength(0); i++)
            {
                Xrow[i] = Xcolumn[i, 0];
            }
            return Xrow;
        }

        private static alglib.complex[,] TransposeVector(alglib.complex[] Xrow)
        {
            alglib.complex[,] Xcolumn = new alglib.complex[Xrow.GetLength(0), 1];
            for (int i = 0; i < Xcolumn.GetLength(0); i++)
            {
                Xcolumn[i, 0] = Xrow[i];
            }
            return Xcolumn;
        }

        #endregion


        #region методы поиска минимума и пеленга

        /// <summary>
        /// Возвращает номер наименьшего числа в массиве
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        static public int FindMinAngleNumber(double[] X)
        {
            double d = double.MaxValue;
            int angleNumber = 0;
            for (int i = 0; i < X.Length; ++i)
            {
                double temp = Math.Abs(X[i]);
                if (d > temp)
                {
                    d = temp;
                    angleNumber = i;
                }
            }
            return angleNumber;
        }

        /// <summary>
        /// Возвращает расстояние на комплексной плоскости между вектором комплексной корелляции двух векторов и 1
        /// </summary>
        /// <param name="Xstandard"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        static public double FindDifference(alglib.complex[] Xstandard, alglib.complex[] Y)
        {
            alglib.complex corr = FindCorellation(Xstandard, Y);
            return Math.Sqrt(corr.Im * corr.Im + (1 - corr.Re) * (1 - corr.Re));
        }

        /// <summary>
        /// Осуществляет поиск минимума функции по методу золотого сечения в  заданном промежутке, используя аппроксимацию аргументов функции
        /// </summary>
        /// <param name="Xstandard"></param>
        /// <param name="angleStartNum"></param>
        /// <param name="angleStopNum"></param>
        /// <param name="measData"></param>
        /// <param name="
        /// "></param>
        /// <returns></returns>
        static public double FminbndCorrelation(alglib.complex[] Xstandard, int angleStartNum, int angleStopNum, MeasDataComplex measData, double freqNum)
        {
            double e, d, fu, fx;
            int si;
            int angleCenterNum = (angleStartNum + angleStopNum) / 2;
            int iter = 0;
            int maxiter = 500;
            double seps = 1.4901e-8;
            double a = angleStartNum;
            double b = angleStopNum;
            double v = a + 0.382 * (b - a);
            double w = v;
            double xf = v;
            double x = xf;
            fx = FindDifference(Xstandard, MeasDataParser.GetRayVector(measData, freqNum, x));
            double fv = fx;
            double fw = fx;
            double xm = (a + b) * 0.5;
            double tol1 = seps * Math.Abs(xf) + 1e-4 / 3.0;
            double tol2 = 2.0 * tol1;


            while ((Math.Abs(xf - xm) > (tol2 - 0.5 * (b - a))) && (iter < maxiter))
            {
                if (xf >= xm)
                    e = a - xf;
                else
                    e = b - xf;
                d = 0.382 * e;
                si = Math.Sign(d) + Convert.ToInt32(d == 0);
                x = xf + si * Math.Max(Math.Abs(d), tol1);
                fu = FindDifference(Xstandard, MeasDataParser.GetRayVector(measData, freqNum, x));

                if (fu <= fx)
                {
                    if (x >= xf)
                        a = xf;
                    else
                        b = xf;


                    v = w;
                    fv = fw;
                    w = xf;
                    fw = fx;
                    xf = x;
                    fx = fu;
                }
                else
                {
                    if (x < xf)
                        a = x;
                    else
                        b = x;
                    if ((fu <= fw) || (w == xf))
                    {
                        v = w;
                        fv = fw;
                        w = x;
                        fw = fu;
                    }
                    else
                        if ((fu <= fv) || (v == xf) || (v == w))
                        {
                            v = x;
                            fv = fu;
                        }
                }

                xm = 0.5 * (a + b);
                tol1 = seps * Math.Abs(xf) + 1e-4 / 3.0;
                tol2 = 2.0 * tol1;
                iter++;
            }
            return xf;
        }


        /// <summary>
        /// Осуществляет поиск минимума функции по методу золотого сечения в  заданном промежутке, используя аппроксимацию аргументов функции
        /// </summary>
        /// <param name="Xstandard"></param>
        /// <param name="angleStartNum"></param>
        /// <param name="angleStopNum"></param>
        /// <param name="measData"></param>
        /// <param name="freqNum"></param>
        /// <returns></returns>
        static public double FminbndMusic(alglib.complex[,] X, int angleStartNum, int angleStopNum, MeasDataComplex measData, double freqNum)
        {
            double e, d, fu, fx;
            int si;
            int angleCenterNum = (angleStartNum + angleStopNum) / 2;
            int iter = 0;
            int maxiter = 500;
            double seps = 1.4901e-8;
            double a = angleStartNum;
            double b = angleStopNum;
            double v = a + 0.382 * (b - a);
            double w = v;
            double xf = v;
            double x = xf;
            fx = FindMusicResult(X, TransposeVector(MeasDataParser.GetRayVector(measData, freqNum, x)));
            double fv = fx;
            double fw = fx;
            double xm = (a + b) * 0.5;
            double tol1 = seps * Math.Abs(xf) + 1e-4 / 3.0;
            double tol2 = 2.0 * tol1;


            while ((Math.Abs(xf - xm) > (tol2 - 0.5 * (b - a))) && (iter < maxiter))
            {
                if (xf >= xm)
                    e = a - xf;
                else
                    e = b - xf;
                d = 0.382 * e;
                si = Math.Sign(d) + Convert.ToInt32(d == 0);
                x = xf + si * Math.Max(Math.Abs(d), tol1);
                fu = FindMusicResult(X, TransposeVector(MeasDataParser.GetRayVector(measData, freqNum, x)));

                if (fu <= fx)
                {
                    if (x >= xf)
                        a = xf;
                    else
                        b = xf;


                    v = w;
                    fv = fw;
                    w = xf;
                    fw = fx;
                    xf = x;
                    fx = fu;
                }
                else
                {
                    if (x < xf)
                        a = x;
                    else
                        b = x;
                    if ((fu <= fw) || (w == xf))
                    {
                        v = w;
                        fv = fw;
                        w = x;
                        fw = fu;
                    }
                    else
                        if ((fu <= fv) || (v == xf) || (v == w))
                        {
                            v = x;
                            fv = fu;
                        }
                }

                xm = 0.5 * (a + b);
                tol1 = seps * Math.Abs(xf) + 1e-4 / 3.0;
                tol2 = 2.0 * tol1;
                iter++;
            }
            return xf;
        }



        /// <summary>
        /// Возвращает пеленг
        /// </summary>
        /// <param name="X"></param>
        /// <param name="measData"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        static public double GetPelengCorrelation(alglib.complex[] X, MeasDataComplex measData, double freq)
        {
            double[] difference = new double[measData.corners.Length];
            alglib.complex[] Xstandard = MakeStandard(X);
            for (int i = 0; i < measData.corners.Length; i++)
            {
                difference[i] = FindDifference(Xstandard, MeasDataParser.GetRayVector(measData, MeasDataParser.FindFreqNumByFreq(measData, freq), i));
            }
            int minAngleNumEst = FindMinAngleNumber(difference);
            double MinAngleNum = FminbndCorrelation(Xstandard, minAngleNumEst - 1, minAngleNumEst + 1, measData, MeasDataParser.FindFreqNumByFreq(measData, freq));
            return MeasDataParser.FindAngleByAngleNum(measData, MinAngleNum);
        }


        static public double GetPelengMUSIC(alglib.complex[,] Xcolumn, MeasDataComplex measData, double freq)
        {

            double[] firstResult = new double[measData.corners.Length];
            alglib.complex[,] aFull = new alglib.complex[measData.rays.Length, 1];
            alglib.complex[,] aCut = new alglib.complex[Xcolumn.GetLength(0), 1];
            for (int i = 0; i < measData.corners.Length; i++)
            {
                firstResult[i] = FindMusicResult(Xcolumn, TransposeVector(MeasDataParser.GetRayVector(measData, MeasDataParser.FindFreqNumByFreq(measData, freq), i)));
            }
            int minAngleNumEst = FindMinAngleNumber(firstResult);

            if (pelengFirstPass != null) pelengFirstPass(null, new PelengFirst(firstResult));

            double MinAngleNum = FminbndMusic(Xcolumn, minAngleNumEst - 1, minAngleNumEst + 1, measData, MeasDataParser.FindFreqNumByFreq(measData, freq));


            return MeasDataParser.FindAngleByAngleNum(measData, MinAngleNum);
        }


        private static double FindMusicResult(alglib.complex[,] Xcolumn, alglib.complex[,] aFull)
        {
            alglib.complex[,] aCut = new alglib.complex[Xcolumn.GetLength(0), 1];
            for (int j = 0; j < aCut.GetLength(0); j++)
            {
                if (Peleng.RefChannelEnable)
                {
                    //aCut[j, 0] = aFull[j, 0] / Math.Exp(aFull[Peleng.RefChannel, 0].Arg);
                    double fi = -aFull[Peleng.RefChannel, 0].Arg;
                    aCut[j, 0] = aFull[j, 0] * (Math.Cos(fi) + new alglib.complex(0, 1) * Math.Sin(fi));
                }
                else
                    aCut[j, 0] = aFull[j, 0];
            }
            return MUSIC(Xcolumn, aCut);
        }




        #endregion

        #region MUSIC
        private static double MUSIC(alglib.complex[,] x, alglib.complex[,] a)
        {
            int N = a.GetLength(0);
            alglib.complex[,] r = new alglib.complex[N, N];
            double[] d = new double[N];
            alglib.complex[,] q = new alglib.complex[N, N];
            alglib.complex[,] qn = new alglib.complex[N, N];
            alglib.complex[,] aHqn = new alglib.complex[1, N];
            alglib.complex[,] result = new alglib.complex[1, 1];

            /*************************************************************************
            This subroutine calculates C = alpha*op1(A)*op2(B) +beta*C where:
            * C is MxN general matrix
            * op1(A) is MxK matrix
            * op2(B) is KxN matrix
            * "op" may be identity transformation, transposition, conjugate transposition*/
            alglib.cmatrixgemm(
                N, //M - matrix size, N > 0
                N, //N - matrix size, N > 0
                1, //K - matrix size, K > 0
                new alglib.complex(1), //Alpha - coefficient
                x, // A - matrix
                0, //IA - submatrix offset
                0, //JA - submatrix offset
                0, //OpTypeA - transformation type:        *0 - no transformation       * 1 - transposition       * 2 - conjugate transposition
                x, // B - matrix
                0, // IB - submatrix offset
                0, //JB - submatrix offset
                2, //OpTypeB - transformation type:        *0 - no transformation       * 1 - transposition       * 2 - conjugate transposition
                new alglib.complex(0), //Beta - coefficient
                ref r, //C - matrix RESULT
                0, //IC - submatrix offset
                0); //JC - submatrix offset

            bool converged = alglib.hmatrixevd(r, N, 1, true, out d, out q);

            /*************************************************************************
            This subroutine calculates C = alpha*op1(A)*op2(B) +beta*C where:
            * C is MxN general matrix
            * op1(A) is MxK matrix
            * op2(B) is KxN matrix
            * "op" may be identity transformation, transposition, conjugate transposition*/
            alglib.cmatrixgemm(
                N, //M - matrix size, N > 0
                N, //N - matrix size, N > 0
                N - 1, //K - matrix size, K > 0
                new alglib.complex(1), //Alpha - coefficient
                q, // A - matrix
                0, //IA - submatrix offset
                0, //JA - submatrix offset
                0, //OpTypeA - transformation type:        *0 - no transformation       * 1 - transposition       * 2 - conjugate transposition
                q, // B - matrix
                0, // IB - submatrix offset
                0, //JB - submatrix offset
                2, //OpTypeB - transformation type:        *0 - no transformation       * 1 - transposition       * 2 - conjugate transposition
                new alglib.complex(0), //Beta - coefficient
                ref qn, //C - matrix RESULT
                0, //IC - submatrix offset
                0); //JC - submatrix offset

            // a^H*qn = r
            alglib.cmatrixgemm(
                1, //M - matrix size, N > 0
                N, //N - matrix size, N > 0
                N, //K - matrix size, K > 0
                new alglib.complex(1), //Alpha - coefficient
                a, // A - matrix
                0, //IA - submatrix offset
                0, //JA - submatrix offset
                2, //OpTypeA - transformation type:        *0 - no transformation       * 1 - transposition       * 2 - conjugate transposition
                qn, // B - matrix
                0, // IB - submatrix offset
                0, //JB - submatrix offset
                0, //OpTypeB - transformation type:        *0 - no transformation       * 1 - transposition       * 2 - conjugate transposition
                new alglib.complex(0), //Beta - coefficient
                ref aHqn, //C - matrix RESULT
                0, //IC - submatrix offset
                0); //JC - submatrix offset

            //aHqn*a = qn
            alglib.cmatrixgemm(
                1, //M - matrix size, N > 0
                1, //N - matrix size, N > 0
                N, //K - matrix size, K > 0
                new alglib.complex(1), //Alpha - coefficient
                aHqn, // A - matrix
                0, //IA - submatrix offset
                0, //JA - submatrix offset
                0, //OpTypeA - transformation type:        *0 - no transformation       * 1 - transposition       * 2 - conjugate transposition
                a, // B - matrix
                0, // IB - submatrix offset
                0, //JB - submatrix offset
                0, //OpTypeB - transformation type:        *0 - no transformation       * 1 - transposition       * 2 - conjugate transposition
                new alglib.complex(0), //Beta - coefficient
                ref result, //C - matrix RESULT
                0, //IC - submatrix offset
                0); //JC - submatrix offset

            return result[0, 0].x;
        }

        #endregion



    }

    public class PelengFirst : EventArgs
    {
        double[] peleng;

        public double[] Peleng
        {
            get { return peleng; }
            set { peleng = value; }
        }
        public PelengFirst(double[] peleng)
        {
            this.peleng = peleng;
        }
    }
}
