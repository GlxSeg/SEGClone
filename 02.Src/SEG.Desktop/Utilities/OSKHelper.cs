using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using TextBox = System.Windows.Controls.TextBox;
using Timer = System.Windows.Forms.Timer;

namespace SEG.Desktop.Utilities
{
    public class OSKHelper
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
        [DllImport("kernel32.dll", SetLastError = true)]

        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]

        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);
        [DllImport("coredll.dll", SetLastError = true)]

        static extern Int32 GetLastError();

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
        private OSKHelper()
        {

        }

        static OSKHelper instance;
        public static OSKHelper Instance
        {
            get { return instance ?? (instance = new OSKHelper()); }
        }

        private Process process;
        private Timer timer;
        private Point lastPoint;
        static Point CalculatePoint(Point point, PasswordBox textbox)
        {
            if (point.Y + OskHeight > Screen.PrimaryScreen.Bounds.Height)
            {
                return new Point(point.X, point.Y - OskHeight);
            }
            else
            {
                return new Point(point.X, point.Y + textbox.ActualHeight);
            }
        }
        static Point CalculatePoint(Point point, TextBox textbox)
        {
            if (point.Y + OskHeight > Screen.PrimaryScreen.Bounds.Height)
            {
                return new Point(point.X, point.Y - OskHeight);
            }
            else
            {
                return new Point(point.X, point.Y + textbox.ActualHeight);
            }
        }
        private object syncRoot = new object();
        public void EnableOsk(TextBox textbox)
        {
            
                  lock (syncRoot)
                {
                    
                    var p = textbox.PointToScreen(new Point(0, 0));
                    lastPoint = CalculatePoint(p, textbox);
                    int x = (int)lastPoint.X;
                    int y = (int)lastPoint.Y;
                    if (process == null || process.HasExited)
                    {
                        var iPtr = new IntPtr();
                        
                        var psi = new ProcessStartInfo
                        {
                            FileName = Environment.GetFolderPath(Environment.SpecialFolder.System) +
                                       Path.DirectorySeparatorChar + "osk.exe",
                            WorkingDirectory =
                                Environment.GetFolderPath(Environment.SpecialFolder.System),
                            WindowStyle = ProcessWindowStyle.Hidden,
                           

                        };
                        
                        process =
                            Process.Start(psi);
                       
                       
                      
                        timer = new Timer { Interval = 1000 };
                        timer.Tick += TimerTick;
                        timer.Start();
                    }
                    else
                    {

                        bool t = SetWindowPos(process.MainWindowHandle, (IntPtr)SpecialWindowHandles.HWND_TOP, x, y,
                                              OskWidth, OskHeight,
                                              SetWindowPosFlags.SWP_SHOWWINDOW | SetWindowPosFlags.SWP_NOACTIVATE);
                    }
                }
                

          
            
        }

        


       
        private const int OskWidth = 500;
        private const int OskHeight = 350;
        void TimerTick(object sender, EventArgs e)
        {
            PosKeyboard();
        }

        void PosKeyboard()
        {
            lock (syncRoot)
            {

                if (process != null && !process.HasExited && process.MainWindowHandle != IntPtr.Zero)
                {
                  
                
                    timer.Tick -= TimerTick;
                    timer.Stop();
                    int x = (int)lastPoint.X;
                    int y = (int)lastPoint.Y;

                    bool t = SetWindowPos(process.MainWindowHandle, (IntPtr)SpecialWindowHandles.HWND_TOP, x, y,
                                          OskWidth, OskHeight,
                                          SetWindowPosFlags.SWP_SHOWWINDOW | SetWindowPosFlags.SWP_NOACTIVATE);
                }
            }
        }
        public void HideOsk()
        {
            if (process != null && !process.HasExited)
            {

                bool t = SetWindowPos(process.MainWindowHandle, (IntPtr)SpecialWindowHandles.HWND_TOP, 1, 1,
                                        OskWidth, OskHeight,
                                        SetWindowPosFlags.SWP_HIDEWINDOW | SetWindowPosFlags.SWP_NOACTIVATE);
             
            }
        }
    }



    [Flags]
    public enum SetWindowPosFlags : uint
    {

        /// <summary>
        ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
        /// </summary>
        SWP_ASYNCWINDOWPOS = 0x4000,

        /// <summary>
        ///     Prevents generation of the WM_SYNCPAINT message.
        /// </summary>
        SWP_DEFERERASE = 0x2000,

        /// <summary>
        ///     Draws a frame (defined in the window's class description) around the window.
        /// </summary>
        SWP_DRAWFRAME = 0x0020,

        /// <summary>
        ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        /// </summary>
        SWP_FRAMECHANGED = 0x0020,

        /// <summary>
        ///     Hides the window.
        /// </summary>
        SWP_HIDEWINDOW = 0x0080,

        /// <summary>
        ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
        /// </summary>
        SWP_NOACTIVATE = 0x0010,

        /// <summary>
        ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
        /// </summary>
        SWP_NOCOPYBITS = 0x0100,

        /// <summary>
        ///     Retains the current position (ignores X and Y parameters).
        /// </summary>
        SWP_NOMOVE = 0x0002,

        /// <summary>
        ///     Does not change the owner window's position in the Z order.
        /// </summary>
        SWP_NOOWNERZORDER = 0x0200,

        /// <summary>
        ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
        /// </summary>
        SWP_NOREDRAW = 0x0008,

        /// <summary>
        ///     Same as the SWP_NOOWNERZORDER flag.
        /// </summary>
        SWP_NOREPOSITION = 0x0200,

        /// <summary>
        ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        /// </summary>
        SWP_NOSENDCHANGING = 0x0400,

        /// <summary>
        ///     Retains the current size (ignores the cx and cy parameters).
        /// </summary>
        SWP_NOSIZE = 0x0001,

        /// <summary>
        ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
        /// </summary>
        SWP_NOZORDER = 0x0004,

        /// <summary>
        ///     Displays the window.
        /// </summary>
        SWP_SHOWWINDOW = 0x0040,
    }

    public enum SpecialWindowHandles
    {
        /// <summary>
        ///     Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
        /// </summary>
        HWND_TOP = 0,
        /// <summary>
        ///     Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
        /// </summary>
        HWND_BOTTOM = 1,
        /// <summary>
        ///     Places the window at the top of the Z order.
        /// </summary>
        HWND_TOPMOST = -1,
        /// <summary>
        ///     Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
        /// </summary>
        HWND_NOTOPMOST = -2
    }
}
