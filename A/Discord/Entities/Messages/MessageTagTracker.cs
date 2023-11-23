// <copyright file="MessageTagTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

public static class MessageTagTracker
{
    private static Dictionary<string, List<Message>> messages = new();

    public static void Add(Message message)
    {
        if (string.IsNullOrEmpty(message.Tag))
        {
            return;
        }

        if (messages.ContainsKey(message.Tag) is false)
        {
            messages.Add(message.Tag, new());
        }

        messages[message.Tag].Add(message);
    }

    public static void Remove(Message message)
    {
        if (messages.ContainsKey(message.Tag))
        {
            messages[message.Tag].Remove(message);
        }
    }

    public static IEnumerable<Message> GetWithTag(string tag) =>
        messages.ContainsKey(tag) ?
            messages[tag] :
            Enumerable.Empty<Message>();
}