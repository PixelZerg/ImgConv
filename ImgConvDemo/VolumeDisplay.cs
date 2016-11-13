using ConvNetSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgConvDemo
{
    public partial class VolumeDisplay : Form
    {
        public VolumeDisplay()
        {
            InitializeComponent();
        }

        public VolumeDisplay(ConvNetSharp.Volume vol)
        {
            InitializeComponent();
            

            this.pictureBox1.Image = VolumeToBitmap(vol);
        }

        public static Bitmap VolumeToBitmap(Volume vol)
        {
            Bitmap bmp = new Bitmap(vol.Width, vol.Height);

            for (int i = 0; i < vol.Width; i++)
            {
                for (int j = 0; j < vol.Height; j++)
                {
                    if (vol.Depth == 1)
                    {
                        int p = (int)(vol.Get(i, j, 0) * 255);
                        bmp.SetPixel(i, j, Color.FromArgb(p, p, p));
                    }
                    else if (vol.Depth == 3)
                    {
                        bmp.SetPixel(i, j, Color.FromArgb((int)(vol.Get(i, j, 0) * 255), (int)(vol.Get(i, j, 1) * 255), (int)(vol.Get(i, j, 2) * 255)));
                    }
                }
            }
            return bmp;
        }
    }
}
