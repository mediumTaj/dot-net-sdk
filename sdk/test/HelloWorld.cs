using IBM.Watson.DeveloperCloud.Logging;
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

      ToneAnalyzerTest toneAnalyzerTest = new ToneAnalyzerTest();
      toneAnalyzerTest.TestToneAnalyzer();

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
              Log.Debug("HelloWorld", resp.sentences_tone.ToString());
      }
      else
      {
        Log.Debug("HelloWorld", "OnGetToneAnalyzed: resp is null!");
      }
    }
  }
}
