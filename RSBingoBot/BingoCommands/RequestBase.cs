// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RSBingo_Framework.Interfaces;

namespace RSBingoBot.BingoCommands;

/// <summary>
/// Base class for requests.
/// </summary>
public abstract class RequestBase
{
    /// <summary>
    /// Does this request require admin rights to run.
    /// </summary>
    internal virtual bool RequiresAdmin => false;


    private protected InteractionContext ctx;
    private protected IDataWorker DataWorker;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ctx">The <see cref="InteractionContext"/> for the request.</param>
    /// <param name="dataWorker">Reference to the dataWorker.</param>
    public RequestBase(InteractionContext ctx, IDataWorker dataWorker)
    {
        this.ctx = ctx;
        DataWorker = dataWorker;
    }

    public async Task<bool> ValidateRequest()
    {
        if (RequiresAdmin && !HasAdminPermission(ctx))
        {
            await InsufficientPermissionsResponse(ctx);
            return false;
        }

        return ValidateSpesificsRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
    public abstract Task<RequestResponse> ProcessRequest();

    private protected abstract bool ValidateSpesificsRequest();

    private protected static DiscordRole? GetTeamRole(InteractionContext ctx, string teamName)
    {
        KeyValuePair<ulong, DiscordRole> pair = ctx.Interaction.Guild.Roles.FirstOrDefault(r => r.Value.Name == teamName);
        if (pair.Equals(default)) { return null; }
        return pair.Value;
    }

    private bool HasAdminPermission(InteractionContext ctx) =>
        ctx.Guild.GetMemberAsync(ctx.User.Id).Result.Permissions.HasPermission(Permissions.Administrator);

    private async Task InsufficientPermissionsResponse(InteractionContext ctx)
    {
        // TODO: JR - Change this to a pre-execution check
        var builder = new DiscordInteractionResponseBuilder()
            .WithContent("You do not have the required permissions to run this command.")
            .AsEphemeral();
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
    }

    private protected RequestResponse RequestFailed(string errorMessage = null) => new RequestResponse(false, errorMessage);
    private protected RequestResponse RequestSuccess(object response = null) => new RequestResponse(true, response);
}
