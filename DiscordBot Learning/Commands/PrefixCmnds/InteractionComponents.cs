using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Learning.Commands.PrefixCmnds
{
    public class InteractionComponents : BaseCommandModule
    {
        [Command("buttons")]
        [DSharpPlus.CommandsNext.Attributes.Description("Sends a message with buttons")]
        public async Task Buttons(CommandContext ctx)
        {

            if (ctx == null) //makes sure theres a context and nothing missing
            {
                return;
            }

            var button = new DiscordButtonComponent(ButtonStyle.Primary, "button1", "Button 1");
            var button2 = new DiscordButtonComponent(ButtonStyle.Primary, "button2", "Button 2");


            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Buttons test")
                    .WithDescription("This is a message testing buttons")
                    .WithColor(DiscordColor.Gold))
                .AddComponents(button, button2);
            await ctx.Channel.SendMessageAsync(message);
            /*
            await ctx.Channel.SendMessageAsync(message, button: new DiscordComponent[]
            {
                new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "button1", "Primary Button"),
                new DiscordButtonComponent(DSharpPlus.ButtonStyle.Secondary, "button2", "Secondary Button"),
                new DiscordButtonComponent(DSharpPlus.ButtonStyle.Success, "button3", "Success Button"),
                new DiscordButtonComponent(DSharpPlus.ButtonStyle.Danger, "button4", "Danger Button"),
                //new DiscordButtonComponent(DSharpPlus.ButtonStyle.Link, "https://www.google.com", "Link Button")
            }); */
        }

    }
}
