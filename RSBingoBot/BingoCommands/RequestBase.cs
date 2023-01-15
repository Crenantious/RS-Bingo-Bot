// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RSBingo_Framework.Interfaces;
using System.Text;

namespace RSBingoBot.BingoCommands;

/// <summary>
/// Base class for requests.
/// </summary>
public abstract class RequestBase
{
    /// <summary>
    /// Gets the permissions required to run the request.
    /// </summary>
    internal virtual List<Permissions> RequiredPermissions => new ();

    private const string MissingPermissionsErrorMessage = "You require the following permissions to run this command:";
    private protected InteractionContext Ctx;
    private protected IDataWorker DataWorker;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ctx">The <see cref="InteractionContext"/> for the request.</param>
    /// <param name="dataWorker">Reference to the dataWorker.</param>
    public RequestBase(InteractionContext ctx, IDataWorker dataWorker)
    {
        Ctx = ctx;
        DataWorker = dataWorker;
    }

    public async Task<bool> ValidateRequest()
    {
        IEnumerable<Permissions> missingPermissions = await GetMissingPermissions();
        if (missingPermissions.Any())
        {
            await InsufficientPermissionsResponse(missingPermissions);
            return false;
        }

        return ValidateSpecificRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
    public abstract Task<RequestResponse> ProcessRequest();

    private protected abstract bool ValidateSpecificRequest();

    private protected DiscordRole? GetTeamRole(string teamName)
    {
        KeyValuePair<ulong, DiscordRole> pair = Ctx.Interaction.Guild.Roles.FirstOrDefault(r => r.Value.Name == teamName);
        if (pair.Equals(default)) { return null; }
        return pair.Value;
    }

    private async Task<IEnumerable<Permissions>> GetMissingPermissions()
    {
        DiscordMember member = await Ctx.Guild.GetMemberAsync(Ctx.User.Id);
        IEnumerable<Permissions> missingPermissions = RequiredPermissions.Where(p => !member.Permissions.HasPermission(p));
        return missingPermissions;
    }

    private async Task InsufficientPermissionsResponse(IEnumerable<Permissions> missingPermissions)
    {
        // TODO: JR - Change this to a pre-execution check

        StringBuilder errorString = new(MissingPermissionsErrorMessage);

        foreach (Permissions permission in missingPermissions)
        {
            errorString.AppendLine($"{permission}");
        }

        var builder = new DiscordInteractionResponseBuilder()
            .WithContent(errorString.ToString())
            .AsEphemeral();
        await Ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
    }

    private protected RequestResponse RequestFailed(string? errorMessage = null) => new RequestResponse(false, errorMessage);

    private protected RequestResponse RequestSuccess(object? response = null) => new RequestResponse(true, response);
}