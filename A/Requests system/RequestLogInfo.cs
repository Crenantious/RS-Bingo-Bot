// <copyright file="RequestLogInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using DiscordLibrary.Requests.Extensions;
using System.Text;

public class RequestLogInfo<TResult>
    where TResult : ResultBase<TResult>, new()
{
    private StringBuilder info = new();

    public string GetInfo(IRequest<TResult> request)
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
        AddInteractionInfo(request.GetDiscordInteraction());
    }

    private void ButtonInfo(IButtonRequest request)
    {
        AddInfo("Component type", nameof(Button));
        AddInfo("Component name", request.Component.Name);
        AddInfo("Component id", request.Component.CustomId);
        AddInteractionInfo(request.GetDiscordInteraction());
    }

    private void ModalInfo(IModalRequest request)
    {
        AddInfo("Modal submission values", request.GetInteractionArgs().Values);
    }

    private void DiscordInfo(IDiscordRequest request)
    {

    }

    private void MessageCreatedInfo(IMessageCreatedRequest request)
    {
        // TODO: JR - use the message info that contains all the images and components etc.
        AddInfo("Message content", request.Message.Content);
        AddInfo("Message id", request.Message.DiscordMessage.Id);
    }

    private void AddInteractionInfo(DiscordInteraction interaction)
    {
        AddInfo("Interaction created by", interaction.User.Username);
        AddInfo("Channel", interaction.Channel.Name);
        AddInfo("Interaction id", interaction.Id);
        AddInfo("Interaction type", interaction.Type);
    }

    private void TryAddInfo<T>(IRequest<TResult> request, Action<T> addInfo)
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

    private bool IsType<T>(IRequest<TResult> request) =>
        typeof(T).IsAssignableFrom(request.GetType());
}