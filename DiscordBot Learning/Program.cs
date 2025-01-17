﻿using DiscordBot_Learning.Commands;
using DiscordBot_Learning.Commands.SlashCmnds;
using DiscordBot_Learning.Config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Learning
{
    internal class Program
    {
        public static DiscordClient Client { get; private set; }
        private static CommandsNextExtension Commands { get; set; }
        static async Task Main(string[] args)
        {
            var jsonreader = new JSONreader();
            await jsonreader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonreader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(discordConfig);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Client.Ready += Client_Ready;
            //Client.MessageCreated += MessageCreatedHandler;
            Client.VoiceStateUpdated += VoiceChannelHandler;


            var commandsconfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonreader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsconfig);
            var slashCommandConfig = Client.UseSlashCommands();

            Commands.CommandErrored += CommandEventHandler;

            // Registering commands
            Commands.RegisterCommands<testCommands>();
            slashCommandConfig.RegisterCommands<SlashBasics>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task CommandEventHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if (e.Exception is ChecksFailedException exception)
            {
                string timeLeft = string.Empty;
                foreach (var check in exception.FailedChecks)
                {
                    var coolDown = (CooldownAttribute)check;
                    timeLeft = coolDown.GetRemainingCooldown(e.Context).ToString(@"hh\:mm\:ss");
                }

                var coolDownMessage = new DiscordEmbedBuilder
                {
                    Title = "Cooldown until command is available",
                    Description = $"You are on cooldown for this command. Please wait {timeLeft} before using this command again",
                    Color = DiscordColor.Red
                };

                //e.Context.RespondAsync(embed: coolDownMessage); //This will not work
                await e.Context.Channel.SendMessageAsync(embed: coolDownMessage); //This will work

            }
        }

        private static async Task VoiceChannelHandler(DiscordClient sender, DSharpPlus.EventArgs.VoiceStateUpdateEventArgs e)
        {
            if (e.Before == null && e.Channel.Name == "Create")
            {
                await e.Channel.SendMessageAsync($"{e.User.Mention} has joined the Voice chat");
            }
        }

        /*private static async Task MessageCreatedHandler(DiscordClient sender, DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            await e.Channel.SendMessageAsync("Event handler was triggered");
        }*/

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
