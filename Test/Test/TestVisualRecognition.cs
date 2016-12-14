

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
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace sdk.test
{
  [TestFixture, Ignore("Ignoring VisualRecognition because of 500 error.")]
  class TestVisualRecognition : IntegrationTest
  {
    private VisualRecognition visualRecognition = new VisualRecognition();

    private string classifierId = null;
    private string classifierName = "dotnet-integration-test-classifier";
    private string className_Giraffe = "giraffe";
    private string className_Turtle = "turtle";
    private string imageURL = "https://c2.staticflickr.com/2/1226/1173659064_8810a06fef_b.jpg";
    private string imageFaceURL = "https://upload.wikimedia.org/wikipedia/commons/e/e9/Official_portrait_of_Barack_Obama.jpg";
    private string imageTextURL = "http://i.stack.imgur.com/ZS6nH.png";
    private string visualRecognitionTestDataPath;
    private string positiveExamplesPath;
    private string negativeExamplesPath;
    private string positiveUpdated;
    Dictionary<string, string> positiveExamples = new Dictionary<string, string>();
    Dictionary<string, string> positiveUpdatedExamples = new Dictionary<string, string>();
    private string[] owners = { "IBM", "me" };
    private string[] classifierIds = null;
    private string classifyImagePath;
    private string detectFaceImagePath;
    private string recognizeTextImagePath;
    private string createdCollectionID;
    private string createdCollectionImage;
    private string collectionImagePath;
    Dictionary<string, string> imageMetadata = new Dictionary<string, string>();
    private string collectionName = "dotnet-integration-test-collection";
    
    public override void Init()
    {
      base.Init();

      if (!isTestInitalized)
      {
        visualRecognitionTestDataPath = testDataPath + "visual-recognition-classifiers" + Path.DirectorySeparatorChar;
        positiveExamplesPath = visualRecognitionTestDataPath + "giraffe_positive_examples.zip";
        negativeExamplesPath = visualRecognitionTestDataPath + "negative_examples.zip";
        positiveUpdated = visualRecognitionTestDataPath + "turtle_positive_examples.zip";
        classifyImagePath = visualRecognitionTestDataPath + "giraffe_to_classify.jpg";
        detectFaceImagePath = visualRecognitionTestDataPath + "obama.jpg";
        recognizeTextImagePath = visualRecognitionTestDataPath + "from_platos_apology.png";
        collectionImagePath = visualRecognitionTestDataPath + "giraffe_to_classify.jpg";
        isTestInitalized = true;
      }
    }

    [Test, Order(1)]
    public void VisualRecognition_TestGetClassifiers()
    {
      Log.Debug("TestVisualRecognition", "Attempting to get classifiers...");
      if (!visualRecognition.GetClassifiers((GetClassifiersTopLevelBrief classifiers, string data) =>
      {
        Log.Debug("TestVisualRecognition", "{0} classifiers found.", classifiers.classifiers.Length);
        Assert.NotNull(classifiers);
        autoEvent.Set();
      }))
      {
        Assert.Fail("Failed to invoke GetClassifiers().");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(2)]
    public void VisualRecognition_TestTrainClassifier()
    {
      Log.Debug("TestVisualRecognition", "Attempting to train classifier...");
      positiveExamples.Add(className_Giraffe, positiveExamplesPath);

      if (!visualRecognition.TrainClassifier((GetClassifiersPerClassifierVerbose classifier, string data) =>
      {
        classifierId = classifier.classifier_id;

        List<string> classifierIdsList = new List<string>();
        classifierIdsList.Add("default");
        classifierIdsList.Add(classifierId);
        classifierIds = classifierIdsList.ToArray();

        Assert.NotNull(classifier);
        autoEvent.Set();
      }, classifierName, positiveExamples, negativeExamplesPath))
      {
        Assert.Fail("Failed to invoke TrainClassifier.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(3)]
    public void VisualRecognition_TestFindClassifier()
    {
      Log.Debug("TestVisualRecognition", "Attempting to find classifier {0}...", classifierName);

      try
      {
        visualRecognition.FindClassifier((GetClassifiersPerClassifierVerbose classifier, string data) =>
        {
          Assert.NotNull(classifier);
          autoEvent.Set();
        }, classifierName);
      }
      catch(Exception e)
      {
        Assert.Fail("Failed to invoke FindClassier(); {0}", e.Message);
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(4)]
    public void VisualRecognition_TestGetClassifier()
    {
      Log.Debug("TestVisualRecognition", "Attempting to get classifier...");

      if (!visualRecognition.GetClassifier((GetClassifiersPerClassifierVerbose classifier, string data) =>
      {
        Assert.NotNull(classifier);
        autoEvent.Set();
      }, classifierId))
      {
        Assert.Fail("Failed to invoke GetClassifier();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(5)]
    public void VisualRecognition_TestUpdateClassifier()
    {
      Log.Debug("TestVisualRecognition", "Attempting to update classifier...");
      positiveUpdatedExamples.Add(className_Turtle, positiveUpdated);

      if (!visualRecognition.UpdateClassifier((GetClassifiersPerClassifierVerbose classifier, string data) =>
      {
        Assert.NotNull(classifier);
        autoEvent.Set();
      }, classifierId, classifierName, positiveUpdatedExamples))
      {
        Assert.Fail("Failed to invoke UpdateClassifier();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(6)]
    public void VisualRecognition_TestClassifyGET()
    {
      Log.Debug("TestVisualRecognition", "Attempting to classify using GET...");

      if (!visualRecognition.Classify((ClassifyTopLevelMultiple classify, string data) =>
      {
        Assert.NotNull(classify);
        autoEvent.Set();
      }, imageURL, owners, classifierIds))
      {
        Assert.Fail("Failed to invoke Classify(); using GET");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(7)]
    public void VisualRecognition_TestClassifyPOST()
    {
      Log.Debug("TestVisualRecognition", "Attempting to classify using POST...");

      if (!visualRecognition.Classify(classifyImagePath, (ClassifyTopLevelMultiple classify, string data) =>
      {
        Assert.NotNull(classify);
        autoEvent.Set();
      }, owners, classifierIds))
      {
        Assert.Fail("Failed to invoke Classify(); using POST");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(8)]
    public void VisualRecognition_TestDetectFacesGET()
    {
      Log.Debug("TestVisualRecognition", "Attempting to detect faces using GET...");

      if (!visualRecognition.DetectFaces((FacesTopLevelMultiple faces, string data) =>
      {
        Assert.NotNull(faces);
        autoEvent.Set();
      }, imageFaceURL))
      {
        Assert.Fail("Failed to invoke DetectFaces(); using GET");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(9)]
    public void VisualRecognition_TestDetectFacesPOST()
    {
      Log.Debug("TestVisualRecognition", "Attempting to detect faces using POST...");

      if (!visualRecognition.DetectFaces(detectFaceImagePath, (FacesTopLevelMultiple faces, string data) =>
      {
        Assert.NotNull(faces);
        autoEvent.Set();
      }))
      {
        Assert.Fail("Failed to invoke DetectFaces(); using POST");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(10)]
    public void VisualRecognition_TestRecognizeTextGET()
    {
      Log.Debug("TestVisualRecognition", "Attempting to recognize text using GET...");

      if (!visualRecognition.RecognizeText((TextRecogTopLevelMultiple text, string data) =>
      {
        Assert.NotNull(text);
        autoEvent.Set();
      }, imageTextURL))
      {
        Assert.Fail("Failed to invoke RecognizeText(); using GET");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(11)]
    public void VisualRecognition_TestRecognizeTextPOST()
    {
      Log.Debug("TestVisualRecognition", "Attempting to recognize text using POST...");

      if (!visualRecognition.RecognizeText(recognizeTextImagePath, (TextRecogTopLevelMultiple text, string data) =>
      {
        Assert.NotNull(text);
        autoEvent.Set();
      }))
      {
        Assert.Fail("Failed to invoke RecognizeText(); using POST");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(12)]
    public void VisualRecognition_TestGetCollections()
    {
      Log.Debug("TestVisualRecognition", "Attempting to get collections...");

      if (!visualRecognition.GetCollections((GetCollections collections, string data) =>
      {
        Assert.NotNull(collections);
        autoEvent.Set();
      }))
      {
        Assert.Fail("Failed to invoke GetCollections();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(13)]
    public void VisualRecognition_TestCreateCollection()
    {
      Log.Debug("TestVisualRecognition", "Attempting to create collection...");

      if (!visualRecognition.CreateCollection((CreateCollection collection, string data) =>
      {
        createdCollectionID = collection.collection_id;
        Assert.NotNull(collection);
        autoEvent.Set();
      }, collectionName))
      {
        Assert.Fail("Failed to invoke CreateCollection();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(14)]
    public void VisualRecognition_TestGetCollection()
    {
      Log.Debug("TestVisualRecognition", "Attempting to get collection...");

      if (!visualRecognition.GetCollection((CreateCollection collection, string data) =>
      {
        Assert.NotNull(collection);
        autoEvent.Set();
      }, createdCollectionID))
      {
        Assert.Fail("Failed to invoke GetCollection();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(15)]
    public void VisualRecognition_TestGetCollectionImages()
    {
      Log.Debug("TestVisualRecognition", "Attempting to get collection images...");

      if (!visualRecognition.GetCollectionImages((GetCollectionImages images, string data) =>
      {
        Assert.NotNull(images);
        autoEvent.Set();
      }, createdCollectionID))
      {
        Assert.Fail("Failed to invoke GetCollectionImages();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(16)]
    public void VisualRecognition_TestAddCollectionImage()
    {
      Log.Debug("TestVisualRecognition", "Attempting to add collection image...");

      imageMetadata.Add("key1", "value1");
      imageMetadata.Add("key2", "value2");
      imageMetadata.Add("key3", "value3");

      if (!visualRecognition.AddCollectionImage((CollectionsConfig config, string data) =>
      {
        createdCollectionImage = config.images[0].image_id;
        Assert.NotNull(config);
        autoEvent.Set();
      }, createdCollectionID, collectionImagePath, imageMetadata))
      {
        Assert.Fail("Failed to invoke AddCollectionImage();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(17)]
    public void VisualRecognition_TestGetImage()
    {
      Log.Debug("TestVisualRecognition", "Attempting to get image details...");

      if (!visualRecognition.GetImage((GetCollectionsBrief image, string data) =>
      {
        Assert.NotNull(image);
        autoEvent.Set();
      }, createdCollectionID, createdCollectionImage))
      {
        Assert.Fail("Failed to invoke GetImage();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }


    [Test, Order(18)]
    public void VisualRecognition_TestGetMetadata()
    {
      Log.Debug("TestVisualRecognition", "Attempting to get metadata...");

      if (!visualRecognition.GetMetadata((object metadata, string data) =>
      {
        Assert.NotNull(metadata);
        autoEvent.Set();
      }, createdCollectionID, createdCollectionImage))
      {
        Assert.Fail("Failed to invoke GetMetadata();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(19)]
    public void VisualRecognition_TestFindSimilar()
    {
      Log.Debug("TestVisualRecognition", "Attempting to FindSimilar...");

      if (!visualRecognition.FindSimilar((SimilarImagesConfig similarImages, string data) =>
       {
         Assert.NotNull(similarImages);
         autoEvent.Set();
       }, createdCollectionID, collectionImagePath))
      {
        Assert.Fail("Failed to invoke FindSimilar();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(20)]
    public void VisualRecognition_TestDeleteImageMetadata()
    {
      Log.Debug("TestVisualRecognition", "Attempting to delete image metadata...");

      if (!visualRecognition.DeleteCollectionImageMetadata((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCollectionID, createdCollectionImage))
      {
        Assert.Fail("Failed to invoke DeleteCollectionImageMetadata();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(21)]
    public void VisualRecognition_TestDeleteCollectionImage()
    {
      Log.Debug("TestVisualRecognition", "Attempting to delte an image from collection...");

      if (!visualRecognition.DeleteCollectionImage((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCollectionID, createdCollectionImage))
      {
        Assert.Fail("Failed to invoke DeleteCollectionImage();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(22)]
    public void VisualRecognition_TestDeleteCollection()
    {
      Log.Debug("TestVisualRecognition", "Attempting to delete collection...");

      if (!visualRecognition.DeleteCollection((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdCollectionID))
      {
        Assert.Fail("Failed to invoke DeleteCollection();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(23)]
    public void VisualRecognition_TestDeleteClassifier()
    {
      Log.Debug("TestVisualRecognition", "Attempting to delete classifier {0}...", classifierId);

      if (!visualRecognition.DeleteClassifier((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, classifierId))
      {
        Assert.Fail("Failed to invoke DeleteClassifier();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }
  }
}
