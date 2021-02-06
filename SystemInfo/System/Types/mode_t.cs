using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.System
{
    public struct mode_t : IEquatable<mode_t>
    {
        private uint __value;
        internal uint Value => __value;

        private mode_t(uint value) => __value = value;

        public static implicit operator mode_t(ushort arg) => new mode_t(arg);

        public static explicit operator ushort(mode_t arg) => (ushort)arg.Value;

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj != null && obj is mode_t v)
            {
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(mode_t v) => this == v;

        public static mode_t operator ~(mode_t v) => new mode_t(~v.Value);
        public static mode_t operator &(mode_t v1, mode_t v2) => new mode_t(v1.Value & v2.Value);
        public static mode_t operator |(mode_t v1, mode_t v2) => new mode_t(v1.Value | v2.Value);
        public static bool operator ==(mode_t v1, mode_t v2) => v1.Value == v2.Value;
        public static bool operator !=(mode_t v1, mode_t v2) => v1.Value != v2.Value;
    }
}
