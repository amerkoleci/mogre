using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Mogre
{
	public sealed class TextureManager : ResourceManager
	{
		private static TextureManager _singleton;

		internal TextureManager(IntPtr handle) : base(handle)
		{

		}

		public static TextureManager Singleton
		{
			get
			{
				if(_singleton == null)
				{
					Interlocked.CompareExchange(ref _singleton, new TextureManager(TextureManager_getSingletonPtr()), null);
				}

				return _singleton;
			}
		}

		public int DefaultNumMipmaps
		{
			get { return (int)TextureManager_getDefaultNumMipmaps(_handle); }
			set
			{
				TextureManager_setDefaultNumMipmaps(_handle, (uint)value);
			}
		}

		#region PInvoke

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr TextureManager_getSingletonPtr();

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern uint TextureManager_getDefaultNumMipmaps(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void TextureManager_setDefaultNumMipmaps(IntPtr handle, uint value);

		#endregion PInvoke
	}
}
