// <copyright file="SelectComponentBackButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

public abstract class SelectComponentBackButtonHandler<TRequest> : ButtonHandler<TRequest>
    where TRequest : SelectComponentBackButtonRequest
{
    protected override bool SendKeepAliveMessage => false;

    /// <summary>
    /// Likely use <see cref="Process(TRequest, SelectComponentPage, SelectComponentPage)"/> instead.<br/>
    /// If this need to be overridden, ensure this is ran on the base class. 
    /// </summary>
    protected override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        await Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredMessageUpdate);

        var pages = request.SelectComponent.SelectedPages;

        if (pages.Count() == 1)
        {
            AddError(new SelectComponentBackButtonFirstItemError());
            return;
        }

        var messageServices = GetRequestService<IDiscordMessageServices>();

        var previousPage = pages.ElementAt(^1);
        var newPage = pages.ElementAt(^2);
        pages.RemoveRange(pages.Count() - 2, 2);

        SelectComponentUpdater.SetSelectedPage(request.SelectComponent, newPage);
        await messageServices.Update(request.SelectComponent.Message!);

        AddSuccess(new SelectComponentBackButtonSuccess(previousPage, newPage));

        Process(request, previousPage, newPage);
    }

    /// <param name="previousPage">The page that was selected before the back button was pressed.</param>
    /// <param name="currentPage">The page that is now selected after the back button has been pressed.</param>
    protected abstract void Process(TRequest request, SelectComponentPage previousPage, SelectComponentPage currentPage);
}

public class SelectComponentBackButtonHandler : SelectComponentBackButtonHandler<SelectComponentBackButtonRequest>
{
    protected override void Process(SelectComponentBackButtonRequest request, SelectComponentPage previousPage, SelectComponentPage currentPage)
    {

    }
}