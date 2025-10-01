using UnityEngine;

namespace Services
{
    public class JsonSerializer<T>
    {
        public string Serialize(T data)
        {
            return JsonUtility.ToJson(data, true);
        }

        public T Deserialize(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}