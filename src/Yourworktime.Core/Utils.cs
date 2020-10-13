using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace Yourworktime.Core
{
    public static class Utils
    {
        private static Random random = new Random();

        internal static string ComputeSha256Hash(string rawData)
        { 
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString());
                }
                return builder.ToString();
            }
        }

        internal static string GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < salt.Length; i++)
            {
                builder.Append(salt[i].ToString());
            }
            return builder.ToString();
        }

        public static Bitmap CreateProfileImage(string initials, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);

            using Graphics graphics = Graphics.FromImage(bitmap);
            using (SolidBrush brush = new SolidBrush(ColorFromHSV(random.Next(0, 360), 0.8, 0.8)))
            graphics.FillRectangle(brush, 0, 0, width, height);

            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                using Font arialFont = new Font("Arial", 38);
                graphics.DrawString(initials, arialFont, Brushes.White, new Rectangle(0, 0, width, height), sf);
            }

            return bitmap;
        }

        public static void CreateAndSaveProfileImage(string initials, int width, int height, string path)
        {
            CreateProfileImage(initials, width, height).Save(path);
        }

        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }

    internal static class Extensions
    {
        public static string UppercaseFirst(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
