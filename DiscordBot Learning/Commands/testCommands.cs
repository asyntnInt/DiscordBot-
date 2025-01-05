using DiscordBot_Learning.Other;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Learning.Commands
{
    public class testCommands : BaseCommandModule
    {
        //Format for ALL commands
        [Command("ping")]
        public async Task TestCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"hello {ctx.User.Mention}");
        }

        /* [Command("add")]
        public async Task Add(CommandContext ctx, int number1, int number2)
        {
            int numSum = number1 + number2;
            await ctx.Channel.SendMessageAsync(Convert.ToString(numSum)); //Or use numSum.ToString
        } */

        [Command("ban")]
        [RequireBotPermissions(DSharpPlus.Permissions.BanMembers)]
        [RequireUserPermissions(DSharpPlus.Permissions.BanMembers)]
        public async Task Ban(CommandContext ctx,
            [Description("User banned")] DiscordMember member,
            [RemainingText, Description("Reason")] string reason)
        {
            await ctx.TriggerTypingAsync();
            DiscordGuild guild = member.Guild;

            try
            {
                await guild.BanMemberAsync(member, Convert.ToInt32(reason));
                await ctx.RespondAsync($"User {member.Mention}#{member.Discriminator} was banned");
            }
            catch (Exception)
            {
                await ctx.RespondAsync($"User {member.Username} cannot be banned");
            }
        }

        [Command("unban")]
        [RequireBotPermissions(DSharpPlus.Permissions.Administrator)]
        [RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task Unban(CommandContext ctx,
            [Description("User unbanned")] DiscordMember member,
            [RemainingText, Description("Reason")] string reason)
        {
            await ctx.TriggerTypingAsync();
            DiscordGuild guild = member.Guild;

            try
            {
                await guild.UnbanMemberAsync(member, reason);
                await ctx.RespondAsync($"User {member.Username}#{member.Discriminator} was unbanned");
            }
            catch (Exception)
            {
                await ctx.RespondAsync($"User {member.Username} cannot be unbanned");
            }
        }



        [Command("profile")]
        public async Task Profile(CommandContext ctx)
        {
            //One way of doing message embeds
            /* var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle($"{ctx.User.Username}'s profile picture")
                .WithImageUrl($"{ctx.User.AvatarUrl}")
                .WithColor(DiscordColor.Aquamarine)); 

            await ctx.Channel.SendMessageAsync(message); */

            //Another(and more preffered) way of making embeds
            var message = new DiscordEmbedBuilder
            {
                Title = $"{ctx.User.Username}'s profile picture",
                ImageUrl = $"{ctx.User.AvatarUrl}",
                Color = DiscordColor.Azure
            };

            await ctx.Channel.SendMessageAsync(embed: message);

        }

        [Command("thanosnap")]
        public async Task Interactivity(CommandContext ctx)
        {
            var interactivity = Program.Client.GetInteractivity();

            await ctx.Channel.SendMessageAsync("Are you sure you want to do this?");

            var messageToGet = await interactivity.WaitForMessageAsync(message => message.Content == "yes");

            //await ctx.Channel.SendMessageAsync(messageToGet.Result.Content);

            if (messageToGet.Result.Content == "yes")
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Mention} attempted to Thanos snap");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("procedure cancelled");
            }

        }

        //reaction of a specific emoji
        [Command("react")]
        public async Task ReactionCommand(CommandContext ctx) //DiscordMember member)
        {
            var emoji = DiscordEmoji.FromName(ctx.Client, ":heart:");
            var message = await ctx.RespondAsync($"{ctx.User.Mention}, react with {emoji}.");

            var result = await message.WaitForReactionAsync(ctx.User, emoji);

            if (!result.TimedOut) await ctx.RespondAsync(":heart:");
        }

        [Command("poll")]
        public async Task Poll(CommandContext ctx, string option1, string option2, string option3, string option4, [RemainingText] string pollTitle)
        {
            await ctx.Message.DeleteAsync(); //deletes the command message for a cleaner look
            var interactivity = Program.Client.GetInteractivity();
            var pollTime = TimeSpan.FromSeconds(20);

            DiscordEmoji[] emojis = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                      DiscordEmoji.FromName(Program.Client, ":two:"),
                                      DiscordEmoji.FromName(Program.Client, ":three:"),
                                      DiscordEmoji.FromName(Program.Client, ":four:") };

            string optionsDescription = $"{emojis[0]} | {option1} \n" +
                                         $"{emojis[1]} | {option2} \n" +
                                         $"{emojis[2]} | {option3} \n" +
                                         $"{emojis[3]} | {option4} \n";
            var pollEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Azure,
                Title = pollTitle,
                Description = optionsDescription
            };

            var embededPoll = await ctx.Channel.SendMessageAsync(embed: pollEmbed);
            //a lot easier than doing embededPoll.CreateReactionAsync(emojis[0]); four times!!!
            foreach (var emoji in emojis)
            {
                await embededPoll.CreateReactionAsync(emoji);
            }

            var totalReactions = await interactivity.CollectReactionsAsync(embededPoll, pollTime);
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            foreach (var emoji in totalReactions)
            {
                if (emoji.Emoji == emojis[0]) count1++;
                if (emoji.Emoji == emojis[1]) count2++;
                if (emoji.Emoji == emojis[2]) count3++;
                if (emoji.Emoji == emojis[3]) count4++;
            }

            int totalVotes = count1 + count2 + count3 + count4;
            string finalResult = $"Total votes: {totalVotes} \n" +
                                 $"{emojis[0]} : {count1} votes \n" +
                                 $"{emojis[1]} : {count2} votes \n" +
                                 $"{emojis[2]} : {count3} votes \n" +
                                 $"{emojis[3]} : {count4} votes \n\n" +
                                 $"Total Votes: {totalVotes}";
            var finalEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Chartreuse,
                Title = "The Final Results Are",
                Description = finalResult
            };

            await ctx.Channel.SendMessageAsync(embed: finalEmbed);
        }

        [Command("cooldown")]
        [Cooldown(5, 10, CooldownBucketType.User)] //10 seconds cooldown per user after 5 uses
        public async Task Cooldown(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Cooldown test");
        }
    }
}
