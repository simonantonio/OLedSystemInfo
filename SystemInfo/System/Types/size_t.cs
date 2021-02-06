using System;

namespace SystemInfo.System.Types
{
    /// <summary>
    /// 64 Bit system
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial struct size_t : IEquatable<size_t>
    {
        public static implicit operator ulong(size_t arg) => arg.ToUInt64();
        public static explicit operator uint(size_t arg) => arg.ToUInt32();
        public static explicit operator int(size_t arg) => (int)arg.Value;

        public static implicit operator size_t(uint arg) => new size_t(arg);
        // .NET uses 'int' mostly to for lengths.
        public static implicit operator size_t(int arg) => new size_t((uint)arg);
        // disambiguate between uint and int overloads
        public static implicit operator size_t(ushort arg) => new size_t((uint)arg);
        public static explicit operator size_t(ulong arg) => new size_t(arg);

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj != null && obj is size_t v)
            {
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(size_t v) => this == v;

        public static size_t operator +(size_t v) => new size_t(v.Value);
        public static size_t operator ~(size_t v) => new size_t(~v.Value);
        public static size_t operator ++(size_t v) => new size_t(v.Value + 1);
        public static size_t operator --(size_t v) => new size_t(v.Value - 1);
        public static size_t operator +(size_t v1, size_t v2) => new size_t(v1.Value + v2.Value);
        public static size_t operator -(size_t v1, size_t v2) => new size_t(v1.Value - v2.Value);
        public static size_t operator *(size_t v1, size_t v2) => new size_t(v1.Value * v2.Value);
        public static size_t operator /(size_t v1, size_t v2) => new size_t(v1.Value / v2.Value);
        public static size_t operator %(size_t v1, size_t v2) => new size_t(v1.Value % v2.Value);
        public static size_t operator &(size_t v1, size_t v2) => new size_t(v1.Value & v2.Value);
        public static size_t operator |(size_t v1, size_t v2) => new size_t(v1.Value | v2.Value);
        public static size_t operator ^(size_t v1, size_t v2) => new size_t(v1.Value ^ v2.Value);
        public static size_t operator <<(size_t v, int i) => new size_t(v.Value << i);
        public static size_t operator >>(size_t v, int i) => new size_t(v.Value >> i);
        public static bool operator ==(size_t v1, size_t v2) => v1.Value == v2.Value;
        public static bool operator !=(size_t v1, size_t v2) => v1.Value != v2.Value;
        public static bool operator <(size_t v1, size_t v2) => v1.Value < v2.Value;
        public static bool operator >(size_t v1, size_t v2) => v1.Value > v2.Value;
        public static bool operator <=(size_t v1, size_t v2) => v1.Value <= v2.Value;
        public static bool operator >=(size_t v1, size_t v2) => v1.Value >= v2.Value;

        private ulong __value;
        internal ulong Value => __value;

        internal size_t(ulong arg) { __value = arg; }
        internal size_t(uint arg) { __value = arg; }
        unsafe internal size_t(void* arg) { __value = (ulong)arg; }
        internal size_t(ssize_t arg) { __value = (ulong)arg.Value; }

        internal uint ToUInt32() => (uint)Value;
        internal ulong ToUInt64() => Value;
    }
}
