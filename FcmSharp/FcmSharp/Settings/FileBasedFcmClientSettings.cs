// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using FcmSharp.Constants;

namespace FcmSharp.Settings
{
    public class FileBasedFcmClientSettings : IFcmClientSettings
    {
        private readonly string apiKey;

        public FileBasedFcmClientSettings(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.apiKey = ReadApiKeyFromFile(fileName);
        }

        public string FcmUrl
        {
            get { return FcmConstants.FcmUrl; }
        }

        public string ApiKey
        {
            get { return apiKey; }
        }

        private string ReadApiKeyFromFile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (!File.Exists(fileName))
            {
                throw new Exception(string.Format("Could not Read API Token. (Reason = File Does Not Exist, FileName = '{0}')", fileName));
            }

            string apiKey = File.ReadAllText(fileName);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception(string.Format("Could not Read API Token. (Reason = File Is Empty, FileName = '{0}')", fileName));
            }

            return apiKey.Trim();
        }
    }
}