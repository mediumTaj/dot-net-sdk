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
using IBM.Watson.DeveloperCloud.Services.LanguageTranslator.v1;
using System.Threading;

namespace sdk.test
{
  [TestFixture]
  public class TestLanguageTranslator
  {
    LanguageTranslator languageTranslator = new LanguageTranslator();
    private string languageModel = "en-es";
    private string query = "Where is the library?";
    private string fromLanguage = "en";
    private string toLanguage = "es";
    AutoResetEvent autoEvent = new AutoResetEvent(false);

    [Test]
    public void TestGetModel()
    {
      if (!languageTranslator.GetModel(languageModel, (TranslationModel model) =>
       {
         Assert.AreNotEqual(model, null);
         autoEvent.Set();
       }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetModels()
    {
      if (!languageTranslator.GetModels((TranslationModels models) =>
       {
         Assert.AreNotEqual(models, null);
         autoEvent.Set();
       }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestGetLanguages()
    {
      if (!languageTranslator.GetLanguages((Languages languages) =>
      {
        Assert.AreNotEqual(languages, null);
        autoEvent.Set();
      }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestIdentify()
    {
      if (!languageTranslator.Identify(query, (string lang) =>
      {
        Assert.AreNotEqual(true, string.IsNullOrEmpty(lang));
        autoEvent.Set();
      }))
      {
        Assert.Fail();
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestTranslate()
    {
      if (!languageTranslator.GetTranslation(query, toLanguage, fromLanguage, (Translations translation) =>
      {
        Assert.AreNotEqual(translation, null);
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
