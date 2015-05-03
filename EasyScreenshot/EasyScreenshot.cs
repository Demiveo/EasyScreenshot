using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace EasyScreenshot
{
    public partial class EasyScreenshot : Form
    {
        //Register and unregister hotkey imports
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //Key codes
        private const int MOD_ALT = 0x0001;
        private const int MOD_SHIFT = 0x0004;
        private const int WM_HOTKEY = 0x0312;


        //Class calls
        Screenshot screenshot = new Screenshot();

        /// <summary>
        /// Constructor
        /// </summary>
        public EasyScreenshot()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Load method. Register Hotkeys on start
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            //register hotkeys by start
            RegisterHotKey(this.Handle, 1, MOD_ALT + MOD_SHIFT, (int)Keys.F3);
            RegisterHotKey(this.Handle, 2, MOD_ALT + MOD_SHIFT, (int)Keys.F4);
        }
        /// <summary>
        /// Unregister hotkeys on closing form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EasyScreenshot_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
        }

        /// <summary>
        /// Detects hotkey usage
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == WM_HOTKEY && (int)message.WParam == 1)
            {
                screenshot.TakeScreenshot();
            }
            else if (message.Msg == WM_HOTKEY && (int)message.WParam == 2)
            {
                DesktopArea da = new DesktopArea();
                da.TakeScreenshotAndChoose();
            }
            base.WndProc(ref message);
        }


        /// <summary>
        /// Changes visibility if its necessary.
        /// </summary>
        /// <param name="e">Event Status</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.Opacity = 0;
            this.Visible = false;
        }
    }
}
