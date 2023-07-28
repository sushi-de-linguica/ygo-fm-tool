using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Domain.Enums
{
    public class CardEnums
    {
        public enum ECardGuardianStar
        {
            None = 0,
            Mars = 1,
            Jupiter = 2,
            Saturn = 3,
            Uranus = 4,
            Pluto = 5,
            Neptune = 6,
            Mercury = 7,
            Sun = 8,
            Moon = 9,
            Venus = 10
        }

        public enum ECardAttribute
        {
            Light = 0,
            Dark = 1,
            Earth = 2,
            Water = 3,
            Fire = 4,
            Wind = 5,
            Spell = 6,
            Trap = 7
        }

        // public enum ECardType
        // {
        //     Monster = 0,
        //     Equip = 1,
        //     Trap = 2,
        // }

        public enum EMonsterCardType
        {
            Dragon = 0,
            Spellcaster = 1,
            Zombie = 2,
            Warrior = 3,
            Beast_Warrior = 4,
            Beast = 5,
            Winged_Beast = 6,
            Fiend = 7,
            Fairy = 8,
            Insect = 9,
            Dinosaur = 10,
            Reptile = 11,
            Fish = 12,
            Sea_Serpent = 13,
            Machine = 14,
            Thunder = 15,
            Aqua = 16,
            Pyro = 17,
            Rock = 18,
            Plant = 19,
            Magic = 20,
            Trap = 21,
            Ritual = 22,
            Equip = 23
        }
    }
}
