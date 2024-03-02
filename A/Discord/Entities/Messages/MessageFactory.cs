// <copyright file="MessageFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordServices;
using DSharpPlus.Entities;
using FluentResults;

public class MessageFactory
{
    public Message Create(DiscordMessage discordMessage, IWebServices webServices)
    {
        Message message = new(discordMessage.Channel);

        message.DiscordMessage = discordMessage;

        AddConent(message);
        AddComponents(discordMessage, message);

        return message;
    }

    private static void AddConent(Message message)
    {
        if (message.Content is not null)
        {
            message.WithContent(message.Content);
        }
    }

    private static void AddComponents(DiscordMessage discordMessage, Message message)
    {
        if (message.Components is not null)
        {
            foreach (var row in discordMessage.Components)
            {
                message.AddComponents(row.Components);
            }
        }
    }

    private async Task AddFiles(DiscordMessage discordMessage, Message message, IWebServices webServices)
    {
        MessageFile messageFile;

        foreach (var attachment in discordMessage.Attachments)
        {
            (string name, string extension) = GetAttachmentPathInfo(attachment);
            string path = GetTempPath(extension);
            Result result = await webServices.DownloadFile(attachment.Url, path);

            if (result.IsFailed)
            {
                continue;
            }

            messageFile = new(name);
            messageFile.SetContent(path);
            message.AddFile(messageFile);
        }
    }

    // TODO: JR - confirm this works for all attachments.
    private (string name, string extension) GetAttachmentPathInfo(DiscordAttachment attachment)
    {
        string suffix = attachment.Url.Split("/")[^-1];
        string fileNameAndExtension = suffix.Substring(0, suffix.LastIndexOf("?"));
        string name = fileNameAndExtension.Substring(0, suffix.LastIndexOf("."));
        string extension = fileNameAndExtension.Substring(suffix.LastIndexOf("."));
        return (name, extension);
    }

    private string GetTempPath(string extension)
    {
        string path = Path.GetTempFileName();
        return Path.ChangeExtension(path, extension);
    }
}