

using IBM.Watson.DeveloperCloud.Logging;
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
using IBM.Watson.DeveloperCloud.Services.ToneAnalyzer.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;
using System.IO;

namespace sdk.test
{
  class TestToneAnalyzer : IntegrationTest
  {
    private ToneAnalyzer toneAnalyzer = new ToneAnalyzer();
    private string toneAnalyzerTestDataPath = "watson_beats_jeopardy.txt";
    private string toneAnalyzerTestDataString;
    
    public override void Init()
    {
      base.Init();

      toneAnalyzerTestDataString = File.ReadAllText(testDataPath + toneAnalyzerTestDataPath);
    }

    [Test]
    public void ToneAnalyzer_TestToneGET()
    {
      Log.Debug("TestToneAnalyzer", "Attempting to get tone using GET...");

      if (!toneAnalyzer.GetToneAnalyze(toneAnalyzerTestDataString, (ToneAnalyzerResponse resp, string data) =>
       {
         Assert.NotNull(resp);
         autoEvent.Set();
       }))
      {
        Assert.Fail("Failed to invoke GetToneAnalyze() GET.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void ToneAnalyzer_TestTonePOST()
    {
      Log.Debug("TestToneAnalyzer", "Attempting to get tone using POST...");

      if (!toneAnalyzer.GetToneAnalyze((ToneAnalyzerResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, toneAnalyzerTestDataString))
      {
        Assert.Fail("Failed to invoke GetToneAnalyze() POST.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }
  }
}
