using System;
using UnityEngine;

public interface IParser<T>
{
    T LoadFrom(string json);
}

public class Parser<T> : IParser<T>
{
    public T LoadFrom(string path)
    {
        //    string wrappedJson = "{\"array\":" + json + "}";
        //    Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
        //    return wrapper.array;
        TextAsset json = Resources.Load<TextAsset>(path);
        
        return JsonUtility.FromJson<T>(json.text);
    }
}