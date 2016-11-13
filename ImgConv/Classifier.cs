using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvNetSharp;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ImgConv
{
    public class Classifier
    {
        public enum InputFormat
        {
            Greyscale,
            RGB,
        }

        private Net net = null;
        private Trainer trainer = null;

        public InputFormat format = InputFormat.Greyscale;
        public int width = -1;
        public int height = -1;

        public Classifier(InputFormat format, int imgWidth, int imgHeight)
        {
            this.format = format;
            this.width = imgWidth;
            this.height = imgHeight;
        }

        /// <param name="steps">Defines the amount of convolution layers</param>
        /// <param name="classes">Defines the amount of different outputs</param>
        public void InstantiateNetwork(int steps, int classes)
        {
            this.InstantiateNetwork(GetInputLayer(this.format, this.width, this.height), steps, classes);
        }

        private void InstantiateNetwork(InputLayer input, int steps, int classes)
        {
            this.net = new Net();
            this.trainer = new Trainer(this.net)
            {
                TrainingMethod = Trainer.Method.Adadelta,
            };

            this.net.AddLayer(input);
            for (int i = 0; i < steps; i++)
            {
                this.net.AddLayer(new ConvLayer(5, 5, 16) { Stride = 1, Pad = 2, Activation = Activation.Relu });
                this.net.AddLayer(new PoolLayer(2, 2) { Stride = 2 });
            }
            this.net.AddLayer(new SoftmaxLayer(classes));
            //NB: in new Volume(); 'depth' is image_channels
        }

        private InputLayer GetInputLayer(InputFormat format, int width, int height)
        {
            switch (format)
            {
                case InputFormat.Greyscale:
                    return new InputLayer(width, height, 1);
                case InputFormat.RGB:
                    return new InputLayer(width, height, 3);
            }
            throw new InstantiationException("Switch statement on the input format exited without returning a value.");
        }

        private Volume GetVolumeFromBitmap(Bitmap bmp)
        {
            Bitmap fbmp = this.FormatBitmap(bmp);
            Volume vol = new Volume(fbmp.Width, fbmp.Height, this.GetDepth(this.format), 0.0);

            for (int i = 0; i < fbmp.Width; i++)
            {
                for (int j = 0; j < fbmp.Height; j++)
                {
                    Color pixel = fbmp.GetPixel(i, j);

                    switch (this.format)
                    {
                        case InputFormat.Greyscale:
                            vol.Set(i, j, 0, pixel.GetBrightness());
                            break;
                        case InputFormat.RGB:
                            vol.Set(i, j, 0, pixel.R/255.0);
                            vol.Set(i, j, 1, pixel.G/255.0);
                            vol.Set(i, j, 2, pixel.B/255.0);
                            break;

                        throw new Exception("Switch statement on the input format exited without breaking.");
                    }
                }
            }

            return vol;
        }

        private Bitmap FormatBitmap(Bitmap bmp)
        {
            return ResizeBitmap(bmp, this.width, this.height);
        }

        private int GetDepth(InputFormat format)
        {
            switch (format)
            {
                case InputFormat.Greyscale:
                    return 1;
                case InputFormat.RGB:
                    return 3;
            }
            throw new Exception("Switch statement on the input format exited without returning a value.");
        }

        private Bitmap ResizeBitmap(Bitmap srcImage, int newWidth, int newHeight)
        {
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.None;
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.PixelOffsetMode = PixelOffsetMode.None;
                gr.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
            }
            return newImage;
        }

        public double[] Run(Bitmap bmp)
        {
            //Volume prob = this.net.Forward(this.GetVolumeFromBitmap(bmp), false);
            //return prob.Weights;
            return this.net.Forward(this.GetVolumeFromBitmap(bmp), false).Weights;
        }
    }

    #region exceptions
    [Serializable]
    public class InstantiationException : Exception
    {
        public InstantiationException(string message) : base(message)
        {
        }
    }
    #endregion

}
