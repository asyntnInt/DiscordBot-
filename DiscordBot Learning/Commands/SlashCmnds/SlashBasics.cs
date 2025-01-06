using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Learning.Commands.SlashCmnds
{
    public class SlashBasics : ApplicationCommandModule
    {
        [SlashCommand("test", "this is a test")]
        public async Task SlashTestCommand(InteractionContext ctx)
        {
            await ctx.DeferAsync(); // This is required to send a response later, do NOT use await ctx.CreateResponseAsync

            var slashEmbed = new DiscordEmbedBuilder()
            {
                Title = "Hello World!",
                Description = "This is a test command",
                Color = new DiscordColor(0xFF00FF)
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(slashEmbed));
        }

        [SlashCommand("parameters", "This slash command allows parameters")]
        public async Task SlashParametersCommand(InteractionContext ctx,
            [Option("param1", "This is the first parameter")] string param1,
            [Option("param2", "This is the second parameter")] string param2)
        {
            await ctx.DeferAsync();
            var slashEmbed = new DiscordEmbedBuilder()
            {
                Title = "Parameters",
                Description = $"Parameter 1: {param1}  Parameter 2: {param2}",
                Color = new DiscordColor(0x00FF00)
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(slashEmbed));
        }
    }
}
