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

using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;

namespace sdk.test
{
  class TestTextToSpeech : IntegrationTest
  {
    private TextToSpeech textToSpeech = new TextToSpeech();
    private string customizationIDToTest;
    private string customizationToCreateName = "unity-integration-test-created-customization";
    private string customizationToCreateLanguage = "en-US";
    private string customizationToCreateDescription = "A text to speech voice customization created within Unity.";
    private string updateWord0 = "hello";
    private string updateWord1 = "goodbye";
    private string updateTranslation0 = "hullo";
    private string updateTranslation1 = "gbye";
    private Word updateWordObject0 = new Word();
    private Word updateWordObject1 = new Word();
    private string toSpeechStringGET = "Hello world using GET";
    private string toSpeechStringPOST = "Hello world using POST";
    private string customizationIdCreated;

    public override void Init()
    {
      base.Init();

      customizationIDToTest = Config.Instance.GetVariableValue("TextToSpeech_IntegrationTestCustomVoiceModel");
      updateWordObject0.word = updateWord0;
      updateWordObject0.translation = updateTranslation0;
      updateWordObject1.word = updateWord1;
      updateWordObject1.translation = updateTranslation1;
    }

    [Test]
    public void TextToSpeech_TestGet()
    {
      if (!textToSpeech.ToSpeech(toSpeechStringGET, (AudioClip clip) =>
       {
         Assert.NotNull(clip);
         autoEvent.Set();
       }))
      {
        Assert.Fail("Failed to invoke ToSpeech(); using GET.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    //[Test]
    //public void TextToSpeech_TestPost()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TextToSpeech_TestGetVoices()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TextToSpeech_TestGetVoice()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TextToSpeech_TestGetPronunciation()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TextToSpeech_TestGetCustomizations()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test, Order(4)]
    //public void TextToSpeech_TestCreateCustomizations()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test, Order(5)]
    //public void TextToSpeech_TestGetCustomization()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test, Order(6)]
    //public void TextToSpeech_TestUpdateCustomization()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test, Order(7)]
    //public void TextToSpeech_TestGetCustomizationWords()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test, Order(8)]
    //public void TextToSpeech_TestAddCustomizationWords()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test, Order(9)]
    //public void TextToSpeech_TestGetCustomizationWord()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test, Order(10)]
    //public void TextToSpeech_TestDeleteCustomizationWord()
    //{


    //  autoEvent.WaitOne();
    //}

    //[Test, Order(11)]
    //public void TextToSpeech_TestDeleteCustomizations()
    //{


    //  autoEvent.WaitOne();
    //}
  }
}
