using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.Management;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;

namespace SEG.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotFocusEvent,
                                    new RoutedEventHandler(GotFocus_Event_TextBox), true);

            EventManager.RegisterClassHandler(typeof(PasswordBox), UIElement.GotFocusEvent,
                                    new RoutedEventHandler(GotFocus_Event_PassBox), true);


            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.LostFocusEvent,
                                    new RoutedEventHandler(LostFocus_Event), true);

            EventManager.RegisterClassHandler(typeof(PasswordBox), UIElement.LostFocusEvent,
                                    new RoutedEventHandler(LostFocus_Event), true);

            base.OnStartup(eventArgs);
        }

        private static void GotFocus_Event_TextBox(object sender, RoutedEventArgs e)
        {
            // TestKeyboard();
            OpenWindows8TouchKeyboard_TextBox(sender, e);
        }

        private static void GotFocus_Event_PassBox(object sender, RoutedEventArgs e)
        {
            // TestKeyboard();
            OpenWindows8TouchKeyboard_PassBox(sender, e);
        }


        //http://www.c-sharpcorner.com/UploadFile/29d7e0/get-the-key-board-details-of-your-system-in-windows-form/
        private static bool IsSurfaceKeyboardAttached()
        {
            SelectQuery Sq = new SelectQuery("Win32_Keyboard");
            ManagementObjectSearcher objOSDetails = new ManagementObjectSearcher(Sq);
            ManagementObjectCollection osDetailsCollection = objOSDetails.Get();
            //Windows 8 tablet are returnign 2 device when keyboard is connecto
            //My application won't be used for Desktop so this condition is fine
            //But u might want to see if keyboard is usb and == 1 so you are 
            //returning true or not on tablet.  
            return osDetailsCollection.Count <= 1 ? true : false;
        }

        private static void OpenWindows8TouchKeyboard_TextBox(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null && IsSurfaceKeyboardAttached())
            {
                var path = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
                if (!File.Exists(path))
                {
                    // older windows versions
                    path = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\osk.exe";
                }
                Process.Start(path);
                textBox.BringIntoView();//SetFocus so u dont lose focused area
            }
        }

        private static void OpenWindows8TouchKeyboard_PassBox(object sender, RoutedEventArgs e)
        {
            var passBox = e.OriginalSource as PasswordBox;
            if (passBox != null && IsSurfaceKeyboardAttached())
            {
                var path = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
                if (!File.Exists(path))
                {
                    // older windows versions
                    path = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\osk.exe";
                }
                Process.Start(path);
                passBox.BringIntoView();//SetFocus so u dont lose focused area
            }
        }


        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;

        private void CloseOnscreenKeyboard()
        {
            // retrieve the handler of the window  
            int iHandle = FindWindow("IPTIP_Main_Window", "");
            if (iHandle > 0)
            {
                // close the window using API        
                SendMessage(iHandle, WM_SYSCOMMAND, SC_CLOSE, 0);
            }
        }

        private void LostFocus_Event(object sender, EventArgs e)
        {
            // It's time to close the onscreen keyboard.
            CloseOnscreenKeyboard();
        }
    }
}
