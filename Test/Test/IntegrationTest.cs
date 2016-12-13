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

using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;
using System.IO;

namespace sdk.test
{
  public class IntegrationTest
  {
    public bool isInitalized = false;
    public string testDataPath;

    [SetUp]
    public virtual void Init()
    {
      if (!isInitalized)
      {
        Constants.Path.dataPath = TestContext.CurrentContext.TestDirectory + Path.DirectorySeparatorChar;
        testDataPath = Constants.Path.dataPath + Constants.Path.APP_DATA + Path.DirectorySeparatorChar;

        if (!Config.Instance.ConfigLoaded)
        {
          string configPath = testDataPath + Constants.Path.CONFIG_FILE;
          string configJson = File.ReadAllText(configPath);
          if (!Config.Instance.LoadConfig(configJson))
          {
            Log.Debug("IntegrationTest", "Failed to load config");
          }
          else
          {
            isInitalized = true;
          }
        }

        if (!Config.Instance.ConfigLoaded)
          Assert.Fail("Failed to load Config.");
      }
    }
  }
}
