// <copyright file="ComponentInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using RSBingo_Common;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingoBot;
    using RSBingoBot.Discord_event_handlers;
    using static RSBingo_Framework.DAL.DataFactory;

    /// <summary>
    /// Handles the callback when a component is interacted with.
    /// </summary>
    public abstract class ComponentInteractionHandler : IDisposable
    {
        private static readonly Dictionary<string, (Type, InitialisationInfo)> RegisteredComponentIds = new ();
        private static readonly List<ComponentInteractionHandler> Instances = new ();
        private static readonly ComponentInteractionDEH componentInteractionDEH = null!;
        private static readonly MessageCreatedDEH messageCreatedDEH = null!;
        private static readonly ModalSubmittedDEH modalSubmittedDEH = null!;

        // TODO: JR - re-factor to not have to wrap every DEH subscription.
        private readonly List<(ComponentInteractionDEH.Constraints,
                               Func<DiscordClient,
                               ComponentInteractionCreateEventArgs, Task>)> subscribedComponentsInfo = new ();

        private readonly List<(MessageCreatedDEH.Constraints,
                               Func<DiscordClient,
                               MessageCreateEventArgs, Task>)> subscribedMessagesInfo = new ();

        private readonly List<(ModalSubmittedDEH.Constraints,
                       Func<DiscordClient,
                       ModalSubmitEventArgs, Task>)> subscribedModalInfo = new ();

        static ComponentInteractionHandler()
        {
            componentInteractionDEH = (ComponentInteractionDEH)General.DI.GetService(typeof(ComponentInteractionDEH));
            messageCreatedDEH = (MessageCreatedDEH)General.DI.GetService(typeof(MessageCreatedDEH));
            modalSubmittedDEH = (ModalSubmittedDEH)General.DI.GetService(typeof(ModalSubmittedDEH));
        }

        /// <summary>
        /// Gets the <see cref="IDataWorker"/> instance.
        /// </summary>
        protected IDataWorker DataWorker { get; private set; } = CreateDataWorker();

        /// <summary>
        /// Gets or sets the user that interacted with the component.
        /// </summary>
        protected User? User { get; set; } = null!;

        /// <summary>
        /// Gets a value indicating whether or not the interaction should continue
        /// if the user that interacted with the component is not found in the database.
        /// </summary>
        protected abstract bool ContinueWithNullUser { get; }

        /// <summary>
        /// Gets a value indicating whether or not the interaction should continue
        /// if the user that interacted with the component is not found to be on the <see cref="Info.Team"/> team.<br/>
        /// This is ignored if the team is null.
        /// </summary>
        protected virtual bool UserMustBeInTeam { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether or not the team with the name <see cref="Info.Team.Name"/> must
        /// exist in the database to continue. This is ignore if the <see cref="Info.Team"/> is null.
        /// </summary>
        protected virtual bool TeamMustExist { get { return true; } }

        /// <summary>
        /// Gets the <see cref="Team"/> this handler is registered to. Null if it is either not registered to one
        ///  or the team does not exist in the database.
        /// </summary>
        protected Team? Team { get; private set; }

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
        public static void Register<T>(string customId, InitialisationInfo info = default) where T : ComponentInteractionHandler
        {
            RegisteredComponentIds[customId] = (typeof(T), info);
            componentInteractionDEH.Subscribe(
                new ComponentInteractionDEH.Constraints(customId: customId),
                RegisteredComponentInteracted);
        }

        /// <summary>
        /// The method to be called when a component is interacted with. This will attempt to create an instance of
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
                User? user = instance.DataWorker.Users.GetByDiscordId(args.User.Id);
                Team? team = info.Item2.Team != null ?
                    instance.DataWorker.Teams.GetByName(info.Item2.Team.Name) :
                    null;

                if (user == null)
                {
                    if (!instance.ContinueWithNullUser)
                    {
                        // TODO: notify admins of this and tell the user they have been notified
                        throw new NullReferenceException("User is not in the database.");
                    }
                }

                if (info.Item2.Team != null)
                {
                    if (instance.TeamMustExist && team == null)
                    {
                        // TODO: notify admins of this and tell the user they have been notified
                        throw new NullReferenceException($"The team with name {instance.Info.Team.Name} does not exist in the database.");
                    }

                    if (instance.UserMustBeInTeam)
                    {
                        if (team == null || user.Team != team)
                        {
                            var builder = new DiscordInteractionResponseBuilder()
                                .WithContent($"You are required to be in the team '{info.Item2.Team.Name}' to interact with this.")
                                .AsEphemeral();
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
                            return;
                        }
                    }
                }

                Instances.Add(instance);
                instance.Client = discordClient;
                instance.CustomId = args.Interaction.Data.CustomId;
                instance.User = user;
                instance.Team = team;
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

        #region DEH subscription wrappers

        /// <summary>
        /// Subscribes the component to <see cref="ComponentInteractionDEH"/> for interaction callbacks and keeps
        /// track of which components have been subscribed so they can be unsubscribed when the interaction has concluded.
        /// </summary>
        /// <param name="constraints"><inheritdoc cref="ComponentInteractionDEH.Subscribe(ComponentInteractionDEH.ConstraintsBase, Func{DiscordClient, ComponentInteractionCreateEventArgs, Task})" path="/param[@name='constraints']"/></param>
        /// <param name="callback"><inheritdoc cref="ComponentInteractionDEH.Subscribe(ComponentInteractionDEH.ConstraintsBase, Func{DiscordClient, ComponentInteractionCreateEventArgs, Task})" path="/param[@name='callback']"/></param>
        protected void SubscribeComponent(ComponentInteractionDEH.Constraints constraints,
            Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> callback)
        {
            componentInteractionDEH.Subscribe(constraints, callback);
            subscribedComponentsInfo.Add((constraints, callback));
        }

        /// <summary>
        /// Subscribes the <paramref name="callback"/> to <see cref="MessageCreatedDEH"/> and keeps
        /// track of which messages have been subscribed so they can be unsubscribed when the interaction has concluded.
        /// </summary>
        /// <param name="constraints"><inheritdoc cref="MessageCreatedDEH.Subscribe(MessageCreatedDEH.ConstraintsBase, Func{DiscordClient, MessageCreateEventArgs, Task})" path="/param[@name='constraints']"/></param>
        /// <param name="callback"><inheritdoc cref="MessageCreatedDEH.Subscribe(MessageCreatedDEH.ConstraintsBase, Func{DiscordClient, MessageCreateEventArgs, Task})" path="/param[@name='callback']"/></param>
        protected void SubscribeMessage(MessageCreatedDEH.Constraints constraints,
            Func<DiscordClient, MessageCreateEventArgs, Task> callback)
        {
            messageCreatedDEH.Subscribe(constraints, callback);
            subscribedMessagesInfo.Add((constraints, callback));
        }

        /// <summary>
        /// Subscribes the <paramref name="callback"/> to <see cref="MessageCreatedDEH"/> and keeps
        /// track of which messages have been subscribed so they can be unsubscribed when the interaction has concluded.
        /// </summary>
        /// <param name="constraints"><inheritdoc cref="MessageCreatedDEH.Subscribe(MessageCreatedDEH.ConstraintsBase, Func{DiscordClient, MessageCreateEventArgs, Task})" path="/param[@name='constraints']"/></param>
        /// <param name="callback"><inheritdoc cref="MessageCreatedDEH.Subscribe(MessageCreatedDEH.ConstraintsBase, Func{DiscordClient, MessageCreateEventArgs, Task})" path="/param[@name='callback']"/></param>
        protected void SubscribeModal(ModalSubmittedDEH.Constraints constraints,
            Func<DiscordClient, ModalSubmitEventArgs, Task> callback)
        {
            modalSubmittedDEH.Subscribe(constraints, callback);
            subscribedModalInfo.Add((constraints, callback));
        }

        #endregion

        /// <summary>
        /// Checks if the a user with the given id is in the database. Then possibly post a response, notify an admin,
        /// and/or conclude the interaction.
        /// </summary>
        /// <param name="discordId">The id of the <see cref="DiscordUser"/> in question.</param>
        /// <param name="shouldBeInBD">Weather or not the user is supposed to be in the database.</param>
        /// <param name="args">The args for the interaction.
        /// If this is not null, a response will be posted telling the user they are (not) on a team,
        /// if they are (not) supposed to be.<br/>
        /// Being in the database means they are on a team (and vice versa).
        /// <param name="isAnError">If the user is in the database when they are not suppose to be (or vice versa),
        /// an admin will be notified of this error if this parameter is true. The user will also be notified that an admin
        /// has been notified if <paramref name="postStandardResponse"/> is true.</param>
        /// <param name="concludeInteraction">If the user is in the database when they are not suppose to be (or vice versa)
        /// and this parameter is true, the <see cref="InteractionConcluded"/> method will be called.</param>
        /// <returns>
        /// 0: if the user is in the database when they are suppose to be. <br/>
        /// 1: if the user is in the database when they are not suppose to be.
        /// </returns>
        protected async Task<int> UserInDBCheck(ulong discordId, bool shouldBeInBD,
            InteractionCreateEventArgs? args = null, bool isAnError = false,
            bool concludeInteraction = true)
        {
            bool interacctionAlreadyRespondedTo = true;

            if (args != null)
            {
                try
                {
                    //var builder = new DiscordInteractionResponseBuilder()
                    //    .WithContent("Thinking...")
                    //    .AsEphemeral();
                    //await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                }
                catch
                {
                    interacctionAlreadyRespondedTo = true;
                }
            }

            int returnValue = 0;
            User? user = DataWorker.Users.GetByDiscordId(discordId);
            string content = string.Empty;

            if (user == null && shouldBeInBD)
            {
                content = "You are not on a team.";
            }
            else if (user != null && !shouldBeInBD)
            {
                content = "You are already on a team. Contact an admin if you would like to be removed from it.";
            }

            if (content != string.Empty)
            {
                returnValue = -1;

                if (args != null)
                {
                    if (isAnError)
                    {
                        // TODO: notify admins of this
                        content += "\nThis appears to be an error so an admin has been notified.";
                    }

                    if (interacctionAlreadyRespondedTo)
                    {
                        var builder = new DiscordFollowupMessageBuilder()
                            .WithContent(content)
                            .AsEphemeral();
                        await args.Interaction.CreateFollowupMessageAsync(builder);
                    }
                    else
                    {
                        var builder = new DiscordWebhookBuilder()
                            .WithContent(content);
                        await args.Interaction.EditOriginalResponseAsync(builder);
                    }
                }
                else if (args != null)
                {
                    await args.Interaction.DeleteOriginalResponseAsync();
                }

                if (concludeInteraction) { await InteractionConcluded(); }
            }

            return returnValue;
        }

        protected async Task NotifyAdmins(string message)
        {
            // TODO: notify admins
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cleans up messages and unsubscribes all subscribed callbacks from
        /// <see cref="ComponentInteractionDEH"/> and <see cref="MessageCreatedDEH"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async virtual Task InteractionConcluded()
        {
            foreach (DiscordMessage? message in MessagesForCleanup)
            {
                if (message != null)
                {
                    // The message may have already been deleted, which will throw an error.
                    try
                    {
                        await message.DeleteAsync();
                    }
                    catch { }
                }
            }

            foreach (var componentSubscriptionInfo in subscribedComponentsInfo)
            {
                componentInteractionDEH.UnSubscribe(componentSubscriptionInfo.Item1, componentSubscriptionInfo.Item2);
            }

            foreach (var messageSubscriptionInfo in subscribedMessagesInfo)
            {
                messageCreatedDEH.UnSubscribe(messageSubscriptionInfo.Item1, messageSubscriptionInfo.Item2);
            }

            foreach (var modalSubscriptionInfo in subscribedModalInfo)
            {
                modalSubmittedDEH.UnSubscribe(modalSubscriptionInfo.Item1, modalSubscriptionInfo.Item2);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Information used to initialize a <see cref="ComponentInteractionHandler"/>.
        /// </summary>
        public struct InitialisationInfo
        {
            /// <summary>
            /// The team the handler represents.
            /// </summary>
            public InitialiseTeam Team;
        }
    }
}