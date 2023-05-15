// <copyright file="ComponentInteractionHandlerException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions;

using DSharpPlus.EventArgs;

public class ComponentInteractionHandlerException : RSBingoException
{
    public bool ConcludeInteraction { get; }
    public bool IsEphemeral { get; }
    public ErrorResponseType ResponseType { get; }

    public enum ErrorResponseType
    {
        EditOriginalResponse,
        CreateFollowUpResponse
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messsage"></param>
    /// <param name="args"></param>
    /// <param name="concludeInteraction"></param>
    /// <param name="responseType"></param>
    /// <param name="isEphemeral">If the response is ephemeral, this is ignored if <paramref name="responseType"/> ==
    /// <see cref="ErrorResponseType.EditOriginalResponse"/>.</param>
    public ComponentInteractionHandlerException(string messsage, ComponentInteractionCreateEventArgs args,
        bool concludeInteraction, ErrorResponseType responseType, bool isEphemeral = false) : base(messsage)
    {
        ConcludeInteraction = concludeInteraction;
        ResponseType = responseType;
        IsEphemeral = isEphemeral;
    }
}