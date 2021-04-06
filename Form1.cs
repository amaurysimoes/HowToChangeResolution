using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.IO;
using System.Media;

namespace howto_change_resolution
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Bitmap OriginalBitmap = null;

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            if (ofdOriginal.ShowDialog() == DialogResult.OK)
            {
                OriginalBitmap = new Bitmap(ofdOriginal.FileName);
                pictureBox1.Image = OriginalBitmap;
                using (Graphics gr = Graphics.FromImage(OriginalBitmap))
                {
                    txtDpiX.Text = gr.DpiX.ToString();
                    txtDpiY.Text = gr.DpiY.ToString();
                }
                txtWidth.Text = OriginalBitmap.Width.ToString();
                txtHeight.Text = OriginalBitmap.Height.ToString();
                mnuFileSaveAs.Enabled = true;
            }
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            if (sfdNew.ShowDialog() == DialogResult.OK)
            {
                int old_wid = OriginalBitmap.Width;
                int old_hgt = OriginalBitmap.Height;
                int new_wid = int.Parse(txtWidth.Text);
                int new_hgt = int.Parse(txtHeight.Text);

                using (Bitmap bm = new Bitmap(new_wid, new_hgt))
                {
                    Point[] points =
                    {
                        new Point(0, 0),
                        new Point(new_wid, 0),
                        new Point(0, new_hgt),
                    };
                    using (Graphics gr = Graphics.FromImage(bm))
                    {
                        gr.DrawImage(OriginalBitmap, points);
                    }
                    float dpix = float.Parse(txtDpiX.Text);
                    float dpiy = float.Parse(txtDpiY.Text);
                    bm.SetResolution(dpix, dpiy);
                    SaveImage(bm, sfdNew.FileName);
                    SystemSounds.Beep.Play();
                }
            }
        }

        // Save the file with the appropriate format.
        public void SaveImage(Image image, string filename)
        {
            string extension = Path.GetExtension(filename);
            switch (extension.ToLower())
            {
                case ".bmp":
                    image.Save(filename, ImageFormat.Bmp);
                    break;
                case ".exif":
                    image.Save(filename, ImageFormat.Exif);
                    break;
                case ".gif":
                    image.Save(filename, ImageFormat.Gif);
                    break;
                case ".jpg":
                case ".jpeg":
                    image.Save(filename, ImageFormat.Jpeg);
                    break;
                case ".png":
                    image.Save(filename, ImageFormat.Png);
                    break;
                case ".tif":
                case ".tiff":
                    image.Save(filename, ImageFormat.Tiff);
                    break;
                default:
                    throw new NotSupportedException(
                        "Unknown file extension " + extension);
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
