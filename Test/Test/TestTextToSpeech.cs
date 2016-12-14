

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
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;

namespace sdk.test
{
  [TestFixture]
  class TestTextToSpeech : IntegrationTest
  {
    private TextToSpeech textToSpeech = new TextToSpeech();
    private string customizationIDToTest;
    private string customizationToCreateName = "dotnet-integration-test-created-customization";
    private string customizationToCreateLanguage = "en-US";
    private string customizationToCreateDescription = "A text to speech voice customization created within dotnet.";
    private string updateWord0 = "hello";
    private string updateWord1 = "goodbye";
    private string updateTranslation0 = "hullo";
    private string updateTranslation1 = "gbye";
    private Word updateWordObject0 = new Word();
    private Word updateWordObject1 = new Word();
    private string toSpeechStringGET = "Hello world using GET";
    private string toSpeechStringPOST = "Hello world using POST";
    private string customizationIdCreated;
    private string testWord = "Watson";

    public override void Init()
    {
      base.Init();

      if (!isTestInitalized)
      {
        customizationIDToTest = Config.Instance.GetVariableValue("TextToSpeech_DotnetIntegrationTestCustomVoiceModel");
        updateWordObject0.word = updateWord0;
        updateWordObject0.translation = updateTranslation0;
        updateWordObject1.word = updateWord1;
        updateWordObject1.translation = updateTranslation1;
        isTestInitalized = true;
      }
    }

    [Test]
    public void TextToSpeech_TestGet()
    {
      Log.Debug("TestTextToSpeech", "Attempting to speech using GET...");

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

    [Test]
    public void TextToSpeech_TestPost()
    {
      Log.Debug("TestTextToSpeech", "Attempting to speech using POST...");

      if (!textToSpeech.ToSpeech(toSpeechStringPOST, (AudioClip clip) =>
      {
        Assert.NotNull(clip);
        autoEvent.Set();
      }, true))
      {
        Assert.Fail("Failed to invoke ToSpeech(); using POST.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TextToSpeech_TestGetVoices()
    {
      Log.Debug("TestTextToSpeech", "Attempting to get voices...");

      if (!textToSpeech.GetVoices((Voices voices) =>
      {
        Assert.NotNull(voices);
        autoEvent.Set();
      }))
      {
        Assert.Fail("Failed to invoke GetVoices();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TextToSpeech_TestGetVoice()
    {
      Log.Debug("TestTextToSpeech", "Attempting to get voice...");

      if (!textToSpeech.GetVoice((Voice voice) =>
      {
        Log.Debug("TestTextToSpeech", "GetVoice() invoked: {0}", voice.name);
        Assert.NotNull(voice);
        autoEvent.Set();
      }, VoiceType.en_US_Allison))
      {
        Assert.Fail("Failed to invoke GetVoice();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TextToSpeech_TestGetPronunciation()
    {
      Log.Debug("TestTextToSpeech", "Attempting to get pronunciation...");

      if (!textToSpeech.GetPronunciation((Pronunciation pronunciation) =>
      {
        if(!string.IsNullOrEmpty(pronunciation.pronunciation[0].ToString()))
          Log.Debug("TestTextToSpeech", "GetPronunciation() invoked: {0}", pronunciation.pronunciation[0]);

        Assert.NotNull(pronunciation);
        autoEvent.Set();
      }, testWord))
      {
        Assert.Fail("Failed to invoke GetPronunciation();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TextToSpeech_TestGetCustomizations()
    {
      Log.Debug("TestTextToSpeech", "Attempting to get customizations...");

      if (!textToSpeech.GetCustomizations((Customizations customizations, string data) =>
      {
        Assert.NotNull(customizations);
        autoEvent.Set();
      }))
      {
        Assert.Fail("Failed to invoke GetCustomizations();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(4)]
    public void TextToSpeech_TestCreateCustomization()
    {
      Log.Debug("TestTextToSpeech", "Attempting to create customization...");

      if (!textToSpeech.CreateCustomization((CustomizationID customizationID, string data) =>
      {
        Log.Debug("TestTextToSpeech", "CreateCustomization() invoked: {0}", customizationID.customization_id);
        Assert.NotNull(customizationID);
        customizationIdCreated = customizationID.customization_id;
        autoEvent.Set();
      }, customizationToCreateName, customizationToCreateLanguage, customizationToCreateDescription))
      {
        Assert.Fail("Failed to invoke CreateCustomization();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(5)]
    public void TextToSpeech_TestGetCustomization()
    {
      Log.Debug("TestTextToSpeech", "Attempting to get customization...");

      if (!textToSpeech.GetCustomization((Customization customization, string data) =>
      {
        Assert.NotNull(customization);
        autoEvent.Set();
      }, customizationIdCreated))
      {
        Assert.Fail("Failed to invoke GetCustomization();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(6)]
    public void TextToSpeech_TestUpdateCustomization()
    {
      Log.Debug("TestTextToSpeech", "Attempting to update customization...");

      Word[] words = { updateWordObject0 };
      CustomVoiceUpdate customVoiceUpdate = new CustomVoiceUpdate();
      customVoiceUpdate.words = words;

      if (!textToSpeech.UpdateCustomization((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, customizationIdCreated, customVoiceUpdate))
      {
        Assert.Fail("Failed to invoke UpdateCustomization();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(7)]
    public void TextToSpeech_TestGetCustomizationWords()
    {
      Log.Debug("TestTextToSpeech", "Attempting to get customization words...");

      if (!textToSpeech.GetCustomizationWords((Words words, string data) =>
      {
        Assert.NotNull(words);
        autoEvent.Set();
      }, customizationIdCreated))
      {
        Assert.Fail("Failed to invoke GetCustomizationWords();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(8)]
    public void TextToSpeech_TestAddCustomizationWords()
    {
      Log.Debug("TestTextToSpeech", "Attempting to get add customization words...");

      Word[] wordArray = { updateWordObject1 };
      Words wordsObject = new Words();
      wordsObject.words = wordArray;

      if (!textToSpeech.AddCustomizationWords((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, customizationIdCreated, wordsObject))
      {
        Assert.Fail("Failed to invoke AddCustomizationWords();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(9)]
    public void TextToSpeech_TestGetCustomizationWord()
    {
      Log.Debug("TestTextToSpeech", "Attempting to get customization word...");

      if (!textToSpeech.GetCustomizationWord((Translation translation, string data) =>
      {
        Assert.NotNull(translation);
        autoEvent.Set();
      }, customizationIdCreated, updateWord1))
      {
        Assert.Fail("Failed to invoke GetCustomizationWord();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(10)]
    public void TextToSpeech_TestDeleteCustomizationWord()
    {
      Log.Debug("TestTextToSpeech", "Attempting to delete customization word...");

      if (!textToSpeech.DeleteCustomizationWord((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, customizationIdCreated, updateWord1))
      {
        Assert.Fail("Failed to invoke DeleteCustomizationWord();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(11)]
    public void TextToSpeech_TestDeleteCustomizations()
    {
      Log.Debug("TestTextToSpeech", "Attempting to delete customization...");

      if (!textToSpeech.DeleteCustomization((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, customizationIdCreated))
      {
        Assert.Fail("Failed to invoke DeleteCustomization();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }
  }
}
