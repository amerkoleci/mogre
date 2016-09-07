// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mogre
{
	public static class Utilities
	{
		/// <summary>
		/// Swaps the value between two references.
		/// </summary>
		/// <typeparam name="T">Type of a data to swap.</typeparam>
		/// <param name="left">The left value.</param>
		/// <param name="right">The right value.</param>
		public static void Swap<T>(ref T left, ref T right)
		{
			var temp = left;
			left = right;
			right = temp;
		}

		/// <summary>
		/// Safely dispose a reference if not null, and set it to null after dispose.
		/// </summary>
		/// <typeparam name="T">The type of COM interface to dispose.</typeparam>
		/// <param name="disposable">Object to dispose.</param>
		/// <remarks>
		/// The reference will be set to null after dispose.
		/// </remarks>
		public static void Dispose<T>(ref T disposable) where T : class, IDisposable
		{
			if (disposable != null)
			{
				disposable.Dispose();
				disposable = null;
			}
		}

		/// <summary>
		/// Transforms an <see cref="IEnumerable{T}"/> to an array of T.
		/// </summary>
		/// <typeparam name="T">Type of the element</typeparam>
		/// <param name="source">The enumerable source.</param>
		/// <returns>an array of T</returns>
		public static T[] ToArray<T>(IEnumerable<T> source)
		{
			return new Buffer<T>(source).ToArray();
		}

		/// <summary>
		/// Read stream to a byte[] buffer.
		/// </summary>
		/// <param name="stream">Input stream.</param>
		/// <returns>A byte[] buffer.</returns>
		public static byte[] ReadStream(Stream stream)
		{
			int readLength = 0;
			return ReadStream(stream, ref readLength);
		}

		/// <summary>
		/// Read stream to a byte[] buffer.
		/// </summary>
		/// <param name="stream">Input stream.</param>
		/// <param name="readLength">Length to read.</param>
		/// <returns>A byte[] buffer.</returns>
		public static byte[] ReadStream(Stream stream, ref int readLength)
		{
			Debug.Assert(stream != null);
			Debug.Assert(stream.CanRead);

			int num = readLength;
			Debug.Assert(num <= (stream.Length - stream.Position));
			if (num == 0)
				readLength = (int)(stream.Length - stream.Position);
			num = readLength;

			Debug.Assert(num >= 0);
			if (num == 0)
				return new byte[0];

			byte[] buffer = new byte[num];
			int bytesRead = 0;
			if (num > 0)
			{
				do
				{
					bytesRead += stream.Read(buffer, bytesRead, readLength - bytesRead);
				} while (bytesRead < readLength);
			}
			return buffer;
		}

		/// <summary>
		/// Compares two collection, element by elements.
		/// </summary>
		/// <param name="left">A "from" enumerator.</param>
		/// <param name="right">A "to" enumerator.</param>
		/// <returns><c>true</c> if lists are identical, <c>false</c> otherwise.</returns>
		public static bool Compare(IEnumerable left, IEnumerable right)
		{
			if (ReferenceEquals(left, right))
				return true;
			if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
				return false;

			return Compare(left.GetEnumerator(), right.GetEnumerator());
		}

		/// <summary>
		/// Compares two collection, element by elements.
		/// </summary>
		/// <param name="leftIt">A "from" enumerator.</param>
		/// <param name="rightIt">A "to" enumerator.</param>
		/// <returns><c>true</c> if lists are identical; otherwise, <c>false</c>.</returns>
		public static bool Compare(IEnumerator leftIt, IEnumerator rightIt)
		{
			if (ReferenceEquals(leftIt, rightIt))
				return true;
			if (ReferenceEquals(leftIt, null) || ReferenceEquals(rightIt, null))
				return false;

			bool hasLeftNext;
			bool hasRightNext;
			while (true)
			{
				hasLeftNext = leftIt.MoveNext();
				hasRightNext = rightIt.MoveNext();
				if (!hasLeftNext || !hasRightNext)
					break;

				if (!Equals(leftIt.Current, rightIt.Current))
					return false;
			}

			// If there is any left element
			if (hasLeftNext != hasRightNext)
				return false;

			return true;
		}

		/// <summary>
		/// Compares two collection, element by elements.
		/// </summary>
		/// <param name="left">The collection to compare from.</param>
		/// <param name="right">The collection to compare to.</param>
		/// <returns><c>true</c> if lists are identical (but not necessarily of the same time); otherwise , <c>false</c>.</returns>
		public static bool Compare(ICollection left, ICollection right)
		{
			if (ReferenceEquals(left, right))
				return true;
			if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
				return false;

			if (left.Count != right.Count)
				return false;

			int count = 0;
			var leftIt = left.GetEnumerator();
			var rightIt = right.GetEnumerator();
			while (leftIt.MoveNext() && rightIt.MoveNext())
			{
				if (!Equals(leftIt.Current, rightIt.Current))
					return false;
				count++;
			}

			// Just double check to make sure that the iterator actually returns
			// the exact number of elements
			if (count != left.Count)
				return false;

			return true;
		}

		/// <summary>
		/// Compute a FNV1-modified Hash from <a href="http://bretm.home.comcast.net/~bretm/hash/6.html">Fowler/Noll/Vo Hash</a> improved version.
		/// </summary>
		/// <param name="data">Data to compute the hash from.</param>
		/// <returns>A hash value.</returns>
		public static int ComputeHashFNVModified(byte[] data)
		{
			const uint p = 16777619;
			uint hash = 2166136261;
			foreach (byte b in data)
				hash = (hash ^ b) * p;
			hash += hash << 13;
			hash ^= hash >> 7;
			hash += hash << 3;
			hash ^= hash >> 17;
			hash += hash << 5;
			return unchecked((int)hash);
		}

		/// <summary>
		/// Gets the custom attribute.
		/// </summary>
		/// <typeparam name="T">Type of the custom attribute.</typeparam>
		/// <param name="memberInfo">The member info.</param>
		/// <param name="inherited">if set to <c>true</c> [inherited].</param>
		/// <returns>The custom attribute or null if not found.</returns>
		public static T GetCustomAttribute<T>(MemberInfo memberInfo, bool inherited = false) where T : Attribute
		{
			return memberInfo.GetCustomAttribute<T>(inherited);
		}

		/// <summary>
		/// Gets the custom attributes.
		/// </summary>
		/// <typeparam name="T">Type of the custom attribute.</typeparam>
		/// <param name="memberInfo">The member info.</param>
		/// <param name="inherited">if set to <c>true</c> [inherited].</param>
		/// <returns>The custom attribute or null if not found.</returns>
		public static IEnumerable<T> GetCustomAttributes<T>(MemberInfo memberInfo, bool inherited = false) where T : Attribute
		{
			return memberInfo.GetCustomAttributes<T>(inherited);
		}

		/// <summary>
		/// Determines whether fromType can be assigned to toType.
		/// </summary>
		/// <param name="toType">To type.</param>
		/// <param name="fromType">From type.</param>
		/// <returns>
		/// <c>true</c> if [is assignable from] [the specified to type]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAssignableFrom(Type toType, Type fromType)
		{
			return toType.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
		}

		/// <summary>
		/// Determines whether the specified type to test is an enum.
		/// </summary>
		/// <param name="typeToTest">The type to test.</param>
		/// <returns>
		/// <c>true</c> if the specified type to test is an enum; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsEnum(Type typeToTest)
		{
			return typeToTest.GetTypeInfo().IsEnum;
		}

		/// <summary>
		/// Determines whether the specified type to test is a value type.
		/// </summary>
		/// <param name="typeToTest">The type to test.</param>
		/// <returns>
		/// <c>true</c> if the specified type to test is a value type; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValueType(Type typeToTest)
		{
			return typeToTest.GetTypeInfo().IsValueType;
		}

		/// <summary>
		/// Allocate an aligned memory buffer.
		/// </summary>
		/// <param name="sizeInBytes">Size of the buffer to allocate.</param>
		/// <param name="align">Alignment, 16 bytes by default.</param>
		/// <returns>A pointer to a buffer aligned.</returns>
		/// <remarks>
		/// To free this buffer, call <see cref="FreeMemory"/>.
		/// </remarks>
		public unsafe static IntPtr AllocateMemory(int sizeInBytes, int align = 16)
		{
			int mask = align - 1;
			if ((align & mask) != 0)
			{
				throw new ArgumentException("Alignment is not power of 2", "align");
			}
			var memPtr = Marshal.AllocHGlobal(sizeInBytes + mask + sizeof(void*));
			byte* ptr = (byte*)((ulong)(memPtr + sizeof(void*) + mask) & ~(ulong)mask);
			((IntPtr*)ptr)[-1] = memPtr;
			return new IntPtr(ptr);
		}

		/// <summary>
		/// Determines whether the specified memory pointer is aligned in memory.
		/// </summary>
		/// <param name="memoryPtr">The memory pointer.</param>
		/// <param name="align">The align.</param>
		/// <returns><c>true</c> if the specified memory pointer is aligned in memory; otherwise, <c>false</c>.</returns>
		public static bool IsMemoryAligned(IntPtr memoryPtr, int align = 16)
		{
			return ((memoryPtr.ToInt64() & (align - 1)) == 0);
		}

		/// <summary>
		/// Allocate an aligned memory buffer.
		/// </summary>
		/// <returns>A pointer to a buffer aligned.</returns>
		/// <remarks>
		/// The buffer must have been allocated with <see cref="AllocateMemory"/>.
		/// </remarks>
		public unsafe static void FreeMemory(IntPtr alignedBuffer)
		{
			if (alignedBuffer == IntPtr.Zero) return;
			Marshal.FreeHGlobal(((IntPtr*)alignedBuffer)[-1]);
		}

		internal struct Buffer<TElement>
		{
			internal TElement[] items;
			internal int count;

			internal Buffer(IEnumerable<TElement> source)
			{
				var array = (TElement[])null;
				int length = 0;
				var collection = source as ICollection<TElement>;
				if (collection != null)
				{
					length = collection.Count;
					if (length > 0)
					{
						array = new TElement[length];
						collection.CopyTo(array, 0);
					}
				}
				else
				{
					foreach (TElement element in source)
					{
						if (array == null)
							array = new TElement[4];
						else if (array.Length == length)
						{
							var elementArray = new TElement[checked(length * 2)];
							Array.Copy(array, 0, elementArray, 0, length);
							array = elementArray;
						}
						array[length] = element;
						++length;
					}
				}
				items = array;
				count = length;
			}

			internal TElement[] ToArray()
			{
				if (count == 0)
					return new TElement[0];
				if (items.Length == count)
					return items;
				var elementArray = new TElement[count];
				Array.Copy(items, 0, elementArray, 0, count);
				return elementArray;
			}
		}
	}
}
