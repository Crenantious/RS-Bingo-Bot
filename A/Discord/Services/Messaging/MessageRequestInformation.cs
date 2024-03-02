// <copyright file="MessageRequestInformation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using System.Text;

// TODO: JR - decide if this is needed. Probably not since server audit logs exist.
public static class MessageRequestInformation
{
    public static string Get(Message message, DiscordChannel channel) =>
        $"Channel name: {channel.Name}, channel id: {channel.Id}." +
        $"Message content: {message.Content}, message components: {GetComponentNames(message)}.";

    private static string GetComponentNames(Message message)
    {
        StringBuilder sb = new();

        foreach (List<IComponent> rows in message.Components.GetRows())
        {
            StringBuilder row = new("(");
            foreach (IComponent component in rows)
            {
                row.AppendJoin(", ", rows.Select(c => c.Name));
            }
            row.Append(")");

            // TODO: JR - see if this appends with the comma or if that's just used for the elements passed in.
            sb.AppendJoin(", ", row);
        }

        return sb.ToString();
    }

    private static string GetImageNames(Message message)
    {
        // TODO: JR - implement then add to Log.
        throw new NotImplementedException();
    }
}