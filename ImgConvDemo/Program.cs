using ImgConv;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgConvDemo
{
    static class Program
    {
        public static Classifier classifier = new Classifier(Classifier.InputFormat.RGB, 16, 16);
        public static Dictionary<Bitmap, int> dict = new Dictionary<Bitmap, int>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            classifier.InstantiateNetwork(3, 2);
            Console.WriteLine("Instantiated Network!");

            DirectoryInfo dCats = new DirectoryInfo("C:/Users/PixelZerg/Pictures/_Classification/Cats");
            DirectoryInfo dCars = new DirectoryInfo("C:/Users/PixelZerg/Pictures/_Classification/Cars");


            Console.WriteLine("Learning what a cat is...");
            int no = 0;
            foreach (FileInfo f in dCats.GetFiles("*.jpg", SearchOption.TopDirectoryOnly))
            {
                Bitmap bmp = new Bitmap(f.FullName);
                classifier.Train(bmp, 0);
                //Console.Write(".");
                Console.WriteLine("\t" + classifier.trainer.Loss);
                dict.Add(bmp,0);
                if (no > 300)
                {
                    break;
                }
                no++;
            }
            Console.WriteLine("[OK!]");

            Console.WriteLine("Learning what a car is...");
            no = 0;
            foreach (FileInfo f in dCars.GetFiles("*.jpg", SearchOption.TopDirectoryOnly))
            {
                Bitmap bmp = new Bitmap(f.FullName);
                classifier.Train(bmp, 1);
                //Console.Write(".");
                Console.WriteLine("\t" + classifier.trainer.Loss);
                dict.Add(bmp, 1);
                if (no > 300)
                {
                    break;
                }
                no++;
            }
            Console.WriteLine("[OK!]");

            for (int i = 0; i < 5; i++)
            {
                Revise();
            }

            Application.Run(new Display());

            //Classifier c = new Classifier(Classifier.InputFormat.Greyscale, 64, 64);
            //c.InstantiateNetwork(5, 10);
            //Console.WriteLine(string.Join(Environment.NewLine,c.Run(new System.Drawing.Bitmap("C:/Users/PixelZerg/Pictures/pzerg.png"))));
        }

        public static void Revise()
        {
            Console.WriteLine("Revising what cats and cars are...");
            foreach (var pair in dict.OrderRandomly())
            {
                classifier.Train(pair.Key, pair.Value);
                Console.WriteLine(classifier.trainer.Loss);
            }
            Console.WriteLine("[OK!]");
        }

        public static IEnumerable<T> OrderRandomly<T>(this IEnumerable<T> sequence)
        {
            Random random = new Random();
            List<T> copy = sequence.ToList();

            while (copy.Count > 0)
            {
                int index = random.Next(copy.Count);
                yield return copy[index];
                copy.RemoveAt(index);
            }
        }
    }
}
