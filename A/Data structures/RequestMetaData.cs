// <copyright file="RequestMetaData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DataStructures;

using RSBingo_Common;

public class RequestMetaData
{
    private readonly Dictionary<string, object> data = new();

    /// <param name="key">If null, the name of the type of <paramref name="value"/> will be used.</param>
    internal void Add(string? key, object value) =>
        data.Add(key ?? value.GetType().Name, value);

    /// <param name="metaData">If a key is null, the name of the type of value will be used.</param>
    internal void Add(params (string? key, object value)[] metaData) =>
        metaData.ForEach(d => Add(d.key, d.value));

    internal void Remove(string key) =>
       data.Remove(key);

    internal void Remove<T>() =>
        data.Remove(typeof(T).Name);

    public bool TryGet(string key, out object value) =>
        data.TryGetValue(key, out value);

    public bool TryGet<T>(out object value) =>
        data.TryGetValue(typeof(T).Name, out value);

    public T Get<T>(string key) =>
        (T)data[key];

    /// <summary>
    /// Will use the name of <typeparamref name="T"/> as the key.
    /// </summary>
    public T Get<T>() =>
        (T)data[typeof(T).Name];
}