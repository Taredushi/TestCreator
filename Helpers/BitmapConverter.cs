using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TestCreator.Helpers
{
    public class BitmapConverter
    {
        public static byte[] ConvertBitmapToBytes(BitmapImage imageC)
        {
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public static BitmapImage ConvertBytesToBitmap(byte[] imageData)
        {
            if (imageData == null || (imageData?[0].Equals(0)??false)) return null;

            using (var ms = new System.IO.MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }


    }
}
