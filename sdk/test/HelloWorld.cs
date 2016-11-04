using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.AlchemyAPI.v1;
using IBM.Watson.DeveloperCloud.Services.ToneAnalyzer.v3;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using System;
using System.IO;

namespace IBM.Watson.DeveloperCloud.Test
{
  class HelloWorld
  {
    public static void Main()
    {
      LogSystem.InstallDefaultReactors();

      Log.Debug("HelloWorld", "Hello, World! {0}", Directory.GetCurrentDirectory());
      Log.Debug("HelloWorld", "Press any key to continue...");

      //ToneAnalyzerTest toneAnalyzerTest = new ToneAnalyzerTest();
      //toneAnalyzerTest.TestToneAnalyzer();

      //AlchemyAPITest alchemyAPITest = new AlchemyAPITest();
      //alchemyAPITest.TestAlchemyAPI();

      //VisualRecognitionTest visualRecognitionTest = new VisualRecognitionTest();
      //visualRecognitionTest.TestVisualRecognition();

      Console.ReadKey();
    }
  }

  #region Test Tone Analyzer
  class ToneAnalyzerTest
  {
    private ToneAnalyzer m_ToneAnalyzer = new ToneAnalyzer();
    private string m_TextString = "Hello, what a beautiful day!";
    public ToneAnalyzerTest()
    {
      Log.Debug("ToneAnalyzerTest", "Constructor!");
    }

    public void TestToneAnalyzer()
    {
      Log.Debug("ToneAnalyzerTest", "Attempting to analyze tone on '{0}'", m_TextString);
      if (!m_ToneAnalyzer.GetToneAnalyze(OnGetToneAnalyzed, m_TextString))
        Console.WriteLine("Failed to get tone!");
    }

    private void OnGetToneAnalyzed(ToneAnalyzerResponse resp, string data)
    {
      if (resp != null)
      {
        if (resp.document_tone != null)
        {
          if (resp.document_tone.tone_categories != null && resp.document_tone.tone_categories.Length > 0)
          {
            foreach (ToneCategory category in resp.document_tone.tone_categories)
            {
              Log.Debug("ToneAnalyzerTest", category.ToString());
            }
          }
        }

        if (resp.sentences_tone != null && resp.sentences_tone.Length > 0)
        {
          Log.Debug("ToneAnalyzerTest", resp.sentences_tone.ToString());
        }
      }
      else
      {
        Log.Debug("ToneAnalyzerTest", "OnGetToneAnalyzed: resp is null!");
      }
    }
  }
  #endregion

  #region Test Alchemy API
  class AlchemyAPITest
  {
    private AlchemyAPI m_AlchemyAPI = new AlchemyAPI();
    private string m_TestString = "Show changes between the working tree and the index or a tree, changes between the index and a tree, changes between two trees, changes between two blob objects, or changes between two files on disk.";

    public AlchemyAPITest()
    {
      Log.Debug("AlchemyAPITest", "Constructor!");
    }

    public void TestAlchemyAPI()
    {
      Log.Debug("AlchemyAPITest", "Attempting to invoke combined call on '{0}'", m_TestString);

      if (!m_AlchemyAPI.GetCombinedData(OnGetCombinedData, m_TestString))
        Console.WriteLine("Failed to get combined call!");
    }
    
    public void OnGetCombinedData(CombinedCallData combinedData, string data)
    {
      if (combinedData != null)
      {
        Log.Debug("AlchemyAPITest", "status: {0}", combinedData.status);
        Log.Debug("AlchemyAPITest", "url: {0}", combinedData.url);
        Log.Debug("AlchemyAPITest", "language: {0}", combinedData.language);
        Log.Debug("AlchemyAPITest", "text: {0}", combinedData.text);
        Log.Debug("AlchemyAPITest", "image: {0}", combinedData.image);

        if (combinedData.imageKeywords != null && combinedData.imageKeywords.Length > 0)
          foreach (ImageKeyword imageKeyword in combinedData.imageKeywords)
            Log.Debug("AlchemyAPITest", "ImageKeyword: {0}, Score: {1}", imageKeyword.text, imageKeyword.score);

        if (combinedData.publicationDate != null)
          Log.Debug("AlchemyAPITest", "publicationDate: {0}, Score: {1}", combinedData.publicationDate.date, combinedData.publicationDate.confident);

        if (combinedData.authors != null && combinedData.authors.names.Length > 0)
          foreach (string authors in combinedData.authors.names)
            Log.Debug("AlchemyAPITest", "Authors: {0}", authors);

        if (combinedData.docSentiment != null)
          Log.Debug("AlchemyAPITest", "DocSentiment: {0}, Score: {1}, Mixed: {2}", combinedData.docSentiment.type, combinedData.docSentiment.score, combinedData.docSentiment.mixed);

        if (combinedData.feeds != null && combinedData.feeds.Length > 0)
          foreach (Feed feed in combinedData.feeds)
            Log.Debug("AlchemyAPITest", "Feeds: {0}", feed.feed);

        if (combinedData.keywords != null && combinedData.keywords.Length > 0)
          foreach (Keyword keyword in combinedData.keywords)
            Log.Debug("AlchemyAPITest", "Keyword: {0}, relevance: {1}", keyword.text, keyword.relevance);

        if (combinedData.concepts != null && combinedData.concepts.Length > 0)
          foreach (Concept concept in combinedData.concepts)
            Log.Debug("AlchemyAPITest", "Concept: {0}, Relevance: {1}", concept.text, concept.relevance);

        if (combinedData.entities != null && combinedData.entities.Length > 0)
          foreach (Entity entity in combinedData.entities)
            Log.Debug("AlchemyAPITest", "Entity: {0}, Type: {1}, Relevance: {2}", entity.text, entity.type, entity.relevance);

        if (combinedData.relations != null && combinedData.relations.Length > 0)
          foreach (Relation relation in combinedData.relations)
            Log.Debug("AlchemyAPITest", "Relations: {0}", relation.subject.text);

        if (combinedData.taxonomy != null && combinedData.taxonomy.Length > 0)
          foreach (Taxonomy taxonomy in combinedData.taxonomy)
            Log.Debug("AlchemyAPITest", "Taxonomy: {0}, Score: {1}, Confident: {2}", taxonomy.label, taxonomy.score, taxonomy.confident);

        if (combinedData.dates != null && combinedData.dates.Length > 0)
          foreach (Date date in combinedData.dates)
            Log.Debug("AlchemyAPITest", "Dates", date.text, date.date);

        if (combinedData.docEmotions != null && combinedData.docEmotions.Length > 0)
          foreach (DocEmotions emotions in combinedData.docEmotions)
            Log.Debug("AlchemyAPITest", "Doc Emotions: anger: {0}, disgust: {1}, fear: {2}, joy: {3}, sadness: {4}", emotions.anger, emotions.disgust, emotions.fear, emotions.joy, emotions.sadness);
      }
      else
      {
        Log.Debug("AlchemyAPITest", "Failed to get combined data!");
      }
    }
  }
  #endregion

  #region Test Visual Recognition
  class VisualRecognitionTest
  {
    private VisualRecognition m_VisualRecognition = new VisualRecognition();
    private string m_CollectionName = "dot-net-sdk-test-collection";
    private string m_CollectionID = "";
    public VisualRecognitionTest()
    {
      Log.Debug("VisualRecognitionTest", "Constructor!");
    }

    public void TestVisualRecognition()
    {
      CreateCollection(m_CollectionName);
    }

    #region Create Collection
    private void CreateCollection(string collectionName)
    {
      Log.Debug("VisualRecognitionTest", "Attempting to create collection '{0}'", collectionName);
      if (!m_VisualRecognition.CreateCollection(OnCreateCollection, collectionName))
        Log.Debug("VisualRecognitionTest", "Failed to create collection!");
    }

    private void OnCreateCollection(CreateCollection collection, string data)
    {
      if(collection != null)
      {
        Log.Debug("VisualRecognitionTest", "Collection: name: {0} | id: {1} | status: {2}, created: {3}, images: {4}", 
          collection.name, 
          collection.collection_id, 
          collection.status,
          collection.created,
          collection.images
          );

        if (!string.IsNullOrEmpty(collection.collection_id))
        {
          m_CollectionID = collection.collection_id;
          DeleteCollection(m_CollectionID);
        }
        else
        {
          Log.Debug("VisualRecognitonTest", "Collection creation failed! collection_id is missing! '{0}'", collection.collection_id);
        }
      }
      else
      {
        Log.Debug("VisualRecognitonTest", "Collection creation failed! Collection is null!");
      }
    }

    #region Delete Collection
    private void DeleteCollection(string collectionID)
    {
      Log.Debug("VisualRecognitionTest", "Attempting to delete collection '{0}'", collectionID);

      if (!m_VisualRecognition.DeleteCollection(OnDeleteCollection, collectionID))
        Log.Debug("VisualRecognitionTest", "Failed to create collection!");
    }

    private void OnDeleteCollection(bool success, string data)
    {
      if(success)
      {
        Log.Debug("VisualRecognitonTest", "Collection '{0}' deletion succeeded!", m_CollectionID);

        GetCollection(m_CollectionID);
      }
      else
      {
        Log.Debug("VisualRecognitonTest", "Collection '{0}' deletion failed!", m_CollectionID);
      }
    }
    #endregion

    #region Get Collections
    private void GetCollection(string collectionID)
    {
      Log.Debug("VisualRecognitionTest", "Attempting to get collection '{0}'. This should return nothing.", collectionID);

      if (!m_VisualRecognition.GetCollection(OnGetCollection, collectionID))
        Log.Debug("VisualRecognitionTest", "Failed to get collection!");
    }

    private void OnGetCollection(CreateCollection collection, string data)
    {
      if(!string.IsNullOrEmpty(collection.collection_id))
      {
        Log.Debug("VisualRecognitionTest", "This is not expected: Collection found!: name: {0} | id: {1} | status: {2}, created: {3}, images: {4}",
          collection.name,
          collection.collection_id,
          collection.status,
          collection.created,
          collection.images
          );
      }
      else
      {
        Log.Debug("VisualRecognitonTest", "This is expected! Failed to get collection '{0}'! Collection does not exist!", m_CollectionID);
      }
    }
    #endregion
  }
  #endregion
  #endregion
}
