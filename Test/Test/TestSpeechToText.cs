
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
using System.IO;
using System;
using System.Collections.Generic;

namespace sdk.test
{
  class TestSpeechToText : IntegrationTest
  {
    private SpeechToText speechToText = new SpeechToText();
    private string createdCustomizationID;
    private string createdCustomizationName = "dotnet-integration-test-customization";
    private string createdCorpusName = "dotnet-integration-test-corpus";
    private string createdCustomizationDescription = "dotnet integration test speech to text customization.";
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
    public void SpeechToText_TestGetModels()
    {
      Log.Debug("TestSpeechToText", "Attempting to get models...");

      if (!speechToText.GetModels((Model[] models) =>
      {
        Assert.NotNull(models);
        autoEvent.Set();
      }))
        Assert.Fail("Failed to invoke GetModels();");

      autoEvent.WaitOne();
    }

    [Test]
    public void SpeechToText_TestGetModel()
    {
      Log.Debug("TestSpeechToText", "Attempting to get model {0}...", speechToTextModelEnglish);

      if (!speechToText.GetModel((Model model) =>
      {
        Assert.NotNull(model);
        autoEvent.Set();
      }, speechToTextModelEnglish))
        Assert.Fail("Failed to invoke GetModel();");

      autoEvent.WaitOne();
    }

    [Test]
    public void SpeechToText_TestGetCustomizations()
    {
      Log.Debug("TestSpeechToText", "Attempting to get customizations...");

      if (!speechToText.GetCustomizations((Customizations customizations, string data) =>
      {
        Assert.NotNull(customizations);
        autoEvent.Set();
      }))
        Assert.Fail("Failed to invoke GetCustomizations();");

      autoEvent.WaitOne();
    }

    [Test, Order(5)]
    public void SpeechToText_TestCreateCustomization()
    {
      Log.Debug("TestSpeechToText", "Attempting to create customization...");

      if (!speechToText.CreateCustomization((CustomizationID customizationID, string data) =>
      {
        createdCustomizationID = customizationID.customization_id;
        Assert.NotNull(customizationID);
        autoEvent.Set();
      }, createdCustomizationName, speechToTextModelEnglish, createdCustomizationDescription))
        Assert.Fail("Failed to invoke CreateCustomization();");

      autoEvent.WaitOne();
    }

    [Test, Order(6)]
    public void SpeechToText_TestGetCustomization()
    {
      Log.Debug("TestSpeechToText", "Attempting to get customization {0}...", createdCustomizationID);

      if (!speechToText.GetCustomization((Customization customization, string data) =>
      {
        Assert.NotNull(customization);
        autoEvent.Set();
      }, createdCustomizationID))
        Assert.Fail("Failed to invoke GetCustomization();");

      autoEvent.WaitOne();
    }

    [Test]
    public void SpeechToText_TestGetCustomCorpora()
    {
      Log.Debug("TestSpeechToText", "Attempting to get custom corpora...");

      if (!speechToText.GetCustomCorpora((Corpora corpora, string data) =>
      {
        Assert.NotNull(corpora);
        autoEvent.Set();
      }, createdCustomizationID))
        Assert.Fail("Failed to invoke GetCustomCorpora();");

      autoEvent.WaitOne();
    }

    [Test, Order(7)]
    public void SpeechToText_TestAddCustomCorpusUsingFile()
    {
      Log.Debug("TestSpeechToText", "Attempting to add custom corpus using file...");

      if (!speechToText.AddCustomCorpus((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCustomizationID, createdCorpusName, allowOverwrite, customCorpusFilePath))
        Assert.Fail("Failed to invoke AddCustomCorpus(); using file");

      autoEvent.WaitOne();
    }

    [Test, Order(8)]
    public void SpeechToText_TestAddCustomCorpusUsingData()
    {
      byte[] customCorpusData = null ;

      Log.Debug("TestSpeechToText", "Attempting to add custom corpus using data...");
      try
      {
        customCorpusData = File.ReadAllBytes(customCorpusFilePath);
      }
      catch(Exception e)
      {
        Assert.Fail("Failed to read file. " + e.Message);
        autoEvent.Set();
      }

      if (customCorpusData != null)
      {
        if (!speechToText.AddCustomCorpus((bool success, string data) =>
        {
          Assert.True(success);
          autoEvent.Set();
        }, createdCustomizationID, createdCorpusName, allowOverwrite, customCorpusData))
          Assert.Fail("Failed to invoke AddCustomCorpus(); using data");
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void SpeechToText_TestGetCustomWords()
    {
      Log.Debug("TestSpeechToText", "Attempting to get custom words...");

      if (!speechToText.GetCustomWords((WordsList wordList, string data) =>
      {
        Assert.NotNull(wordList);
        autoEvent.Set();
      }, createdCustomizationID))
        Assert.Fail("Failed to invoke GetCustomWords();");

      autoEvent.WaitOne();
    }

    [Test, Order(9)]
    public void SpeechToText_TestAddCustomWordsUsingFile()
    {
      Log.Debug("TestSpeechToText", "Attempting to add custom words using file...");

      if (!speechToText.AddCustomWords((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCustomizationID, customWordsFilePath))
        Assert.Fail("Failed to invoke AddCustomWords(); file");

      autoEvent.WaitOne();
    }

    [Test, Order(10)]
    public void SpeechToText_TestAddCustomWordsUsingObject()
    {
      Log.Debug("TestSpeechToText", "Attempting to add custom words using object...");

      Words words = new Words();
      Word w0 = new Word();
      List<Word> wordList = new List<Word>();
      w0.word = "mikey";
      w0.sounds_like = new string[1];
      w0.sounds_like[0] = "my key";
      w0.display_as = "Mikey";
      wordList.Add(w0);
      Word w1 = new Word();
      w1.word = "charlie";
      w1.sounds_like = new string[1];
      w1.sounds_like[0] = "char lee";
      w1.display_as = "Charlie";
      wordList.Add(w1);
      Word w2 = new Word();
      w2.word = "bijou";
      w2.sounds_like = new string[1];
      w2.sounds_like[0] = "be joo";
      w2.display_as = "Bijou";
      wordList.Add(w2);
      words.words = wordList.ToArray();

      if (!speechToText.AddCustomWords((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCustomizationID, words))
        Assert.Fail("Failed to invoke AddCustomWords(); object");

      autoEvent.WaitOne();
    }

    [Test, Order(11)]
    public void SpeechToText_TestGetCustomWord()
    {
      Log.Debug("TestSpeechToText", "Attempting to get custom word...");

      if (!speechToText.GetCustomWord((WordData word, string data) =>
      {
        Assert.NotNull(word);
        autoEvent.Set();
      }, createdCustomizationID, wordToGet))
        Assert.Fail("Failed to invoke GetCustomWord();");

      autoEvent.WaitOne();
    }

    [Test, Order(12)]
    public void SpeechToText_TestTrainCustomization()
    {
      Log.Debug("TestSpeechToText", "Attempting to get train customization...");

      if (!speechToText.TrainCustomization((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCustomizationID))
        Assert.Fail("Failed to invoke TrainCustomization();");

      autoEvent.WaitOne();
    }

    [Test, Order(13)]
    public void SpeechToText_TestDeleteCustomCorpus()
    {
      Log.Debug("TestSpeechToText", "Attempting to delete custom corpus...");

      if (!speechToText.DeleteCustomCorpus((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCustomizationID, createdCorpusName))
        Assert.Fail("Failed to invoke DeleteCustomCorpus();");

      autoEvent.WaitOne();
    }

    [Test, Order(14)]
    public void SpeechToText_TestDeleteCustomWord()
    {
      Log.Debug("TestSpeechToText", "Attempting to delete custom word...");

      if (!speechToText.DeleteCustomWord((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCustomizationID, wordToGet))
        Assert.Fail("Failed to invoke DeleteCustomWord();");

      autoEvent.WaitOne();
    }

    [Test, Order(15)]
    public void SpeechToText_TestResetCustomization()
    {
      Log.Debug("TestSpeechToText", "Attempting to reset customization...");

      if (!speechToText.ResetCustomization((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCustomizationID))
        Assert.Fail("Failed to invoke ResetCustomization();");

      autoEvent.WaitOne();
    }

    [Test, Order(16)]
    public void SpeechToText_TestDeleteCustomization()
    {
      Log.Debug("TestSpeechToText", "Attempting to delete customization...");

      if (!speechToText.DeleteCustomization((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCustomizationID))
        Assert.Fail("Failed to invoke DeleteCustomization();");

      autoEvent.WaitOne();
    }
  }
}
