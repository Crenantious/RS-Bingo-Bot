// <copyright file="SelectComponentBackButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

internal class SelectComponentBackButtonHandler : ButtonHandler<SelectComponentBackButtonRequest>
{
    protected override bool SendKeepAliveMessage => false;

    protected override async Task Process(SelectComponentBackButtonRequest request, CancellationToken cancellationToken)
    {
        await Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredMessageUpdate);

        var pages = request.SelectComponent.SelectedPages;

        if (pages.Count() == 1)
        {
            AddError(new SelectComponentBackButtonFirstItemError());
            return;
        }

        var messageServices = GetRequestService<IDiscordMessageServices>();

        var pageFrom = pages.ElementAt(^1);
        var pageTo = pages.ElementAt(^2);
        pages.RemoveRange(pages.Count() - 2, 2);

        SelectComponentUpdater.SetSelectedPage(request.SelectComponent, pageTo);
        await messageServices.Update(request.SelectComponent.Message!);

        AddSuccess(new SelectComponentBackButtonSuccess(pageFrom, pageTo));
    }
}