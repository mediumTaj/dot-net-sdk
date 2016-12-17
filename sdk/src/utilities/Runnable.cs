
#define ENABLE_RUNNABLE_DEBUGGING

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
using System.Collections;
using System.Collections.Generic;

namespace IBM.Watson.DeveloperCloud.Utilities
{
    /// <summary>
    /// Helper class for running co-routines without having to inherit from MonoBehavior.
    /// </summary>
    public class Runnable
    {
        #region Public Properties
        /// <summary>
        /// Returns the Runnable instance.
        /// </summary>
        public static Runnable Instance { get { return Singleton<Runnable>.Instance; } }
        #endregion

        #region Public Interface
        /// <summary>
        /// Start a co-routine function.
        /// </summary>
        /// <param name="routine">The IEnumerator returns by the co-routine function the user is invoking.</param>
        /// <returns>Returns a ID that can be passed into Stop() to halt the co-routine.</returns>
        public static int Run(IEnumerator routine)
        {
            Routine r = new Routine(routine);
            return r.ID;
        }

        /// <summary>
        /// Stops a active co-routine.
        /// </summary>
        /// <param name="ID">THe ID of the co-routine to stop.</param>
        public static void Stop(int ID)
        {
            Routine r = null;
            if (Instance.m_Routines.TryGetValue(ID, out r))
                r.Stop = true;
        }

        /// <summary>
        /// Check if a routine is still running.
        /// </summary>
        /// <param name="id">The ID returned by Run().</param>
        /// <returns>Returns true if the routine is still active.</returns>
        static public bool IsRunning(int id)
        {
            return Instance.m_Routines.ContainsKey(id);
        }

        #endregion

        #region Private Types
        /// <summary>
        /// This class handles a running co-routine.
        /// </summary>
        public class Routine : IEnumerator
        {
            #region Public Properties
            public int ID { get; private set; }
            public bool Stop { get; set; }
            #endregion

            #region Private Data
            private bool m_bMoveNext = false;
            private IEnumerator m_Enumerator = null;
            #endregion

            public Routine(IEnumerator a_enumerator)
            {
                m_Enumerator = a_enumerator;

                Runnable.Instance.StartCoroutine(this);

                Stop = false;
                ID = Runnable.Instance.m_NextRoutineId++;

                Runnable.Instance.m_Routines[ID] = this;
#if ENABLE_RUNNABLE_DEBUGGING
                Log.Debug("Runnable", string.Format("Coroutine {0} started.", ID));
#endif
            }

            #region IEnumerator Interface
            public object Current { get { return m_Enumerator.Current; } }
            public bool MoveNext()
            {
                m_bMoveNext = m_Enumerator.MoveNext();
                if (m_bMoveNext && Stop)
                    m_bMoveNext = false;

                if (!m_bMoveNext)
                {
                    Runnable.Instance.m_Routines.Remove(ID);      // remove from the mapping
#if ENABLE_RUNNABLE_DEBUGGING
                    Log.Debug("Runnable", string.Format("Coroutine {0} stopped.", ID));
#endif
                }

                return m_bMoveNext;
            }
            public void Reset() { m_Enumerator.Reset(); }
            #endregion
        }

        public Coroutine StartCoroutine(Routine r)
        {
            return new Coroutine(r);
        }

        public class Coroutine
        {
            private Routine m_Routine;
            private CoroutineState m_State;
            public CoroutineState State
            {
                get { return m_State; }
                set
                {
                    m_State = value;
                }
            }
            public Coroutine(Routine r)
            {
                m_Routine = r;
                State = CoroutineState.Ready;
                Start();
            }

            public IEnumerator Start()
            {
                if (State != CoroutineState.Ready)
                    throw new WatsonException("Unable to start coroutine in state " + State);

                State = CoroutineState.Running;
                while (m_Routine.MoveNext())
                {
                    yield return m_Routine.Current;
                    while (State == CoroutineState.Paused)
                    {
                        yield return null;
                    }
                    if (State == CoroutineState.Finished)
                    {
                        yield break;
                    }
                }

                State = CoroutineState.Finished;
            }

            public void Stop()
            {
                if (State != CoroutineState.Running && State != CoroutineState.Paused)
                    throw new System.InvalidOperationException("Unable to stop coroutine in state: " + State);

                State = CoroutineState.Finished;
            }

            public void Pause()
            {
                if (State != CoroutineState.Running)
                    throw new System.InvalidOperationException("Unable to pause coroutine in state: " + State);

                State = CoroutineState.Paused;
            }

            public void Resume()
            {
                if (State != CoroutineState.Paused)
                    throw new System.InvalidOperationException("Unable to resume coroutine in state: " + State);

                State = CoroutineState.Running;
            }
        }
        #endregion

        #region Coroutine States
        public enum CoroutineState
        {
            Ready,
            Running,
            Paused,
            Finished
        }
        #endregion

        #region Private Data
        private Dictionary<int, Routine> m_Routines = new Dictionary<int, Routine>();
        private int m_NextRoutineId = 1;
        #endregion

        /// <summary>
        /// THis can be called by the user to force all co-routines to get a time slice, this is usually
        /// invoked from an EditorApplication.Update callback so we can use runnable in Editor mode.
        /// </summary>
        public void UpdateRoutines()
        {
            if (m_Routines.Count > 0)
            {
                // we are not in play mode, so we must manually update our co-routines ourselves
                List<Routine> routines = new List<Routine>();
                foreach (var kp in m_Routines)
                    routines.Add(kp.Value);

                foreach (var r in routines)
                    r.MoveNext();
            }
        }
    }
}
