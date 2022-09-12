using DSharpPlus;
using DSharpPlus.EventArgs;

namespace RSBingoBot.Component_interaction_handlers;

// Listen for MessageDeleted event a delete the instance if nessassary
internal class ComponentInteractionHandler
{
    readonly static Dictionary<string, (Type, Team)> registeredIds = new();
    readonly static List<ComponentInteractionHandler> instances = new();

    internal ComponentInteractionCreateEventArgs interactionArgs = null!;
    internal Team team = null!;


    static ComponentInteractionHandler()
    {
        Bot.Client.ComponentInteractionCreated += Callback;
    }

    public static void Register<T>(string customId, Team team) where T : ComponentInteractionHandler
    {
        registeredIds.Add(customId, (typeof(T), team));
    }

    static async Task Callback(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        if (registeredIds.ContainsKey(args.Interaction.Data.CustomId))
        {
            (Type, Team) info = registeredIds[args.Interaction.Data.CustomId];
            ComponentInteractionHandler? instance = (ComponentInteractionHandler?)Activator.CreateInstance(info.Item1);

            if (instance != null)
            {
                instances.Add(instance);
                await instance.Initialise(args, info.Item2);
            }
            else
            {
                // Log error
            }
        }
    }

    public async virtual Task Initialise(ComponentInteractionCreateEventArgs args, Team team) 
    {
        interactionArgs = args;
        this.team = team;
    }
}