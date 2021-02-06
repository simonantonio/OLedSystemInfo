using System;
using System.Runtime.InteropServices;
using SystemInfo.System.Types;
using static SystemInfo.System.LibraryNames;

namespace SystemInfo.System
{
    internal static class LibraryNames
    {
        // ReSharper disable once InconsistentNaming
        public const string libc = "libc.so.6";
    }

    internal static unsafe partial class LibC
    {
        [DllImport(libc, SetLastError = true)]
        public static extern int ioctl(int fd, int request);

        [DllImport(libc, SetLastError = true)]
        public static extern int ioctl(int fd, int request, int arg);

        [DllImport(libc, SetLastError = true)]
        public static extern int ioctl(int fd, int request, void* arg);

        [DllImport(libc, SetLastError = true)]
        public static extern int open(byte* pathname, int flags, mode_t mode = default(mode_t));

        [DllImport(libc, SetLastError = true)]
        public static extern int close(int fd);

        [DllImport(libc, SetLastError = true)]
        //public static extern ssize_t write(int fd, void* buf, size_t count);
        public static extern int write(int fd, void* buf, size_t count);

        public static mode_t S_IFMT => 0xf000;
        public static mode_t S_IFDIR => 0x4000;
        public static mode_t S_IFCHR => 0x2000;
        public static mode_t S_IFBLK => 0x6000;
        public static mode_t S_IFREG => 0x8000;
        public static mode_t S_IFIFO => 0x1000;
        public static mode_t S_IFLNK => 0xa000;
        public static mode_t S_IFSOCK => 0xc000;
        public static mode_t S_ISUID => 0x800;
        public static mode_t S_ISGID => 0x400;
        public static mode_t S_ISVTX => 0x200;
        public static mode_t S_IRUSR => 0x100;
        public static mode_t S_IWUSR => 0x80;
        public static mode_t S_IXUSR => 0x40;
        public static mode_t S_IRWXU => 0x1c0;
        public static mode_t S_IRGRP => 0x20;
        public static mode_t S_IWGRP => 0x10;
        public static mode_t S_IXGRP => 0x8;
        public static mode_t S_IRWXG => 0x38;
        public static mode_t S_IROTH => 0x4;
        public static mode_t S_IWOTH => 0x2;
        public static mode_t S_IXOTH => 0x1;
        public static mode_t S_IRWXO => 0x7;

        public static int O_RDWR => 2;
    }
}
