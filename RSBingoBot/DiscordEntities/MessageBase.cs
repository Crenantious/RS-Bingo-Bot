// <copyright file="MessageBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordEntities;

using DSharpPlus.Entities;
using RSBingoBot.Interfaces;

public abstract class MessageBase<T>
    where T : MessageBase<T>
{
    // Max capacity is based on Discord limitations.
    // TODO: JR - Make the max capacity a constant somewhere.
    private List<DiscordActionRowComponent> componentRows = new(5);

    public IReadOnlyList<DiscordActionRowComponent> Components => componentRows.AsReadOnly();

    public string Content { get; set; }

    public MessageBase() { }

    public MessageBase(string content) => WithContent(content);

    public T WithContent(string content)
    {
        Content = content;
        return (T)this;
    }

    public T AddComponents(params DiscordComponent[] components)
    { 
        componentRows.Add(new(components));  
        return (T)this;
    }

    public IMessage AddFile()
    {
        throw new NotImplementedException();
    }

    public IMessage AddImage(Image image)
    {
        throw new NotImplementedException();
    }

    // Probably make the GetBuilder methods extensions.
    public DiscordMessageBuilder GetMessageBuilder() =>
        GetBaseMessageBuilder(new DiscordMessageBuilder());

    public DiscordWebhookBuilder GetWebhookBuilder() =>
        GetBaseMessageBuilder(new DiscordWebhookBuilder());

    /// <summary>
    /// Builds the base message builder using <see cref="Content"/> and <see cref="Components"/>.
    /// </summary>
    /// <typeparam name="T">The type of the builder.</typeparam>
    /// <param name="builder">A new instance of the builder.</param>
    protected TBuilder GetBaseMessageBuilder<TBuilder>(TBuilder builder) where TBuilder : IDiscordMessageBuilder
    {
        builder.Content = Content;
        componentRows.ForEach(r => builder.AddComponents(r));
        return builder;
    }

    //public static MessageBase operator +(MessageBase suffixMessage)
    //{
    //    // Use this to append messages. Combines content, components etc.
    //    throw new NotImplementedException();
    //}
}