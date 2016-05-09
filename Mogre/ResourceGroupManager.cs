using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Mogre
{
	public sealed class ResourceGroupManager : OgreNativeObject
	{
		private static ResourceGroupManager _singleton;

		internal ResourceGroupManager(IntPtr handle) : base(handle)
		{

		}

		public static ResourceGroupManager Singleton
		{
			get
			{
				if(_singleton == null)
				{
					Interlocked.CompareExchange(ref _singleton, new ResourceGroupManager(ResourceGroupManager_getSingletonPtr()), null);
				}

				return _singleton;
			}
		}

		public static string DefaultResourceGroupName
		{
			get
			{
				using(var str = new WrapperString(ResourceGroupManager_DEFAULT_RESOURCE_GROUP_NAME()))
				{
					return str.StringValue;
				}
			}
		}

		public static string InternalResourceGroupName
		{
			get
			{
				using (var str = new WrapperString(ResourceGroupManager_INTERNAL_RESOURCE_GROUP_NAME()))
				{
					return str.StringValue;
				}
			}
		}

		public static string AutoDetectResourceGroupName
		{
			get
			{
				using (var str = new WrapperString(ResourceGroupManager_AUTODETECT_RESOURCE_GROUP_NAME()))
				{
					return str.StringValue;
				}
			}
		}

		public void InitialiseAllResourceGroups()
		{
			ResourceGroupManager_initialiseAllResourceGroups(_handle);
		}

        public void AddResourceLocation(string name,
            string locType,
            string resGroup,
            bool recursive = false,
            bool readOnly = true)
        {
            ResourceGroupManager_addResourceLocation(_handle, name, locType, resGroup, recursive, readOnly);
        }

        public void AddResourceLocation(string name, string locType)
        {
            ResourceGroupManager_addResourceLocation(_handle, name, locType, ResourceGroupManager.DefaultResourceGroupName, false, true);
        }

        #region PInvoke

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr ResourceGroupManager_getSingletonPtr();

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern void ResourceGroupManager_initialiseAllResourceGroups(IntPtr handle);

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr ResourceGroupManager_DEFAULT_RESOURCE_GROUP_NAME();

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr ResourceGroupManager_INTERNAL_RESOURCE_GROUP_NAME();

		[DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr ResourceGroupManager_AUTODETECT_RESOURCE_GROUP_NAME();

        [DllImport(OgreLibrary.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        static extern void ResourceGroupManager_addResourceLocation(IntPtr handle, 
            string name,
            string locType,
            string resGroup,
            [MarshalAs(UnmanagedType.U1)] bool recursive,
            [MarshalAs(UnmanagedType.U1)]bool readOnly);

        #endregion PInvoke
    }
}
