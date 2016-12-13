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

using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.RetrieveAndRank.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using NUnit.Framework;
using System.IO;

namespace sdk.test
{
  [TestFixture]
  public class TestRetrieveAndRank : IntegrationTest
  {
    private RetrieveAndRank retrieveAndRank = new RetrieveAndRank();

    //  from config variables
    private string exampleClusterID;
    private string exampleConfigName;
    private string exampleRankerID;
    private string exampleCollectionName;

    private string integrationTestQuery = "What is the basic mechanisim of the transonic aileron buzz";
    private string[] fl = { "title", "id", "body", "author", "bibliography" };

    //  data paths to local files
    private string integrationTestClusterConfigPath;
    private string integrationTestRankerTrainingPath;
    private string integrationTestRankerAnswerDataPath;
    private string integrationTestIndexDataPath;

    private string clusterToCreateName = "dotnet-integration-test-cluster";
    private string clusterToCreateSize = "3";
    private int sizeToResizeCluster = 4;
    private string createdClusterID;

    private string configToCreateName = "dotnet-integration-test-config";
    private string collectionToCreateName = "dotnet-integration-test-collection";
    private string createdRankerID;

    private string rankerToCreateName = "dotnet-integration-test-ranker";

    private string retrieveAndRankDataPath;

    override public void Init()
    {
      base.Init();

      retrieveAndRankDataPath = testDataPath + "RetrieveAndRank" + Path.DirectorySeparatorChar;
      integrationTestClusterConfigPath = retrieveAndRankDataPath + "cranfield_solr_config.zip";
      integrationTestRankerTrainingPath = retrieveAndRankDataPath + "ranker_training_data.csv";
      integrationTestRankerAnswerDataPath = retrieveAndRankDataPath + "ranker_answer_data.csv";
      integrationTestIndexDataPath = retrieveAndRankDataPath + "cranfield_data.json";

      exampleClusterID = Config.Instance.GetVariableValue("RetrieveAndRank_IntegrationTestClusterID");
      exampleConfigName = Config.Instance.GetVariableValue("RetrieveAndRank_IntegrationTestConfigName");
      exampleRankerID = Config.Instance.GetVariableValue("RetrieveAndRank_IntegrationTestRankerID");
      exampleCollectionName = Config.Instance.GetVariableValue("RetrieveAndRank_IntegrationTestCollectionName");
    }

    [Test, Order(0)]
    public void RetrieveAndRank_TestGetClusters()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to get clusters...");
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

    [Test, Order(15)]
    public void RetrieveAndRank_TestGetClusterConfigs()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to list cluster configs for {0}...", createdClusterID);
      if (!retrieveAndRank.GetClusterConfigs((SolrConfigList resp, string data) =>
       {
         Log.Debug("TestRetrieveAndRank", "Listed cluster configs for {0}...", createdClusterID);
         Assert.NotNull(resp);
         autoEvent.Set();
       }, createdClusterID))
      {
        Assert.Fail("Failed to invoke GetClusterConfigs.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test,Order(16)]
    public void RetrieveAndRank_TestForwardCollectionRequestList()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to forward collection request: List {0}...", collectionToCreateName);

      if (!retrieveAndRank.ForwardCollectionRequest((CollectionsResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdClusterID, CollectionsAction.LIST, collectionToCreateName, configToCreateName))
      {
        Assert.Fail("Failed to invoke ForwardCollectionRequest: List.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(3)]
    public void RetrieveAndRank_TestGetRankers()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to get rankers...");

      if (!retrieveAndRank.GetRankers((ListRankersPayload resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }))
      {
        Assert.Fail("Failed to invoke GetRankers();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(4)]
    public void RetrieveAndRank_TestCreateCluster()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to create cluster...");
      if (!retrieveAndRank.CreateCluster((SolrClusterResponse resp, string data) =>
      {
        createdClusterID = resp.solr_cluster_id;
        Log.Debug("TestRetrieveAndRank", "Created cluster {0}.", createdClusterID);
        Assert.NotNull(resp);
        autoEvent.Set();
      }, clusterToCreateName, clusterToCreateSize))
      {
        Assert.Fail("Failed to invoke CreateCluster();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(5)]
    public void RetrieveAndRank_TestGetCluster()
    {
      if (!string.IsNullOrEmpty(createdClusterID))
      {
        Log.Debug("TestRetrieveAndRank", "Attempting to get cluster {0}...", createdClusterID);
        if (!retrieveAndRank.GetCluster((SolrClusterResponse resp, string data) =>
         {
           Assert.NotNull(resp);
           autoEvent.Set();
         }, createdClusterID))
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

    [Test, Order(6)]
    public void RetrieveAndRank_TestUploadClusterConfig()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to upload cluster configs for {0}...", createdClusterID);

      if (!retrieveAndRank.UploadClusterConfig((UploadResponse resp, string data) =>
         {
           Assert.NotNull(resp);
           autoEvent.Set();
         }, createdClusterID, configToCreateName, integrationTestClusterConfigPath))
      {
        Assert.Fail("Failed to invoke UploadClusterConfig();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(7)]
    public void RetrieveAndRank_TestGetClusterConfig()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to get cluster config {0} for {1}...", clusterToCreateName, createdClusterID);

      if (!retrieveAndRank.GetClusterConfig((byte[] resp, string data) =>
       {
         Assert.NotNull(resp);
         autoEvent.Set();
       }, createdClusterID, configToCreateName))
      {
        Assert.Fail("Failed to invoke GetClusterConfig();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(8)]
    public void RetrieveAndRank_TestForwardCollectionRequestCreate()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to forward collection request: Create {0}...", collectionToCreateName);

      if (!retrieveAndRank.ForwardCollectionRequest((CollectionsResponse resp, string data) =>
       {
         Assert.NotNull(resp);
         autoEvent.Set();
       }, createdClusterID, CollectionsAction.CREATE, collectionToCreateName, configToCreateName))
      {
        Assert.Fail("Failed to invoke ForwardCollectionRequest: Create.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(9)]
    public void RetrieveAndRank_TestIndexDocuments()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to get index documents...");

      if (!retrieveAndRank.IndexDocuments((IndexResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, integrationTestIndexDataPath, createdClusterID, collectionToCreateName))
      {
        Assert.Fail("Failed to invoke IndexDocuments();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(10)]
    public void RetrieveAndRank_TestStandardSearch()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting standard search...");

      if (!retrieveAndRank.Search((SearchResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdClusterID, collectionToCreateName, integrationTestQuery, fl))
      {
        Assert.Fail("Failed to invoke Search(); Standard");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(11)]
    public void RetrieveAndRank_TestRankedSearch()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting ranked search...");

      if (!retrieveAndRank.Search((SearchResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdClusterID, collectionToCreateName, integrationTestQuery, fl, true, createdRankerID))
      {
        Assert.Fail("Failed to invoke Search(); Ranked");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(12)]
    public void RetrieveAndRank_TestGetClusterStatistics()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to get cluster statistics...");

      if (!retrieveAndRank.GetClusterStats((StatsResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdClusterID))
      {
        Assert.Fail("Failed to invoke GetClusterStats();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(13)]
    public void RetrieveAndRank_TestResizeSOLRCluster()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to resize SOLR Cluster...");

      if (!retrieveAndRank.ResizeCluster((ResizeResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdClusterID, sizeToResizeCluster))
      {
        Assert.Fail("Failed to invoke ResizeCluster();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(14)]
    public void RetrieveAndRank_TestGetClusterResizeStatus()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to get cluster resize status...");

      if (!retrieveAndRank.GetClusterResizeStatus((ResizeResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdClusterID))
      {
        Assert.Fail("Failed to invoke GetClusterResizeStatus();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(17)]
    public void RetrieveAndRank_TestCreateRanker()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to create ranker...");

      if (!retrieveAndRank.CreateRanker((RankerStatusPayload resp, string data) =>
      {
        createdRankerID = resp.ranker_id;
        Assert.NotNull(resp);
        autoEvent.Set();
      }, integrationTestRankerTrainingPath, rankerToCreateName))
      {
        Assert.Fail("Failed to invoke CreateRanker();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(18)]
    public void RetrieveAndRank_TestRank()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to rank...");

      if (!retrieveAndRank.Rank((RankerOutputPayload resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdRankerID, integrationTestRankerAnswerDataPath))
      {
        Assert.Fail("Failed to invoke Rank();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(19)]
    public void RetrieveAndRank_TestGetRankerInfo()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to get ranker info...");

      if (!retrieveAndRank.GetRanker((RankerStatusPayload resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdRankerID))
      {
        Assert.Fail("Failed to invoke GetRanker();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(20)]
    public void RetrieveAndRank_TestForwardCollectionRequestDelete()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to forward collection request: Delete {0}...", collectionToCreateName);

      if (!retrieveAndRank.ForwardCollectionRequest((CollectionsResponse resp, string data) =>
      {
        Assert.NotNull(resp);
        autoEvent.Set();
      }, createdClusterID, CollectionsAction.DELETE, collectionToCreateName, configToCreateName))
      {
        Assert.Fail("Failed to invoke ForwardCollectionRequest: Delete.");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(21)]
    public void RetrieveAndRank_TestDeleteClusterConfig()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to delete cluster config {0} for {1}...", clusterToCreateName, createdClusterID);

      if (!retrieveAndRank.DeleteClusterConfig((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdClusterID, configToCreateName))
      {
        Assert.Fail("Failed to invoke DeleteClusterConfig();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(22)]
    public void RetrieveAndRank_TestDeleteRanker()
    {
      Log.Debug("TestRetrieveAndRank", "Attempting to delete ranker {0}...", createdRankerID);

      if (!retrieveAndRank.DeleteRanker((bool success, string data) =>
      {
        Assert.True(success);
        autoEvent.Set();
      }, createdRankerID))
      {
        Assert.Fail("Failed to invoke DeleteRanker();");
        autoEvent.Set();
      }

      autoEvent.WaitOne();
    }

    [Test, Order(23)]
    public void RetrieveAndRank_TestDeleteCluster()
    {
      if (!string.IsNullOrEmpty(createdClusterID))
      {
        Log.Debug("TestRetrieveAndRank", "Attempting to delete cluster {0}...", createdClusterID);
        if (!retrieveAndRank.DeleteCluster((bool success, string data) =>
        {
          Assert.True(success);
          autoEvent.Set();
        }, createdClusterID))
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
  }
}
