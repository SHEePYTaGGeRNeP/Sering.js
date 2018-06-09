using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlobStorageAzure;
using CamTimer;
using SHEePYTaGGeRNeP.ConsoleColorWriter;

namespace Webcammer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleWriter.WriteColoredText("Webcammer woohoo!!!", ConsoleColor.Black, ConsoleColor.White);
            foreach (Webcam wc in WebcamManager.Enumerate())
            {
                ConsoleWriter.WriteColoredText(wc.ToString(), ConsoleColor.DarkBlue, ConsoleColor.Yellow);
            }
            Webcam cam2 = WebcamManager.Enumerate().First(x => x.ToString().ToUpper().Contains("LIFE"))
                ?? WebcamManager.Enumerate().First();
            ConsoleWriter.WriteColoredText("Using webcam: " + cam2, ConsoleColor.Green, ConsoleColor.DarkYellow);
            ConsoleWriter.WriteQuestionMessage("Interval in seconds: ", false);
            if (!Double.TryParse(Console.ReadLine(), out var interval))
                interval = 3;
            while (true)
            {
                var pic = cam2.TakePicture();
                var picCompressed = Bitmapper.CompressImage(pic);
                SaveImage(picCompressed, ImageFormat.Jpeg, "webcam.jpg");
                Wait((int)(interval * 1000));
            }
        }

        private static void Wait(int timeInMs)
        {
            int timeLeft = timeInMs;
            int step = timeInMs / 10;
            while (timeLeft > 0)
            {
                ConsoleWriter.WriteText("Time left " + timeLeft + " ms", pWriteTime:false);
                timeLeft -= step;
                Thread.Sleep(step);
            }
        }

        private static void SaveImage(Image image, ImageFormat format, string fileName)
        {
            MemoryStream ms = new MemoryStream();// Convert Image to byte[]
            image.Save(ms, format);
            byte[] imageBytes = ms.ToArray();
            ms.Seek(0, SeekOrigin.Begin);
            File.WriteAllBytes(fileName, imageBytes);
            BlobStorageManagerAzure bsma = new BlobStorageManagerAzure();
            Task.Run(async () =>
            {
                await bsma.PublishFile("images", fileName, ms);
                ms.Dispose();
                ConsoleWriter.WriteColoredText("Uploaded to server!", ConsoleColor.Magenta, ConsoleColor.Cyan);
            });
        }
    }
}
