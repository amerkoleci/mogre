// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Mogre.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SampleInfoAttribute : Attribute
    {
        public string Name { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public string Description { get; private set; }

        public SampleInfoAttribute(string name, string thumbnailUrl = "", string description = "")
        {
            Name = name;
            ThumbnailUrl = thumbnailUrl;
            Description = description;
        }
    }
}
