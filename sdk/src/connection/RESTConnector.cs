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

namespace IBM.Watson.DeveloperCloud.Connection
{
  /// <summary>
  /// REST connector class.
  /// </summary>
  public class RESTConnector
  {
    #region Response
    /// <summary>
    /// The class is returned by a Request object containing the response to a request made
    /// by the client.
    /// </summary>
    public class Response
    {
      #region Public Properties
      /// <summary>
      /// True if the request was successful.
      /// </summary>
      public bool Success { get; set; }
      /// <summary>
      /// Error message if Success is false.
      /// </summary>
      public string Error { get; set; }
      /// <summary>
      /// The data returned by the request.
      /// </summary>
      public byte[] Data { get; set; }
      /// <summary>
      /// The amount of time in seconds it took to get this response from the server.
      /// </summary>
      public float ElapsedTime { get; set; }
      #endregion
    }
    #endregion

    #region Request
    /// <summary>
    /// This class is created to make a request to send to the server.
    /// </summary>
    public class Request
    {
      /// <summary>
      /// Default constructor.
      /// </summary>
      public Request()
      {
        Parameters = new Dictionary<string, object>();
        Headers = new Dictionary<string, string>();
      }

      #region Public Properties
      /// <summary>
      /// Custom timeout for this Request. This timeout is used if this timeout is larger than the value in the Config class.
      /// </summary>
      public float Timeout { get; set; }
      /// <summary>
      /// If true, then request will be cancelled.
      /// </summary>
      public bool Cancel { get; set; }
      /// <summary>
      /// True to send a delete method.
      /// </summary>
      public bool Delete { get; set; }
      /// <summary>
      /// The name of the function to invoke on the server.
      /// </summary>
      public string Function { get; set; }
      /// <summary>
      /// The parameters to pass to the function on the server.
      /// </summary>
      public Dictionary<string, object> Parameters { get; set; }
      /// <summary>
      /// Additional headers to provide in the request.
      /// </summary>
      public Dictionary<string, string> Headers { get; set; }
      /// <summary>
      /// The data to send through the connection. Do not use Forms if set.
      /// </summary>
      public byte[] Send { get; set; }
      /// <summary>
      /// Multi-part form data that needs to be sent. Do not use Send if set.
      /// </summary>
      public Dictionary<string, Form> Forms { get; set; }
      /// <summary>
      /// The callback that is invoked when a response is received.
      /// </summary>
      public ResponseEvent OnResponse { get; set; }
      /// <summary>
      /// This callback is invoked to provide progress on the WWW download.
      /// </summary>
      public ProgressEvent OnDownloadProgress { get; set; }
      /// <summary>
      /// This callback is invoked to provide upload progress.
      /// </summary>
      public ProgressEvent OnUploadProgress { get; set; }
      #endregion
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Base URL for REST requests.
    /// </summary>
    public string URL { get; set; }
    /// <summary>
    /// Credentials used to authenticate with the server.
    /// </summary>
    public Credentials Authentication { get; set; }
    /// <summary>
    /// Additional headers to attach to all requests.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; }
    #endregion

    #region Private Data
    //! Dictionary of connectors by service & function.
    private static Dictionary<string, RESTConnector_Unity> sm_Connectors = new Dictionary<string, RESTConnector_Unity>();
    private int m_ActiveConnections = 0;
    private Queue<Request> m_Requests = new Queue<Request>();
    #endregion

    #region Connectors
    /// <summary>
    /// This function returns a RESTConnector object for the given service and function. 
    /// </summary>
    /// <param name="serviceID">The ID of the service.</param>
    /// <param name="function">The name of the function.</param>
    /// <param name="useCache">If true, then the connections will use a static cache.</param>
    /// <returns>Returns a RESTConnector object or null on error.</returns>
    public static RESTConnector_Unity GetConnector(string serviceID, string function, bool useCache = true)
    {
      RESTConnector_Unity connector = null;

      string connectorID = serviceID + function;
      if (useCache && sm_Connectors.TryGetValue(connectorID, out connector))
        return connector;

      Config cfg = Config.Instance;
      Config.CredentialInfo cred = cfg.FindCredentials(serviceID);
      if (cred == null)
      {
        Log.Error("Config", "Failed to find credentials for service {0}.", serviceID);
        return null;
      }

      connector = new RESTConnector_Unity();
      connector.URL = cred.m_URL + function;
      if (cred.HasCredentials())
        connector.Authentication = new Credentials(cred.m_User, cred.m_Password);
      if (useCache)
        sm_Connectors[connectorID] = connector;

      return connector;
    }

    /// <summary>
    /// Flush all connectors from the static pool.
    /// </summary>
    public static void FlushConnectors()
    {
      sm_Connectors.Clear();
    }
    #endregion

    #region Send Interface
    /// <summary>
    /// Send a request to the server. The request contains a callback that is invoked
    /// when a response is received. The request may be queued if multiple requests are
    /// made at once.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <returns>true is returned on success, false is returned if the Request can't be sent.</returns>
    public bool Send(Request request)
    {
      if (request == null)
        throw new ArgumentNullException("request");

      m_Requests.Enqueue(request);

      // if we are not already running a co-routine to send the Requests
      // then start one at this point.
      if (m_ActiveConnections < Config.Instance.MaxRestConnections)
      {
        // This co-routine will increment m_ActiveConnections then yield back to us so
        // we can return from the Send() as quickly as possible.
        Runnable.Run(ProcessRequestQueue());
      }

      return true;
    }
    #endregion

    #region AddHeaders
    private void AddHeaders(Dictionary<string, string> headers)
    {
      if (Authentication != null)
      {
        if (headers == null)
          throw new ArgumentNullException("headers");
        headers.Add("Authorization", Authentication.CreateAuthorization());
      }

      if (Headers != null)
      {
        foreach (var kp in Headers)
          headers[kp.Key] = kp.Value;
      }

      headers.Add("User-Agent", Constants.String.VERSION);
    }
    #endregion

    #region ProcessRequestQueue
    private IEnumerator ProcessRequestQueue()
    {
      m_ActiveConnections += 1;
      yield return null;

      while(m_Requests.Count > 0)
      {
        Request req = m_Requests.Dequeue();

        if (req.Cancel)
          continue;
        
        string url = URL;
        if (!string.IsNullOrEmpty(req.Function))
          url += req.Function;

        StringBuilder args = null;
        foreach(var kp in req.Parameters)
        {
          var key = kp.Key;
          var value = kp.Value;

          if (value is string)
            value = Uri.EscapeUriString((string)value);
          else if (value is byte[])
            value = Convert.ToBase64String((byte[])value);
          else if (value is Int32 || value is Int64 || value is UInt32 || value is UInt64)
            value = value.ToString();
          else if (value != null)
            Log.Warning("RESTConnector", "Unsupported parameter value type {0}", value.GetType().Name);
          else
            Log.Error("RESTConnector", "Parameger {0} value is null", key);

          if (args == null)
            args = new StringBuilder();
          else
            args.Append("&");

          args.Append(key + "=" + value);

          if (args != null && args.Length > 0)
            url += "?" + args.ToString();

          AddHeaders(req.Headers);

          WebHeaderCollection headers = new WebHeaderCollection();
          foreach (KeyValuePair<string, string> kv in req.Headers)
            headers.Add(kv.Key, kv.Value);
        }
      }
    }
    #endregion
  }
}
