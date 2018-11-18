// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FcmSharp.BackOff;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace FcmSharp.Settings
{
    public static class FileBasedFcmClientSettings
    {
        public static FcmClientSettings CreateFromFile(string credentialsFileName)
        {
            var credentials = ReadCredentialsFromFile(credentialsFileName);
            var project = GetProjectId(credentialsFileName, credentials);

            return new FcmClientSettings(project, credentials);
        }

        public static FcmClientSettings CreateFromFile(string credentialsFileName, ExponentialBackOffSettings exponentialBackOffSettings)
        {
            var credentials = ReadCredentialsFromFile(credentialsFileName);
            var project = GetProjectId(credentialsFileName, credentials);

            return new FcmClientSettings(project, credentials, exponentialBackOffSettings);
        }

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

        private static string GetProjectId(string serviceAccountKeyFile, string serviceAccountKeyJson)
        {
            var serviceAccountKeyDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(serviceAccountKeyJson);

            if (!serviceAccountKeyDictionary.ContainsKey("project_id"))
            {
                throw new Exception($"Could not read Project ID from ServiceAccountKey File '{serviceAccountKeyFile}'");
            }

            return serviceAccountKeyDictionary["project_id"];
        }
    }
}