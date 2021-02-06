using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.System;
using SystemInfo.System.Types;

namespace SystemInfo.Hardware
{
    public partial class Display
    {
        private int _ic2File;
        private DisplayInfo _displayInfo;

        public void Close()
        {
            if (_ic2File >= 0) LibC.close(_ic2File);
        }

        public unsafe bool Open(string filename)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(filename);
                fixed (byte* buffer = bytes)
                {
                    _ic2File = LibC.open(buffer, LibC.O_RDWR);

                    //if verbose
                    var currentColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Open - fd:{_ic2File}");
                    Console.ForegroundColor = currentColor;//restore
                }


                var address = OLED_I2C_ADDR;

                //ioctl(disp->file, I2C_SLAVE, disp->address) 
                if (_ic2File >= 0)
                {
                    var result = LibC.ioctl(_ic2File, I2C_SLAVE, address);
                    if (result >= 0)
                    {
                        //if verbose
                        var currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"ioctl result {result}");
                        Console.ForegroundColor = currentColor;//restore

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.Write($"Failed to open {filename}");
                Console.Error.Write($"{ex}");
            }

            return false;
        }

        private unsafe void Send(byte[] data)
        {
            fixed (byte* buffer = data)
            {
                var size = new size_t(data.Length);
                var result = LibC.write(_ic2File, buffer, size);

                if (result != data.Length)
                {
                    Console.Error.WriteLine("Failed to send packet successfully");
                }

                ////if verbose
                //var currentColor = Console.ForegroundColor;
                //Console.ForegroundColor = ConsoleColor.Cyan;
                //Console.WriteLine($"Send - {size.ToUInt32()} bytes, sent - {result}-{data.Length} - {BitConverter.ToString(data)}");
                //Console.ForegroundColor = currentColor;//restore
            }
        }

        public void Init()
        {
            Send(_displayConfiguration);

            _displayInfo = new DisplayInfo()
            {
                Buffer = new byte[][]
                {
                    new byte[128],
                    new byte[128],
                    new byte[128],
                    new byte[128],
                    new byte[128],
                    new byte[128],
                    new byte[128],
                    new byte[128],
                },
                Font = Font.Font1,
            };
        }

        //send buffer data to device
        public void Flush()
        {
            foreach (var row in _displayInfo.Buffer)
            {
                var packets = new List<byte>()
                {
                    OLED_CTRL_BYTE_DATA_STREAM,
                };
                packets.AddRange(row);
                Send(packets.ToArray());
            }
        }

        public void Clear()
        {
            //lazy AF
            _displayInfo.Buffer = new byte[][]
            {
                new byte[128],
                new byte[128],
                new byte[128],
                new byte[128],
                new byte[128],
                new byte[128],
                new byte[128],
                new byte[128],
            };
            Flush();
        }


        public void Write(byte line, string message)
        {
            //break into pixel width of fonts
            for (var i = 0; i < message.Length; i++)
            {
                var c = message[i];

                if (i >= 128 / _displayInfo.Font.Width) break; // no text wrap

                // memcpy ( void * destination, const void * source, size_t num )
                //memcpy(disp->buffer[line] + i*fwidth + fspacing, &disp->font.data[(a-foffset) * fwidth], fwidth);

                //Array.Copy(
                //    _displayInfo.Font.Data[(c - _displayInfo.Font.Offset) * _displayInfo.Font.Width],
                //    0,
                //    _displayInfo.Buffer[line], i * _displayInfo.Font.Width + _displayInfo.Font.Spacing,
                //    _displayInfo.Font.Width);
                //todo
            }
        }

        public void WriteTo(byte x, byte y, string message)
        {
            foreach (var ch in message)
            {
                for (var i = 0; i < _displayInfo.Font.Width; i++)
                {
                    for (var j = 0; j < _displayInfo.Font.Height; j++)
                    {
                        var charDataIndex = (ch - _displayInfo.Font.Offset) * _displayInfo.Font.Width + i;
                        var pixelState = (_displayInfo.Font.Data[charDataIndex] >> j) & 0x01;

                        var posX = x + i;
                        var posY = y + j;
                        DrawPixel((byte)posX, (byte)posY, pixelState != 0);
                    }
                }

                x += Convert.ToByte(_displayInfo.Font.Width + _displayInfo.Font.Spacing);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawPixel(byte x, byte y, bool pixelState)
        {
            var pageId = (byte)y / 8;
            var bitOffset = (byte)y % 8;

            if (x >= 128 - 2) return;

            if (pixelState)
            {
                _displayInfo.Buffer[pageId][x] |= (byte)(1 << bitOffset);
            }
            else
            {
                _displayInfo.Buffer[pageId][x] &= (byte)~(1 << bitOffset);
            }
        }
    }
}
