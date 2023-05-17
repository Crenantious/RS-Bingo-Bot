// <copyright file="MessageUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using static RSBingo_Common.General;

public static class MessageUtilities
{
    public static async Task<bool> Respond(DiscordInteraction args, string content, bool isEphemeral)
    {
        var builder = new DiscordInteractionResponseBuilder()
        {
            Content = content,
            IsEphemeral = isEphemeral
        };

        return await Respond(args, builder);
    }

    public static async Task<bool> Respond(DiscordInteraction args, DiscordInteractionResponseBuilder builder)
    {
        try
        {
            await args.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
            return true;
        }
        catch { return false; }
    }

    public static async Task<bool> EditResponse(DiscordInteraction args, string content) =>
        await EditResponse(args, new DiscordWebhookBuilder().WithContent(content));

    public static async Task<bool> EditResponse(DiscordInteraction args, DiscordWebhookBuilder builder)
    {
        try
        {
            await args.EditOriginalResponseAsync(builder);
            return true;
        }
        catch { return false; }
    }

    public static async Task<bool> DeleteResponse(DiscordInteraction args)
    {
        try
        {
            await args.DeleteOriginalResponseAsync();
            return true;
        }
        catch { return false; }
    }

    public static async Task<bool> Followup(DiscordInteraction args, string content, bool isEphemeral)
    {
        var builder = new DiscordFollowupMessageBuilder()
        {
            Content = content,
            IsEphemeral = isEphemeral
        };

        return await Followup(args, builder);
    }

    public static async Task<bool> Followup(DiscordInteraction args, DiscordFollowupMessageBuilder builder)
    {
        try
        {
            await args.CreateFollowupMessageAsync(builder);
            return true;
        }
        catch { return false; }
    }

    public static async Task<bool> EditFollowup(DiscordInteraction args, ulong messageId, string content) =>
       await EditFollowup(args, messageId, new DiscordWebhookBuilder().WithContent(content));

    public static async Task<bool> EditFollowup(DiscordInteraction args, ulong messageId, DiscordWebhookBuilder builder)
    {
        try
        {
            await args.EditFollowupMessageAsync(messageId, builder);
            return true;
        }
        catch { return false; }
    }

    /// <summary>
    /// <paramref name="messages"/> are added in turn to a compiled message. If adding the message would cause the compiled message
    /// to exceed <paramref name="maxMessageChars"/> in length, a new compiled message is created with that message instead.
    /// </summary>
    /// <param name="messages">The messages to be compiled.</param>
    /// <param name="maxMessageChars">The maximum amount of characters each compiled message can contain.</param>
    /// <returns>The compiled messages.</returns>
    public static IEnumerable<string> GetCompiledMessages(IEnumerable<string> messages, int maxMessageChars = MaxCharsPerDiscordMessage)
    {
        List<string> compiledMessages = new();

        if (messages.Any() is false) { return compiledMessages; }

        while (messages.Any())
        {
            (string compiledMessage, messages) = GetFirstCompiledMessage(messages, maxMessageChars);
            compiledMessages.Add(compiledMessage);
        }

        return compiledMessages;
    }

    private static (string message, IEnumerable<string> reminingWarnings) GetFirstCompiledMessage(IEnumerable<string> messages, int maxMessageChars)
    {
        StringBuilder compiledMessage = new();
        int messageIndex = 0;

        foreach (string message in messages)
        {
            if (compiledMessage.Length + message.Length == maxMessageChars)
            {
                compiledMessage.Append(message);
                messageIndex++;
                break;
            }

            if (compiledMessage.Length + message.Length + Environment.NewLine.Length > maxMessageChars)
            {
                if (messageIndex == 0)
                {
                    compiledMessage.Append(message.Substring(0, maxMessageChars));
                    string[] remainingMessages = messages.ToArray();
                    remainingMessages[0] = remainingMessages[0].Substring(maxMessageChars);
                    messages = remainingMessages;
                }
                break;
            }

            if (messageIndex == 0) { compiledMessage.Append(message); }
            else { compiledMessage.Append(Environment.NewLine + message); }

            messageIndex++;
        }

        return (compiledMessage.ToString(), messages.Skip(messageIndex));
    }
}