using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Utils
{
    public class HttpRequestUnity
    {
        public static Job Get(string url, System.Action<string> OnGetReponse, System.Action<string> OnGetError)
        {
#if UNITY_EDITOR
            Debug.Log(url);
#endif
            return Job.Make(SendHttpRequestGet(url, OnGetReponse, OnGetError));
        }

        public static Job Post(string url, Dictionary<string, string> post,
            System.Action<string> OnGetReponse, System.Action<string> OnGetError)
        {
            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> post_arg in post)
            {
                form.AddField(post_arg.Key, post_arg.Value);
            }
            return Job.Make(SendHttpRequestPost(url, form, OnGetReponse, OnGetError));
        }

        private static IEnumerator SendHttpRequestGet(string url,
            System.Action<string> OnGetResponse, System.Action<string> OnGetError)
        {
            WWW www = new WWW(url);
            yield return www;

            if (www.error == null)
            {
                if (OnGetResponse != null)
                {
                    OnGetResponse(www.text);
                }
            }
            else
            {
                if (OnGetError != null)
                {
                    OnGetError(www.error);
                }
            }
        }

        private static IEnumerator SendHttpRequestPost(string url, WWWForm form,
            System.Action<string> OnGetResponse, System.Action<string> OnGetError)
        {
            WWW www = new WWW(url, form);
            yield return www;

            if (www.error == null)
            {
                if (OnGetResponse != null)
                {
                    OnGetResponse(www.text);
                }
            }
            else
            {
                if (OnGetError != null)
                {
                    OnGetError(www.error);
                }
            }
        }
    }
}
