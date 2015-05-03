using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace EasyScreenshot
{
    class Screenshot
    {
        //Some variables to store screenshot, screen dimensions etc.
        public Bitmap screenshotBmp;
        public Graphics graphics;
        public int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        public int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Take Screenshot and Save on Desktop
        /// </summary>
        public void TakeScreenshot()
        {
            TakeScreenshotWholeDesktop();
            SaveImage();
        }

        /// <summary>
        /// Take Screenshot and store in screenshotBmp
        /// </summary>
        private void TakeScreenshotWholeDesktop()
        {
            screenshotBmp = new Bitmap(screenWidth, screenHeight);
            graphics = Graphics.FromImage(screenshotBmp);
            graphics.CopyFromScreen(0, 0, 0, 0, screenshotBmp.Size);
        }

        /// <summary>
        /// Return Screenshot after taking it
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap ReturnScreenshot()
        {
            TakeScreenshotWholeDesktop();
            return screenshotBmp;
        }

        /// <summary>
        /// Save image on Desktop
        /// </summary>
        public void SaveImage()
        {
            try
            {
                screenshotBmp.Save(desktopPath + "\\" + GenerateFilename(), System.Drawing.Imaging.ImageFormat.Png);
            }
            catch
            {
                MessageBox.Show("Could not save image!");
            }
        }

        /// <summary>
        /// Generate Filename with current date and time
        /// </summary>
        /// <returns>String</returns>
        private string GenerateFilename()
        {
            DateTime dateTime = DateTime.Now;
            string dateTimeFormat = "dd-MM-yyyy_HH-mm-ss";
            string dateTimeStamp = dateTime.ToString(dateTimeFormat);
            string fileName = "Screenshot_" + dateTimeStamp + ".png";
            return fileName;
        }

        /// <summary>
        /// Crop image method
        /// </summary>
        /// <param name="source">Bitmap</param>
        /// <param name="section">Rectangle</param>
        /// <returns>Bitmap</returns>
        public Bitmap CropImage(Bitmap bitmap, Rectangle rectangle)
        {
            Bitmap cropBmp = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics graphics2 = Graphics.FromImage(bitmap);
            graphics2.DrawImage(bitmap, 0, 0, rectangle, GraphicsUnit.Pixel);
            return cropBmp;
        }
    }
}
