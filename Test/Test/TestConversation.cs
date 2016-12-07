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
using IBM.Watson.DeveloperCloud.Services.Conversation.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;
using System.Threading;

namespace sdk.test
{
  [TestFixture]
  class TestConversation
  {
    Conversation conversation = new Conversation();
    private string workspaceID;
    private string input = "Can you unlock the door?";
    private AutoResetEvent autoEvent = new AutoResetEvent(false);

    [SetUp]
    public void Init()
    {
      if (Config.Instance.FindCredentials(conversation.GetServiceID()) == null)
        Assert.Fail("Failed to find credentials");

      workspaceID = Config.Instance.GetVariableValue("ConversationV1_ID");
      if (string.IsNullOrEmpty(workspaceID))
        Assert.Fail("Failed to find workspaceID");
    }

    [Test]
    public void TestMessageObject()
    {
      MessageResponse messageResponse = null;

      if (!conversation.Message((MessageResponse resp, string data) =>
      {
        messageResponse = resp;
        autoEvent.Set();
      }, workspaceID, input))
        Assert.Fail("Failed to send message! {0}", input);

      autoEvent.WaitOne();
      Assert.AreNotEqual(messageResponse, null);
    }

    [Test]
    public void TestMessageInput()
    {
      MessageResponse messageResponse = null;

      MessageRequest messageRequest = new MessageRequest();
      messageRequest.InputText = input;

      if (!conversation.Message((MessageResponse resp, string data) =>
      {
        messageResponse = resp;
        autoEvent.Set();
      }, workspaceID, messageRequest))
        Assert.Fail("Failed to send message! {0}", messageRequest.input);

      autoEvent.WaitOne();
      Assert.AreNotEqual(messageResponse, null);
    }
  }
}
