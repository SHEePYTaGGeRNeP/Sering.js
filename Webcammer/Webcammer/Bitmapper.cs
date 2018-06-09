using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webcammer
{
    class Bitmapper
    {
        public static Bitmap CreateCompressedScreenshot()
        {
            Bitmap screenShot = TakeScreenshot();
            screenShot = CompressImage(screenShot);
            return screenShot;
        }

        private static Bitmap TakeScreenshot()
        {
            Rectangle totalSize = Rectangle.Empty;

            foreach (Screen s in Screen.AllScreens)
                totalSize = Rectangle.Union(totalSize, s.Bounds);

            Bitmap screenShotBMP = new Bitmap(totalSize.Width, totalSize.Height, PixelFormat.
                Format32bppArgb);

            Graphics screenShotGraphics = Graphics.FromImage(screenShotBMP);

            screenShotGraphics.CopyFromScreen(totalSize.X, totalSize.Y, 0, 0, totalSize.Size,
                CopyPixelOperation.SourceCopy);

            screenShotGraphics.Dispose();

            return screenShotBMP;
        }


        public static Bitmap CompressImage(Bitmap image)
        {
            //Or you do can use buil-in method
            //ImageCodecInfo jgpEncoder GetEncoderInfo("image/gif");//"image/jpeg",...
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            MemoryStream ms = new MemoryStream();
            image.Save(ms, jgpEncoder, myEncoderParameters);
            ms.Seek(0, SeekOrigin.Begin);
            Bitmap returnImage = new Bitmap(ms);
            return returnImage;
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}
