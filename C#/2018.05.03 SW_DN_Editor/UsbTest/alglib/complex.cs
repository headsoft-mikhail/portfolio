using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNmeasure
{

        /********************************************************************
        Class defining a complex number with double precision.
        ********************************************************************/
        public struct complex
        {
            public double x;
            public double y;

            public complex(double _x)
            {
                x = _x;
                y = 0;
            }
            public complex(double _x, double _y)
            {
                x = _x;
                y = _y;
            }
            public double Re
            {
                get { return x; }
                set { x = value; }
            }
            public double Im
            {
                get { return y; }
                set { y = value; }
            }
            public double Abs
            {
                get { return Math.Sqrt(x * x + y * y); }
            }
            public double Arg
            {
                get { return Math.Atan2(y , x); }
            }
            public complex GetConjugate()
            {
                return new complex(x, -y);
            }
            public static implicit operator complex(double _x)
            {
                return new complex(_x);
            }
            public static bool operator ==(complex lhs, complex rhs)
            {
                return ((double)lhs.x == (double)rhs.x) & ((double)lhs.y == (double)rhs.y);
            }
            public static bool operator !=(complex lhs, complex rhs)
            {
                return ((double)lhs.x != (double)rhs.x) | ((double)lhs.y != (double)rhs.y);
            }
            public static complex operator +(complex lhs)
            {
                return lhs;
            }
            public static complex operator -(complex lhs)
            {
                return new complex(-lhs.x, -lhs.y);
            }
            public static complex operator +(complex lhs, complex rhs)
            {
                return new complex(lhs.x + rhs.x, lhs.y + rhs.y);
            }
            public static complex operator -(complex lhs, complex rhs)
            {
                return new complex(lhs.x - rhs.x, lhs.y - rhs.y);
            }
            public static complex operator *(complex lhs, complex rhs)
            {
                return new complex(lhs.x * rhs.x - lhs.y * rhs.y, lhs.x * rhs.y + lhs.y * rhs.x);
            }
        public static complex operator *(complex lhs, double rhs)
        {
            return new complex(lhs.x * rhs,  lhs.y * rhs);
        }
        public static complex operator /(complex lhs, complex rhs)
            {
                complex result;
                double e;
                double f;
                if (System.Math.Abs(rhs.y) < System.Math.Abs(rhs.x))
                {
                    e = rhs.y / rhs.x;
                    f = rhs.x + rhs.y * e;
                    result.x = (lhs.x + lhs.y * e) / f;
                    result.y = (lhs.y - lhs.x * e) / f;
                }
                else
                {
                    e = rhs.x / rhs.y;
                    f = rhs.y + rhs.x * e;
                    result.x = (lhs.y + lhs.x * e) / f;
                    result.y = (-lhs.x + lhs.y * e) / f;
                }
                return result;
            }
            public override int GetHashCode()
            {
                return x.GetHashCode() ^ y.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj is byte)
                    return Equals(new complex((byte)obj));
                if (obj is sbyte)
                    return Equals(new complex((sbyte)obj));
                if (obj is short)
                    return Equals(new complex((short)obj));
                if (obj is ushort)
                    return Equals(new complex((ushort)obj));
                if (obj is int)
                    return Equals(new complex((int)obj));
                if (obj is uint)
                    return Equals(new complex((uint)obj));
                if (obj is long)
                    return Equals(new complex((long)obj));
                if (obj is ulong)
                    return Equals(new complex((ulong)obj));
                if (obj is float)
                    return Equals(new complex((float)obj));
                if (obj is double)
                    return Equals(new complex((double)obj));
                if (obj is decimal)
                    return Equals(new complex((double)(decimal)obj));
                return base.Equals(obj);
            }
            public override string ToString()
            {
                string res = "";

                if (x != 0.0)
                {
                    res = x.ToString();
                }

                if (y != 0.0)
                {
                    if (y > 0)
                    {
                        res += "+";
                    }

                    res += y.ToString() + "i";
                }

                return res;
            }
            
        }    


}
