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

using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.RetrieveAndRank.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;
using System.IO;
using System.Threading;

namespace sdk.test
{
  [TestFixture]
  public class TestRetrieveAndRank : IntegrationTest
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
    private string retrieveAndRankDataPath;

    override public void Init()
    {
      base.Init();

      retrieveAndRankDataPath = testDataPath + "RetrieveAndRank" + Path.DirectorySeparatorChar;
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

    [Test, Order(99)]
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

    [Test, Order(3)]
    public void TestGetClusterConfigs()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to list cluster configs for {0}...", m_CreatedClusterID);
      if (!retrieveAndRank.GetClusterConfigs((SolrConfigList resp, string data) =>
       {
         Log.Debug("TestRetrieveAndRank", "Listed cluster configs for {0}...", m_CreatedClusterID);
         Assert.NotNull(resp);
         autoEvent.Set();
       }, m_CreatedClusterID))
      {
        Assert.Fail("Failed to invoke GetClusterConfigs.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(2)]
    public void TestUploadClusterConfig()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to upload cluster configs for {0}...", m_CreatedClusterID);

      if (!retrieveAndRank.UploadClusterConfig((UploadResponse resp, string data) =>
         {
           Assert.NotNull(resp);
           autoEvent.Set();
         }, m_CreatedClusterID, m_ConfigToCreateName, m_IntegrationTestClusterConfigPath))
      {
        Assert.Fail("Failed to invoke UploadClusterConfig();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(3)]
    public void TestGetClusterConfig()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to get cluster config {0} for {1}...", m_ClusterToCreateName, m_CreatedClusterID);

      if (!retrieveAndRank.GetClusterConfig((byte[] resp, string data) =>
       {
         Assert.NotNull(resp);
         autoEvent.Set();
       }, m_CreatedClusterID, m_ConfigToCreateName))
      {
        Assert.Fail("Failed to invoke GetClusterConfig();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(4)]
    public void TestDeleteClusterConfig()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to delete cluster config {0} for {1}...", m_ClusterToCreateName, m_CreatedClusterID);

      if (!retrieveAndRank.DeleteClusterConfig((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, m_CreatedClusterID, m_ConfigToCreateName))
      {
        Assert.Fail("Failed to invoke DeleteClusterConfig();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestForwardCollectionRequestCreate()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to forward collection request: Create {0}...", m_CollectionToCreateName);

      if(!retrieveAndRank.ForwardCollectionRequest((CollectionsResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, m_CreatedClusterID, CollectionsAction.CREATE, m_CollectionToCreateName, m_ConfigToCreateName))
      {
        Assert.Fail("Failed to invoke ForwardCollectionRequest: Create.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestForwardCollectionRequestList()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to forward collection request: List {0}...", m_CollectionToCreateName);

      if (!retrieveAndRank.ForwardCollectionRequest((CollectionsResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, m_CreatedClusterID, CollectionsAction.LIST, m_CollectionToCreateName, m_ConfigToCreateName))
      {
        Assert.Fail("Failed to invoke ForwardCollectionRequest: List.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test]
    public void TestForwardCollectionRequestDelete()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to forward collection request: Delete {0}...", m_CollectionToCreateName);

      if (!retrieveAndRank.ForwardCollectionRequest((CollectionsResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, m_CreatedClusterID, CollectionsAction.DELETE, m_CollectionToCreateName, m_ConfigToCreateName))
      {
        Assert.Fail("Failed to invoke ForwardCollectionRequest: Delete.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

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
