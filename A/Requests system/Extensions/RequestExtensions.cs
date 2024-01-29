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
    private static readonly RequestsTracker tracker;

    static RequestExtensions()
    {
        tracker = (RequestsTracker)General.DI.GetService(typeof(RequestsTracker))!;
    }

    internal static RequestTracker GetTracker(this IBaseRequest request) =>
        tracker.Trackers[request];

    public static DiscordInteraction GetDiscordInteraction(this IInteractionRequest request) =>
        request.Get<DiscordInteraction>();

    public static ModalSubmitEventArgs GetInteractionArgs(this IModalRequest request) =>
        request.Get<ModalSubmitEventArgs>();

    public static ComponentInteractionCreateEventArgs GetInteractionArgs<TComponent>(this IComponentInteractionRequest<TComponent> request)
        where TComponent : Component =>
        request.Get<ComponentInteractionCreateEventArgs>();

    public static TComponent GetComponent<TComponent>(this IComponentRequest<TComponent> request)
        where TComponent : Component =>
        request.Get<TComponent>();

    public static Message GetMessage(this IMessageCreatedRequest request) =>
        request.Get<Message>();

    private static T Get<T>(this IBaseRequest request) =>
        tracker.Trackers[request].MetaData.Get<T>();
}