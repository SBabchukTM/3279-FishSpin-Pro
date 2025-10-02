using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class ApiSender : MonoBehaviour
{
    public IEnumerator SendExtraParams(Dictionary<string, string> data, System.Action<string> callback = null)
    {Debug.Log(" ----- SendExtraParams");
        string query = "";
        foreach (var kv in data)
        {
            if (query.Length > 0) query += "&";
            query += UnityWebRequest.EscapeURL(kv.Key) + "=" + UnityWebRequest.EscapeURL(kv.Value);
        }

        string url = Settings.GetPushNotificationApiUrl() + "?" + query;

        Debug.Log($" 🔗 Url: {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Extra params sent (GET): " + request.downloadHandler.text);
                callback?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("❌ Error (GET): " + request.error);
            }
        }
    }


    [System.Serializable]
    private class SerializableDict
    {
        public List<string> keys = new List<string>();
        public List<string> values = new List<string>();

        public SerializableDict(Dictionary<string, string> dict)
        {
            foreach (var kv in dict)
            {
                keys.Add(kv.Key);
                values.Add(kv.Value);
            }
        }
    }
}