using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.AlchemyAPI.v1;
using IBM.Watson.DeveloperCloud.Services.ToneAnalyzer.v3;
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
      Log.Debug("HelloWorld", "Testing Tone Analyzer....");

      //ToneAnalyzerTest toneAnalyzerTest = new ToneAnalyzerTest();
      //toneAnalyzerTest.TestToneAnalyzer();

      AlchemyAPITest alchemyAPITest = new AlchemyAPITest();
      alchemyAPITest.TestAlchemyAPI();
      
      Console.ReadKey();
    }
  }

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
      if (!m_ToneAnalyzer.GetToneAnalyze(m_TextString, OnGetToneAnalyzed))
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
              Log.Debug("HelloWorld", category.ToString());
            }
          }
        }

        if (resp.sentences_tone != null && resp.sentences_tone.Length > 0)
        {
          Log.Debug("HelloWorld", resp.sentences_tone.ToString());
        }
      }
      else
      {
        Log.Debug("HelloWorld", "OnGetToneAnalyzed: resp is null!");
      }
    }
  }

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
      if (!m_AlchemyAPI.GetCombinedData(OnGetCombinedData, m_TestString))
        Console.WriteLine("Failed to get combined call!");
    }
    
    public void OnGetCombinedData(CombinedCallData combinedData, string data)
    {
      if (combinedData != null)
      {
        Log.Debug("ExampleAlchemyLanguage", "status: {0}", combinedData.status);
        Log.Debug("ExampleAlchemyLanguage", "url: {0}", combinedData.url);
        Log.Debug("ExampleAlchemyLanguage", "language: {0}", combinedData.language);
        Log.Debug("ExampleAlchemyLanguage", "text: {0}", combinedData.text);
        Log.Debug("ExampleAlchemyLanguage", "image: {0}", combinedData.image);

        if (combinedData.imageKeywords != null && combinedData.imageKeywords.Length > 0)
          foreach (ImageKeyword imageKeyword in combinedData.imageKeywords)
            Log.Debug("ExampleAlchemyLanguage", "ImageKeyword: {0}, Score: {1}", imageKeyword.text, imageKeyword.score);

        if (combinedData.publicationDate != null)
          Log.Debug("ExampleAlchemyLanguage", "publicationDate: {0}, Score: {1}", combinedData.publicationDate.date, combinedData.publicationDate.confident);

        if (combinedData.authors != null && combinedData.authors.names.Length > 0)
          foreach (string authors in combinedData.authors.names)
            Log.Debug("ExampleAlchemyLanguage", "Authors: {0}", authors);

        if (combinedData.docSentiment != null)
          Log.Debug("ExampleAlchemyLanguage", "DocSentiment: {0}, Score: {1}, Mixed: {2}", combinedData.docSentiment.type, combinedData.docSentiment.score, combinedData.docSentiment.mixed);

        if (combinedData.feeds != null && combinedData.feeds.Length > 0)
          foreach (Feed feed in combinedData.feeds)
            Log.Debug("ExampleAlchemyLanguage", "Feeds: {0}", feed.feed);

        if (combinedData.keywords != null && combinedData.keywords.Length > 0)
          foreach (Keyword keyword in combinedData.keywords)
            Log.Debug("ExampleAlchemyLanguage", "Keyword: {0}, relevance: {1}", keyword.text, keyword.relevance);

        if (combinedData.concepts != null && combinedData.concepts.Length > 0)
          foreach (Concept concept in combinedData.concepts)
            Log.Debug("ExampleAlchemyLanguage", "Concept: {0}, Relevance: {1}", concept.text, concept.relevance);

        if (combinedData.entities != null && combinedData.entities.Length > 0)
          foreach (Entity entity in combinedData.entities)
            Log.Debug("ExampleAlchemyLanguage", "Entity: {0}, Type: {1}, Relevance: {2}", entity.text, entity.type, entity.relevance);

        if (combinedData.relations != null && combinedData.relations.Length > 0)
          foreach (Relation relation in combinedData.relations)
            Log.Debug("ExampleAlchemyLanguage", "Relations: {0}", relation.subject.text);

        if (combinedData.taxonomy != null && combinedData.taxonomy.Length > 0)
          foreach (Taxonomy taxonomy in combinedData.taxonomy)
            Log.Debug("ExampleAlchemyLanguage", "Taxonomy: {0}, Score: {1}, Confident: {2}", taxonomy.label, taxonomy.score, taxonomy.confident);

        if (combinedData.dates != null && combinedData.dates.Length > 0)
          foreach (Date date in combinedData.dates)
            Log.Debug("ExampleAlchemyLanguage", "Dates", date.text, date.date);

        if (combinedData.docEmotions != null && combinedData.docEmotions.Length > 0)
          foreach (DocEmotions emotions in combinedData.docEmotions)
            Log.Debug("ExampleAlchemyLanguage", "Doc Emotions: anger: {0}, disgust: {1}, fear: {2}, joy: {3}, sadness: {4}", emotions.anger, emotions.disgust, emotions.fear, emotions.joy, emotions.sadness);
      }
      else
      {
        Log.Debug("ExampleAlchemyLanguage", "Failed to get combined data!");
      }
    }
  }
}
