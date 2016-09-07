/*
 Miyagi v1.2.1
 Copyright (c) 2008 - 2012 Tobias Bohnen

 Permission is hereby granted, free of charge, to any person obtaining a copy of this
 software and associated documentation files (the "Software"), to deal in the Software
 without restriction, including without limitation the rights to use, copy, modify, merge,
 publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 to whom the Software is furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all copies or
 substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 DEALINGS IN THE SOFTWARE.
 */
namespace Miyagi.Internals
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    internal static class ExtensionMethods
    {
        #if USE_SYSTEM_DRAWING

        [SecurityPermission(SecurityAction.LinkDemand)]
        public static byte[] ToByteArray(this System.Drawing.Bitmap self)
        {
            var data = self.LockBits(
                new System.Drawing.Rectangle(System.Drawing.Point.Empty, self.Size),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int byteCount = self.Height * self.Width * 4;
            var bmpBytes = new byte[byteCount];

            Marshal.Copy(data.Scan0, bmpBytes, 0, byteCount);
            self.UnlockBits(data);

            return bmpBytes;
        }

        #endif

        #region Methods

        #region Public Static Methods

        public static T Clamp<T>(this T val, T min, T max)
            where T : IComparable<T>
        {
            return val.CompareTo(min) < 0 ? min : val.CompareTo(max) > 0 ? max : val;
        }

        public static T GetAttribute<T>(this PropertyDescriptor self)
            where T : Attribute
        {
            return (T)self.Attributes[typeof(T)];
        }

        public static T GetAttribute<T>(this PropertyInfo self)
            where T : Attribute
        {
            var atts = self.GetCustomAttributes(typeof(T), true);
            if (atts.Length > 0)
            {
                return (T)atts[0];
            }

            return null;
        }

        public static bool HasAttribute<T>(this Type self)
            where T : Attribute
        {
            return self.GetCustomAttributes(typeof(T), true).Length > 0;
        }

        public static bool IsFlagSet(this Enum self, Enum matchTo)
        {
            uint i = Convert.ToUInt32(matchTo);
            return i == (Convert.ToUInt32(self) & i);
        }

        public static int NextPowerOfTwo(this double self)
        {
            return (int)Math.Pow(2, Math.Ceiling(Math.Log(self, 2)));
        }

        public static T ParseEnum<T>(this string self)
        {
            return (T)Enum.Parse(typeof(T), self);
        }

        public static string ReplaceFirst(this string self, string oldValue, string newValue)
        {
            int pos = self.IndexOf(oldValue);
            if (pos < 0)
            {
                return self;
            }

            return self.Substring(0, pos) + newValue + self.Substring(pos + oldValue.Length);
        }

        public static Version ToVersion(this string self)
        {
            string[] s = self.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            int major = 0, minor = 0, build = 0;

            if (s.Length > 0)
            {
                major = int.Parse(s[0]);
            }

            if (s.Length > 1)
            {
                minor = int.Parse(s[1]);
            }

            if (s.Length > 2)
            {
                build = int.Parse(s[2]);
            }

            return new Version(major, minor, build);
        }

        #endregion Public Static Methods

        #endregion Methods
    }
}