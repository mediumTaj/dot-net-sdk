
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
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Logging;

namespace sdk.test
{
  class TestSpeechToText : IntegrationTest
  {
    private SpeechToText speechToText = new SpeechToText();
    private string createdCustomizationID;
    private string createCustomizationName = "dotnet-integration-test-customization";
    private string createdCorpusName = "dotnet-integration-test-corpus";
    private string customCorpusFilePath;
    private string speechToTextModelEnglish = "en-US_BroadbandModel";
    private string customWordsFilePath;
    private bool allowOverwrite = true;
    private string wordToGet = "watson";

    public override void Init()
    {
      base.Init();

      customCorpusFilePath = testDataPath + "test-stt-corpus.txt";
      customWordsFilePath = testDataPath + "test-stt-words.json";
    }

    [Test]
    public void TestGetModels()
    {
      Log.Debug("TestSpeechToText", "Attempting to get models...");

    }

    [Test]
    public void TestGetModel()
    {

    }

    [Test]
    public void TestGetCustomizations()
    {

    }

    [Test]
    public void TestCreateCustomizations()
    {

    }

    [Test]
    public void TestGetCustomization()
    {

    }

    [Test]
    public void TestGetCustomCorpora()
    {

    }

    [Test]
    public void TestAddCustomCorpus()
    {

    }

    [Test]
    public void TestGetCustomWords()
    {

    }

    [Test]
    public void TestAddCustomWordsUsingFile()
    {

    }

    [Test]
    public void TestAddCustomWordsUsingObject()
    {

    }

    [Test]
    public void TestGetCustomWord()
    {

    }

    [Test]
    public void TestTrainCustomization()
    {

    }

    [Test]
    public void TestDeleteCustomCorpus()
    {

    }

    [Test]
    public void TestDeleteCustomWord()
    {

    }

    [Test]
    public void TestResetCustomization()
    {

    }

    [Test]
    public void TestDeleteCustomization()
    {

    }
  }
}
