// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Mogre
{
	public sealed class AxisAlignedBox
	{
		private enum Extent
		{
			Null = 0,
			Finite = 1,
			Infinite = 2,
			EXTENT_NULL = 0,
			EXTENT_FINITE = 1,
			EXTENT_INFINITE = 2
		}

		public enum CornerEnum
		{
			FarLeftBottom = 0,
			FarLeftTop = 1,
			FarRightTop = 2,
			FarRightBottom = 3,
			NearRightTop = 4,
			NearLeftTop = 5,
			NearLeftBottom = 6,
			NearRightBottom = 7,
			FAR_LEFT_BOTTOM = FarLeftBottom,
			FAR_LEFT_TOP = FarLeftTop,
			FAR_RIGHT_TOP = FarRightTop,
			FAR_RIGHT_BOTTOM = FarRightBottom,
			NEAR_RIGHT_BOTTOM = NearRightBottom,
			NEAR_LEFT_BOTTOM = NearLeftBottom,
			NEAR_LEFT_TOP = NearLeftTop,
			NEAR_RIGHT_TOP = NearRightTop
		}

		private Vector3 _minimum;

		private Vector3 _maximum;

		private Extent _extent;

		[NonSerialized]
		private Vector3[] _corners;

		/// <summary>
		/// Gets the half-size of the box.
		/// </summary>
		public Vector3 HalfSize
		{
			get
			{
				if (_extent != Extent.Null)
				{
					if (_extent != Extent.Finite)
					{
						if (_extent != Extent.Infinite)
						{
							throw new Exception("Should never reach here");
						}

						return new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
					}

					return (_maximum - _minimum) * 0.5f;
				}

				return Vector3.Zero;
			}
		}

		/// <summary>
		/// Gets the size of the box.
		/// </summary>
		public Vector3 Size
		{
			get
			{
				if (_extent != Extent.Null)
				{
					if (_extent != Extent.Finite)
					{
						if (_extent != Extent.Infinite)
						{
							throw new Exception("Should never reach here");
						}

						return new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
					}

					return _maximum - _minimum;
				}

				return Vector3.Zero;
			}
		}

		/// <summary>Gets the centre of the box. </summary>
		public Vector3 Center
		{
			get
			{
				if (_extent != AxisAlignedBox.Extent.Finite)
				{
					throw new Exception("Can't get center of a null or infinite AAB");
				}

				return new Vector3(
					_maximum.X + _minimum.X * 0.5f,
					_maximum.Y + _minimum.Y * 0.5f,
					_maximum.Z + _minimum.Z * 0.5f);
			}
		}

		/// <summary>Returns true if the box is infinite. </summary>
		public bool IsInfinite
		{
			get
			{
				return _extent == Extent.Infinite;
			}
		}

		/// <summary>Returns true if the box is finite. </summary>
		public bool IsFinite
		{
			get
			{
				return _extent == Extent.Finite;
			}
		}

		public bool IsNull
		{
			get
			{
				return _extent == Extent.Null;
			}
		}

		/// <summary>
		/// Gets the minimum corner of the box.
		/// </summary>
		public Vector3 Minimum
		{
			get
			{
				return _minimum;
			}
		}

		/// <summary>
		/// Gets the maximum corner of the box.
		/// </summary>
		public Vector3 Maximum
		{
			get
			{
				return _maximum;
			}
		}

		public AxisAlignedBox(float mx, float my, float mz, float Mx, float My, float Mz)
		{
			SetExtents(mx, my, mz, Mx, My, Mz);
		}

		public AxisAlignedBox(Vector3 min, Vector3 max)
		{
			SetExtents(min, max);
		}

		public AxisAlignedBox(AxisAlignedBox rkBox)
		{
			if (rkBox.IsNull)
			{
				SetNull();
			}
			else if (rkBox.IsInfinite)
			{
				SetInfinite();
			}
			else
			{
				SetExtents(rkBox._minimum, rkBox._maximum);
			}
		}

		public AxisAlignedBox()
		{
			SetMinimum(-0.5f, -0.5f, -0.5f);
			SetMaximum(0.5f, 0.5f, 0.5f);
			_extent = Extent.Null;
		}

		public void SetMinimum(float x, float y, float z)
		{
			_extent = Extent.Finite;
			_minimum.X = x;
			_minimum.Y = y;
			_minimum.Z = z;
		}

		/// <summary>Sets the minimum corner of the box. </summary>
		public void SetMinimum(Vector3 vec)
		{
			_extent = Extent.Finite;
			_minimum = vec;
		}

		/// <summary>Changes one of the components of the minimum corner of the box used to resize only one dimension of the box </summary>
		public void SetMinimumX(float x)
		{
			_minimum.X = x;
		}

		public void SetMinimumY(float y)
		{
			_minimum.Y = y;
		}

		public void SetMinimumZ(float z)
		{
			_minimum.Z = z;
		}

		public void SetMaximum(float x, float y, float z)
		{
			_extent = Extent.Finite;
			_maximum.X = x;
			_maximum.Y = y;
			_maximum.Z = z;
		}

		/// <summary>Sets the maximum corner of the box. </summary>
		public void SetMaximum(Vector3 vec)
		{
			_extent = Extent.Finite;
			_maximum = vec;
		}

		/// <summary>Changes one of the components of the maximum corner of the box used to resize only one dimension of the box </summary>
		public void SetMaximumX(float x)
		{
			_maximum.x = x;
		}

		public void SetMaximumY(float y)
		{
			_maximum.y = y;
		}

		public void SetMaximumZ(float z)
		{
			_maximum.z = z;
		}

		public void SetExtents(float mx, float my, float mz, float Mx, float My, float Mz)
		{
			_extent = Extent.Finite;
			_minimum.x = mx;
			_minimum.y = my;
			_minimum.z = mz;
			_maximum.x = Mx;
			_maximum.y = My;
			_maximum.z = Mz;
		}

		/// <summary>Sets both minimum and maximum extents at once. </summary>
		public void SetExtents(Vector3 min, Vector3 max)
		{
			_extent = Extent.Finite;
			_minimum = min;
			_maximum = max;
		}

		public Vector3[] GetAllCorners()
		{
			if (_extent != Extent.Finite)
			{
				throw new Exception("Can't get corners of a null or infinite AAB");
			}

			if (_corners == null)
			{
				_corners = new Vector3[8];
			}

			_corners[0] = _minimum;
			_corners[1].x = _minimum.x;
			_corners[1].y = _maximum.y;
			_corners[1].z = _minimum.z;
			_corners[2].x = _maximum.x;
			_corners[2].y = _maximum.y;
			_corners[2].z = _minimum.z;
			_corners[3].x = _maximum.x;
			_corners[3].y = _minimum.y;
			_corners[3].z = _minimum.z;
			_corners[4] = _maximum;
			_corners[5].x = _minimum.x;
			_corners[5].y = _maximum.y;
			_corners[5].z = _maximum.z;
			_corners[6].x = _minimum.x;
			_corners[6].y = _minimum.y;
			_corners[6].z = _maximum.z;
			_corners[7].x = _maximum.x;
			_corners[7].y = _minimum.y;
			_corners[7].z = _maximum.z;

			return _corners;
		}

		public Vector3 GetCorner(CornerEnum cornerToGet)
		{
			Vector3 result;
			switch (cornerToGet)
			{
				case CornerEnum.FarLeftBottom:
					return _minimum;

				case CornerEnum.FarLeftTop:
					return new Vector3(_minimum.x, _maximum.y, _minimum.z);

				case CornerEnum.FarRightTop:
					return new Vector3(_maximum.x, _maximum.y, _minimum.z);
				case CornerEnum.FarRightBottom:
					{
						Vector3 vector3 = new Vector3(_maximum.x, _minimum.y, _minimum.z);
						result = vector3;
						break;
					}

				case CornerEnum.NearRightTop:
					return _maximum;

				case CornerEnum.NearLeftTop:
					{
						Vector3 vector4 = new Vector3(_minimum.x, _maximum.y, _maximum.z);
						result = vector4;
						break;
					}
				case CornerEnum.NearLeftBottom:
					{
						Vector3 vector5 = new Vector3(_minimum.x, _minimum.y, _maximum.z);
						result = vector5;
						break;
					}
				case CornerEnum.NearRightBottom:
					{
						Vector3 vector6 = new Vector3(_maximum.x, _minimum.y, _maximum.z);
						result = vector6;
						break;
					}
				default:
					return default(Vector3);
			}

			return result;
		}

		public override string ToString()
		{
			if (_extent != Extent.Null)
			{
				if (_extent != Extent.Finite)
				{
					if (_extent != Extent.Infinite)
					{
						throw new Exception("Should never reach here");
					}

					return "AxisAlignedBox(infinite)";
				}

				return "AxisAlignedBox(min=" + _minimum + ", max=" + _maximum + ")";
			}

			return "AxisAlignedBox(null)";
		}

		/// <summary>Extends the box to encompass the specified point (if needed). </summary>
		public void Merge(Vector3 point)
		{
			if (_extent != Extent.Null)
			{
				if (_extent != Extent.Finite)
				{
					if (_extent != Extent.Infinite)
					{
						throw new Exception(" Should never reach here");
					}
				}
				else
				{
					_maximum.MakeCeil(point);
					_minimum.MakeFloor(point);
				}
			}
			else
			{
				SetExtents(point, point);
			}
		}

		/// <summary>
		/// Merges the passed in box into the current box. The result is the box which encompasses both.
		/// </summary>
		public void Merge(AxisAlignedBox rhs)
		{
			if (rhs._extent != Extent.Null &&
				_extent != Extent.Infinite)
			{
				if (rhs._extent == Extent.Infinite)
				{
					_extent = Extent.Infinite;
				}
				else if (_extent == Extent.Null)
				{
					SetExtents(rhs._minimum, rhs._maximum);
				}
				else
				{
					Vector3 min = _minimum;
					Vector3 max = _maximum;
					max.MakeCeil(rhs._maximum);
					min.MakeFloor(rhs._minimum);
					SetExtents(min, max);
				}
			}
		}

		///// <summary>Transforms the box according to the matrix supplied. By calling this method you get the axis-aligned box which surrounds the transformed version of this box. Therefore each corner of the box is transformed by the matrix, then the extents are mapped back onto the axes to produce another AABB. Useful when you have a local AABB for an object which is then transformed. </summary>
		//public void Transform(Matrix4x4 matrix)
		//{
		//    if (this.mExtent == Extent.EXTENT_FINITE)
		//    {
		//        Vector3 vector = default(Vector3);
		//        Vector3 vector2 = default(Vector3);
		//        Vector3 v = default(Vector3);
		//        vector = this.mMinimum;
		//        vector2 = this.mMaximum;
		//        v = vector;
		//        Vector3 point = matrix * v;
		//        this.Merge(point);
		//        v.z = vector2.z;
		//        Vector3 point2 = matrix * v;
		//        this.Merge(point2);
		//        v.y = vector2.y;
		//        Vector3 point3 = matrix * v;
		//        this.Merge(point3);
		//        v.z = vector.z;
		//        Vector3 point4 = matrix * v;
		//        this.Merge(point4);
		//        v.x = vector2.x;
		//        Vector3 point5 = matrix * v;
		//        this.Merge(point5);
		//        v.z = vector2.z;
		//        Vector3 point6 = matrix * v;
		//        this.Merge(point6);
		//        v.y = vector.y;
		//        Vector3 point7 = matrix * v;
		//        this.Merge(point7);
		//        v.z = vector.z;
		//        Vector3 point8 = matrix * v;
		//        this.Merge(point8);
		//    }
		//}

		///// <summary>Transforms the box according to the affine matrix supplied. By calling this method you get the axis-aligned box which surrounds the transformed version of this box. Therefore each corner of the box is transformed by the matrix, then the extents are mapped back onto the axes to produce another AABB. Useful when you have a local AABB for an object which is then transformed. The matrix must be an affine matrix.Matrix4::isAffine. </summary>
		//public void TransformAffine(Matrix4x4 m)
		//{
		//    if (!m.IsAffine)
		//    {
		//        throw new ArgumentException("Matrix should be Affine", "m");
		//    }

		//    if (this.mExtent == AxisAlignedBox.Extent.EXTENT_FINITE)
		//    {
		//        Vector3 center = this.Center;
		//        Vector3 halfSize = this.HalfSize;
		//        Vector3 lvec = m.TransformAffine(center);
		//        Vector3 rvec = new Vector3((float)((double)Math.Abs(m.m00) * (double)halfSize.x + (double)Mogre.Math.Abs(m.m01) * (double)halfSize.y + (double)Mogre.Math.Abs(m.m02) * (double)halfSize.z), (float)((double)Mogre.Math.Abs(m.m10) * (double)halfSize.x + (double)Mogre.Math.Abs(m.m11) * (double)halfSize.y + (double)Mogre.Math.Abs(m.m12) * (double)halfSize.z), (float)((double)Mogre.Math.Abs(m.m20) * (double)halfSize.x + (double)Mogre.Math.Abs(m.m21) * (double)halfSize.y + (double)Mogre.Math.Abs(m.m22) * (double)halfSize.z));
		//        Vector3 max = lvec + rvec;
		//        Vector3 min = lvec - rvec;
		//        this.SetExtents(min, max);
		//    }
		//}

		/// <summary>Sets the box to a 'null' value i.e. not a box. </summary>
		public void SetNull()
		{
			_extent = Extent.Null;
		}

		/// <summary>Sets the box to 'infinite' </summary>
		public void SetInfinite()
		{
			_extent = Extent.Infinite;
		}

		/// <summary>Returns whether or not this box intersects another. </summary>

		public bool Intersects(Vector3 v)
		{
			if (_extent != Extent.Null)
			{
				if (_extent != Extent.Finite)
				{
					if (_extent != Extent.Infinite)
					{
						throw new System.Exception("Should never reach here");
					}
					return true;
				}

				if (v.x >= _minimum.x &&
					v.x <= _maximum.x &&
					v.y >= _minimum.y &&
					v.y <= _maximum.y &&
					v.z >= _minimum.z &&
					v.z <= _maximum.z)
				{
					return true;
				}

				return false;
			}

			return false;
		}

		/// <summary>
		/// Returns whether or not this box intersects another.</summary>
		public bool Intersects(Plane p)
		{
			return Math.Intersects(p, this);
		}

		///// <summary>Returns whether or not this box intersects another. </summary>

		//public bool Intersects(Sphere s)
		//{
		//    return Mogre.Math.Intersects(s, this);
		//}

		/// <summary>
		/// Returns whether or not this box intersects another.
		/// </summary>
		public bool Intersects(AxisAlignedBox other)
		{
			// Early-fail for nulls
			if (IsNull || other.IsNull)
				return false;

			// Early-success for infinites
			if (IsInfinite || other.IsInfinite)
				return true;

			// Use up to 6 separating planes
			if (_maximum.x < other._minimum.x)
				return false;
			if (_maximum.y < other._minimum.y)
				return false;
			if (_maximum.z < other._minimum.z)
				return false;

			if (_minimum.x > other._maximum.x)
				return false;
			if (_minimum.y > other._maximum.y)
				return false;
			if (_minimum.z > other._maximum.z)
				return false;

			// otherwise, must be intersecting
			return true;
		}

		/// <summary>
		/// Calculate the area of intersection of this box and another.
		/// </summary>
		public AxisAlignedBox Intersection(AxisAlignedBox other)
		{
			if (IsNull || other.IsNull)
			{
				return new AxisAlignedBox();
			}
			if (IsInfinite)
			{
				return new AxisAlignedBox(other);
			}

			if (other.IsInfinite)
			{
				return new AxisAlignedBox(this);
			}

			Vector3 min = _minimum;
			Vector3 max = _maximum;
			Vector3 minimum = other.Minimum;
			min.MakeCeil(minimum);
			Vector3 maximum = other.Maximum;
			max.MakeFloor(maximum);
			if (min.x < max.x &&
				min.y < max.y &&
				min.z < max.z)
			{
				return new AxisAlignedBox(min, max);
			}

			return new AxisAlignedBox();
		}

		/// <summary>
		/// Calculate the volume of this box.
		/// </summary>
		public float Volume()
		{
			if (_extent != Extent.Null)
			{
				if (_extent != Extent.Finite)
				{
					if (_extent != Extent.Infinite)
					{
						throw new Exception("Should never reach here");
					}

					return float.PositiveInfinity;
				}

				Vector3 vector = _maximum - _minimum;
				return vector.x * vector.y * vector.z;
			}

			return 0.0f;
		}

		/// <summary>Scales the AABB by the vector given. </summary>
		public void Scale(Vector3 s)
		{
			if (_extent == Extent.Finite)
			{
				Vector3 min = _minimum * s;
				Vector3 max = _maximum * s;
				SetExtents(min, max);
			}
		}

		/// <summary>
		/// Tests whether the given point contained by this box. 
		/// </summary>

		public bool Contains(AxisAlignedBox other)
		{
			if (other.IsNull || IsInfinite)
			{
				return true;
			}
			if (IsNull || other.IsInfinite)
			{
				return false;
			}

			if (_minimum.x <= other._minimum.x &&
			   _minimum.y <= other._minimum.y &&
			   _minimum.z <= other._minimum.z &&
			   other._maximum.x <= _maximum.x &&
			   other._maximum.y <= _maximum.y &&
			   other._maximum.z <= _maximum.z)
			{
				return true;
			}

			return false;
		}

		/// <summary>Tests whether the given point contained by this box. </summary>

		public bool Contains(Vector3 v)
		{
			bool result;
			if (IsNull)
			{
				result = false;
			}
			else if (this.IsInfinite)
			{
				result = true;
			}
			else
			{
				int num;
				if ((double)_minimum.x <= (double)v.x && (double)v.x <= (double)_maximum.x && (double)_minimum.y <= (double)v.y && (double)v.y <= (double)_maximum.y && (double)_minimum.z <= (double)v.z && (double)v.z <= (double)_maximum.z)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				result = (num != 0);
			}
			return result;
		}


		public bool Equals(AxisAlignedBox rhs)
		{
			bool result;
			if (_extent != rhs._extent)
			{
				result = false;
			}
			else if (!this.IsFinite)
			{
				result = true;
			}
			else
			{
				int num;
				if (_minimum == rhs._minimum && _maximum == rhs._maximum)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				result = (num != 0);
			}
			return result;
		}

		public override bool Equals(object obj)
		{
			var axisAlignedBox = obj as AxisAlignedBox;
			return
				axisAlignedBox != null &&
				Equals(axisAlignedBox);
		}

		public override int GetHashCode()
		{
			return _minimum.GetHashCode() ^ _maximum.GetHashCode() ^ _extent.GetHashCode();
		}
	}
}
