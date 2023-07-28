using FM.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Application
{
    public class Configurations
    {
        public static string GameBinPath = string.Empty;
        public static string SlusPath;
        public static string WaPath;
        public static string IsoPath;

        public static StarterDeck[] StarterDecks = new StarterDeck[Static.MAX_STARTER_DECKS];
        public static StarterDeck[] VanillaStarterDeck = new StarterDeck[Static.MAX_STARTER_DECKS_VANILLA];
        public static Card[] Cards = new Card[Static.MAX_CARDS];

        public static bool LoadedIso = false;
        public static bool UsedIso = false;
        public static string patheticFileName = "fm";

        public static int[] DropCardsVanillaStarterDeck = new int[Static.MAX_STARTER_DECKS_VANILLA];

        public static Dictionary<byte, char> CharacterTableDict = new Dictionary<byte, char>();
        public static Dictionary<char, byte> CharacterTableRDict = new Dictionary<char, byte>();

        public static List<RuleItem> Rules = new List<RuleItem>();
    }
}
