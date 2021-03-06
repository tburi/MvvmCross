﻿// MvxEmbeddedJsonDictionaryTextProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using System.Reflection;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Plugins.JsonLocalization
{
    [Preserve(AllMembers = true)]
	public class MvxEmbeddedJsonDictionaryTextProvider
        : MvxJsonDictionaryTextProvider
    {
        public MvxEmbeddedJsonDictionaryTextProvider(bool maskErrors = true)
            : base(maskErrors)
        {
        }

        public override void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath)
        {
            var json = GetTextFromEmbeddedResource(namespaceKey, resourcePath);
            if (string.IsNullOrEmpty(json))
                throw new FileNotFoundException("Unable to find resource file " + resourcePath);
            LoadJsonFromText(namespaceKey, typeKey, json);
        }

        private string GetTextFromEmbeddedResource(string namespaceKey, string resourcePath)
        {
            string path = namespaceKey + "." + GenerateResourceNameFromPath(resourcePath);

            try
            {
                string text = null;
                Stream stream = Assembly.Load(new AssemblyName(namespaceKey)).GetManifestResourceStream(path);
                if (stream == null)
                    return null;

                using (var textReader = new StreamReader(stream))
                {
                    text = textReader.ReadToEnd();
                }

                return text;
            }
            catch (Exception ex)
            {
                throw ex.MvxWrap("Cannot load resource {0}", path);
            }
        }
    }
}
