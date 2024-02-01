// <copyright file="RequestExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Extensions;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MediatR;
using RSBingo_Common;

public static class RequestExtensions
{
    private static readonly RequestsTracker trackers;

    static RequestExtensions()
    {
        trackers = (RequestsTracker)General.DI.GetService(typeof(RequestsTracker))!;
    }

    internal static RequestTracker GetTracker(this IBaseRequest request) =>
        trackers.GetActive(request);

    public static DiscordInteraction GetDiscordInteraction(this IInteractionRequest request) =>
        request.GetFromMetaData<DiscordInteraction>();

    public static ModalSubmitEventArgs GetInteractionArgs(this IModalRequest request) =>
        request.GetFromMetaData<ModalSubmitEventArgs>();

    public static ComponentInteractionCreateEventArgs GetInteractionArgs<TComponent>(this IComponentInteractionRequest<TComponent> request)
        where TComponent : Component =>
        request.GetFromMetaData<ComponentInteractionCreateEventArgs>();

    public static TComponent GetComponent<TComponent>(this IComponentRequest<TComponent> request)
        where TComponent : Component =>
        request.GetFromMetaData<TComponent>();

    public static Message GetMessage(this IMessageCreatedRequest request) =>
        request.GetFromMetaData<Message>();

    private static T GetFromMetaData<T>(this IBaseRequest request) =>
        trackers.GetActive(request).MetaData.Get<T>();
}