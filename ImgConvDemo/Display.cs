using ImgConv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgConvDemo
{
    public partial class Display : Form
    {
        public Bitmap curBitmap = null;

        public Display()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                //try
                //{
                    curBitmap = new Bitmap(openFileDialog1.FileName);
                //}
                //catch (IOException)
                //{
                //    return;
                //}
            }
            pictureBox1.Image = curBitmap;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            //ConvNetSharp.Volume v = Program.classifier.RunV(curBitmap);
            //new VolumeDisplay(v).ShowDialog();
            double[] output = Program.classifier.Run(curBitmap);
            for (int i = 0; i < output.Length; i++)
            {
                listBox1.Items.Add(output[i]);
            }

            if (output[0] > output[1])
            {
                double confidence = (output[0] * 100) - (output[1] * 100);

                //is cat
                label4.Text = "Cat. Confidence: " + Math.Round(confidence, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                double confidence = (output[1] * 100) - (output[0] * 100);

                //is dog
                label4.Text = "Car. Confidence: " + Math.Round(confidence, 2, MidpointRounding.AwayFromZero);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
