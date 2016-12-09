

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
using IBM.Watson.DeveloperCloud.Services.AlchemyAPI.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;
using System.IO;
using System.Threading;

namespace sdk.test
{
  [TestFixture]
  public class TestAlchemyLanguage
  {
    private AlchemyAPI alchemyLanguage = new AlchemyAPI();
    private string exampleURL_article = "http://www.nytimes.com/2011/02/17/science/17jeopardy-watson.html?_r=0";
    private string exampleText_article = "Computer Wins on 'Jeopardy!': Trivial, It's Not\nBy JOHN MARKOFF\nYORKTOWN HEIGHTS, N.Y. — In the end, the humans on \"Jeopardy!\" surrendered meekly.\n\nFacing certain defeat at the hands of a room-size I.B.M. computer on Wednesday evening, Ken Jennings, famous for winning 74 games in a row on the TV quiz show, acknowledged the obvious. \"I, for one, welcome our new computer overlords,\" he wrote on his video screen, borrowing a line from a \"Simpsons\" episode.\n\nFrom now on, if the answer is \"the computer champion on \"Jeopardy!,\" the question will be, \"What is Watson?\"\n\nFor I.B.M., the showdown was not merely a well-publicized stunt and a $1 million prize, but proof that the company has taken a big step toward a world in which intelligent machines will understand and respond to humans, and perhaps inevitably, replace some of them.\n\nWatson, specifically, is a \"question answering machine\" of a type that artificial intelligence researchers have struggled with for decades — a computer akin to the one on \"Star Trek\" that can understand questions posed in natural language and answer them.\n\nWatson showed itself to be imperfect, but researchers at I.B.M. and other companies are already developing uses for Watson's technologies that could have a significant impact on the way doctors practice and consumers buy products.\n\n\"Cast your mind back 20 years and who would have thought this was possible?\" said Edward Feigenbaum, a Stanford University computer scientist and a pioneer in the field.\n\nIn its \"Jeopardy!\" project, I.B.M. researchers were tackling a game that requires not only encyclopedic recall, but also the ability to untangle convoluted and often opaque statements, a modicum of luck, and quick, strategic button pressing.\n\nThe contest, which was taped in January here at the company's T. J. Watson Research Laboratory before an audience of I.B.M. executives and company clients, played out in three televised episodes concluding Wednesday. At the end of the first day, Watson was in a tie with Brad Rutter, another ace human player, at $5,000 each, with Mr. Jennings trailing with $2,000.\n\nBut on the second day, Watson went on a tear. By night's end, Watson had a commanding lead with a total of $35,734, compared with Mr. Rutter's $10,400 and Mr. Jennings's $4,800.\n\nVictory was not cemented until late in the third match, when Watson was in Nonfiction. \"Same category for $1,200,\" it said in a manufactured tenor, and lucked into a Daily Double. Mr. Jennings grimaced.\n\nEven later in the match, however, had Mr. Jennings won another key Daily Double it might have come down to Final Jeopardy, I.B.M. researchers acknowledged.\n\nThe final tally was $77,147 to Mr. Jennings's $24,000 and Mr. Rutter's $21,600.\n\nMore than anything, the contest was a vindication for the academic field of artificial intelligence, which began with great promise in the 1960s with the vision of creating a thinking machine and which became the laughingstock of Silicon Valley in the 1980s, when a series of heavily financed start-up companies went bankrupt.\n\nDespite its intellectual prowess, Watson was by no means omniscient. On Tuesday evening during Final Jeopardy, the category was U.S. Cities and the clue was: \"Its largest airport is named for a World War II hero; its second largest for a World War II battle.\"\n\nWatson drew guffaws from many in the television audience when it responded \"What is Toronto?????\"\n\nThe string of question marks indicated that the system had very low confidence in its response, I.B.M. researchers said, but because it was Final Jeopardy, it was forced to give a response. The machine did not suffer much damage. It had wagered just $947 on its result. (The correct answer is, \"What is Chicago?\")\n\n\"We failed to deeply understand what was going on there,\" said David Ferrucci, an I.B.M. researcher who led the development of Watson. \"The reality is that there's lots of data where the title is U.S. cities and the answers are countries, European cities, people, mayors. Even though it says U.S. cities, we had very little confidence that that's the distinguishing feature.\"\n\nThe researchers also acknowledged that the machine had benefited from the \"buzzer factor.\"\n\nBoth Mr. Jennings and Mr. Rutter are accomplished at anticipating the light that signals it is possible to \"buzz in,\" and can sometimes get in with virtually zero lag time. The danger is to buzz too early, in which case the contestant is penalized and \"locked out\" for roughly a quarter of a second.\n\nWatson, on the other hand, does not anticipate the light, but has a weighted scheme that allows it, when it is highly confident, to hit the buzzer in as little as 10 milliseconds, making it very hard for humans to beat. When it was less confident, it took longer to  buzz in. In the second round, Watson beat the others to the buzzer in 24 out of 30 Double Jeopardy questions.\n\n\"It sort of wants to get beaten when it doesn't have high confidence,\" Dr. Ferrucci said. \"It doesn't want to look stupid.\"\n\nBoth human players said that Watson's button pushing skill was not necessarily an unfair advantage. \"I beat Watson a couple of times,\" Mr. Rutter said.\n\nWhen Watson did buzz in, it made the most of it. Showing the ability to parse language, it responded to, \"A recent best seller by Muriel Barbery is called 'This of the Hedgehog,' \" with \"What is Elegance?\"\n\nIt showed its facility with medical diagnosis. With the answer: \"You just need a nap. You don't have this sleep disorder that can make sufferers nod off while standing up,\" Watson replied, \"What is narcolepsy?\"\n\nThe coup de grâce came with the answer, \"William Wilkenson's 'An Account of the Principalities of Wallachia and Moldavia' inspired this author's most famous novel.\" Mr. Jennings wrote, correctly, Bram Stoker, but realized that he could not catch up with Watson's winnings and wrote out his surrender.\n\nBoth players took the contest and its outcome philosophically.\n\n\"I had a great time and I would do it again in a heartbeat,\" said Mr. Jennings. \"It's not about the results; this is about being part of the future.\"\n\nFor I.B.M., the future will happen very quickly, company executives said. On Thursday it plans to announce that it will collaborate with Columbia University and the University of Maryland to create a physician's assistant service that will allow doctors to query a cybernetic assistant. The company also plans to work with Nuance Communications Inc. to add voice recognition to the physician's assistant, possibly making the service available in as little as 18 months.\n\n\"I have been in medical education for 40 years and we're still a very memory-based curriculum,\" said Dr. Herbert Chase, a professor of clinical medicine at Columbia University who is working with I.B.M. on the physician's assistant. \"The power of Watson- like tools will cause us to reconsider what it is we want students to do.\"\n\nI.B.M. executives also said they are in discussions with a major consumer electronics retailer to develop a version of Watson, named after I.B.M.'s founder, Thomas J. Watson, that would be able to interact with consumers on a variety of subjects like buying decisions and technical support.\n\nDr. Ferrucci sees none of the fears that have been expressed by theorists and science fiction writers about the potential of computers to usurp humans.\n\n\"People ask me if this is HAL,\" he said, referring to the computer in \"2001: A Space Odyssey.\" \"HAL's not the focus; the focus is on the computer on 'Star Trek,' where you have this intelligent information seek dialogue, where you can ask follow-up questions and the computer can look at all the evidence and tries to ask follow-up questions. That's very cool.\"\n\nThis article has been revised to reflect the following correction:\n\nCorrection: February 24, 2011\n\n\nAn article last Thursday about the I.B.M. computer Watson misidentified the academic field vindicated by Watson's besting of two human opponents on \"Jeopardy!\" It is artificial intelligence — not computer science, a broader field that includes artificial intelligence.";
    private string example_html_article;
    private string exampleURL_feed = "https://news.ycombinator.com/";
    private string exampleURL_microformats = "http://microformats.org/wiki/hcard";
    AutoResetEvent autoEvent = new AutoResetEvent(false);

    [SetUp]
    public void Init()
    {
      string testDataPath = TestContext.CurrentContext.TestDirectory + Path.DirectorySeparatorChar + Constants.Path.APP_DATA + Path.DirectorySeparatorChar;

      if (!Config.Instance.ConfigLoaded)
      {
        string configPath = testDataPath + Constants.Path.CONFIG_FILE;
        string configJson = File.ReadAllText(configPath);
        Config.Instance.LoadConfig(configJson);
      }

      if (!Config.Instance.ConfigLoaded)
        Assert.Fail("Failed to load Config.");

      example_html_article = testDataPath + "watson_beats_jeopardy.html";
    }

    [Test]
    public void TestGetAuthorsURL()
    {
      if (!alchemyLanguage.GetAuthors((AuthorsData authors, string data) =>
       {
         Assert.AreNotEqual(authors, null);
         autoEvent.Set();
       }, exampleURL_article))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetAuthorsHTML()
    {
      if (!alchemyLanguage.GetAuthors((AuthorsData authors, string data) =>
      {
        Assert.AreNotEqual(authors, null);
        autoEvent.Set();
      }, example_html_article))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRankedConceptsHTML()
    {
      if (!alchemyLanguage.GetRankedConcepts((ConceptsData concepts, string data) =>
      {
        Assert.AreNotEqual(concepts, null);
        autoEvent.Set();
      }, example_html_article, 8, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRankConceptsURL()
    {
      if (!alchemyLanguage.GetRankedConcepts((ConceptsData concepts, string data) =>
      {
        Assert.AreNotEqual(concepts, null);
        autoEvent.Set();
      }, exampleURL_article, 8, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRankedConceptsText()
    {
      if (!alchemyLanguage.GetRankedConcepts((ConceptsData concepts, string data) =>
      {
        Assert.AreNotEqual(concepts, null);
        autoEvent.Set();
      }, exampleText_article, 8, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetDatesURL()
    {
      if (!alchemyLanguage.GetDates((DateData dates, string data) =>
      {
        Assert.AreNotEqual(dates, null);
        autoEvent.Set();
      }, exampleURL_article, null, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetDatesText()
    {
      if (!alchemyLanguage.GetDates((DateData dates, string data) =>
      {
        Assert.AreNotEqual(dates, null);
        autoEvent.Set();
      }, exampleText_article, null, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetDatesHTML()
    {
      if (!alchemyLanguage.GetDates((DateData dates, string data) =>
      {
        Assert.AreNotEqual(dates, null);
        autoEvent.Set();
      }, example_html_article, null, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetEmotionsURL()
    {
      if (!alchemyLanguage.GetEmotions((EmotionData emotions, string data) =>
      {
        Assert.AreNotEqual(emotions, null);
        autoEvent.Set();
      }, exampleURL_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetEmotionsText()
    {
      if (!alchemyLanguage.GetEmotions((EmotionData emotions, string data) =>
      {
        Assert.AreNotEqual(emotions, null);
        autoEvent.Set();
      }, exampleText_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetEmotionsHTML()
    {
      if (!alchemyLanguage.GetEmotions((EmotionData emotions, string data) =>
      {
        Assert.AreNotEqual(emotions, null);
        autoEvent.Set();
      }, example_html_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestExtractEntitiesURL()
    {
      if (!alchemyLanguage.ExtractEntities((EntityData entityData, string data) =>
      {
        Assert.AreNotEqual(entityData, null);
        autoEvent.Set();
      }, exampleURL_article, 50, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestExtractEntitiesText()
    {
      if (!alchemyLanguage.ExtractEntities((EntityData entityData, string data) =>
      {
        Assert.AreNotEqual(entityData, null);
        autoEvent.Set();
      }, exampleText_article, 50, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestExtractEntitiesHTML()
    {
      if (!alchemyLanguage.ExtractEntities((EntityData entityData, string data) =>
      {
        Assert.AreNotEqual(entityData, null);
        autoEvent.Set();
      }, example_html_article, 50, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestDetectFeeds()
    {
      if (!alchemyLanguage.DetectFeeds((FeedData feedData, string data) =>
      {
        Assert.AreNotEqual(feedData, null);
        autoEvent.Set();
      }, exampleURL_feed))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestExtractKeywordsURL()
    {
      if (!alchemyLanguage.ExtractKeywords((KeywordData keywordData, string data) =>
      {
        Assert.AreNotEqual(keywordData, null);
        autoEvent.Set();
      }, exampleURL_article, 50, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestExtractKeywordsText()
    {
      if (!alchemyLanguage.ExtractKeywords((KeywordData keywordData, string data) =>
      {
        Assert.AreNotEqual(keywordData, null);
        autoEvent.Set();
      }, exampleText_article, 50, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestExtractKeywordsHTML()
    {
      if (!alchemyLanguage.ExtractKeywords((KeywordData keywordData, string data) =>
      {
        Assert.AreNotEqual(keywordData, null);
        autoEvent.Set();
      }, example_html_article, 50, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetLanguagesURL()
    {
      if (!alchemyLanguage.GetLanguages((LanguageData languages, string data) =>
      {
        Assert.AreNotEqual(languages, null);
        autoEvent.Set();
      }, exampleURL_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetLanguagesText()
    {
      if (!alchemyLanguage.GetLanguages((LanguageData languages, string data) =>
      {
        Assert.AreNotEqual(languages, null);
        autoEvent.Set();
      }, exampleText_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetLanguagesHTML()
    {
      if (!alchemyLanguage.GetLanguages((LanguageData languages, string data) =>
      {
        Assert.AreNotEqual(languages, null);
        autoEvent.Set();
      }, example_html_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetMicroformatsURL()
    {
      if (!alchemyLanguage.GetMicroformats((MicroformatData microformats, string data) =>
      {
        Assert.AreNotEqual(microformats, null);
        autoEvent.Set();
      }, exampleURL_microformats))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetPublicationDateURL()
    {
      if (!alchemyLanguage.GetPublicationDate((PubDateData pubDates, string data) =>
      {
        Assert.AreNotEqual(pubDates, null);
        autoEvent.Set();
      }, exampleURL_article))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetPublicationDateHTML()
    {
      if (!alchemyLanguage.GetPublicationDate((PubDateData pubDates, string data) =>
      {
        Assert.AreNotEqual(pubDates, null);
        autoEvent.Set();
      }, example_html_article))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRelationsURL()
    {
      if (!alchemyLanguage.GetRelations((RelationsData relationsData, string data) =>
      {
        Assert.AreNotEqual(relationsData, null);
        autoEvent.Set();
      }, exampleURL_article, 50, true, true, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRelationsText()
    {
      if (!alchemyLanguage.GetRelations((RelationsData relationsData, string data) =>
      {
        Assert.AreNotEqual(relationsData, null);
        autoEvent.Set();
      }, exampleText_article, 50, true, true, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRelationsHTML()
    {
      if (!alchemyLanguage.GetRelations((RelationsData relationsData, string data) =>
      {
        Assert.AreNotEqual(relationsData, null);
        autoEvent.Set();
      }, example_html_article, 50, true, true, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTextSentimentURL()
    {
      if (!alchemyLanguage.GetTextSentiment((SentimentData sentimentData, string data) =>
      {
        Assert.AreNotEqual(sentimentData, null);
        autoEvent.Set();
      }, exampleURL_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTextSentimentText()
    {
      if (!alchemyLanguage.GetTextSentiment((SentimentData sentimentData, string data) =>
      {
        Assert.AreNotEqual(sentimentData, null);
        autoEvent.Set();
      }, exampleText_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTextSentimentHTML()
    {
      if (!alchemyLanguage.GetTextSentiment((SentimentData sentimentData, string data) =>
      {
        Assert.AreNotEqual(sentimentData, null);
        autoEvent.Set();
      }, example_html_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTargetedSentimentURL()
    {
      if (!alchemyLanguage.GetTargetedSentiment((TargetedSentimentData sentimentData, string data) =>
      {
        Assert.AreNotEqual(sentimentData, null);
        autoEvent.Set();
      }, exampleURL_article, "Jeopardy|Jennings|Watson", true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTargetedSentimentText()
    {
      if (!alchemyLanguage.GetTargetedSentiment((TargetedSentimentData sentimentData, string data) =>
      {
        Assert.AreNotEqual(sentimentData, null);
        autoEvent.Set();
      }, exampleText_article, "Jeopardy|Jennings|Watson", true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTargetedSentimentHTML()
    {
      if (!alchemyLanguage.GetTargetedSentiment((TargetedSentimentData sentimentData, string data) =>
      {
        Assert.AreNotEqual(sentimentData, null);
        autoEvent.Set();
      }, example_html_article, "Jeopardy|Jennings|Watson", true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRankedTaxonomyURL()
    {
      if (!alchemyLanguage.GetRankedTaxonomy((TaxonomyData taxonomyData, string data) =>
      {
        Assert.AreNotEqual(taxonomyData, null);
        autoEvent.Set();
      }, exampleURL_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRankedTaxonomyText()
    {
      if (!alchemyLanguage.GetRankedTaxonomy((TaxonomyData taxonomyData, string data) =>
      {
        Assert.AreNotEqual(taxonomyData, null);
        autoEvent.Set();
      }, exampleText_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRankedTaxonomyHTML()
    {
      if (!alchemyLanguage.GetRankedTaxonomy((TaxonomyData taxonomyData, string data) =>
      {
        Assert.AreNotEqual(taxonomyData, null);
        autoEvent.Set();
      }, example_html_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTextURL()
    {
      if (!alchemyLanguage.GetText((TextData textData, string data) =>
      {
        Assert.AreNotEqual(textData, null);
        autoEvent.Set();
      }, exampleURL_article, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTextHTML()
    {
      if (!alchemyLanguage.GetText((TextData textData, string data) =>
      {
        Assert.AreNotEqual(textData, null);
        autoEvent.Set();
      }, example_html_article, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRawTextURL()
    {
      if (!alchemyLanguage.GetRawText((TextData textData, string data) =>
      {
        Assert.AreNotEqual(textData, null);
        autoEvent.Set();
      }, exampleURL_article))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetRawTextHTML()
    {
      if (!alchemyLanguage.GetRawText((TextData textData, string data) =>
      {
        Assert.AreNotEqual(textData, null);
        autoEvent.Set();
      }, example_html_article))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTitleURL()
    {
      if (!alchemyLanguage.GetTitle((Title titleData, string data) =>
      {
        Assert.AreNotEqual(titleData, null);
        autoEvent.Set();
      }, exampleURL_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetTitleHTML()
    {
      if (!alchemyLanguage.GetTitle((Title titleData, string data) =>
      {
        Assert.AreNotEqual(titleData, null);
        autoEvent.Set();
      }, example_html_article, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetCombinedDataURL()
    {
      if (!alchemyLanguage.GetCombinedData((CombinedCallData combinedData, string data) =>
      {
        Assert.AreNotEqual(combinedData, null);
        autoEvent.Set();
      }, exampleURL_article, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetCombinedDataText()
    {
      if (!alchemyLanguage.GetCombinedData((CombinedCallData combinedData, string data) =>
      {
        Assert.AreNotEqual(combinedData, null);
        autoEvent.Set();
      }, exampleText_article, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetCombinedDataHTML()
    {
      if (!alchemyLanguage.GetCombinedData((CombinedCallData combinedData, string data) =>
      {
        Assert.AreNotEqual(combinedData, null);
        autoEvent.Set();
      }, example_html_article, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }
  }
}
