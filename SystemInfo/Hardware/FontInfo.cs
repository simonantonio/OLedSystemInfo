using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Hardware
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct FontInfo
    {
        public byte Width;
        public byte Height;
        public byte Spacing;
        public byte Offset;
        //public byte* Data;
        public byte[] Data;
    }
}
