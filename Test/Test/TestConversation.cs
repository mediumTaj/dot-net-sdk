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

namespace sdk.test
{
    [TestFixture]
    public class TestConversation : IntegrationTest
    {
        Conversation conversation = new Conversation();
        private string workspaceID;
        private string input = "Can you unlock the door?";

        override public void Init()
        {
            base.Init();

            if (!isTestInitalized)
            {
                workspaceID = Config.Instance.GetVariableValue("ConversationV1_ID");
                if (string.IsNullOrEmpty(workspaceID))
                    Assert.Fail("Failed to find workspaceID");

                isTestInitalized = true;
            }
        }

        [Test]
        public void Conversation_TestMessageObject()
        {
            if (!conversation.Message((MessageResponse resp, string data) =>
            {
                Assert.NotNull(resp);
                autoEvent.Set();
            }, workspaceID, input))
            {
                Assert.Fail("Failed to send message! {0}", input);
                autoEvent.Set();
            }

            autoEvent.WaitOne();
        }

        [Test]
        public void Conversation_TestMessageInput()
        {
            MessageRequest messageRequest = new MessageRequest();
            messageRequest.InputText = input;

            if (!conversation.Message((MessageResponse resp, string data) =>
            {
                //Assert.NotNull(resp);
                Assert.Pass();
                autoEvent.Set();
            }, workspaceID, messageRequest))
            {
                Assert.Fail("Failed to send message! {0}", messageRequest.input);
                autoEvent.Set();
            }
            
            autoEvent.WaitOne();
        }
    }
}
