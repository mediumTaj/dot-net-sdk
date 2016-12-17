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

using IBM.Watson.DeveloperCloud.Services.PersonalityInsights.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;

namespace sdk.test
{
    [TestFixture]
    public class TestPersonalityInsights : IntegrationTest
    {
        PersonalityInsights personalityInsights = new PersonalityInsights();
        private string testString = "test";
        string dataPath = Constants.Path.APP_DATA + "/personalityInsights.json";

        [Test]
        public void PersonalityInsights_TestGetProfileText()
        {
            if (!personalityInsights.GetProfile((Profile profile, string data) =>
             {
                 Assert.AreNotEqual(profile, null);
                 autoEvent.Set();
             }, testString))
            {
                Assert.Fail("Failed to invoke GetProfile with text.");
                autoEvent.Set();
            }

            autoEvent.WaitOne();
        }

        [Test]
        public void PersonalityInsights_TestGetProfileJson()
        {
            if (!personalityInsights.GetProfile((Profile profile, string data) =>
             {
                 Assert.AreNotEqual(profile, null);
                 autoEvent.Set();
             }, System.IO.Path.GetFullPath(dataPath), "application/json"))
            {
                Assert.Fail("Failed to invoke GetProfile with json.");
                autoEvent.Set();
            }

            autoEvent.WaitOne();
        }
    }
}
