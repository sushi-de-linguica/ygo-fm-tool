using FM.Core.Application;
using FM.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Usecases
{
    public class DropCardsFromDeckUseCase
    {
        public static StarterDeck[] ExecuteToDeckList(StarterDeck[] decks)
        {
            List<StarterDeck> List = new List<StarterDeck>();

            foreach(StarterDeck Deck in decks)
            {
                List.Add(Execute(Deck, Deck.Dropped));
            }

            return List.ToArray();
        }

        public static StarterDeck Execute(StarterDeck originalDeck, int numCardsToDrop)
        {
            // Clone the original deck to avoid modifying it directly
            StarterDeck newDeck = new StarterDeck
            {
                Dropped = 0,
                Cards = (int[])originalDeck.Cards.Clone()
            };

            // Calculate the total drop rate as the sum of all card values
            int totalDropRate = 0;
            foreach (int dropRate in newDeck.Cards)
            {
                totalDropRate += dropRate;
            }

            // Normalize the drop rates to represent probabilities
            double[] probabilities = new double[Static.MAX_CARDS];
            for (int i = 0; i < Static.MAX_CARDS; i++)
            {
                probabilities[i] = (double)newDeck.Cards[i] / totalDropRate;
            }

            // Create a list of card indices based on their probabilities
            List<int> cardIndices = new List<int>();
            for (int i = 0; i < Static.MAX_CARDS; i++)
            {
                int numEntries = (int)(probabilities[i] * 100000); // Multiply by a large number for precision
                for (int j = 0; j < numEntries; j++)
                {
                    cardIndices.Add(i);
                }
            }

            // Select the specified number of cards with weighted probability
            Random random = new Random();
            for (int i = 0; i < numCardsToDrop; i++)
            {
                int randomIndex = random.Next(cardIndices.Count);
                int cardIndex = cardIndices[randomIndex];
                newDeck.Cards[cardIndex] = 1;
                newDeck.Dropped++;
                cardIndices.RemoveAt(randomIndex);
            }

            // Set the remaining cards to 0 (not chosen)
            for (var i=0; i < newDeck.Cards.Length; i++)
            {
                var RemoveOldDropCardRate = newDeck.Cards[i] > 1;
                if (RemoveOldDropCardRate)
                {
                    newDeck.Cards[i] = 0;
                }
            }

            return newDeck;
        }
    }
}
