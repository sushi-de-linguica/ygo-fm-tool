using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FM.Core.Domain.Enums.CardEnums;

namespace FM.Core.Domain.Models
{
    public class RuleItem
    {
        public int MinQuantity { get; set; } = 0;
        public int MaxQuantity { get; set; } = 0;
        public string CardType { get; set; } = null;
        public EMonsterCardType? MonsterType { get; set; }
        public int MinAttack { get; set; } = 0;
        public int MaxAttack { get; set; } = 0;
        public int MinDefense { get; set; } = 0;
        public int MaxDefense { get; set; } = 0;
        public int[] CardsFrom { get; set; }

        /// <summary>
        /// frontend fields
        /// </summary>
        public int DropsCardsFromIndex { get; set; } = 0;
        public int CardTypeIndex { get; set; } = 0;
        public int MonsterTypeIndex { get; set; } = 0;
        public string Custom { get; set; } = "";
    }
}
