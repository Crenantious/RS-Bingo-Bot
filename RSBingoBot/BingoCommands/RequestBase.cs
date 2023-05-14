// <copyright file="RequestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using System.Text;

/// <summary>
/// Base class for requests.
/// </summary>
// TODO: JR - since this can take a while, make sure each request posts progress messages.
public abstract class RequestBase
{
    /// <summary>
    /// Gets the permissions required to run the request.
    /// </summary>
    internal virtual List<Permissions> RequiredPermissions => new();

    private const string MissingPermissionsErrorMessage = "You require the following permissions to run this command:";
    private protected InteractionContext Ctx;
    private protected IDataWorker DataWorker;

    public IEnumerable<string> ResponseMessage { get; private set; } = Enumerable.Empty<string>();

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
        RSBingoBot.DiscordTeam.GetInstance(team).Role;

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

        SetResponseMessage(errorString.ToString());
        return false;
    }

    private protected bool ProcessFailure(IEnumerable<string> failureMessages)
    {
        SetResponseMessage(failureMessages);
        return false;
    }

    private protected bool ProcessSuccess(IEnumerable<string> successMessages)
    {
        SetResponseMessage(successMessages);
        return true;
    }

    private protected bool ProcessFailure(string failureMessage)
    {
        SetResponseMessage(failureMessage);
        return false;
    }

    private protected bool ProcessSuccess(string successMessage)
    {
        SetResponseMessage(successMessage);
        return true;
    }

    protected void SetResponseMessage(string responseMessage) =>
        ResponseMessage = new string[] { responseMessage };

    protected void SetResponseMessage(IEnumerable<string> responseMessage) =>
        ResponseMessage = responseMessage;
}