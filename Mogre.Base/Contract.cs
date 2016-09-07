// Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
	using System;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// Helper class for validation condition and throwing exceptions.
	/// </summary>
	public static class Contract
	{
		/// <summary>
		/// If <paramref name="condition"/> is false, throw the exception type specified by the generic T</summary>
		/// <typeparam name="TException">The <see cref="Exception"/> derived type to throw if <paramref name = "condition"/> is false</typeparam>
		/// <param name="condition">The 'condition' to evaluate</param>
		public static void Requires<TException>(bool condition) where TException : Exception, new()
		{
			if (!condition)
			{
				throw new TException();
			}
		}

		/// <summary>
		/// If <paramref name="condition"/> is false, throw the exception type specified by the generic T</summary>
		/// <typeparam name="TException">The <see cref="Exception"/> derived type to throw if <paramref name = "condition"/> is false</typeparam>
		/// <param name="condition">The 'condition' to evaluate</param>
		/// <param name="message">The <see cref="Exception.Message"/> if <paramref name="condition"/> is false</param>
		public static void Requires<TException>(bool condition, string message) where TException : Exception
		{
			if (!condition)
			{
				throw (TException)Activator.CreateInstance(typeof(TException), message);
			}
		}

		/// <summary>
		/// Throws an <see cref="ArgumentNullException"/> if the
		/// provided object is null</summary>
		/// <param name="obj">The object to test for null</param>
		/// <param name="message">The exception message</param>
		public static void NotNull(object obj, string message)
		{
			Requires<ArgumentNullException>((obj != null), message);
		}

		/// <summary>
		/// Throws an <see cref="ArgumentNullException"/> if the provided string is null.
		/// Throws an <see cref="ArgumentOutOfRangeException"/> if the provided string is empty.</summary>
		/// <param name="stringParameter">The object to test for null and empty</param>
		/// <param name="message">The exception message</param>
		public static void NotNullOrEmpty(string stringParameter, string message)
		{
			NotNull(stringParameter, message);
			Requires<ArgumentOutOfRangeException>((stringParameter != string.Empty), message);
		}

		/// <summary>
		/// Checks an argument to ensure it isn't null.
		/// </summary>
		/// <param name="argumentValue">The argument value to check.</param>
		/// <param name="argumentName">The name of the argument.</param>
		/// <exception cref="ArgumentNullException"><paramref name="argumentValue"/> is a null reference.</exception>
		public static void ArgumentNotNull(object argumentValue, string argumentName)
		{
			if (argumentValue == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
	}
}
