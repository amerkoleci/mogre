// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Mogre
{
    using System;

    public struct ObjectTableOwnershipType
    {
        public Type Type
        {
            get; set;
        }

        public object Owner
        {
            get; set;
        }

        public ObjectTableOwnershipType(object owner, Type type)
        {
            Owner = owner;
            Type = type;
        }
    }
}
