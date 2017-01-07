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

// uncomment to enable debugging
//#define ENABLE_DEBUGGING

using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Http;

namespace IBM.Watson.DeveloperCloud.Connection
{
    /// <summary>
    /// REST connector class.
    /// </summary>
    public class RESTConnector
    {
        #region Public Types
        /// <summary>
        /// This delegate type is declared for a Response handler function.
        /// </summary>
        /// <param name="req">The original request object.</param>
        /// <param name="resp">The response object.</param>
        public delegate void ResponseEvent(Request req, Response resp);

        /// <summary>
        /// This delegate is invoked to provide download progress.
        /// </summary>
        /// <param name="progress"></param>
        public delegate void ProgressEvent(float progress);
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
        };

        /// <summary>
        /// Multi-part form data class.
        /// </summary>
        public class Form
        {
            /// <summary>
            /// Make a multi-part form object from a string.
            /// </summary>
            /// <param name="s">The string data.</param>
            public Form(string s)
            {
                IsBinary = false;
                BoxedObject = s;
            }
            /// <summary>
            /// Make a multi-part form object from an int.
            /// </summary>
            /// <param name="n">The int data.</param>
            public Form(int n)
            {
                IsBinary = false;
                BoxedObject = n;
            }

            /// <summary>
            /// Make a multi-part form object from binary data.
            /// </summary>
            /// <param name="contents">The binary data.</param>
            /// <param name="fileName">The filename of the binary data.</param>
            /// <param name="mimeType">The mime type of the data.</param>
            public Form(byte[] contents, string fileName = null, string mimeType = null)
            {
                IsBinary = true;
                Contents = contents;
                FileName = fileName;
                MimeType = mimeType;
            }

            /// <summary>
            /// True if the contained data is binary.
            /// </summary>
            public bool IsBinary { get; set; }
            /// <summary>
            /// The boxed POD data type, only set if IsBinary is false.
            /// </summary>
            public object BoxedObject { get; set; }
            /// <summary>
            /// If IsBinary is true, then this will contain the binary data.
            /// </summary>
            public byte[] Contents { get; set; }
            /// <summary>
            /// The filename of the binary data.
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// The Mime-Type of the binary data.
            /// </summary>
            public string MimeType { get; set; }
        };

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
            /// True to send request as PUT.
            /// </summary>
            public bool Put { get; set; }
            /// <summary>
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
        private static float sm_LogResponseTime = 3.0f;
        /// <summary>
        /// Specify a time to log to the logging system when a response takes longer than this amount.
        /// </summary>
        public static float LogResponseTime { get { return sm_LogResponseTime; } set { sm_LogResponseTime = value; } }
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
        private static Dictionary<string, RESTConnector> sm_Connectors = new Dictionary<string, RESTConnector>();
        private AutoResetEvent autoEvent = new AutoResetEvent(false);
        #endregion

        /// <summary>
        /// This function returns a RESTConnector object for the given service and function. 
        /// </summary>
        /// <param name="serviceID">The ID of the service.</param>
        /// <param name="function">The name of the function.</param>
        /// <param name="useCache">If true, then the connections will use a static cache.</param>
        /// <returns>Returns a RESTConnector object or null on error.</returns>
        public static RESTConnector GetConnector(string serviceID, string function, bool useCache = true)
        {
            RESTConnector connector = null;

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

            connector = new RESTConnector();
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

            ProcessRequestQueue();
            autoEvent.WaitOne();

            return true;
        }
        #endregion

        #region Private Data
        private int m_ActiveConnections = 0;
        private Queue<Request> m_Requests = new Queue<Request>();
        #endregion

        #region Private Functions
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

        private async void ProcessRequestQueue()
        {
            m_ActiveConnections += 1;

            while (m_Requests.Count > 0)
            {
                Request req = m_Requests.Dequeue();
                if (req.Cancel)
                    continue;
                string url = URL;
                if (!string.IsNullOrEmpty(req.Function))
                    url += req.Function;

                StringBuilder args = null;
                foreach (var kp in req.Parameters)
                {
                    var key = kp.Key;
                    var value = kp.Value;

                    if (value is string)
                        value = Uri.EscapeDataString((string)value);             // escape the value
                    else if (value is byte[])
                        value = Convert.ToBase64String((byte[])value);    // convert any byte data into base64 string
                    else if (value is Int32 || value is Int64 || value is UInt32 || value is UInt64)
                        value = value.ToString();
                    else if (value != null)
                        Log.Warning("RESTConnector", "Unsupported parameter value type {0}", value.GetType().Name);
                    else
                        Log.Error("RESTConnector", "Parameter {0} value is null", key);

                    if (args == null)
                        args = new StringBuilder();
                    else
                        args.Append("&");                  // append separator

                    args.Append(key + "=" + value);       // append key=value
                }

                if (args != null && args.Length > 0)
                    url += "?" + args.ToString();

                AddHeaders(req.Headers);

                Response resp = new Response();

                DateTime startTime = DateTime.Now;
                if (!req.Delete)
                {
                    //HttpResponseMessage response;

                    using (HttpClient www = new HttpClient())
                    {
                        if (req.Forms != null)
                        {
                            //  POST
                            if (req.Send != null)
                                Log.Warning("RESTConnector", "Do not use both Send & Form fields in a Request object.");

                            MultipartFormDataContent form = new MultipartFormDataContent();

                            try
                            {
                                foreach (var formData in req.Forms)
                                {
                                    if (formData.Value.IsBinary)
                                        form.Add(new ByteArrayContent(formData.Value.Contents), formData.Key, formData.Value.FileName);
                                    else if (formData.Value.BoxedObject is string)
                                        form.Add(new StringContent((string)formData.Value.BoxedObject), formData.Key);
                                    else if (formData.Value.BoxedObject is int)
                                        form.Add(new StringContent(formData.Value.BoxedObject.ToString()), formData.Key);
                                    else if (formData.Value.BoxedObject != null)
                                        Log.Warning("RESTConnector", "Unsupported form field type {0}", formData.Value.BoxedObject.GetType().ToString());
                                }

                                foreach (var headerData in form.Headers)
                                {
                                    req.Headers[headerData.Key] = headerData.Value.ToString();
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Error("RESTConnector", "Exception when initializing WWWForm: {0}", e.ToString());
                            }

                            try
                            {
                                using (HttpResponseMessage response = await www.PostAsync(url, form))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resp.Success = true;
                                        resp.Data = await response.Content.ReadAsByteArrayAsync();

                                        autoEvent.Set();
                                    }
                                    else
                                    {
                                        Log.Debug("RESTConnector", "An error occured... Status code {0}", response.StatusCode);
                                        autoEvent.Set();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Debug("RESTConnector", "Error: {0}", e.Message);
                            }
                        }
                        else if (req.Send == null)
                        {
                            //  GET
                            try
                            {
                                using (HttpResponseMessage response = await www.GetAsync(url))
                                {
                                    using (HttpContent content = response.Content)
                                    {
                                        if (response.IsSuccessStatusCode)
                                        {
                                            resp.Success = true;
                                            resp.Data = await content.ReadAsByteArrayAsync();
                                        }
                                        else
                                        {
                                            Log.Debug("RESTConnector", "An error occured... Status code {0}", response.StatusCode);
                                        }

                                        autoEvent.Set();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Debug("RESTConnector", "Error: {0}", e.Message);
                                autoEvent.Set();
                            }
                        }
                        //else
                        //{
                        //    //  POST Body
                        //    try
                        //    {
                        //        HttpContent byteArrayContent = new ByteArrayContent(req.Send);

                        //        if (req.Put)
                        //        {
                        //            response = await www.PutAsync(url, byteArrayContent);
                        //        }
                        //        else
                        //        {
                        //            //HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url);
                        //            //message.Content = byteArrayContent;
                        //            ////message.Content.Headers.Add("Content-Type", req.Headers["Content-Type"]);
                        //            ////MediaTypeHeaderValue contentType;
                        //            ////MediaTypeHeaderValue.TryParse(req.Headers["Content-Type"], out contentType);
                        //            ////message.Content.Headers.ContentType = contentType;
                        //            //message.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(req.Headers["ContentType"]);
                        //            //www.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(req.Headers["Accept"]));
                        //            ////response = await www.PostAsync(url, byteArrayContent);
                        //            //response = await www.SendAsync(message);

                        //            //using (MemoryStream requestStream = new MemoryStream())
                        //            //{
                        //            //    HttpContent streamContent = new StreamContent(requestStream);
                        //            //    requestStream.Write(req.Send, 0, req.Send.Length);

                        //            //    response = await www.PostAsync(url, streamContent);
                        //            //}


                        //            response = await www.PostAsync(url, byteArrayContent);
                        //        }

                        //        if (response.IsSuccessStatusCode)
                        //        {
                        //            resp.Success = true;
                        //            resp.Data = await response.Content.ReadAsByteArrayAsync();
                        //        }
                        //        else
                        //        {
                        //            Log.Debug("RESTConnector", "An error occured... Status code {0}", response.StatusCode);
                        //        }
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        Log.Debug("RESTConnector", "Error: {0}", e.Message);
                        //    }
                        //}

#if ENABLE_DEBUGGING
                    Log.Debug("RESTConnector", "URL: {0}", url);
#endif

                        if (req.OnResponse != null)
                        {
                            req.OnResponse(req, resp);
                        }

                    }
                }
            }

            // reduce the connection count before we exit..
            m_ActiveConnections -= 1;
        }

        private class DeleteRequest
        {
            public string URL { get; set; }
            public Dictionary<string, string> Headers { get; set; }
            public bool IsComplete { get; set; }
            public bool Success { get; set; }

            private Thread m_Thread = null;

            public bool Send(string url, Dictionary<string, string> headers)
            {
#if ENABLE_DEBUGGING
                Log.Debug("RESTConnector", "DeleteRequest, Send: {0}, m_Thread:{1}", url, m_Thread);
#endif
                if (m_Thread != null && m_Thread.IsAlive)
                    return false;

                URL = url;
                Headers = new Dictionary<string, string>();
                foreach (var kp in headers)
                {
                    if (kp.Key != "User-Agent")
                        Headers[kp.Key] = kp.Value;
                }

                m_Thread = new Thread(ProcessRequest);

                m_Thread.Start();
                return true;
            }

            private void ProcessRequest()
            {
#if ENABLE_DEBUGGING
                Log.Debug("RESTConnector", "DeleteRequest, ProcessRequest {0}", URL);
#endif

                WebRequest deleteReq = WebRequest.Create(URL);

                foreach (var kp in Headers)
                    deleteReq.Headers.Add(kp.Key, kp.Value);
                deleteReq.Method = "DELETE";

#if ENABLE_DEBUGGING
                Log.Debug("RESTConnector", "DeleteRequest, sending deletereq {0}", deleteReq);
#endif
                HttpWebResponse deleteResp = deleteReq.GetResponse() as HttpWebResponse;
#if ENABLE_DEBUGGING
                Log.Debug("RESTConnector", "DELETE Request SENT: {0}", URL);
#endif
                Success = deleteResp.StatusCode == HttpStatusCode.OK || deleteResp.StatusCode == HttpStatusCode.NoContent;
#if ENABLE_DEBUGGING
                Log.Debug("RESTConnector", "DELETE Request COMPLETE: {0}", URL);
#endif
                IsComplete = true;
            }
        }

        #endregion
    }
}
