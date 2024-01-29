// <copyright file="RequestMetaData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DataStructures;

using RSBingo_Common;

public class RequestMetaData
{
    private readonly Dictionary<string, object> data = new();

    internal void Add(string key, object value) =>
        data.Add(key, value);

    internal void Add(params (string key, object value)[] metaData) =>
        metaData.ForEach(d => data.Add(d.key, d.value));

    internal void Remove(string key) =>
       data.Remove(key);

    public bool TryGet(string key, out object value) =>
        data.TryGetValue(key, out value);

    public T Get<T>(string key) =>
        (T)data[key];

    /// <summary>
    /// Will use the name of <typeparamref name="T"/> as the key.
    /// </summary>
    public T Get<T>() =>
        (T)data[typeof(T).Name];
}