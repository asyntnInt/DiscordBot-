using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Learning.Commands.SlashCmnds
{
    public class SlashBasics : ApplicationCommandModule
    {
        [SlashCommandGroup("utility", "Usefull Commands")]
        public class GroupContainer : ApplicationCommandModule
        {
            
            [SlashCommand("userinfo", "Get information about a user")]
            public async Task UserInfo(InteractionContext ctx, [Option("user", "The user to get information about")] DiscordUser user = null)
            {
                await ctx.DeferAsync();
                if (user == null)
                {
                    user = ctx.User;
                }
                var embed = new DiscordEmbedBuilder()
                    .WithTitle("User Information")
                    .WithThumbnail(user.AvatarUrl)
                    .AddField("Username", user.Username + "#" + user.Discriminator)
                    .AddField("User ID", user.Id.ToString())
                    .AddField("Is Bot", user.IsBot.ToString())
                    .WithColor(DiscordColor.Gold);

                //await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));

            }

            /*
            [SlashCommand("infouser", "This slash command gets user info")]
            public async Task UserInfo2(InteractionContext ctx,
            [Option("user", "The user you want to get info on")] DiscordUser user)
            {
                await ctx.DeferAsync();

                var member = (DiscordMember)user; //gets the in server info of the user

                //var x = await ctx.Guild.GetMemberAsync(user.Id); find out what this does later

                var slashEmbed = new DiscordEmbedBuilder()
                {
                    Title = "User Info",
                    Description = $"Username: {user.Username}#{user.Discriminator}, " +
                    $"Name: {member.Nickname}",
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = user.AvatarUrl },
                    Color = new DiscordColor(0x0000FF)
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(slashEmbed));
            }*/
            

            [SlashCommand("help", "List all available slash commands")]
            public async Task Help(InteractionContext ctx)
            {
                await ctx.DeferAsync();

                var commands = ctx.Client.GetSlashCommands().RegisteredCommands;
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Available Slash Commands",
                    Color = DiscordColor.Azure
                };

                foreach (var commandGroup in commands)
                {
                    foreach (var command in commandGroup.Value)
                    {
                        embed.AddField(command.Name, command.Description ?? "No description available");
                    }
                }

                //await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));

            }



        }

        [SlashCommand("parameters", "This slash command allows parameters")]
        public async Task SlashParametersCommand(InteractionContext ctx,
        [Option("param1", "This is the first parameter")] string param1,
        [Option("param2", "This is the second parameter")] string param2,
        [Option("numParam", "This parameter is a number")] long number)
        {
            await ctx.DeferAsync();
            var slashEmbed = new DiscordEmbedBuilder()
            {
                Title = "Parameters",
                Description = $"Parameter 1: {param1}  Parameter 2: {param2}  Number: {number}",
                Color = new DiscordColor(0x00FF00)
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(slashEmbed));
        }

    }
}
