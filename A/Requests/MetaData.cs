// <copyright file="MetaData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

public class MetaData
{
    private Dictionary<string, object> metaData = new();

    /// <summary>
    /// Adds the kvp. Throws if the key already exists.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void Add(string key, object value) =>
        metaData.Add(key, value);

    /// <summary>
    /// Adds the value associated with <typeparamref name="T"/>.
    /// Throws if an association with <typeparamref name="T"/> already exists.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void Add<T>(T value) =>
        Add(GetKey<T>(), value);

    /// <returns>The value associated with <paramref name="key"/>.</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public object Get(string key) =>
        metaData[key];

    /// <returns>The value associated with <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public T Get<T>() =>
        (T)Get(GetKey<T>());

    /// <returns><see langword="true"/> if there is a value associated with <paramref name="key"/>.<br/>
    /// <see langword="false"/> otherwise.</returns>
    public bool TryGet(string key, out object? value)
    {
        value = metaData.ContainsKey(key) ? metaData[key] : null;
        return value is not null;
    }

    /// <returns><see langword="true"/> if there is a value associated with <typeparamref name="T"/>
    /// and it can be cast to <typeparamref name="T"/>.<br/>
    /// <see langword="false"/> otherwise.</returns>
    public bool TryGet<T>(out T? value)
    {
        if (Exists<T>())
        {
            object o = Get<T>();
            bool correctType = typeof(T).IsAssignableFrom(o.GetType());

            if (correctType)
            {
                value = (T)o;
                return true;
            }
        }
        value = default;
        return false;
    }

    private string GetKey<T>() =>
        typeof(T).Name;

    private bool Exists<T>() =>
        metaData.ContainsKey(GetKey<T>());
}