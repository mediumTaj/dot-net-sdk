/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/


using FullSerializer;
using IBM.Watson.DeveloperCloud.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using NAudio.Wave;

namespace IBM.Watson.DeveloperCloud.Utilities
{
    /// <summary>
    /// Utility functions.
    /// </summary>
    static public class Utility
    {
        private static fsSerializer sm_Serializer = new fsSerializer();
        private static string sm_MacAddress = null;

        /// <summary>
        /// This helper functions returns all Type's that inherit from the given type.
        /// </summary>
        /// <param name="type">The Type to find all types that inherit from the given type.</param>
        /// <returns>A array of all Types that inherit from type.</returns>
        public static Type[] FindAllDerivedTypes(Type type)
        {
            List<Type> types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in assembly.GetTypes())
                {
                    if (t == type || t.IsAbstract)
                        continue;
                    if (type.IsAssignableFrom(t))
                        types.Add(t);
                }
            }

            return types.ToArray();
        }

        /// <summary>
        /// Approximately the specified a, b and tolerance.
        /// </summary>
        /// <param name="a">The first component.</param>
        /// <param name="b">The second component.</param>
        /// <param name="tolerance">Tolerance.</param>
        public static bool Approximately(double a, double b, double tolerance = 0.0001)
        {
            return (Math.Abs(a - b) < tolerance);
        }

        /// <summary>
        /// Approximately the specified a, b and tolerance.
        /// </summary>
        /// <param name="a">The first component.</param>
        /// <param name="b">The second component.</param>
        /// <param name="tolerance">Tolerance.</param>
        public static bool Approximately(float a, float b, float tolerance = 0.0001f)
        {
            return (Math.Abs(a - b) < tolerance);
        }

        /// <summary>
        /// Get the MD5 hash of a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetMD5(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.Default.GetBytes(s);
            byte[] result = md5.ComputeHash(data);

            StringBuilder output = new StringBuilder();
            foreach (var b in result)
                output.Append(b.ToString("x2"));

            return output.ToString();
        }

        /// <summary>
        /// Removes any tags from a string. (e.g. <title></title>)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveTags(string s, char tagStartChar = '<', char tagEndChar = '>')
        {
            if (!string.IsNullOrEmpty(s))
            {
                int tagStart = s.IndexOf(tagStartChar);
                while (tagStart >= 0)
                {
                    int tagEnd = s.IndexOf(tagEndChar, tagStart);
                    if (tagEnd < 0)
                        break;

                    string pre = tagStart > 0 ? s.Substring(0, tagStart) : string.Empty;
                    string post = tagEnd < s.Length ? s.Substring(tagEnd + 1) : string.Empty;
                    s = pre + post;

                    tagStart = s.IndexOf(tagStartChar);
                }
            }

            return s;
        }

        /// <summary>
        /// Gets the on off string.
        /// </summary>
        /// <returns>The on off string.</returns>
        /// <param name="b">If set to <c>true</c> b.</param>
        public static string GetOnOffString(bool b)
        {
            return b ? "ON" : "OFF";
        }

        /// <summary>
        /// Strips the prepending ! statment from string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="s">S.</param>
        public static string StripString(string s)
        {
            string[] delimiter = new string[] { "!", "! " };
            string[] newString = s.Split(delimiter, System.StringSplitOptions.None);
            if (newString.Length > 1)
            {
                return newString[1];
            }
            else
            {
                return s;
            }

        }

        /// <summary>
        /// Gets the EPOCH time in UTC time zome
        /// </summary>
        /// <returns>Double EPOCH in UTC</returns>
        public static double GetEpochUTCMilliseconds()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (DateTime.UtcNow - epoch).TotalMilliseconds;
        }

        /// <summary>
        /// Gets the epoch UTC seconds.
        /// </summary>
        /// <returns>The epoch UTC seconds.</returns>
        public static double GetEpochUTCSeconds()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (DateTime.UtcNow - epoch).TotalSeconds;
        }

        /// <summary>
        /// Gets the date time from epoch.
        /// </summary>
        /// <returns>The date time from epoch.</returns>
        /// <param name="epochTime">Epoch time.</param>
        /// <param name="kind">Kind.</param>
        public static DateTime GetLocalDateTimeFromEpoch(double epochTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            try
            {
                dateTime = dateTime.AddSeconds(epochTime).ToLocalTime();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Log.Debug("Utility", "Time conversion assuming time is in Milliseconds: {0}, {1}", epochTime, ex.Message);
                dateTime = dateTime.AddMilliseconds(epochTime).ToLocalTime();
            }

            return dateTime;
        }


        /// <summary>
        /// Returns First valid Mac address of the local machine
        /// </summary>
        public static string MacAddress
        {
            get
            {
                if (string.IsNullOrEmpty(sm_MacAddress))
                {
                    foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        string macAddress = adapter.GetPhysicalAddress().ToString();
                        if (!string.IsNullOrEmpty(macAddress))
                        {
                            string regex = "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})";
                            string replace = "$1:$2:$3:$4:$5:$6";
                            sm_MacAddress = Regex.Replace(macAddress, regex, replace);

                            break;
                        }
                    }
                }

                return sm_MacAddress;
            }
        }

        #region Cache Generic Deserialization
        /// <summary>
        /// Save value to data cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionaryCache"></param>
        /// <param name="cacheDirectoryId"></param>
        /// <param name="cacheId"></param>
        /// <param name="objectToCache"></param>
        /// <param name="prefix"></param>
        /// <param name="maxCacheSize"></param>
        /// <param name="maxCacheAge"></param>
        public static void SaveToCache<T>(Dictionary<string, DataCache> dictionaryCache, string cacheDirectoryId, string cacheId, T objectToCache, string prefix = "", long maxCacheSize = 1024 * 1024 * 50, double maxCacheAge = 24 * 7) where T : class, new()
        {
            if (objectToCache != null)
            {
                DataCache cache = null;

                if (!dictionaryCache.TryGetValue(cacheDirectoryId, out cache))
                {
                    cache = new DataCache(prefix + cacheDirectoryId, maxCacheSize: maxCacheSize, maxCacheAge: double.MaxValue);   //We will store the values as max time
                    dictionaryCache[cacheDirectoryId] = cache;
                }

                fsData data = null;
                fsResult r = sm_Serializer.TrySerialize(objectToCache.GetType(), objectToCache, out data);
                if (!r.Succeeded)
                    throw new WatsonException(r.FormattedMessages);

                cache.Save(cacheId, Encoding.UTF8.GetBytes(fsJsonPrinter.PrettyJson(data)));
            }
        }

        /// <summary>
        /// Get value from the data cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionaryCache"></param>
        /// <param name="cacheDirectoryId"></param>
        /// <param name="cacheId"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static T GetFromCache<T>(Dictionary<string, DataCache> dictionaryCache, string cacheDirectoryId, string cacheId, string prefix = "") where T : class, new()
        {
            T cachedObject = null;

            DataCache cache = null;
            if (!dictionaryCache.TryGetValue(cacheDirectoryId, out cache))
            {
                cache = new DataCache(prefix + cacheDirectoryId);
                dictionaryCache[cacheDirectoryId] = cache;
            }

            byte[] cached = cache.Find(cacheId);
            if (cached != null)
            {
                cachedObject = DeserializeResponse<T>(cached);
                if (cachedObject != null)
                {
                    return cachedObject;
                }
            }

            return cachedObject;
        }

        #endregion

        #region De-Serialization

        /// <summary>
        /// Deserializes the response.
        /// </summary>
        /// <returns>The response.</returns>
        /// <param name="resp">Resp.</param>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T DeserializeResponse<T>(byte[] resp, object obj = null) where T : class, new()
        {
            return DeserializeResponse<T>(Encoding.UTF8.GetString(resp), obj);
        }

        /// <summary>
        /// Deserializes the response.
        /// </summary>
        /// <returns>The response.</returns>
        /// <param name="json">Json string of object</param>
        /// <param name="obj">Object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T DeserializeResponse<T>(string json, object obj = null) where T : class, new()
        {
            try
            {
                fsData data = null;
                fsResult r = fsJsonParser.Parse(json, out data);
                if (!r.Succeeded)
                    throw new WatsonException(r.FormattedMessages);

                if (obj == null)
                    obj = new T();

                r = sm_Serializer.TryDeserialize(data, obj.GetType(), ref obj);
                if (!r.Succeeded)
                    throw new WatsonException(r.FormattedMessages);

                return (T)obj;
            }
            catch (Exception e)
            {
                Log.Error("Utility", "DeserializeResponse Exception: {0}", e.ToString());
            }

            return null;
        }
        #endregion

        #region Is Local File
        /// <summary>
        /// Determines if a string is a file path.
        /// </summary>
        /// <param name="s">String to query.</param>
        /// <returns></returns>
        public static bool IsLocalFile(string s)
        {
            try
            {
                return new Uri(s).IsFile;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Has Invalid Characters
        public static bool FilePathHasInvalidChars(string path)
        {
            return (!string.IsNullOrEmpty(path) && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0);
        }
        #endregion

        #region Get Source Type
        public static SourceType GetSourceType(string source)
        {
            if (source.StartsWith("http://") || source.StartsWith("https://"))
            {
                Log.Debug("Utility", "source type: {0}, source: {1}", SourceType.URL, source);
                return SourceType.URL;
            }
            else
            {
                try
                {
                    bool extension = Path.HasExtension(source);
                    return SourceType.PATH;
                }
                catch
                {
                    return SourceType.TEXT;
                }
            }
        }
        public enum SourceType
        {
            URL,
            PATH,
            TEXT
        }
        #endregion

        #region ClassifierData
        public class ClassifierData
        {
            [fsIgnore]
            public string FileName { get; set; }
            public bool Expanded { get; set; }
            public bool InstancesExpanded { get; set; }
            public bool ClassesExpanded { get; set; }
            public string Name { get; set; }
            public string Language { get; set; }
            public Dictionary<string, List<string>> Data { get; set; }
            public Dictionary<string, bool> DataExpanded { get; set; }

            public void Import(string filename)
            {
                if (Data == null)
                    Data = new Dictionary<string, List<string>>();

                string[] lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    int nSeperator = line.LastIndexOf(',');
                    if (nSeperator < 0)
                        continue;

                    string c = line.Substring(nSeperator + 1);
                    string phrase = line.Substring(0, nSeperator);

                    if (!Data.ContainsKey(c))
                        Data[c] = new List<string>();
                    Data[c].Add(phrase);
                }
            }

            public string Export()
            {
                StringBuilder sb = new StringBuilder();
                foreach (var kp in Data)
                {
                    foreach (var p in kp.Value)
                    {
                        sb.Append(p + "," + kp.Key + "\n");
                    }
                }

                return sb.ToString();
            }

            public void Save(string filename)
            {
                fsData data = null;
                fsResult r = sm_Serializer.TrySerialize(typeof(ClassifierData), this, out data);
                if (!r.Succeeded)
                    throw new Exception("Failed to serialize ClassifierData: " + r.FormattedMessages);

                File.WriteAllText(filename, fsJsonPrinter.PrettyJson(data));
                FileName = filename;
            }

            public void Save()
            {
                Save(FileName);
            }

            public bool Load(string filename)
            {
                try
                {
                    string json = File.ReadAllText(filename);

                    fsData data = null;
                    fsResult r = fsJsonParser.Parse(json, out data);
                    if (!r.Succeeded)
                        throw new Exception(r.FormattedMessages);

                    object obj = this;
                    r = sm_Serializer.TryDeserialize(data, obj.GetType(), ref obj);
                    if (!r.Succeeded)
                        throw new Exception(r.FormattedMessages);
                }
                catch (Exception e)
                {
                    Log.Error("NaturalLanguageClassifierEditor", "Failed to load classifier data {1}: {0}", e.ToString(), filename);
                    return false;
                }

                FileName = filename;
                return true;
            }
        };
        #endregion

        #region 16 bit to float
        public static float[] Convert16BitToFloat(byte[] input)
        {
            int inputSamples = input.Length / 2; // 16 bit input, so 2 bytes per sample
            float[] output = new float[inputSamples];
            int outputIndex = 0;
            for (int n = 0; n < inputSamples; n++)
            {
                short sample = BitConverter.ToInt16(input, n * 2);
                output[outputIndex++] = sample / 32768f;
            }
            return output;
        }
        #endregion

        #region Read WAVE file
        public static AudioClip ReadWaveFile(string filePath)
        {
            AudioClip clip = null;

            try
            {
                using (WaveFileReader fileReader = new WaveFileReader(filePath))
                {
                    byte[] buffer = new byte[fileReader.Length];
                    fileReader.Read(buffer, 0, (int)fileReader.Length);

                    clip = AudioClip.Create("audioClip", (int)fileReader.SampleCount, fileReader.WaveFormat.Channels, fileReader.WaveFormat.SampleRate, false);
                    clip.SetData(Convert16BitToFloat(buffer), 0);
                }
            }
            catch (Exception e)
            {
                Log.Debug("Unable to read WavFile: {0}", e.Message);
            }

            return clip;
        }
        #endregion
    }
}
