using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.System.Types
{
    /// <summary>
    /// 64 bit system
    /// </summary>
    // ReSharper disable once IdentifierTypo
    public partial struct ssize_t : IEquatable<ssize_t>
    {
        public static implicit operator long(ssize_t arg) => arg.ToInt64();
        public static explicit operator int(ssize_t arg) => arg.ToInt32();
        public static explicit operator size_t(ssize_t arg) => new size_t(arg);

        public static implicit operator ssize_t(int arg) => new ssize_t(arg);
        public static explicit operator ssize_t(long arg) => new ssize_t(arg);

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj != null && obj is ssize_t v)
            {
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(ssize_t v) => this == v;

        public static ssize_t operator +(ssize_t v) => new ssize_t(v.Value);
        public static ssize_t operator -(ssize_t v) => new ssize_t(-v.Value);
        public static ssize_t operator ~(ssize_t v) => new ssize_t(~v.Value);
        public static ssize_t operator ++(ssize_t v) => new ssize_t(v.Value + 1);
        public static ssize_t operator --(ssize_t v) => new ssize_t(v.Value - 1);
        public static ssize_t operator +(ssize_t v1, ssize_t v2) => new ssize_t(v1.Value + v2.Value);
        public static ssize_t operator -(ssize_t v1, ssize_t v2) => new ssize_t(v1.Value - v2.Value);
        public static ssize_t operator *(ssize_t v1, ssize_t v2) => new ssize_t(v1.Value * v2.Value);
        public static ssize_t operator /(ssize_t v1, ssize_t v2) => new ssize_t(v1.Value / v2.Value);
        public static ssize_t operator %(ssize_t v1, ssize_t v2) => new ssize_t(v1.Value % v2.Value);
        public static ssize_t operator &(ssize_t v1, ssize_t v2) => new ssize_t(v1.Value & v2.Value);
        public static ssize_t operator |(ssize_t v1, ssize_t v2) => new ssize_t(v1.Value | v2.Value);
        public static ssize_t operator ^(ssize_t v1, ssize_t v2) => new ssize_t(v1.Value ^ v2.Value);
        public static ssize_t operator <<(ssize_t v, int i) => new ssize_t(v.Value << i);
        public static ssize_t operator >>(ssize_t v, int i) => new ssize_t(v.Value >> i);
        public static bool operator ==(ssize_t v1, ssize_t v2) => v1.Value == v2.Value;
        public static bool operator !=(ssize_t v1, ssize_t v2) => v1.Value != v2.Value;
        public static bool operator <(ssize_t v1, ssize_t v2) => v1.Value < v2.Value;
        public static bool operator >(ssize_t v1, ssize_t v2) => v1.Value > v2.Value;
        public static bool operator <=(ssize_t v1, ssize_t v2) => v1.Value <= v2.Value;
        public static bool operator >=(ssize_t v1, ssize_t v2) => v1.Value >= v2.Value;

        private long __value;
        internal long Value => __value;

        internal ssize_t(long arg) { __value = arg; }
        internal ssize_t(int arg) { __value = arg; }

        internal int ToInt32() => (int)Value;
        internal long ToInt64() => Value;
    }
}
