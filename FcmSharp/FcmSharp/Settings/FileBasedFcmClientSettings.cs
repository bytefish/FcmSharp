// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using FcmSharp.BackOff;

namespace FcmSharp.Settings
{
    public static class FileBasedFcmClientSettings
    {
        public static FcmClientSettings CreateFromFile(string project, string credentialsFileName)
        {
            var credentials = ReadCredentialsFromFile(credentialsFileName);

            return new FcmClientSettings(project, credentials);
        }

        public static FcmClientSettings CreateFromFile(string project, string credentialsFileName, ExponentialBackOffSettings exponentialBackOffSettings)
        {
            var credentials = ReadCredentialsFromFile(credentialsFileName);

            return new FcmClientSettings(project, credentials, exponentialBackOffSettings);
        }

        private static string ReadCredentialsFromFile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (!File.Exists(fileName))
            {
                throw new Exception($"Could not Read Credentials. (Reason = File Does Not Exist, FileName = '{fileName}')");
            }

            string credentials = File.ReadAllText(fileName);

            if (string.IsNullOrWhiteSpace(credentials))
            {
                throw new Exception($"Could not Read Credentials. (Reason = File Is Empty, FileName = '{fileName}')");
            }

            return credentials;
        }
    }
}