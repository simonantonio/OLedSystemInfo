using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Hardware
{
    public partial class Display
    {
        public const int I2C_SLAVE = 0x0703; //from /usr/include/linux/i2c-dev.h

        public const int OLED_I2C_ADDR = 0x3c;

        // Control byte
        public const byte OLED_CTRL_BYTE_CMD_SINGLE = 0x80;
        public const byte OLED_CTRL_BYTE_CMD_STREAM = 0x00;
        public const byte OLED_CTRL_BYTE_DATA_STREAM = 0x40;
        // Fundamental commands (page 28)
        public const byte OLED_CMD_SET_CONTRAST = 0x81;
        public const byte OLED_CMD_DISPLAY_RAM = 0xA4;
        public const byte OLED_CMD_DISPLAY_ALLON = 0xA5;
        public const byte OLED_CMD_DISPLAY_NORMAL = 0xA6;
        public const byte OLED_CMD_DISPLAY_INVERTED = 0xA7;
        public const byte OLED_CMD_DISPLAY_OFF = 0xAE;
        public const byte OLED_CMD_DISPLAY_ON = 0xAF;
        // Addressing Command Table (page 30)
        public const byte OLED_CMD_SET_MEMORY_ADDR_MODE = 0x20;
        public const byte OLED_CMD_SET_COLUMN_RANGE = 0x21;
        public const byte OLED_CMD_SET_PAGE_RANGE = 0x22;
        // Hardware Config (page 31)
        public const byte OLED_CMD_SET_DISPLAY_START_LINE = 0x40;
        public const byte OLED_CMD_SET_SEGMENT_REMAP = 0xA1;
        public const byte OLED_CMD_SET_MUX_RATIO = 0xA8;
        public const byte OLED_CMD_SET_COM_SCAN_MODE = 0xC8;
        public const byte OLED_CMD_SET_DISPLAY_OFFSET = 0xD3;
        public const byte OLED_CMD_SET_COM_PIN_MAP = 0xDA;
        public const byte OLED_CMD_NOP = 0xE3;
        // Timing and Driving Scheme (page 32)
        public const byte OLED_CMD_SET_DISPLAY_CLK_DIV = 0xD5;
        public const byte OLED_CMD_SET_PRECHARGE = 0xD9;
        public const byte OLED_CMD_SET_VCOMH_DESELCT = 0xDB;
        // Charge Pump (page 62)
        public const byte OLED_CMD_SET_CHARGE_PUMP = 0x8D;
        // SH1106 Display
        public const byte OLED_SET_PAGE_ADDRESS = 0xB0;

        readonly byte[] _displayConfiguration = new byte[]
        {
            OLED_CTRL_BYTE_CMD_STREAM,
            OLED_CMD_DISPLAY_OFF,

            OLED_SET_PAGE_ADDRESS,
            0x02, /*set lower column address*/
            0x10, /*set higher column address*/

            OLED_CMD_SET_MUX_RATIO, 0x3F,
            // Set the display offset to 0
            OLED_CMD_SET_DISPLAY_OFFSET, 0x00,
            // Display start line to 0
            OLED_CMD_SET_DISPLAY_START_LINE,
            // Mirror the x-axis. In case you set it up such that the pins are north.
            // 0xA0 - in case pins are south - default
            OLED_CMD_SET_SEGMENT_REMAP,
            // Mirror the y-axis. In case you set it up such that the pins are north.
            // 0xC0 - in case pins are south - default
            OLED_CMD_SET_COM_SCAN_MODE,
            // Default - alternate COM pin map
            OLED_CMD_SET_COM_PIN_MAP, 0x12,
            // set contrast
            OLED_CMD_SET_CONTRAST, 0x7F,
            // Set display to enable rendering from GDDRAM (Graphic Display Data RAM)
            OLED_CMD_DISPLAY_RAM,
            // Normal mode!
            OLED_CMD_DISPLAY_NORMAL,
            // Default oscillator clock
            OLED_CMD_SET_DISPLAY_CLK_DIV, 0x80,
            // Enable the charge pump
            OLED_CMD_SET_CHARGE_PUMP, 0x14,
            // Set precharge cycles to high cap type
            OLED_CMD_SET_PRECHARGE, 0x22,
            // Set the V_COMH deselect voltage to max
            OLED_CMD_SET_VCOMH_DESELCT, 0x30,
            // Horizonatal addressing mode - same as the KS108 GLCD
            OLED_CMD_SET_MEMORY_ADDR_MODE, 0x00,
            // Turn the Display ON
            OLED_CMD_DISPLAY_ON
        };

        readonly byte[] _displayDraw = new byte[]
        {
            OLED_CTRL_BYTE_CMD_STREAM,
            // column 0 to 127
            OLED_CMD_SET_COLUMN_RANGE,
            0x00,
            0x7F,
            // page 0 to 7
            OLED_CMD_SET_PAGE_RANGE,
            0x00,
            0x07
        };


    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct DisplayInfo
    {
        public int Address;
        public int File;
        public FontInfo Font;
        public byte[][] Buffer;//[8,128]
    }
}
