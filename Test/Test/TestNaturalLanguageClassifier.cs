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

using System.IO;
using NUnit.Framework;
using IBM.Watson.DeveloperCloud.Services.NaturalLanguageClassifier.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using static IBM.Watson.DeveloperCloud.Utilities.Utility;
using IBM.Watson.DeveloperCloud.Logging;
using System;
using System.Threading;

namespace Test
{
  [TestFixture]
  public class TestNaturalLanguageClassifier
  {
    private NaturalLanguageClassifier naturalLanguageClassifier = new NaturalLanguageClassifier();
    private string classifierID = "d67c63x141-nlc-1055";
    private string classifierName = "dot-net-integration-test-classifier12/9/2016 10:44:30 AM";
    private string createdClassifierID;
    private string createdClassifierNameBase = "dot-net-integration-test-classifier";
    private string createdClassifierName;
    private string createdClassifierLanguage = "en";
    private string trainingDataPath = Constants.Path.APP_DATA + Path.DirectorySeparatorChar + "naturalLanguageClassifierTrainingData.json";
    private string classifyText = "Will it rain today?";
    private ClassifierData data = new ClassifierData();
    AutoResetEvent autoEvent = new AutoResetEvent(false);

    [SetUp]
    public void Init()
    {
      Constants.Path.dataPath = TestContext.CurrentContext.TestDirectory + Path.DirectorySeparatorChar;
      string testDataPath = Constants.Path.dataPath + Constants.Path.APP_DATA + Path.DirectorySeparatorChar;
      Log.Debug("TestNaturalLanguageClassifier", "Test data path: {0}", testDataPath);

      if (!Config.Instance.ConfigLoaded)
      {
        string configPath = testDataPath + Constants.Path.CONFIG_FILE;
        string configJson = File.ReadAllText(configPath);
        Config.Instance.LoadConfig(configJson);
      }

      if (!Config.Instance.ConfigLoaded)
        Assert.Fail("Failed to load Config.");

      naturalLanguageClassifier.DisableCache = true;
      data.Load(TestContext.CurrentContext.TestDirectory + Path.DirectorySeparatorChar + trainingDataPath);
    }

    [Test, Order(0)]
    public void TestTrainClassifier()
    {
      Log.Debug("TestNaturalLanguageClassifier", "Attempting to train classifier...");
      createdClassifierName = createdClassifierNameBase + DateTime.Now;

      if (!naturalLanguageClassifier.TrainClassifier(createdClassifierName, createdClassifierLanguage, data.Export(), (Classifier classifier) =>
        {
          Log.Debug("TestNaturalLanguageClassifier", "Created classifier id: {0}.", classifier.classifier_id);

          if (classifier != null)
          {
            createdClassifierID = classifier.classifier_id;
          }

          Assert.AreEqual(classifier.name, createdClassifierName);
          autoEvent.Set();
        }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(1)]
    public void TestFindClassifier()
    {
      Log.Debug("TestNaturalLanguageClassifier", "Attempting to find classifier {0}...", createdClassifierNameBase);
      naturalLanguageClassifier.FindClassifier(createdClassifierNameBase, (Classifier classifier) =>
      {
        Log.Debug("TestNaturalLanguageClassifier", "Found classifier {0}, {1}.", classifier.name, classifier.classifier_id);
        Assert.AreNotEqual(classifier, null);
        autoEvent.Set();
      });

      autoEvent.WaitOne();
    }

    [Test, Order(2)]
    public void TestGetClassifier()
    {
      Log.Debug("TestNaturalLanguageClassifier", "Attempting to get classifier {0}...", createdClassifierID);

      if (string.IsNullOrEmpty(createdClassifierID))
      {
        Assert.Fail("GetClassifier(); createdClassifierID and classifierName are null. We need a classifier name to find the classifier.");
        return;
      }

      if (!naturalLanguageClassifier.GetClassifier(createdClassifierID, (Classifier classifier) =>
       {
         Log.Debug("TestNaturalLanguageClassifier", "Got classifier {0}.", classifier.classifier_id);
         Assert.AreNotEqual(classifier, null);
         autoEvent.Set();
       }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetClassifiers()
    {
      if (!naturalLanguageClassifier.GetClassifiers((Classifiers classifiers) =>
      {
        foreach (Classifier classifier in classifiers.classifiers)
        {
          if (classifier.status == "Available")
          {
            classifierID = classifier.classifier_id;
            classifierName = classifier.name;
            break;
          }
        }

        Assert.AreNotEqual(classifiers, null);
        autoEvent.Set();
      }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestClassify()
    {
      if (string.IsNullOrEmpty(classifierID))
      {
        Assert.Fail("classifierID is null. An classifier is required to test classify.");
      }

      if (!naturalLanguageClassifier.Classify(classifierID, classifyText, (ClassifyResult result) =>
      {
        Assert.AreNotEqual(result, null);
        autoEvent.Set();
      }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(3)]
    public void TestDeleteClassifier()
    {

      if (string.IsNullOrEmpty(createdClassifierID))
      {
        Assert.Fail("createdClassifierID is null. Cannot test delete.");
      }
      else
      {
        Log.Debug("TestNaturalLanguageClassifier", "Attempting to delete classifier {0}...", createdClassifierID);
      }

      if (!naturalLanguageClassifier.DeleteClassifer(createdClassifierID, (bool success) =>
      {
        Log.Debug("TestNaturalLanguageClassifier", "Deleted classifier {0}.", createdClassifierID);
        Assert.AreEqual(success, true);
        autoEvent.Set();
      }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }
  }
}
