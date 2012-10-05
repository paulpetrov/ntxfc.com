using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace FlyingClub.BusinessLogic
{
    public static class ImageHelper
    {
        public static double AspectRatio = (double)3 / (double)2;
        public static Size SmallSize = new Size(144, 96);
        public static Size MediumSize = new Size(350, 233);
        public static Size LargeSize = new Size(800, 533);

        //public List<string> CheckImageSizeErros(string srcUrl)
        //{
        //    List<string> errors = new List<string>();
        //    using (Image srcImage = Image.FromFile(srcUrl))
        //    {
        //        if (srcImage.Width < LargeSize.Width)
        //            errors.Add("Image width must be not less than 800 pxels to ensure good quality. Please select another suitable image.");
        //        double srcRatio = srcImage.Width / srcImage.Height;
        //        if (srcRatio < 1 || srcRatio > 1.5)
        //            errors.Add("Image width to height ratio must be between 1 and 1.5. Resize image or select another one to ensure adequate cropping.");
        //    }

        //    return errors;
        //}

        public static List<string> CreateImageSet(string srcUrl)
        {
            string fileName = String.Empty;
            List<string> errors = new List<string>();
            try
            {
                using (Image srcImage = Image.FromFile(srcUrl))
                {
                    if (srcImage.Width < MediumSize.Width)
                        errors.Add(String.Format("Image width must be not less than {0} pxels to ensure good quality. Please select another suitable image.", MediumSize.Width));

                    if (srcImage.Height < MediumSize.Height)
                        errors.Add(String.Format("Image height must be not less than {0} pxels to ensure good quality. Please select another suitable image.", MediumSize.Height));

                    // crop to required aspect ratio
                    Rectangle cropRectangle = new Rectangle();
                    double srcRatio = (double)srcImage.Width / (double)srcImage.Height;
                    if (srcRatio < 1.3 || srcRatio > 1.7)
                        errors.Add("Image width to height ratio must be between 1.3 and 1.7. Resize image or select another one to ensure adequate cropping.");

                    double targetRatio = AspectRatio;
                    if (srcRatio < targetRatio)
                    {
                        cropRectangle.Width = srcImage.Width;
                        cropRectangle.Height = (int)(srcImage.Width / targetRatio);
                        if (cropRectangle.Height < MediumSize.Height)
                            errors.Add("Image size (height) is not sufficient given aspect ratio. Please resize and/or crop image or select another one with aspect ratio closer to 3:2.");
                    }
                    else if (srcRatio > targetRatio)
                    {
                        cropRectangle.Width = (int)(srcImage.Height * targetRatio);
                        cropRectangle.Height = srcImage.Height;
                        if (cropRectangle.Width < MediumSize.Width)
                            errors.Add("Image size (width) is not sufficient given aspect ratio. Please resize and/or crop image or select another one with aspect ratio closer to 3:2.");
                    }
                    else
                    {
                        cropRectangle.Width = 0;
                    }

                    if(errors.Count > 0)
                        return errors;

                    Image croppedImage = null;
                    if (cropRectangle.Width != 0)
                        croppedImage = CropImage(srcImage, cropRectangle);
                    else
                        croppedImage = srcImage;

                    string srcFileName = Path.GetFileName(srcUrl);
                    string path = Path.GetDirectoryName(srcUrl);
                    fileName = GetSmallFileName(srcFileName);

                    Image smallImage = ResizeImage(croppedImage, SmallSize);
                    smallImage.Save(Path.Combine(path, fileName));

                    fileName = GetMediumFileName(srcFileName);
                    Image mediumImage = ResizeImage(croppedImage, MediumSize);
                    mediumImage.Save(Path.Combine(path, fileName));

                    fileName = GetLargeFileName(srcFileName);
                    Image largeImage = ResizeImage(croppedImage, LargeSize);
                    largeImage.Save(Path.Combine(path, fileName));
                }

                return errors;
            }
            catch (Exception ex)
            {
                string msg = "Error while creating file " + fileName + " from source file at " + srcUrl;
                throw new ApplicationException(msg, ex);
            }
        }

        public static string GetSmallFileName(string origName)
        {
            return Path.GetFileNameWithoutExtension(origName) + ".small" + Path.GetExtension(origName);
        }

        public static string GetMediumFileName(string origName)
        {
            return Path.GetFileNameWithoutExtension(origName) + ".med" + Path.GetExtension(origName);
        }

        public static string GetLargeFileName(string origName)
        {
            return Path.GetFileNameWithoutExtension(origName) + ".lrg" + Path.GetExtension(origName);
        }

        public static Image CropImage(Image image, Rectangle cropRectangle)
        {
            Bitmap srcBitmap = new Bitmap(image);
            Bitmap croppedBitmap = srcBitmap.Clone(cropRectangle, srcBitmap.PixelFormat);

            return (Image)croppedBitmap;
        }

        public static Image ResizeImage(Image srcImage, Size size)
        {
            int sourceWidth = srcImage.Width;
            int sourceHeight = srcImage.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);

            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(srcImage, 0, 0, destWidth, destHeight);
                g.Dispose();
            }

            return (Image)b;
        }

        public static void ScaleToWidth(string srcUrl, string destUrl, int targetWidth)
        {
            using (Image srcImage = Image.FromFile(srcUrl))
            {
                float ratio = ((float)targetWidth) / ((float)srcImage.Width);

                int destWidth = (int)(srcImage.Width * ratio);
                int destHeight = (int)(srcImage.Height * ratio);

                Bitmap b = new Bitmap(destWidth, destHeight);
                using (Graphics g = Graphics.FromImage((Image)b))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.DrawImage(srcImage, 0, 0, destWidth, destHeight);
                    g.Dispose();
                }

                b.Save(destUrl);
            }
        }
    }
}
