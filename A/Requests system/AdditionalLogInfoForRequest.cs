// <copyright file="AdditionalLogInfoForRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using FluentResults;
using System.Text;

public class AdditionalLogInfoForRequest
{
    private StringBuilder info = new();

    public string GetInfo(IResultBase request)
    {
        TryAddInfo<ISelectComponentRequest>(request, SelectComponentInfo);
        TryAddInfo<IButtonRequest>(request, ButtonInfo);
        TryAddInfo<IModalRequest>(request, ModalInfo);
        TryAddInfo<IDiscordRequest>(request, DiscordInfo);
        TryAddInfo<IMessageCreatedRequest>(request, MessageCreatedInfo);
        return info.ToString();
    }

    private void SelectComponentInfo(ISelectComponentRequest request)
    {
        AddInfo("Component type", nameof(SelectComponent));
        AddInfo("Component name", request.Component.Name);
        AddInfo("Component id", request.Component.CustomId);
        AddInteractionInfo(request.InteractionArgs);
    }

    private void ButtonInfo(IButtonRequest request)
    {
        AddInfo("Component type", nameof(Button));
        AddInfo("Component name", request.Component.Name);
        AddInfo("Component id", request.Component.CustomId);
        AddInteractionInfo(request.InteractionArgs);
    }

    private void ModalInfo(IModalRequest request)
    {
        AddInfo("Modal submission values", request.InteractionArgs.Values);
    }

    private void DiscordInfo(IDiscordRequest request)
    {
        // Maybe; may not need any info other than it being a Discord request.
        AddInfo("Discord request type", request.RequestTpye);
    }

    private void MessageCreatedInfo(IMessageCreatedRequest request)
    {
        // TODO: JR - use the message info that contains all the images and components etc.
        AddInfo("Message content", request.Message.Content);
        AddInfo("Message id", request.Message.DiscordMessage.Id);
    }

    private void AddInteractionInfo(InteractionCreateEventArgs args)
    {
        AddInfo("Interaction created by", args.Interaction.User.Username);
        AddInfo("Channel", args.Interaction.Channel.Name);
        AddInfo("Interaction id", args.Interaction.Id);
        AddInfo("Interaction type", args.Interaction.Type);
    }

    private void TryAddInfo<T>(IResultBase request, Action<T> addInfo)
    {
        if (IsType<T>(request))
        {
            AddInfo("Request type", request.GetType().Name);
            addInfo((T)request);
        }
    }

    private void AddInfo(string category, object value)
    {
        info.AppendLine($"{category}: {value}.");
    }

    private bool IsType<T>(IResultBase request) =>
        typeof(T).IsAssignableFrom(request.GetType());
}