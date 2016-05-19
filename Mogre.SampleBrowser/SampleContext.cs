// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Mogre.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Mogre.SampleBrowser
{
    public class SampleContext
    {
        static readonly List<SampleInfo> _samples = new List<SampleInfo>();

        public IEnumerable<SampleInfo> Samples
        {
            get { return _samples; }
        }

        static SampleContext()
        {
            var sampleTypeInfo = typeof(Sample).GetTypeInfo();

            var lookupFolder = Path.GetDirectoryName(typeof(SampleContext).GetTypeInfo().Assembly.Location);

            foreach (var exeFile in Directory.GetFiles(lookupFolder, "*.exe"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(exeFile);
                    foreach (var typeInfo in assembly.DefinedTypes)
                    {
                        if (typeInfo.IsAbstract == false &&
                            sampleTypeInfo.IsAssignableFrom(typeInfo))
                        {
                            var sampleInfoAttribute = typeInfo.GetCustomAttribute<SampleInfoAttribute>();

                            _samples.Add(new SampleInfo(
                                sampleInfoAttribute != null ? sampleInfoAttribute.Name : typeInfo.Name,
                                sampleInfoAttribute != null ? sampleInfoAttribute.Description : typeInfo.Name,
                                sampleInfoAttribute != null ? sampleInfoAttribute.ThumbnailUrl : string.Empty,
                                exeFile));
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

    }

    public sealed class SampleInfo
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public string Executable { get; private set; }

        public BitmapSource Thumbnail { get; private set; }

        public SampleInfo(string name, string description, string thumbnailUrl, string executable)
        {
            Name = name;
            Description = description;
            Executable = executable;

            var thumbBasePath = Path.GetFullPath("../../../Media/thumbnails/");

            if (string.IsNullOrEmpty(thumbnailUrl) == false &&
                File.Exists(Path.Combine(thumbBasePath, thumbnailUrl)))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(Path.Combine(thumbBasePath, thumbnailUrl));
                image.EndInit();

                Thumbnail = image;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
