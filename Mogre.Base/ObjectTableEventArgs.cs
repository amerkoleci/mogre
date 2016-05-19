// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
    using System;

    public class ObjectTableEventArgs : EventArgs
    {
        public object ManagedObject
        {
            get; set;
        }

        public long UnmanagedObject
        {
            get; set;
        }

        public ObjectTableEventArgs(long unmanagedObject, object managedObject)
        {
            UnmanagedObject = unmanagedObject;
            ManagedObject = managedObject;
        }
    }
}
