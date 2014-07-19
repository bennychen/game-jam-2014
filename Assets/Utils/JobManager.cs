using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JobManager : MonoSingleton<JobManager>
{
}

namespace GameJam.Utils
{
    public class Job
    {
        public event System.Action<bool> OnComplete;

        public bool Running { get; private set; }
        public bool Paused { get; private set; }

        public Job(IEnumerator coroutine)
            : this(coroutine, true)
        { }

        public Job(IEnumerator coroutine, bool shouldStart)
        {
            _coroutine = coroutine;

            if (shouldStart)
                Start();
        }

        [System.Obsolete("This function is dangerous to cause bug.")]
        public Job CreateAndAddChildJob(IEnumerator coroutine)
        {
            var j = new Job(coroutine, false);
            AddChildJob(j);
            return j;
        }

        [System.Obsolete("This function is dangerous to cause bug.")]
        public void AddChildJob(Job childJob)
        {
            if (_childJobStack == null)
                _childJobStack = new Stack<Job>();
            _childJobStack.Push(childJob);
        }

        [System.Obsolete("This function is dangerous to cause bug.")]
        public void RemoveChildJob(Job childJob)
        {
            if (_childJobStack.Contains(childJob))
            {
                var childStack = new Stack<Job>(_childJobStack.Count - 1);
                var allCurrentChildren = _childJobStack.ToArray();
                System.Array.Reverse(allCurrentChildren);

                for (var i = 0; i < allCurrentChildren.Length; i++)
                {
                    var j = allCurrentChildren[i];
                    if (j != childJob)
                        childStack.Push(j);
                }

                // assign the new stack
                _childJobStack = childStack;
            }
        }

        public void Start()
        {
            Running = true;
            JobManager.Instance.StartCoroutine(DoWork());
        }

        public IEnumerator StartAsCoroutine()
        {
            Running = true;
            yield return JobManager.Instance.StartCoroutine(DoWork());
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Unpause()
        {
            Paused = false;
        }

        public void Kill()
        {
            _jobWasKilled = true;
            Running = false;
            Paused = false;
        }

        public void Kill(float delayInSeconds)
        {
            var delay = (int)(delayInSeconds * 1000);
            new System.Threading.Timer(obj =>
            {
                lock (this)
                {
                    Kill();
                }
            }, null, delay, System.Threading.Timeout.Infinite);
        }

        public static Job Make(IEnumerator coroutine)
        {
            return new Job(coroutine);
        }

        public static Job Make(IEnumerator coroutine, bool shouldStart)
        {
            return new Job(coroutine, shouldStart);
        }

        #region Private

        private IEnumerator DoWork()
        {
            // null out the first run through in case we start paused
            yield return null;

            while (Running)
            {
                if (Paused)
                {
                    yield return null;
                }
                else
                {
                    // run the next iteration and stop if we are done
                    if (_coroutine.MoveNext())
                    {
                        yield return _coroutine.Current;
                    }
                    else
                    {
                        // run our child jobs if we have any
                        if (_childJobStack != null)
                            yield return JobManager.Instance.StartCoroutine(RunChildJobs());
                        Running = false;
                    }
                }
            }

            // fire off a complete event
            if (OnComplete != null)
                OnComplete(_jobWasKilled);
        }

        private IEnumerator RunChildJobs()
        {
            if (_childJobStack != null && _childJobStack.Count > 0)
            {
                do
                {
                    Job childJob = _childJobStack.Pop();
                    yield return JobManager.Instance.StartCoroutine(childJob.StartAsCoroutine());
                }
                while (_childJobStack.Count > 0);
            }
        }

        private IEnumerator _coroutine;
        private bool _jobWasKilled;
        private Stack<Job> _childJobStack;

        #endregion
    }
}