﻿/**
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

using System;

namespace IBM.Watson.DeveloperCloud.Utilities
{
    /// <summary>
    /// Singleton pattern class. This class detects if T is a MonoBehavior and will 
    /// make a containing GameObject.  
    /// </summary>
    /// <typeparam name="T">The typename of the class to create as a singleton object.</typeparam>
    public class Singleton<T> where T : class
    {
        #region Private Data
        static private T sm_Instance = null;
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns the Singleton instance of T.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (sm_Instance == null)
                    CreateInstance();
                return sm_Instance;
            }
        }
        #endregion

        #region Singleton Creation
        /// <summary>
        /// Create the singleton instance.
        /// </summary>
        private static void CreateInstance()
        {
            sm_Instance = Activator.CreateInstance(typeof(T)) as T;

            if (sm_Instance == null)
                throw new WatsonException("Failed to create instance " + typeof(T).Name);
        }
        #endregion
    }
}
