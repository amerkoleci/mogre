// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ObjectTable
    {
        public static event EventHandler<ObjectTableEventArgs> ObjectAdded;
        public static event EventHandler<ObjectTableEventArgs> ObjectRemoved;

        // A collection of native objects to their managed version
        static readonly Dictionary<long, object> _objectTable = new Dictionary<long, object>();
        // A collection of managed objects to their managed owner
        static readonly Dictionary<IDisposable, IDisposable> _ownership = new Dictionary<IDisposable, IDisposable>();
        // A collection of ownership-type pairs to a collection of objects
        // This dictionary is used to lookup objects which are owned by X and of type Y. (e.g. property Physics.Cloths > Key: Owner: physics, Type: Cloth yields a collection of Cloth).
        static readonly Dictionary<ObjectTableOwnershipType, List<object>> _ownerTypeLookup = new Dictionary<ObjectTableOwnershipType, List<object>>();

        static bool _performingDisposal;

        public static void Add<T>(long pointer, T @object, IDisposable owner = null) where T : IDisposable
        {
            if (@object == null)
            {
                throw new ArgumentNullException("Invalid pointer added to Object Table", nameof(owner));
            }

            EnsureUnmanagedObjectIsOnlyWrappedOnce(pointer, @object.GetType());
            if (owner != null)
            {
                AddObjectOwner(@object, owner);
                AddOwnerTypeLookup<T>(owner, @object);
            }

            try
            {
                _objectTable.Add(pointer, @object);
            }
            catch (Exception)
            {
                throw;
            }

            ObjectAdded?.Invoke(null, new ObjectTableEventArgs(pointer, @object));
        }

        public static void AddObjectOwner(IDisposable @object, IDisposable owner)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            @object.OnDisposing += disposableObject_OnDisposing;
            _ownership.Add(@object, owner);
        }

        public static void AddOwnerTypeLookup<T>(object owner, T @object) where T : IDisposable
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            Type type = typeof(T);
            var objectTableOwnershipType = new ObjectTableOwnershipType(owner, type);
            var key = objectTableOwnershipType;
            if (!_ownerTypeLookup.ContainsKey(key))
            {
                _ownerTypeLookup.Add(key, new List<object>());
            }
            _ownerTypeLookup[key].Add(@object);
        }

        public static void Clear()
        {
            _objectTable.Clear();
            _ownership.Clear();
            _ownerTypeLookup.Clear();
        }

        public static bool Contains(object @object)
        {
            return _objectTable.ContainsValue(@object);
        }

        public static bool Contains(long pointer)
        {
            return _objectTable.ContainsKey(pointer);
        }

        public static void EnsureUnmanagedObjectIsOnlyWrappedOnce(long unmanaged, Type managedType)
        {
            if (_objectTable.ContainsKey(unmanaged))
            {
                object obj = _objectTable[unmanaged];
                if (obj.GetType() == managedType)
                {
                    throw new InvalidOperationException(string.Format("There is already a managed instance of type '{0}' wrapping this unmanaged object. Instead, retrieve the managed object from the ObjectTable using the unmanaged pointer as the lookup key.", managedType.FullName));
                }
            }
        }

        public static bool Remove(object @object)
        {
            foreach (var pair in _objectTable)
            {
                if (pair.Value == @object)
                {
                    return Remove(pair.Key);
                }
            }

            return false;
        }

        public static bool Remove(long pointer)
        {
            object @object = _objectTable[pointer];

            // Unbind the OnDisposing event
            var disposableObject = @object as Mogre.IDisposable;
            if (disposableObject != null)
            {
                disposableObject.OnDisposing -= disposableObject_OnDisposing;
            }

            // Remove from the pointer-object dictionary
            bool result = _objectTable.Remove(pointer);

            // Remove the from owner-type dictionary
            if (disposableObject != null)
            {
                IDisposable owner = _ownership[disposableObject];

                var ownerTypeKey = new ObjectTableOwnershipType(owner, @object.GetType());

                if (_ownerTypeLookup.ContainsKey(ownerTypeKey))
                {
                    _ownerTypeLookup[ownerTypeKey].Remove(@object);

                    if (_ownerTypeLookup[ownerTypeKey].Count == 0)
                        _ownerTypeLookup.Remove(ownerTypeKey);
                }
            }

            // Remove from the ownership dictionary
            if (disposableObject != null)
            {
                _ownership.Remove(disposableObject);
            }

            // Raise event
            ObjectRemoved?.Invoke(null, new ObjectTableEventArgs(pointer, @object));
            return result;
        }

        public static T TryGetObject<T>(long pointer)
        {
            if (_objectTable.ContainsKey(pointer) == false)
            {
                return default(T);
            }

            return (T)(_objectTable[pointer]);
        }

        public static object TryGetObject(long pointer)
        {
            return TryGetObject<object>(pointer);
        }

        public static object GetOrCreateObject(long pointer)
        {
            return GetOrCreateObject<object>(pointer);
        }

        public static T GetOrCreateObject<T>(long pointer)
        {
            if (pointer == 0L)
            {
                return default(T);
            }

            object result;
            if (!_objectTable.TryGetValue(pointer, out result))
            {
                object[] args = { pointer };
                object @object = Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, args, null);
                _objectTable.Add(pointer, @object);
                result = @object;
            }
            else
            {
                result = _objectTable[pointer];
            }

            return (T)result;
        }

        public static long GetObject(object @object)
        {
            foreach (var pair in _objectTable)
            {
                if (pair.Value == @object)
                {
                    return pair.Key;
                }
            }

            throw new ArgumentException("Cannot find the unmanaged object");
        }
        public static T GetObject<T>(long pointer)
        {
            if (pointer == 0L)
            {
                return default(T);
            }

            if (!_objectTable.ContainsKey(pointer))
            {
                throw new ArgumentException(string.Format("Cannot find managed object with pointer address '{0}' (of type '{1}')", pointer, typeof(T).FullName));
            }

            return (T)(_objectTable[pointer]);
        }

        public static object GetObject(long pointer)
        {
            return GetObject<object>(pointer);
        }

        public static IEnumerable<T> GetObjectsOfOwnerAndType<T>(object owner) where T : class
        {
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            var key = new ObjectTableOwnershipType(owner, typeof(T));
            if (!_ownerTypeLookup.ContainsKey(key))
            {
                return new T[0];
            }

            List<object> items = _ownerTypeLookup[key];
            return items.Cast<T>().ToArray<T>();
        }

        public static T[] GetObjectsOfType<T>() where T : class
        {
            List<T> objects = new List<T>();
            foreach (var obj in _objectTable.Values)
            {
                if (obj.GetType() == typeof(T))
                {
                    objects.Add((T)obj);
                }
            }

            return objects.ToArray();
        }


        static void disposableObject_OnDisposing(object sender, EventArgs e)
        {
            if (!_performingDisposal)
            {
                _performingDisposal = true;
                DisposeOfObjectAndDependents(sender as IDisposable);
                Remove(sender);
                _performingDisposal = false;
            }
        }

        static void GetDependents(IDisposable disposable, List<IDisposable> disposables)
        {
            foreach (var pair in _ownership)
            {
                IDisposable dependent = pair.Key;
                if (dependent != null)
                {
                    if (pair.Value == disposable)
                    {
                        GetDependents(dependent, disposables);
                        disposables.Add(dependent);
                    }
                }
            }
        }

        static IDisposable[] GetDependents(IDisposable disposable)
        {
            List<IDisposable> allDependents = new List<IDisposable>();
            GetDependents(disposable, allDependents);
            return allDependents.ToArray();
        }

        static void DisposeOfObjectAndDependents(IDisposable disposable)
        {
            if (disposable != null &&
                disposable.IsDisposed == false &&
                _ownership.ContainsKey(disposable))
            {
                IDisposable[] dependents = GetDependents(disposable);
                for (int i = 0; i < dependents.Length; i++)
                {
                    IDisposable dependent = dependents[i];
                    if (dependent != null)
                    {
                        dependent.Dispose();
                    }
                }

                if (disposable != null)
                {
                    disposable.Dispose();
                }

                for (int j = 0; j < dependents.Length; j++)
                {
                    IDisposable dependent = dependents[j];
                    Remove(dependent);
                }
            }
        }
    }
}
