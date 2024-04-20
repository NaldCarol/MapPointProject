using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using System.Collections;
using System;

namespace SideProject.Network
{
    public sealed class GASManager
    {
        private static readonly GASManager instance = new GASManager();

        public static GASManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void DoGet (string url, string parameter, Action<string> callback)
        {
            var urlParameter = UnityWebRequest.EscapeURL(parameter);
            var urlString = string.Format("{0}?name={1}", url, urlParameter);
            var request = UnityWebRequest.Get(urlString);
            var asyncOperation = request.SendWebRequest();
            string result = string.Empty;
            asyncOperation.completed += (operation) =>
            {
                if (request.result != UnityWebRequest.Result.Success)
                {
                    result = request.error;
                }
                else
                {
                    result = request.downloadHandler.text;
                }
                callback(result);
            };
        }

        public void DoPost (string url, string data)
        {
            var request = UnityWebRequest.Post(url, data, "text/plain");
            var asyncOperation = request.SendWebRequest();
            asyncOperation.completed += (operation) =>
            {
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning(request.error);
                    return;
                }
                Debug.Log(request.downloadHandler.text);
            };
        }
    }
}