using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot_Learning.Other
{
    public class CardSystem
    {
        private int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        private string[] cardSuits = { "Hearts", "Diamonds", "Spades", "Clubs" };

        public int selectedNum { get; set; }
        public string selectedCard { get; set; }

        public CardSystem()
        {
            var random = new Random();
            int numberIndex = random.Next(0, cardNumbers.Length - 1);
            int suitIndex = random.Next(0, cardSuits.Length - 1);

            this.selectedNum = cardNumbers[numberIndex];
            this.selectedCard = $"{cardNumbers[numberIndex]} of {cardSuits[suitIndex]}";


        }
    }
}

//copy paste in testCommands.cs to use the card system
/*         [Command("cards")]
        public async Task Cards(CommandContext ctx)
        {
            var userCard = new CardSystem();

            var userCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"Your card is {userCard.selectedCard}",
                Color = DiscordColor.Violet
            };

            await ctx.Channel.SendMessageAsync(embed: userCardEmbed);


            var botCard = new CardSystem();

            var botCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"The Bot drew a {botCard.selectedCard}",
                Color = DiscordColor.CornflowerBlue
            };

            await ctx.Channel.SendMessageAsync(embed: botCardEmbed);

            if (userCard.selectedNum > botCard.selectedNum)
            {
                //user wins
                var winMessage = new DiscordEmbedBuilder
                {
                    Title = "Congratulations you win",
                    Color = DiscordColor.Gold
                };

                await ctx.Channel.SendMessageAsync(embed: winMessage);
            }
            else
            {
                //bot wins
                var loseMessage = new DiscordEmbedBuilder
                {
                    Title = "You lose, better luck next time",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed: loseMessage);
            }
        } */