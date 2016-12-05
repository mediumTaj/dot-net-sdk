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
using NUnit.Framework;
using System.Collections.Generic;

namespace sdk.test
{
  [TestFixture]
  public class TestAlchemyDataNews
  {
    private AlchemyAPI alchemyDataNews = new AlchemyAPI();
    private string[] returnFields = { Fields.ENRICHED_URL_ENTITIES, Fields.ENRICHED_URL_KEYWORDS };
    private Dictionary<string, string> queryFields = new Dictionary<string, string>();
    
    [Test]
    public void TestDataNews()
    {
      queryFields.Add(Fields.ENRICHED_URL_RELATIONS_RELATION_SUBJECT_TEXT, "Obama");
      queryFields.Add(Fields.ENRICHED_URL_CLEANEDTITLE, "Washington");

      if (!alchemyDataNews.GetNews((NewsResponse newsData, string data) =>
      {
        Assert.AreNotEqual(newsData, null);
      }, returnFields, queryFields))
        Assert.Fail();
    }
  }
}