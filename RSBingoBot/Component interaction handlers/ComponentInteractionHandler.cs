// <copyright file="ComponentInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Common;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot;
using RSBingoBot.Discord_event_handlers;
using static RSBingoBot.MessageUtilities;
using static RSBingo_Framework.DAL.DataFactory;

/// <summary>
/// Handles the callback when a component is interacted with.
/// </summary>
//TODO: refactor. This is horrible.
public abstract class ComponentInteractionHandler : IDisposable
{
    protected const string AlreadyOnATeamMessage = "You are already on a team. Contact an admin if you would like to be removed from it.";
    protected const string NotOnATeamMessage = "You are not on a team.";
    protected const string AlreadyInteractingMessage = "You are already interacting with another component.";

    private static readonly Dictionary<string, (Type, InitialisationInfo)> RegisteredComponentIds = new();
    private static readonly Dictionary<DiscordUser, HashSet<Type>> UserActiveInstances = new();
    private static readonly List<ComponentInteractionHandler> Instances = new();
    private static readonly ComponentInteractionDEH componentInteractionDEH = null!;
    private static readonly MessageCreatedDEH messageCreatedDEH = null!;
    private static readonly ModalSubmittedDEH modalSubmittedDEH = null!;

    // TODO: JR - re-factor to not have to wrap every DEH subscription.
    private readonly List<(ComponentInteractionDEH.Constraints,
                           Func<DiscordClient,
                           ComponentInteractionCreateEventArgs, Task>)> subscribedComponentsInfo = new();

    private readonly List<(MessageCreatedDEH.Constraints,
                           Func<DiscordClient,
                           MessageCreateEventArgs, Task>)> subscribedMessagesInfo = new();

    private readonly List<(ModalSubmittedDEH.Constraints,
                   Func<DiscordClient,
                   ModalSubmitEventArgs, Task>)> subscribedModalInfo = new();

    static ComponentInteractionHandler()
    {
        componentInteractionDEH = (ComponentInteractionDEH)General.DI.GetService(typeof(ComponentInteractionDEH))!;
        messageCreatedDEH = (MessageCreatedDEH)General.DI.GetService(typeof(MessageCreatedDEH))!;
        modalSubmittedDEH = (ModalSubmittedDEH)General.DI.GetService(typeof(ModalSubmittedDEH))!;
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
    /// Automatically creates a response to the interaction so that it does not time out.
    /// </summary>
    protected abstract bool CreateAutoResponse { get; }

    /// <summary>
    /// If false and the user has a active instance with another component,
    /// a response will be generated notifying them and this interaction will be concluded.
    /// </summary>
    protected virtual bool AllowInteractionWithAnotherComponent { get; } = false;

    /// <summary>
    /// Automatically registers the interaction for use with <see cref="AllowInteractionWithAnotherComponent"/>.
    /// </summary>
    protected virtual bool AutoRegisterInteraction { get; } = true;

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
    protected List<DiscordMessage> MessagesForCleanup { get; private set; } = new();

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
    /// Gets the event args for the most recent interaction with components registered by this handler.
    /// </summary>
    protected ComponentInteractionCreateEventArgs CurrentInteractionArgs { get; private set; } = null!;

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
    public static void Register<T>(string customId, InitialisationInfo? info = null) where T : ComponentInteractionHandler
    {
        RegisteredComponentIds[customId] = (typeof(T), info ?? new());
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

        if (instance == null)
        {
            // Log error
            return;
        }

        if (instance.AllowInteractionWithAnotherComponent is false &&
            UserActiveInstances.ContainsKey(args.User) &&
            UserActiveInstances[args.User].Any())
        {
            await Respond(args, AlreadyInteractingMessage, true);
            return;
        }

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
        instance.OriginalInteractionArgs = args;
        instance.Info = info.Item2;
        if (instance.AutoRegisterInteraction) { instance.RegisterUserInstance(); }

        await ComponentInteracted(instance, discordClient, args,
            (client, args) => instance.InitialiseAsync(args, info.Item2), false, "Loading...", instance.CreateAutoResponse);
    }

    /// <summary>
    /// Initializes the <see cref="ComponentInteractionHandler"/>.
    /// </summary>
    /// <param name="args">Event args of the component that was interacted with.</param>
    /// <param name="info">Info relating to the handler and the component it is registered to.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public async virtual Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info) { }

    #region DEH subscription wrappers

    /// <summary>
    /// Subscribes the component to <see cref="ComponentInteractionDEH"/> for interaction callbacks and keeps
    /// track of which components have been subscribed so they can be unsubscribed when the interaction has concluded.<br/>
    /// Creates an automatic response to the component to ensure it does not timeout.
    /// </summary>
    /// <param name="constraints"><inheritdoc cref="ComponentInteractionDEH.Subscribe(ComponentInteractionDEH.ConstraintsBase, Func{DiscordClient, ComponentInteractionCreateEventArgs, Task})" path="/param[@name='constraints']"/></param>
    /// <param name="callback"><inheritdoc cref="ComponentInteractionDEH.Subscribe(ComponentInteractionDEH.ConstraintsBase, Func{DiscordClient, ComponentInteractionCreateEventArgs, Task})" path="/param[@name='callback']"/></param>
    protected void SubscribeComponent(ComponentInteractionDEH.Constraints constraints,
        Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> callback, bool ephemeralResponse, string responseContent = "")
    {
        componentInteractionDEH.Subscribe(constraints,
            (client, args) => ComponentInteracted(args, callback, ephemeralResponse, responseContent));
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
    /// Called when a component is subscribed via <see cref="SubscribeComponent(ComponentInteractionDEH.Constraints, Func{DiscordClient, ComponentInteractionCreateEventArgs, Task}, bool, string)"/>.
    /// Creates an automatic response to ensure the interaction does not timeout, then calls <paramref name="callback"/> which was passed when subscribed.
    /// </summary>
    /// <param name="args"></param>
    /// <param name="callback"></param>
    /// <param name="ephemeralResponse"></param>
    /// <param name="responseContent"></param>
    /// <returns></returns>
    protected static async Task ComponentInteracted(ComponentInteractionHandler instance, DiscordClient client,
        ComponentInteractionCreateEventArgs args, Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> callback,
        bool ephemeralResponse, string responseContent = "", bool createResponse = true)
    {
        instance.CurrentInteractionArgs = args;

        if (createResponse)
        {
            // TODO: JR - create a timer to update the message every 0.5s or so to animate the ellipsis to show that
            // the bot is loading and hasn't crashed if it's taking a long time.
            // E.g. message 1: "Loading.", message 2: "Loading..", message 3: "Loading..." etc.

            if (responseContent == "")
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            }
            else
            {
                var builder = new DiscordInteractionResponseBuilder()
                {
                    IsEphemeral = ephemeralResponse,
                    Content = responseContent
                };
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
            }
        }

        try
        {
            await callback(client, args);
        }
        catch (ComponentInteractionHandlerException e)
        {
            if (e.ResponseType == ComponentInteractionHandlerException.ErrorResponseType.EditOriginalResponse)
            {
                var errorBuilder = new DiscordWebhookBuilder()
                    .WithContent(e.Message);
                await args.Interaction.EditOriginalResponseAsync(errorBuilder);
            }

            if (e.ResponseType == ComponentInteractionHandlerException.ErrorResponseType.CreateFollowUpResponse)
            {
                var exceptionBuilder = new DiscordFollowupMessageBuilder()
                {
                    Content = e.Message,
                    IsEphemeral = e.IsEphemeral
                };
                await args.Interaction.CreateFollowupMessageAsync(exceptionBuilder);
            }

            if (e.ConcludeInteraction)
            {
                await instance.ConcludeInteraction();
            }
        }
        catch (Exception e)
        {
            var exceptionBuilder = new DiscordFollowupMessageBuilder()
                .WithContent("And internal error has occurred.")
                .AsEphemeral(true);
            await args.Interaction.CreateFollowupMessageAsync(exceptionBuilder);

            throw e;
        }
    }

    /// <summary>
    /// Called when a component is subscribed via <see cref="SubscribeComponent(ComponentInteractionDEH.Constraints, Func{DiscordClient, ComponentInteractionCreateEventArgs, Task}, bool, string)"/>.
    /// Creates an automatic response to ensure the interaction does not timeout, then calls <paramref name="callback"/> which was passed when subscribed.
    /// </summary>
    /// <param name="args"></param>
    /// <param name="callback"></param>
    /// <param name="ephemeralResponse"></param>
    /// <param name="responseContent"></param>
    /// <returns></returns>
    protected async Task ComponentInteracted(ComponentInteractionCreateEventArgs args,
        Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> callback,
        bool ephemeralResponse, string responseContent = "")
    {
        await ComponentInteracted(this, Client, args, callback, ephemeralResponse, responseContent);
    }

    protected void RegisterUserInstance()
    {
        DiscordUser user = OriginalInteractionArgs.User;
        if (UserActiveInstances.ContainsKey(user) is false)
        {
            UserActiveInstances[user] = new();
        }
        UserActiveInstances[user].Add(GetType());
    }

    /// <summary>
    /// Checks if a user is on a team in the database. Then possibly post a response should that
    /// fail to meet the <paramref name="shouldBeOnATeam"/> requirement.
    /// </summary>
    /// <param name="discordId">The id of the <see cref="DiscordUser"/> in question.</param>
    /// <param name="shouldBeOnATeam">Whether or not the user is supposed to be on a team.</param>
    /// <param name="args">The args for the interaction.
    /// If this is not <see langword="null"/> and the user incorrectly (not) on a team, a response will be posted
    /// notifying the user.
    /// <returns>
    /// <see langword="true"/> if an error message was sent.<br/>
    /// <see langword="false"/ otherwise>.
    /// </returns>
    protected async Task<bool> TrySendUserTeamStatusErrorMessage(ulong discordId, bool shouldBeOnATeam,
        InteractionCreateEventArgs? args = null)
    {
        User? user = DataWorker.Users.GetByDiscordId(discordId);

        string content = (user, shouldBeOnATeam) switch
        {
            (null, true) => NotOnATeamMessage,
            (not null, false) => AlreadyOnATeamMessage,
            _ => string.Empty,
        };

        if (string.IsNullOrEmpty(content)) { return false; }

        if (args != null)
        {
            var builder = new DiscordFollowupMessageBuilder()
                .WithContent(content)
                .AsEphemeral();
            await args.Interaction.CreateFollowupMessageAsync(builder);
        }

        return true;
    }


    /// <summary>
    /// Cleans up messages and unsubscribes all subscribed callbacks from
    /// <see cref="ComponentInteractionDEH"/> and <see cref="MessageCreatedDEH"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected async virtual Task ConcludeInteraction()
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

        if (UserActiveInstances.ContainsKey(OriginalInteractionArgs.User))
        {
            UserActiveInstances[OriginalInteractionArgs.User].Remove(GetType());
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Information used to initialize a <see cref="ComponentInteractionHandler"/>.
    /// </summary>
    public class InitialisationInfo
    {
        /// <summary>
        /// The team the handler represents.
        /// </summary>
        public RSBingoBot.DiscordTeam Team;
    }
}