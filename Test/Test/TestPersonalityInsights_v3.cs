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
using NUnit.Framework;
using IBM.Watson.DeveloperCloud.Services.PersonalityInsights.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Threading;
using IBM.Watson.DeveloperCloud.Logging;

namespace sdk.test
{
  [TestFixture]
  class TestPersonalityInsights_v3
  {
    PersonalityInsights personalityInsights = new PersonalityInsights();
    private string testString = "test";
    string dataPath = Constants.Path.APP_DATA + "/personalityInsights.json";
    AutoResetEvent autoEvent = new AutoResetEvent(false);

    [Test]
    public void TestGetProfileText()
    {
      personalityInsights.GetProfile((Profile profile, string data) =>
      {
        Log.Debug("Program", profile.ToString());
        Assert.AreNotEqual(profile, null);
        autoEvent.Set();
      }, testString);

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetProfileJson()
    {
      Log.Debug("TestPersonalityInsights", "{0}", System.IO.Path.GetFullPath(dataPath));
      personalityInsights.GetProfile((Profile profile, string data) =>
      {
        Assert.AreNotEqual(profile, null);
        autoEvent.Set();
      }, System.IO.Path.GetFullPath(dataPath), "application/json");

      autoEvent.WaitOne();
    }
  }
}
