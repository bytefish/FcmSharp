// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace FcmSharp.Settings
{
    public static class FileBasedFcmClientSettings
    {
        public static FcmClientSettings CreateFromFile(string project, string credentialsFileName)
        {
            var credentials = ReadCredentialsFromFile(credentialsFileName);

            return new FcmClientSettings(project, credentials);
        }

        private static string ReadCredentialsFromFile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (!File.Exists(fileName))
            {
                throw new Exception(string.Format("Could not Read Credentials. (Reason = File Does Not Exist, FileName = '{0}')", fileName));
            }

            string credentials = File.ReadAllText(fileName);

            if (string.IsNullOrWhiteSpace(credentials))
            {
                throw new Exception(string.Format("Could not Read Credentials. (Reason = File Is Empty, FileName = '{0}')", fileName));
            }

            return credentials;
        }
    }
}