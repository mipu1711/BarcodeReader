using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BarcodeReader
{
    public static class ImageProcessor
    {
        /// <summary>
        /// Tăng độ tương phản của ảnh
        /// </summary>
        public static Bitmap EnhanceContrast(Bitmap original, float contrastLevel)
        {
            try
            {
                Bitmap result = new Bitmap(original.Width, original.Height);
                
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        Color originalColor = original.GetPixel(x, y);
                        
                        // Tăng độ tương phản
                        int r = Clamp((int)((originalColor.R - 128) * contrastLevel + 128));
                        int g = Clamp((int)((originalColor.G - 128) * contrastLevel + 128));
                        int b = Clamp((int)((originalColor.B - 128) * contrastLevel + 128));
                        
                        result.SetPixel(x, y, Color.FromArgb(originalColor.A, r, g, b));
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi tăng độ tương phản: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        /// <summary>
        /// Làm nét ảnh
        /// </summary>
        public static Bitmap SharpenImage(Bitmap original, float sharpenLevel)
        {
            try
            {
                // Kernel làm nét
                float[,] kernel = {
                    { 0, -sharpenLevel, 0 },
                    { -sharpenLevel, 1 + 4 * sharpenLevel, -sharpenLevel },
                    { 0, -sharpenLevel, 0 }
                };
                
                return ApplyConvolution(original, kernel);
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi làm nét ảnh: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        /// <summary>
        /// Điều chỉnh độ sáng
        /// </summary>
        public static Bitmap AdjustBrightness(Bitmap original, float brightnessLevel)
        {
            try
            {
                Bitmap result = new Bitmap(original.Width, original.Height);
                int brightness = (int)(brightnessLevel * 255);
                
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        Color originalColor = original.GetPixel(x, y);
                        
                        int r = Clamp(originalColor.R + brightness);
                        int g = Clamp(originalColor.G + brightness);
                        int b = Clamp(originalColor.B + brightness);
                        
                        result.SetPixel(x, y, Color.FromArgb(originalColor.A, r, g, b));
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi điều chỉnh độ sáng: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        /// <summary>
        /// Gamma correction
        /// </summary>
        public static Bitmap ApplyGammaCorrection(Bitmap original, float gamma)
        {
            try
            {
                Bitmap result = new Bitmap(original.Width, original.Height);
                
                // Tạo bảng gamma lookup
                byte[] gammaTable = new byte[256];
                for (int i = 0; i < 256; i++)
                {
                    gammaTable[i] = (byte)Clamp((int)(255 * Math.Pow(i / 255.0, gamma)));
                }
                
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        Color originalColor = original.GetPixel(x, y);
                        
                        int r = gammaTable[originalColor.R];
                        int g = gammaTable[originalColor.G];
                        int b = gammaTable[originalColor.B];
                        
                        result.SetPixel(x, y, Color.FromArgb(originalColor.A, r, g, b));
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi áp dụng gamma correction: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        /// <summary>
        /// Áp dụng blur với gaussian kernel
        /// </summary>
        public static Bitmap ApplyGaussianBlur(Bitmap original, float radius)
        {
            try
            {
                int kernelSize = (int)(radius * 2) + 1;
                float[,] kernel = CreateGaussianKernel(kernelSize, radius);
                return ApplyConvolution(original, kernel);
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi áp dụng blur: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        /// <summary>
        /// Chuyển đổi thành ảnh đen trắng với threshold
        /// </summary>
        public static Bitmap ApplyBinaryThreshold(Bitmap original, int threshold)
        {
            try
            {
                Bitmap result = new Bitmap(original.Width, original.Height);
                
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        Color originalColor = original.GetPixel(x, y);
                        int grayValue = (int)(originalColor.R * 0.299 + originalColor.G * 0.587 + originalColor.B * 0.114);
                        
                        Color newColor = grayValue >= threshold ? Color.White : Color.Black;
                        result.SetPixel(x, y, newColor);
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi áp dụng binary threshold: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        /// <summary>
        /// Morphological operations - Erosion
        /// </summary>
        public static Bitmap ApplyErosion(Bitmap original, int kernelSize)
        {
            try
            {
                Bitmap result = new Bitmap(original.Width, original.Height);
                int offset = kernelSize / 2;
                
                for (int x = offset; x < original.Width - offset; x++)
                {
                    for (int y = offset; y < original.Height - offset; y++)
                    {
                        bool isBlack = false;
                        
                        // Kiểm tra kernel
                        for (int kx = -offset; kx <= offset && !isBlack; kx++)
                        {
                            for (int ky = -offset; ky <= offset && !isBlack; ky++)
                            {
                                Color pixelColor = original.GetPixel(x + kx, y + ky);
                                if (pixelColor.R < 128) // Pixel đen
                                {
                                    isBlack = true;
                                }
                            }
                        }
                        
                        result.SetPixel(x, y, isBlack ? Color.Black : Color.White);
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi áp dụng erosion: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        /// <summary>
        /// Morphological operations - Dilation
        /// </summary>
        public static Bitmap ApplyDilation(Bitmap original, int kernelSize)
        {
            try
            {
                Bitmap result = new Bitmap(original.Width, original.Height);
                int offset = kernelSize / 2;
                
                for (int x = offset; x < original.Width - offset; x++)
                {
                    for (int y = offset; y < original.Height - offset; y++)
                    {
                        bool hasWhite = false;
                        
                        // Kiểm tra kernel
                        for (int kx = -offset; kx <= offset && !hasWhite; kx++)
                        {
                            for (int ky = -offset; ky <= offset && !hasWhite; ky++)
                            {
                                Color pixelColor = original.GetPixel(x + kx, y + ky);
                                if (pixelColor.R >= 128) // Pixel trắng
                                {
                                    hasWhite = true;
                                }
                            }
                        }
                        
                        result.SetPixel(x, y, hasWhite ? Color.White : Color.Black);
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi áp dụng dilation: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        /// <summary>
        /// Histogram equalization
        /// </summary>
        public static Bitmap ApplyHistogramEqualization(Bitmap original)
        {
            try
            {
                Bitmap result = new Bitmap(original.Width, original.Height);
                
                // Tính histogram
                int[] histogram = new int[256];
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        Color color = original.GetPixel(x, y);
                        int grayValue = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                        histogram[grayValue]++;
                    }
                }
                
                // Tính cumulative distribution function
                int[] cdf = new int[256];
                cdf[0] = histogram[0];
                for (int i = 1; i < 256; i++)
                {
                    cdf[i] = cdf[i - 1] + histogram[i];
                }
                
                int totalPixels = original.Width * original.Height;
                
                // Áp dụng equalization
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        Color originalColor = original.GetPixel(x, y);
                        int grayValue = (int)(originalColor.R * 0.299 + originalColor.G * 0.587 + originalColor.B * 0.114);
                        
                        int newValue = (int)(255.0 * cdf[grayValue] / totalPixels);
                        newValue = Clamp(newValue);
                        
                        result.SetPixel(x, y, Color.FromArgb(originalColor.A, newValue, newValue, newValue));
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi áp dụng histogram equalization: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return new Bitmap(original);
            }
        }

        #region Helper Methods

        private static int Clamp(int value)
        {
            return Math.Max(0, Math.Min(255, value));
        }

        private static Bitmap ApplyConvolution(Bitmap original, float[,] kernel)
        {
            Bitmap result = new Bitmap(original.Width, original.Height);
            int kernelWidth = kernel.GetLength(1);
            int kernelHeight = kernel.GetLength(0);
            int offsetX = kernelWidth / 2;
            int offsetY = kernelHeight / 2;

            for (int x = offsetX; x < original.Width - offsetX; x++)
            {
                for (int y = offsetY; y < original.Height - offsetY; y++)
                {
                    float r = 0, g = 0, b = 0;

                    for (int kx = 0; kx < kernelWidth; kx++)
                    {
                        for (int ky = 0; ky < kernelHeight; ky++)
                        {
                            int pixelX = x + kx - offsetX;
                            int pixelY = y + ky - offsetY;
                            
                            Color pixelColor = original.GetPixel(pixelX, pixelY);
                            float kernelValue = kernel[ky, kx];
                            
                            r += pixelColor.R * kernelValue;
                            g += pixelColor.G * kernelValue;
                            b += pixelColor.B * kernelValue;
                        }
                    }

                    Color originalColor = original.GetPixel(x, y);
                    result.SetPixel(x, y, Color.FromArgb(originalColor.A, 
                        Clamp((int)r), Clamp((int)g), Clamp((int)b)));
                }
            }

            return result;
        }

        private static float[,] CreateGaussianKernel(int size, float sigma)
        {
            float[,] kernel = new float[size, size];
            int offset = size / 2;
            float total = 0;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int distanceX = x - offset;
                    int distanceY = y - offset;
                    float value = (float)(1.0 / (2 * Math.PI * sigma * sigma) * 
                        Math.Exp(-(distanceX * distanceX + distanceY * distanceY) / (2 * sigma * sigma)));
                    kernel[y, x] = value;
                    total += value;
                }
            }

            // Normalize kernel
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    kernel[y, x] /= total;
                }
            }

            return kernel;
        }

        #endregion
    }
}