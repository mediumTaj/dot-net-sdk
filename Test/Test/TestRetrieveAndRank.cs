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

using IBM.Watson.DeveloperCloud.Services.RetrieveAndRank.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;
using System.Threading;

namespace sdk.test
{
  [TestFixture]
  public class TestRetrieveAndRank
  {
    private RetrieveAndRank retrieveAndRank = new RetrieveAndRank();
    private AutoResetEvent autoEvent = new AutoResetEvent(false);

    //  from config variables
    private string m_ExampleClusterID;
    private string m_ExampleConfigName;
    private string m_ExampleRankerID;
    private string m_ExampleCollectionName;

    private string m_IntegrationTestQuery = "What is the basic mechanisim of the transonic aileron buzz";
    private string[] m_Fl = { "title", "id", "body", "author", "bibliography" };

    //  data paths to local files
    private string m_IntegrationTestClusterConfigPath;
    private string m_IntegrationTestRankerTrainingPath;
    private string m_IntegrationTestRankerAnswerDataPath;
    private string m_IntegrationTestIndexDataPath;

    private string m_ClusterToCreateName = "dotnet-integration-test-cluster";
    private string m_ClusterToCreateSize = "3";
    private string m_CreatedClusterID;

    private string m_ConfigToCreateName = "dotnet-integration-test-config";
    private string m_CollectionToCreateName = "dotnet-integration-test-collection";
    private string m_CreatedRankerID;

    private string m_RankerToCreateName = "dotnet-integration-test-ranker";

    private bool m_IsClusterReady = false;
    private bool m_IsRankerReady = false;

    [SetUp]
    public void Init()
    {
      m_IntegrationTestClusterConfigPath = Constants.Path.APP_DATA + "/Watson/Examples/ServiceExamples/TestData/RetrieveAndRank/cranfield_solr_config.zip";
      m_IntegrationTestRankerTrainingPath = Constants.Path.APP_DATA + "/Watson/Examples/ServiceExamples/TestData/RetrieveAndRank/ranker_training_data.csv";
      m_IntegrationTestRankerAnswerDataPath = Constants.Path.APP_DATA + "/Watson/Examples/ServiceExamples/TestData/RetrieveAndRank/ranker_answer_data.csv";
      m_IntegrationTestIndexDataPath = Constants.Path.APP_DATA + "/Watson/Examples/ServiceExamples/TestData/RetrieveAndRank/cranfield_data.json";

      m_ExampleClusterID = Config.Instance.GetVariableValue("RetrieveAndRank_IntegrationTestClusterID");
      m_ExampleConfigName = Config.Instance.GetVariableValue("RetrieveAndRank_IntegrationTestConfigName");
      m_ExampleRankerID = Config.Instance.GetVariableValue("RetrieveAndRank_IntegrationTestRankerID");
      m_ExampleCollectionName = Config.Instance.GetVariableValue("RetrieveAndRank_IntegrationTestCollectionName");
    }
    
    [Test]
    public void TestGetClusters()
    {
      if (!retrieveAndRank.GetClusters((SolrClusterListResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }))
      {
        Assert.Fail("Failed to invoke GetClusters();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [TestCase(TestName = "R&R 00 CreateCluster")]
    public void TestCreateCluster()
    {
      if (!retrieveAndRank.CreateCluster((SolrClusterResponse resp, string data) =>
       {
         m_CreatedClusterID = resp.solr_cluster_id;
         Assert.NotNull(resp);
         autoEvent.Set();
       }, m_ClusterToCreateName, m_ClusterToCreateSize))
      {
        Assert.Fail("Failed to invoke CreateCluster();");
        autoEvent.Set();
      }
      autoEvent.WaitOne();
    }

    [TestCase(TestName = "R&R 01 DeleteCluster")]
    public void TestDeleteCluster()
    {
      if(!string.IsNullOrEmpty(m_CreatedClusterID))
      {
        if (!retrieveAndRank.DeleteCluster((bool success, string data) =>
        {
          Assert.True(success);
          autoEvent.Set();
        }, m_CreatedClusterID))
        {
          Assert.Fail("Failed to invoke DeleteCluster();");
          autoEvent.Set();
        }
      }
      else
      {
        Assert.Fail("createdClusterID is empty. Delete failed.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    //[Test]
    //public void TestGetCluster()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestListClusterConfigs()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestDeleteClusterConfig()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestGetClusterConfig()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestUploadClusterConfig()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestForwardCollectionRequest()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestIndexDocuments()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestStandardSearch()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestRankedSearch()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestGetClusterStatistics()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestGetClusterResizeStatus()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestResizeSOLRCluster()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestGetRankers()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestCreateRanker()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestRank()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestDeleteRanker()
    //{
    //  autoEvent.WaitOne();
    //}

    //[Test]
    //public void TestGetRankerInfo()
    //{
    //  autoEvent.WaitOne();
    //}
  }
}
