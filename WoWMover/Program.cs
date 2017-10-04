using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;

namespace WoWMover
{
    static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int width, int height, uint uFlags);

        private const uint SHOWWINDOW = 0x0040;

        static int x = 0;
        static int y = 0;

        static void Main()
        {
            StreamReader read = new StreamReader(@".\size.ini");
            string[] set = read.ReadToEnd().Split('\n');
            x = Convert.ToInt32(set[0]);
            y = Convert.ToInt32(set[1]);

            List<Process> WoW = new List<Process>();

            foreach (Process p in Process.GetProcessesByName("WoW"))
            {
                WoW.Add(p);
            }

            foreach (Process p in Process.GetProcessesByName("Proxy"))
            {
                WoW.Add(p);
            }
            

            int GlobalHeight = SystemInformation.VirtualScreen.Height;
            int GlobalWidth = SystemInformation.VirtualScreen.Width;

            IntPtr[] Handle = new IntPtr[WoW.Count];
            for (int i = 0; i < WoW.Count; i++)
            {
                Handle[i] = WoW[i].MainWindowHandle;
                WoW[i].PriorityClass = ProcessPriorityClass.Normal;
            }

            int Schieber = 0;
            int Row = 1;

            foreach (IntPtr p in Handle)
            {
                // 310 241
                SetWindowPos(p, IntPtr.Zero, GlobalWidth - (x * Row), 0 + (y * Schieber), x, y, SHOWWINDOW);

                Schieber++;

                if (Schieber > 2)
                {
                    Row++;
                    Schieber = 0;
                }
            }
        }
    }
}
