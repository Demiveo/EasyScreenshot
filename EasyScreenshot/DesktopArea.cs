using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace EasyScreenshot
{
    public partial class DesktopArea : Form
    {
        //Some variables like for storing mouse positions or rectangle size etc.
        private int mousex;
        private int mousey;
        private Rectangle mRect;

        //Demiveo logo color
        private Color demiveoColor = Color.FromArgb(0x1eb3d6);

        //Screenshot class call
        private Screenshot screenshot = new Screenshot();

        /// <summary>
        /// Constructor
        /// </summary>
        public DesktopArea()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Takes screenshot by calling ReturnScreenshot fron screenshot class.
        /// Puts blue color panels to correct positions.
        /// Shows Form with screenshot in background.
        /// </summary>
        public void TakeScreenshotAndChoose()
        {
            this.BackgroundImage = screenshot.ReturnScreenshot();
            panel1.Location = new Point(0, 0);
            panel1.Size = new Size(3, screenshot.screenHeight);
            panel2.Location = new Point(0, 0);
            panel2.Size = new Size(screenshot.screenWidth, 3);
            panel3.Location = new Point(0, screenshot.screenHeight - 3);
            panel3.Size = new Size(screenshot.screenWidth, 3);
            panel4.Location = new Point(screenshot.screenWidth - 3, 0);
            panel4.Size = new Size(3, screenshot.screenHeight);
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Width = screenshot.screenWidth;
            this.Height = screenshot.screenHeight;
            this.DoubleBuffered = true;
            this.Visible = true;
            this.Enabled = true;
            this.Show();
            this.BringToFront();
        }

        /// <summary>
        /// Store mouse X and Y position in mousex and mousey and sets rectangle start points
        /// </summary>
        /// <param name="e">MouseEvent</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            mousex = e.X;
            mousey = e.Y;
            mRect = new Rectangle(e.X, e.Y, 0, 0);
            this.Invalidate();
            this.Refresh();
        }

        /// <summary>
        /// Refresh rectangle information on mouse movement
        /// </summary>
        /// <param name="e">MouseEvent</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (e.X >= mousex && e.Y >= mousey)
                    mRect = new Rectangle(mousex, mousey, e.X - mRect.Left, e.Y - mRect.Top);
                else if (e.X <= mousex && e.Y <= mousey)
                    mRect = new Rectangle(e.X, e.Y, mousex - e.X, mousey - e.Y);
                else if (e.X <= mousex && e.Y >= mousey)
                    mRect = new Rectangle(e.X, mousey, mousex - e.X, e.Y - mRect.Top);
                else if (e.X >= mousex && e.Y <= mousey)
                    mRect = new Rectangle(mousex, e.Y, e.X - mRect.Left, mousey - e.Y);
                this.Invalidate();
                this.Refresh();
            }
        }

        /// <summary>
        /// Draws rectangle on form while mouse moving
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(demiveoColor.R, demiveoColor.G, demiveoColor.B), 1);
            e.Graphics.DrawRectangle(pen, mRect);
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(50, 20, 20, 20));
            e.Graphics.FillRectangle(blueBrush, mRect);
        }

        /// <summary>
        /// Save picture on mouse button release
        /// </summary>
        /// <param name="e">MouseEvent</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((mRect.Width * mRect.Height) >= 1 || (mRect.Width * mRect.Height) <= -1)
            {

                if (mRect.Height < 0)
                {
                    mRect.Y = e.Y;
                    mRect.Height = mousey - e.Y;
                }
                if (mRect.Width < 0)
                {
                    mRect.X = e.X;
                    mRect.Width = mousex - e.X;
                }
                crop();
            }
            Save();
            this.Close();
        }

        /// <summary>
        /// Crops image
        /// </summary>
        private void crop()
        {
            screenshot.screenshotBmp = screenshot.CropImage(screenshot.screenshotBmp, mRect);
        }

        /// <summary>
        /// Saves image by using methods of Screenshot Class.
        /// </summary>
        private void Save()
        {
            screenshot.SaveImage();
        }



        /// <summary>
        /// ESC key detection to escape
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message message, Keys key)
        {
            if (key == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref message, key);
        }
    }
}
