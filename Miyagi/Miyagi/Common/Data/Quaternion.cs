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
namespace Miyagi.Common.Data
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    using Miyagi.Internals;

    /// <summary>
    /// Represents a rotation in three dimensions.
    /// </summary>
    [TypeConverter(typeof(QuaternionConverter))]
    public struct Quaternion
    {
        #region Fields

        /// <summary>
        /// Gets a value that represents a static empty Quaternion.
        /// </summary>
        public static readonly Quaternion Empty = new Quaternion(0, 0, 0, 0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="w">The w.</param>
        public Quaternion(float x, float y, float z, float w)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        #endregion Constructors

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the pitch in radians.
        /// </summary>
        public double Pitch
        {
            get
            {
                // from http://bitbucket.org/sinbad/ogre/src/tip/OgreMain/src/OgreQuaternion.cpp
                float fTx = 2.0f * this.X;
                float fTz = 2.0f * this.Z;
                float fTwx = fTx * this.W;
                float fTxx = fTx * this.X;
                float fTyz = fTz * this.Y;
                float fTzz = fTz * this.Z;
                return Math.Atan2(fTyz + fTwx, 1.0f - (fTxx + fTzz));
            }
        }

        /// <summary>
        /// Gets the roll in radians.
        /// </summary>
        public double Roll
        {
            get
            {
                // from http://bitbucket.org/sinbad/ogre/src/tip/OgreMain/src/OgreQuaternion.cpp
                float fTy = 2.0f * this.Y;
                float fTz = 2.0f * this.Z;
                float fTwz = fTz * this.W;
                float fTxy = fTy * this.X;
                float fTyy = fTy * this.Y;
                float fTzz = fTz * this.Z;
                return Math.Atan2(fTxy + fTwz, 1.0f - (fTyy + fTzz));
            }
        }

        /// <summary>
        /// Gets the W component of the quaternion.
        /// </summary>
        public float W
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the X component of the quaternion.
        /// </summary>
        public float X
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Y component of the quaternion.
        /// </summary>
        public float Y
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the yaw in radians.
        /// </summary>
        public double Yaw
        {
            get
            {
                // from http://bitbucket.org/sinbad/ogre/src/tip/OgreMain/src/OgreQuaternion.cpp
                float fTx = 2.0f * this.X;
                float fTy = 2.0f * this.Y;
                float fTz = 2.0f * this.Z;
                float fTwy = fTy * this.W;
                float fTxx = fTx * this.X;
                float fTxz = fTz * this.X;
                float fTyy = fTy * this.Y;
                return Math.Atan2(fTxz + fTwy, 1.0f - (fTxx + fTyy));
            }
        }

        /// <summary>
        /// Gets the Z component of the quaternion.
        /// </summary>
        public float Z
        {
            get;
            private set;
        }

        #endregion Public Properties

        #endregion Properties

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <returns>The result of the operator.</returns>
        public static Quaternion operator -(Quaternion left)
        {
            return new Quaternion(-left.X, -left.Y, -left.Z, -left.W);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from specified yaw, pitch, and roll angles.
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians, around the y-axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the x-axis.</param>
        /// <param name="roll">The roll angle, in radians, around the z-axis.</param>
        /// <returns></returns>
        public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            return new Quaternion(
                (float)(((Math.Cos(yaw * 0.5f) * Math.Sin(pitch * 0.5f)) * Math.Cos(roll * 0.5f)) + ((Math.Sin(yaw * 0.5f) * Math.Cos(pitch * 0.5f)) * Math.Sin(roll * 0.5f))),
                (float)(((Math.Sin(yaw * 0.5f) * Math.Cos(pitch * 0.5f)) * Math.Cos(roll * 0.5f)) - ((Math.Cos(yaw * 0.5f) * Math.Sin(pitch * 0.5f)) * Math.Sin(roll * 0.5f))),
                (float)(((Math.Cos(yaw * 0.5f) * Math.Cos(pitch * 0.5f)) * Math.Sin(roll * 0.5f)) - ((Math.Sin(yaw * 0.5f) * Math.Sin(pitch * 0.5f)) * Math.Cos(roll * 0.5f))),
                (float)(((Math.Cos(yaw * 0.5f) * Math.Cos(pitch * 0.5f)) * Math.Cos(roll * 0.5f)) + ((Math.Sin(yaw * 0.5f) * Math.Sin(pitch * 0.5f)) * Math.Sin(roll * 0.5f))));
        }

        /// <summary>
        /// Creates a new <see cref="Quaternion"/> from specified yaw, pitch, and roll angles.
        /// </summary>
        /// <param name="yaw">The yaw angle, in degrees, around the y-axis.</param>
        /// <param name="pitch">The pitch angle, in degrees, around the x-axis.</param>
        /// <param name="roll">The roll angle, in degrees, around the z-axis.</param>
        /// <returns></returns>
        public static Quaternion CreateFromYawPitchRollDegrees(float yaw, float pitch, float roll)
        {
            yaw = (float)(yaw / (180 / Math.PI));
            pitch = (float)(pitch / (180 / Math.PI));
            roll = (float)(roll / (180 / Math.PI));
            return Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
        }

        /// <summary>
        /// Converts a string representation to a Quaternion instance.
        /// </summary>
        /// <param name="s">A string representation of a Quaternion instance.</param>
        /// <returns>The Quaternion instance.</returns>
        /// <exception cref="FormatException"><c>FormatException</c>.</exception>
        public static Quaternion Parse(string s)
        {
            s = s.Replace("Quaternion(", string.Empty).Replace(")", string.Empty);
            var values = s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 4)
            {
                return new Quaternion(
                    float.Parse(values[0], CultureInfo.InvariantCulture),
                    float.Parse(values[1], CultureInfo.InvariantCulture),
                    float.Parse(values[2], CultureInfo.InvariantCulture),
                    float.Parse(values[3], CultureInfo.InvariantCulture));
            }

            throw new FormatException();
        }

        /// <summary>
        /// Transforms the specified <see cref="Vector3"/>.
        /// </summary>
        /// <param name="value">The vector.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector3 Transform(Vector3 value, Quaternion rotation)
        {
            float x = rotation.X * 2;
            float y = rotation.Y * 2;
            float z = rotation.Z * 2;
            float xx = rotation.X * x;
            float xy = rotation.X * y;
            float xz = rotation.X * z;
            float yy = rotation.Y * y;
            float yz = rotation.Y * z;
            float zz = rotation.Z * z;
            float wx = rotation.W * x;
            float wy = rotation.W * y;
            float wz = rotation.W * z;
            float newX = ((value.X * ((1f - yy) - zz)) + (value.Y * (xy - wz))) + (value.Z * (xz + wy));
            float newY = ((value.X * (xy + wz)) + (value.Y * ((1f - xx) - zz))) + (value.Z * (yz - wx));
            float newZ = ((value.X * (xz - wy)) + (value.Y * (yz + wx))) + (value.Z * ((1f - xx) - yy));

            return new Vector3(newX, newY, newZ);
        }

        /// <summary>
        /// Transforms the specified <see cref="PointF"/>.
        /// </summary>
        /// <param name="value">The vector.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector3 Transform(PointF value, Quaternion rotation)
        {
            if (rotation == Quaternion.Empty)
            {
                return new Vector3(value.X, value.Y, 0);
            }

            float x = rotation.X * 2;
            float y = rotation.Y * 2;
            float z = rotation.Z * 2;
            float xx = rotation.X * x;
            float xy = rotation.X * y;
            float xz = rotation.X * z;
            float yy = rotation.Y * y;
            float yz = rotation.Y * z;
            float zz = rotation.Z * z;
            float wx = rotation.W * x;
            float wy = rotation.W * y;
            float wz = rotation.W * z;
            float newX = (value.X * ((1f - yy) - zz)) + (value.Y * (xy - wz));
            float newY = (value.X * (xy + wz)) + (value.Y * ((1f - xx) - zz));
            float newZ = (value.X * (xz - wy)) + (value.Y * (yz + wx));

            return new Vector3(newX, newY, newZ);
        }

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="Quaternion"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Quaternion"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Quaternion"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Quaternion other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to.</param>
        public override bool Equals(object obj)
        {
            return obj is Quaternion && this.Equals((Quaternion)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode() ^ this.W.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Quaternion({0},{1},{2},{3})", this.X, this.Y, this.Z, this.W);
        }

        #endregion Public Methods

        #endregion Methods

        #region Nested Types

        private sealed class QuaternionConverter : MiyagiTypeConverter<Quaternion>
        {
            #region Methods

            #region Protected Methods

            protected override Quaternion ConvertFromCore(string s)
            {
                return Quaternion.Parse(s);
            }

            protected override string ConvertToCore(Quaternion value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", value.X, value.Y, value.Z, value.W);
            }

            #endregion Protected Methods

            #endregion Methods
        }

        #endregion Nested Types
    }
}