﻿/**
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
using IBM.Watson.DeveloperCloud.Services.DocumentConversion.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Threading;

namespace sdk.test
{
  [TestFixture]
  class TestDocumentConversion
  {
    private DocumentConversion documentConversion = new DocumentConversion();
    private string examplePath = Constants.Path.APP_DATA + "/watson_beats_jeopardy.html";
    AutoResetEvent autoEvent = new AutoResetEvent(false);

    [Test]
    public void TestConvertDocumentAnswerUnit()
    {
      if (!documentConversion.ConvertDocument((ConvertedDocument documentConversionResponse, string data) =>
      {
        Assert.AreNotEqual(documentConversionResponse, null);
         autoEvent.Set();
      }, examplePath, ConversionTarget.ANSWER_UNITS))
        Assert.Fail();

      autoEvent.WaitOne();
    }

    [Test]
    public void TestConvertDocumentText()
    {
      if (!documentConversion.ConvertDocument((ConvertedDocument documentConversionResponse, string data) =>
      {
        Assert.AreNotEqual(documentConversionResponse, null);
         autoEvent.Set();
      }, examplePath, ConversionTarget.NORMALIZED_TEXT))
        Assert.Fail();

      autoEvent.WaitOne();
    }

    [Test]
    public void ConvertDocumentHTML()
    {
      if (!documentConversion.ConvertDocument((ConvertedDocument documentConversionResponse, string data) =>
      {
        Assert.AreNotEqual(documentConversionResponse, null);
         autoEvent.Set();
      }, examplePath, ConversionTarget.NORMALIZED_HTML))
        Assert.Fail();

      autoEvent.WaitOne();
    }
  }
}