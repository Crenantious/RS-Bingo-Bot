// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
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
    internal virtual List<Permissions> RequiredPermissions => new();

    private const string MissingPermissionsErrorMessage = "You require the following permissions to run this command:";
    private protected InteractionContext Ctx;
    private protected IDataWorker DataWorker;

    public string ResponseMessage { get; private protected set; }

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
            return InsufficientPermissionsResponse(missingPermissions);
        }

        return ValidateSpecificRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
    public abstract Task<bool> ProcessRequest();

    private protected abstract bool ValidateSpecificRequest();

    private protected DiscordRole? GetTeamRole(Team team) =>
        DiscordTeam.GetInstance(team).Role;

    private async Task<IEnumerable<Permissions>> GetMissingPermissions()
    {
        DiscordMember member = await Ctx.Guild.GetMemberAsync(Ctx.User.Id);
        IEnumerable<Permissions> missingPermissions = RequiredPermissions.Where(p => !member.Permissions.HasPermission(p));
        return missingPermissions;
    }

    private bool InsufficientPermissionsResponse(IEnumerable<Permissions> missingPermissions)
    {
        // TODO: JR - Change this to a pre-execution check

        StringBuilder errorString = new(MissingPermissionsErrorMessage);

        foreach (Permissions permission in missingPermissions)
        {
            errorString.AppendLine($"{permission}");
        }

        ResponseMessage = errorString.ToString();
        return false;
    }

    private protected bool ProcessFailure(string failureMessage)
    {
        ResponseMessage = failureMessage;
        return false;
    }

    private protected bool ProcessSuccess(string successMessage)
    {
        ResponseMessage = successMessage;
        return true;
    }
}