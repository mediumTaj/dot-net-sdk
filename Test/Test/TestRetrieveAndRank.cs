

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
using IBM.Watson.DeveloperCloud.Services.RetrieveAndRank.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;
using System.IO;
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
      Constants.Path.dataPath = TestContext.CurrentContext.TestDirectory + Path.DirectorySeparatorChar;
      string testDataPath = Constants.Path.dataPath + Constants.Path.APP_DATA + Path.DirectorySeparatorChar;

      if (!Config.Instance.ConfigLoaded)
      {
        string configPath = testDataPath + Constants.Path.CONFIG_FILE;
        string configJson = File.ReadAllText(configPath);
        Config.Instance.LoadConfig(configJson);
      }

      if (!Config.Instance.ConfigLoaded)
        Assert.Fail("Failed to load Config.");

      string retrieveAndRankDataPath = testDataPath + Path.DirectorySeparatorChar + "RetrieveAndRank" + Path.DirectorySeparatorChar;
      m_IntegrationTestClusterConfigPath = retrieveAndRankDataPath + "cranfield_solr_config.zip";
      m_IntegrationTestRankerTrainingPath = retrieveAndRankDataPath + "ranker_training_data.csv";
      m_IntegrationTestRankerAnswerDataPath = retrieveAndRankDataPath + "ranker_answer_data.csv";
      m_IntegrationTestIndexDataPath = retrieveAndRankDataPath + "cranfield_data.json";

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

    [Test, Order(0)]
    public void TestCreateCluster()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to create cluster...");
      if (!retrieveAndRank.CreateCluster((SolrClusterResponse resp, string data) =>
      {
        m_CreatedClusterID = resp.solr_cluster_id;
        Log.Debug("TestRetrieveAndRank", "Created cluster {0}.", m_CreatedClusterID);
        Assert.NotNull(resp);
        autoEvent.Set();
      }, m_ClusterToCreateName, m_ClusterToCreateSize))
      {
        Assert.Fail("Failed to invoke CreateCluster();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(2)]
    public void TestDeleteCluster()
    {
      if (!string.IsNullOrEmpty(m_CreatedClusterID))
      {
      Log.Debug("TestRetrieveAndRank", "Attempting to delete cluster {0}...", m_CreatedClusterID);
        if (!retrieveAndRank.DeleteCluster((bool success, string data) =>
        {
        Log.Debug("TestRetrieveAndRank", "Deleted cluster {0}.", m_CreatedClusterID);
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

    [Test, Order(1)]
    public void TestGetCluster()
    {
      if (!string.IsNullOrEmpty(m_CreatedClusterID))
      {
        Log.Debug("TestRetrieveAndRank", "Attempting to get cluster {0}...", m_CreatedClusterID);
        if (!retrieveAndRank.GetCluster((SolrClusterResponse resp, string data) =>
         {
           Assert.NotNull(resp);
           autoEvent.Set();
         }, m_CreatedClusterID))
        {
          Assert.Fail("Failed to invoke GetCluster();");
          autoEvent.Set();
        }
      }
      else
      {
        Assert.Fail("createdClusterID is empty. GetCluster failed.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

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
