using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using SystemInfo.Hardware;

namespace SystemInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            //get ip4 address, hostname
            //get cpu, memory, disk free
            //get time, uptime
            //weather (rain, wind, sun, storm) - min/max/current

            //network details
            var networkAddresses = RunCommand("hostname -I").Split(' ').FirstOrDefault();
            var host = RunCommand("hostname").Trim();

            //cpu usage
            var mpstatResult = RunCommand("mpstat").Split('\n');
            var cpuUsage = GetCpuUsage(mpstatResult);

            var memory = RunCommand("free").Split('\n');
            var memoryUsePercent = GetMemoryUsage(memory);

            //var uptime = RunCommand("uptime").Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToList()[2].Replace(",", string.Empty);

            //var weather = QueryUrl("http://wttr.in/wellard?TQ0A");
            var freeSpace = RunCommand("df").Split('\n').FirstOrDefault(x => x.EndsWith("/"))?.Split(' ');
            var freePercent = string.Empty;
            if (freeSpace != null)
            {
                freePercent = freeSpace[^2];
            }

            Console.WriteLine($"{networkAddresses}\n{host}");
            Console.Write($"CPU {(cpuUsage <= -1M ? "error" : cpuUsage.ToString("n0"))}% Memory {(memoryUsePercent <= -1M ? "error" : memoryUsePercent.ToString("n0"))}% ");
            Console.WriteLine($"Disk {freePercent}");

            Console.WriteLine($"Up {TimeSpan.FromMilliseconds(Environment.TickCount64)}");
            //cpu temp?

            var display = new Display();
            if (display.Open("/dev/i2c-0"))
            {
                display.Init();
                display.Clear();
                //display.Write(1, "Hello world");
                display.DrawPixel(3,3, true);
                display.DrawPixel(3,4, true);
                display.DrawPixel(3,5, true);
                display.DrawPixel(4,3, true);
                display.DrawPixel(5,3, true);

                display.WriteTo(0,0, "Hello world");

                display.Flush();

                display.Close();
            }
            else
            {
                Console.Error.WriteLine("Failed to open i2c device");
            }
        }

        private static decimal GetMemoryUsage(string[] memory)
        {
            var inUse = -1M;

            if (memory.Length >= 2)
            {
                var line = memory[2];
                var noSpaceLine = Regex.Replace(line, @"\s+", " "); //replace all the multi space breaks with singles

                var data = noSpaceLine.Split(' ');

                //[1] = total
                //[2] = used
                //var free = data[3];

                if (decimal.TryParse(data[1], out var total) && decimal.TryParse(data[2], out var used)) inUse = used / total * 100M;
            }

            return inUse;
        }

        private static decimal GetCpuUsage(string[] mpstatResult)
        {
            var inUse = -1M;

            if (mpstatResult.Length >= 3)
            {
                var line = mpstatResult[3];
                var noSpaceLine = Regex.Replace(line, @"\s+", " "); //replace all the multi space breaks with singles

                var data = noSpaceLine.Split(' ');

                var cpu = data.LastOrDefault();
                if (decimal.TryParse(cpu, out var idle)) inUse = 100M - idle;
            }

            return inUse;
        }

        private static string QueryUrl(string url)
        {
            using var client = new HttpClient();
            try
            {
                var result = client.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
                var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return content;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to get url result \"{url}\"");
                Console.Error.WriteLine(ex);
            }

            return string.Empty;
        }

        private static string RunCommand(string command)
        {
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{command}\"",
                        RedirectStandardOutput = true,
                    }
                };

                process.Start();

                TextReader tr = process.StandardOutput;
                var result = tr.ReadToEnd();

                process.Dispose();
                tr.Dispose();

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to run command \"{command}\"");
                Console.Error.WriteLine(ex);
            }

            return string.Empty;
        }
    }
}
