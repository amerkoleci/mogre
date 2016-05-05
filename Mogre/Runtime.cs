using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogre
{
	class ReferenceHolder<T> where T : class
	{
		public T StrongRef { get; private set; }
		public WeakReference<T> WeakRef { get; private set; }

		public ReferenceHolder(T obj, bool weak)
		{
			if (weak)
			{
				WeakRef = new WeakReference<T>(obj);
			}
			else
			{
				StrongRef = obj;
			}
		}

		public T Reference
		{
			get
			{
				if (StrongRef != null)
					return StrongRef;


				T target;
				WeakRef.TryGetTarget(out target);

				return target;
			}
		}

		/// <summary>
		/// Change Weak to Strong 
		/// </summary>
		public bool MakeStrong()
		{
			if (StrongRef != null)
				return true;

			T strong = null;
			WeakRef?.TryGetTarget(out strong);

			StrongRef = strong;
			WeakRef = null;
			return StrongRef != null;
		}

		/// <summary>
		/// Change Strong to Weak
		/// </summary>
		public bool MakeWeak()
		{
			if (StrongRef != null)
			{
				WeakRef = new WeakReference<T>(StrongRef);
				StrongRef = null;
				return true;
			}

			return false;
		}
	}

	class NativeObjectCache
	{
		readonly Dictionary<IntPtr, ReferenceHolder<OgreNativeObject>> _objects = new Dictionary<IntPtr, ReferenceHolder<OgreNativeObject>>(256); //based on samples (average)

		public int Count => _objects.Count;

		public void Add(OgreNativeObject @object)
		{
			lock (_objects)
			{
				ReferenceHolder<OgreNativeObject> refHolder;
				if (_objects.TryGetValue(@object.NativeHandle, out refHolder))
				{
					refHolder?.Reference?.Dispose();
				}

				_objects[@object.NativeHandle] = new ReferenceHolder<OgreNativeObject>(@object, weak: false);
			}
		}

		public bool Remove(IntPtr ptr)
		{
			lock (_objects)
			{
				return _objects.Remove(ptr);
			}
		}

		public void Clean()
		{
			lock (_objects)
			{
				foreach (var referenceHolder in _objects.ToArray())
				{
					try
					{
						referenceHolder.Value?.Reference?.Dispose();
					}
					catch (Exception exc)
					{
						Debug.WriteLine(exc);
					}
				}

				_objects.Clear();
			}
		}

		public ReferenceHolder<OgreNativeObject> Get(IntPtr ptr)
		{
			lock (_objects)
			{
				ReferenceHolder<OgreNativeObject> @object;
				_objects.TryGetValue(ptr, out @object);
				return @object;
			}
		}
	}

	public static partial class Runtime
	{
		static readonly NativeObjectCache ObjectCache = new NativeObjectCache();

		public static void UnregisterObject(IntPtr handle)
		{
			ObjectCache.Remove(handle);
		}

		public static void RegisterObject(OgreNativeObject @object)
		{
			ObjectCache.Add(@object);
		}

		internal static void Cleanup()
		{
			ObjectCache.Clean();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}

		public static T LookupObject<T>(IntPtr ptr, Func<IntPtr, T> factory) where T : OgreNativeObject
		{
			if (ptr == IntPtr.Zero)
				return default(T);

			var referenceHolder = ObjectCache.Get(ptr);
			var reference = referenceHolder?.Reference;
			if (reference is T) //possible collisions
				return (T)reference;

			var @object = factory(ptr);
			return @object;
		}
	}
}
