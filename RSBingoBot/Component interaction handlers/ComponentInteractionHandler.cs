// <copyright file="ComponentInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using RSBingoBot;
    using RSBingoBot.Discord_event_handlers;

    // Listen for MessageDeleted event a delete the instance if necessary

    /// <summary>
    /// Handles the callback when a component is interacted with.
    /// </summary>
    public class ComponentInteractionHandler
    {
        private static readonly Dictionary<string, (Type, InitialisationInfo)> RegisteredComponentIds = new ();
        private static readonly List<ComponentInteractionHandler> Instances = new ();

        // TODO: JR - re-factor to not have to wrap every DEH subscription.
        private readonly List<(ComponentInteractionDEH.Constraints,
                               Func<DiscordClient,
                               ComponentInteractionCreateEventArgs, Task>)> subscribedComponentsInfo = new ();

        private readonly List<(MessageCreatedDEH.Constraints,
                               Func<DiscordClient,
                               MessageCreateEventArgs, Task>)> subscribedMessagesInfo = new ();

        /// <summary>
        /// Gets the messages to delete when the original interaction has concluded.
        /// </summary>
        protected List<DiscordMessage> MessagesForCleanup { get; private set; } = new ();

        /// <summary>
        /// Gets the info used to initialize the class instance.
        /// </summary>
        protected InitialisationInfo Info { get; private set; }

        /// <summary>
        /// Gets the custom id of the intractable object registered to the class instance.
        /// </summary>
        protected string CustomId { get; private set; } = null!;

        /// <summary>
        /// Gets the event args for the <see cref="ComponentInteractionCreate"/> event that was triggered when the registered
        /// component was interacted with.
        /// </summary>
        protected ComponentInteractionCreateEventArgs OriginalInteractionArgs { get; private set; } = null!;

        /// <summary>
        /// Gets the client the interaction happened on.
        /// </summary>
        protected DiscordClient Client { get; private set; } = null!;

        /// <summary>
        /// Register a component so that when it is interacted with, an instance of the given <see cref="ComponentInteractionHandler"/>
        /// type will be created to handle it.
        /// </summary>
        /// <typeparam name="T">The type of handler to created.</typeparam>
        /// <param name="customId">The custom id of the component.</param>
        /// <param name="info">Info to pass to the handler when the component is interacted with.</param>
        public static void Register<T>(string customId, InitialisationInfo info) where T : ComponentInteractionHandler
        {
            RegisteredComponentIds.Add(customId, (typeof(T), info));
            ComponentInteractionDEH.Subscribe(new (CustomId: customId), RegisteredComponentInteracted);
        }

        /// <summary>
        /// The method to be called when a component is interacted with. This will create an instance of
        /// <see cref="ComponentInteractionHandler"/> for the component if it is registered.
        /// </summary>
        /// <param name="discordClient">The client the interaction occurred on.</param>
        /// <param name="args">The event args.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task RegisteredComponentInteracted(DiscordClient discordClient, ComponentInteractionCreateEventArgs args)
        {
            (Type, InitialisationInfo) info = RegisteredComponentIds[args.Interaction.Data.CustomId];
            ComponentInteractionHandler? instance = (ComponentInteractionHandler?)Activator.CreateInstance(info.Item1);

            if (instance != null)
            {
                Instances.Add(instance);
                instance.Client = discordClient;
                instance.CustomId = args.Interaction.Data.CustomId;
                await instance.InitialiseAsync(args, info.Item2);
            }
            else
            {
                // Log error
            }
        }

        /// <summary>
        /// Initializes the <see cref="ComponentInteractionHandler"/>.
        /// </summary>
        /// <param name="args">Event args of the component that was interacted with.</param>
        /// <param name="info">Info relating to the handler and the component it is registered to.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async virtual Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            OriginalInteractionArgs = args;
            Info = info;
        }

        /// <summary>
        /// Subscribes the component to <see cref="ComponentInteractionDEH"/> for interaction callbacks and keeps
        /// track of which components have been subscribed so they can be unsubscribed when the interaction has concluded.
        /// </summary>
        /// <param name="constraints"><inheritdoc cref="ComponentInteractionDEH.Subscribe(ComponentInteractionDEH.Constraints, Func{DiscordClient, ComponentInteractionCreateEventArgs, Task})" path="/param[@name='constraints']"/></param>
        /// <param name="callback"><inheritdoc cref="ComponentInteractionDEH.Subscribe(ComponentInteractionDEH.Constraints, Func{DiscordClient, ComponentInteractionCreateEventArgs, Task})" path="/param[@name='callback']"/></param>
        protected void SubscribeComponent(ComponentInteractionDEH.Constraints constraints,
            Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> callback)
        {
            ComponentInteractionDEH.Subscribe(constraints, callback);
            subscribedComponentsInfo.Add((constraints, callback));
        }

        /// <summary>
        /// Subscribes the <paramref name="callback"/> to <see cref="MessageCreatedDEH"/> and keeps
        /// track of which messages have been subscribed so they can be unsubscribed when the interaction has concluded.
        /// </summary>
        /// <param name="constraints"><inheritdoc cref="MessageCreatedDEH.Subscribe(MessageCreatedDEH.Constraints, Func{DiscordClient, MessageCreateEventArgs, Task})" path="/param[@name='constraints']"/></param>
        /// <param name="callback"><inheritdoc cref="MessageCreatedDEH.Subscribe(MessageCreatedDEH.Constraints, Func{DiscordClient, MessageCreateEventArgs, Task})" path="/param[@name='callback']"/></param>
        protected void SubscribeMessage(MessageCreatedDEH.Constraints constraints,
            Func<DiscordClient, MessageCreateEventArgs, Task> callback)
        {
            MessageCreatedDEH.Subscribe(constraints, callback);
            subscribedMessagesInfo.Add((constraints, callback));
        }

        /// <summary>
        /// Cleans up messages and unsubscribes all subscribed callbacks from
        /// <see cref="ComponentInteractionDEH"/> and <see cref="MessageCreatedDEH"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async virtual Task InteractionConcluded()
        {
            foreach (var message in MessagesForCleanup)
            {
                if (message != null)
                {
                    try
                    {
                        await message.DeleteAsync();
                    }
                    catch { }
                }
            }

            foreach (var componentSubscriptionInfo in subscribedComponentsInfo)
            {
                ComponentInteractionDEH.UnSubscribe(componentSubscriptionInfo.Item1, componentSubscriptionInfo.Item2);
            }

            foreach (var messageSubscriptionInfo in subscribedMessagesInfo)
            {
                MessageCreatedDEH.UnSubscribe(messageSubscriptionInfo.Item1, messageSubscriptionInfo.Item2);
            }
        }

        /// <summary>
        /// Information used to initialize a <see cref="ComponentInteractionHandler"/>.
        /// </summary>
        public struct InitialisationInfo
        {
            /// <summary>
            /// The team the handler represents.
            /// </summary>
            public Team Team;
        }
    }
}