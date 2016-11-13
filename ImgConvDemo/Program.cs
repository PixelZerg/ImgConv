using ImgConv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgConvDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Display());

            Classifier c = new Classifier(Classifier.InputFormat.Greyscale, 64, 64);
            c.InstantiateNetwork(5, 10);
            Console.WriteLine(string.Join(Environment.NewLine,c.Run(new System.Drawing.Bitmap("C:/Users/PixelZerg/Pictures/pzerg.png"))));
        }
    }
}
