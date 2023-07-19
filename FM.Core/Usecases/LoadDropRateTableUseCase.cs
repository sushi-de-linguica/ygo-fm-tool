using FM.Core.Application;
using FM.Core.Domain.Models;
using FM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CardData
{
    public int CardId { get; set; }
    public int DropRate { get; set; }
    public int Set { get; set; }
}

namespace FM.Core.Usecases
{
    public class LoadDropRateTableUseCase
    {
        private StarterDeck[] starterDecks;
        private int[] DroppedByDeck = new int[]
        {
            16,
            16,
            4,
            1,
            1,
            1,
            1
        };
        public CardData[] CardSet { get; set; } = new CardData[]
        {
            new CardData { CardId = 9, DropRate = 48, Set = 0 },
            new CardData { CardId = 24, DropRate = 48, Set = 0 },
            new CardData { CardId = 58, DropRate = 48, Set = 0 },
            new CardData { CardId = 105, DropRate = 48, Set = 0 },
            new CardData { CardId = 123, DropRate = 48, Set = 0 },
            new CardData { CardId = 130, DropRate = 48, Set = 0 },
            new CardData { CardId = 137, DropRate = 48, Set = 0 },
            new CardData { CardId = 167, DropRate = 48, Set = 0 },
            new CardData { CardId = 192, DropRate = 48, Set = 0 },
            new CardData { CardId = 197, DropRate = 48, Set = 0 },
            new CardData { CardId = 202, DropRate = 48, Set = 0 },
            new CardData { CardId = 237, DropRate = 48, Set = 0 },
            new CardData { CardId = 289, DropRate = 48, Set = 0 },
            new CardData { CardId = 387, DropRate = 48, Set = 0 },
            new CardData { CardId = 393, DropRate = 48, Set = 0 },
            new CardData { CardId = 394, DropRate = 48, Set = 0 },
            new CardData { CardId = 395, DropRate = 48, Set = 0 },
            new CardData { CardId = 397, DropRate = 48, Set = 0 },
            new CardData { CardId = 398, DropRate = 48, Set = 0 },
            new CardData { CardId = 399, DropRate = 48, Set = 0 },
            new CardData { CardId = 402, DropRate = 48, Set = 0 },
            new CardData { CardId = 410, DropRate = 48, Set = 0 },
            new CardData { CardId = 411, DropRate = 48, Set = 0 },
            new CardData { CardId = 422, DropRate = 48, Set = 0 },
            new CardData { CardId = 436, DropRate = 48, Set = 0 },
            new CardData { CardId = 444, DropRate = 48, Set = 0 },
            new CardData { CardId = 469, DropRate = 48, Set = 0 },
            new CardData { CardId = 475, DropRate = 48, Set = 0 },
            new CardData { CardId = 484, DropRate = 48, Set = 0 },
            new CardData { CardId = 485, DropRate = 48, Set = 0 },
            new CardData { CardId = 488, DropRate = 48, Set = 0 },
            new CardData { CardId = 501, DropRate = 48, Set = 0 },
            new CardData { CardId = 504, DropRate = 48, Set = 0 },
            new CardData { CardId = 516, DropRate = 48, Set = 0 },
            new CardData { CardId = 524, DropRate = 48, Set = 0 },
            new CardData { CardId = 527, DropRate = 48, Set = 0 },
            new CardData { CardId = 547, DropRate = 48, Set = 0 },
            new CardData { CardId = 548, DropRate = 48, Set = 0 },
            new CardData { CardId = 558, DropRate = 56, Set = 0 },
            new CardData { CardId = 563, DropRate = 56, Set = 0 },
            new CardData { CardId = 635, DropRate = 56, Set = 0 },
            new CardData { CardId = 644, DropRate = 56, Set = 0 },
            new CardData { CardId = 5, DropRate = 24, Set = 1 },
            new CardData { CardId = 8, DropRate = 24, Set = 1 },
            new CardData { CardId = 50, DropRate = 24, Set = 1 },
            new CardData { CardId = 75, DropRate = 24, Set = 1 },
            new CardData { CardId = 101, DropRate = 24, Set = 1 },
            new CardData { CardId = 102, DropRate = 24, Set = 1 },
            new CardData { CardId = 104, DropRate = 24, Set = 1 },
            new CardData { CardId = 107, DropRate = 24, Set = 1 },
            new CardData { CardId = 122, DropRate = 24, Set = 1 },
            new CardData { CardId = 129, DropRate = 24, Set = 1 },
            new CardData { CardId = 134, DropRate = 24, Set = 1 },
            new CardData { CardId = 135, DropRate = 24, Set = 1 },
            new CardData { CardId = 145, DropRate = 24, Set = 1 },
            new CardData { CardId = 148, DropRate = 24, Set = 1 },
            new CardData { CardId = 152, DropRate = 24, Set = 1 },
            new CardData { CardId = 154, DropRate = 24, Set = 1 },
            new CardData { CardId = 156, DropRate = 24, Set = 1 },
            new CardData { CardId = 157, DropRate = 24, Set = 1 },
            new CardData { CardId = 158, DropRate = 24, Set = 1 },
            new CardData { CardId = 159, DropRate = 24, Set = 1 },
            new CardData { CardId = 160, DropRate = 24, Set = 1 },
            new CardData { CardId = 161, DropRate = 24, Set = 1 },
            new CardData { CardId = 174, DropRate = 24, Set = 1 },
            new CardData { CardId = 176, DropRate = 24, Set = 1 },
            new CardData { CardId = 179, DropRate = 24, Set = 1 },
            new CardData { CardId = 182, DropRate = 24, Set = 1 },
            new CardData { CardId = 183, DropRate = 24, Set = 1 },
            new CardData { CardId = 184, DropRate = 24, Set = 1 },
            new CardData { CardId = 185, DropRate = 24, Set = 1 },
            new CardData { CardId = 187, DropRate = 24, Set = 1 },
            new CardData { CardId = 191, DropRate = 24, Set = 1 },
            new CardData { CardId = 195, DropRate = 24, Set = 1 },
            new CardData { CardId = 198, DropRate = 24, Set = 1 },
            new CardData { CardId = 200, DropRate = 24, Set = 1 },
            new CardData { CardId = 203, DropRate = 24, Set = 1 },
            new CardData { CardId = 207, DropRate = 24, Set = 1 },
            new CardData { CardId = 208, DropRate = 24, Set = 1 },
            new CardData { CardId = 209, DropRate = 24, Set = 1 },
            new CardData { CardId = 210, DropRate = 24, Set = 1 },
            new CardData { CardId = 211, DropRate = 24, Set = 1 },
            new CardData { CardId = 212, DropRate = 24, Set = 1 },
            new CardData { CardId = 214, DropRate = 24, Set = 1 },
            new CardData { CardId = 222, DropRate = 24, Set = 1 },
            new CardData { CardId = 227, DropRate = 24, Set = 1 },
            new CardData { CardId = 229, DropRate = 24, Set = 1 },
            new CardData { CardId = 232, DropRate = 24, Set = 1 },
            new CardData { CardId = 238, DropRate = 24, Set = 1 },
            new CardData { CardId = 240, DropRate = 24, Set = 1 },
            new CardData { CardId = 242, DropRate = 24, Set = 1 },
            new CardData { CardId = 245, DropRate = 24, Set = 1 },
            new CardData { CardId = 254, DropRate = 24, Set = 1 },
            new CardData { CardId = 261, DropRate = 24, Set = 1 },
            new CardData { CardId = 265, DropRate = 24, Set = 1 },
            new CardData { CardId = 267, DropRate = 24, Set = 1 },
            new CardData { CardId = 268, DropRate = 24, Set = 1 },
            new CardData { CardId = 271, DropRate = 24, Set = 1 },
            new CardData { CardId = 285, DropRate = 24, Set = 1 },
            new CardData { CardId = 292, DropRate = 24, Set = 1 },
            new CardData { CardId = 420, DropRate = 24, Set = 1 },
            new CardData { CardId = 421, DropRate = 24, Set = 1 },
            new CardData { CardId = 451, DropRate = 24, Set = 1 },
            new CardData { CardId = 452, DropRate = 24, Set = 1 },
            new CardData { CardId = 476, DropRate = 24, Set = 1 },
            new CardData { CardId = 486, DropRate = 24, Set = 1 },
            new CardData { CardId = 490, DropRate = 24, Set = 1 },
            new CardData { CardId = 492, DropRate = 24, Set = 1 },
            new CardData { CardId = 506, DropRate = 24, Set = 1 },
            new CardData { CardId = 510, DropRate = 24, Set = 1 },
            new CardData { CardId = 536, DropRate = 24, Set = 1 },
            new CardData { CardId = 537, DropRate = 24, Set = 1 },
            new CardData { CardId = 540, DropRate = 24, Set = 1 },
            new CardData { CardId = 544, DropRate = 24, Set = 1 },
            new CardData { CardId = 549, DropRate = 24, Set = 1 },
            new CardData { CardId = 556, DropRate = 24, Set = 1 },
            new CardData { CardId = 579, DropRate = 24, Set = 1 },
            new CardData { CardId = 586, DropRate = 24, Set = 1 },
            new CardData { CardId = 589, DropRate = 24, Set = 1 },
            new CardData { CardId = 591, DropRate = 24, Set = 1 },
            new CardData { CardId = 598, DropRate = 24, Set = 1 },
            new CardData { CardId = 602, DropRate = 24, Set = 1 },
            new CardData { CardId = 604, DropRate = 32, Set = 1 },
            new CardData { CardId = 606, DropRate = 32, Set = 1 },
            new CardData { CardId = 609, DropRate = 32, Set = 1 },
            new CardData { CardId = 611, DropRate = 32, Set = 1 },
            new CardData { CardId = 29, DropRate = 16, Set = 2 },
            new CardData { CardId = 40, DropRate = 16, Set = 2 },
            new CardData { CardId = 47, DropRate = 16, Set = 2 },
            new CardData { CardId = 48, DropRate = 16, Set = 2 },
            new CardData { CardId = 59, DropRate = 16, Set = 2 },
            new CardData { CardId = 61, DropRate = 16, Set = 2 },
            new CardData { CardId = 65, DropRate = 16, Set = 2 },
            new CardData { CardId = 100, DropRate = 16, Set = 2 },
            new CardData { CardId = 108, DropRate = 16, Set = 2 },
            new CardData { CardId = 113, DropRate = 16, Set = 2 },
            new CardData { CardId = 114, DropRate = 16, Set = 2 },
            new CardData { CardId = 116, DropRate = 16, Set = 2 },
            new CardData { CardId = 118, DropRate = 16, Set = 2 },
            new CardData { CardId = 133, DropRate = 16, Set = 2 },
            new CardData { CardId = 138, DropRate = 16, Set = 2 },
            new CardData { CardId = 139, DropRate = 24, Set = 2 },
            new CardData { CardId = 140, DropRate = 16, Set = 2 },
            new CardData { CardId = 141, DropRate = 16, Set = 2 },
            new CardData { CardId = 142, DropRate = 24, Set = 2 },
            new CardData { CardId = 143, DropRate = 24, Set = 2 },
            new CardData { CardId = 144, DropRate = 24, Set = 2 },
            new CardData { CardId = 155, DropRate = 24, Set = 2 },
            new CardData { CardId = 162, DropRate = 16, Set = 2 },
            new CardData { CardId = 169, DropRate = 16, Set = 2 },
            new CardData { CardId = 172, DropRate = 16, Set = 2 },
            new CardData { CardId = 173, DropRate = 24, Set = 2 },
            new CardData { CardId = 175, DropRate = 24, Set = 2 },
            new CardData { CardId = 177, DropRate = 24, Set = 2 },
            new CardData { CardId = 178, DropRate = 24, Set = 2 },
            new CardData { CardId = 180, DropRate = 24, Set = 2 },
            new CardData { CardId = 181, DropRate = 16, Set = 2 },
            new CardData { CardId = 188, DropRate = 24, Set = 2 },
            new CardData { CardId = 189, DropRate = 24, Set = 2 },
            new CardData { CardId = 190, DropRate = 24, Set = 2 },
            new CardData { CardId = 199, DropRate = 24, Set = 2 },
            new CardData { CardId = 205, DropRate = 24, Set = 2 },
            new CardData { CardId = 206, DropRate = 24, Set = 2 },
            new CardData { CardId = 215, DropRate = 24, Set = 2 },
            new CardData { CardId = 218, DropRate = 24, Set = 2 },
            new CardData { CardId = 219, DropRate = 16, Set = 2 },
            new CardData { CardId = 224, DropRate = 24, Set = 2 },
            new CardData { CardId = 226, DropRate = 24, Set = 2 },
            new CardData { CardId = 228, DropRate = 24, Set = 2 },
            new CardData { CardId = 231, DropRate = 16, Set = 2 },
            new CardData { CardId = 239, DropRate = 24, Set = 2 },
            new CardData { CardId = 243, DropRate = 24, Set = 2 },
            new CardData { CardId = 244, DropRate = 24, Set = 2 },
            new CardData { CardId = 247, DropRate = 24, Set = 2 },
            new CardData { CardId = 250, DropRate = 16, Set = 2 },
            new CardData { CardId = 253, DropRate = 24, Set = 2 },
            new CardData { CardId = 257, DropRate = 16, Set = 2 },
            new CardData { CardId = 260, DropRate = 24, Set = 2 },
            new CardData { CardId = 264, DropRate = 24, Set = 2 },
            new CardData { CardId = 266, DropRate = 24, Set = 2 },
            new CardData { CardId = 269, DropRate = 16, Set = 2 },
            new CardData { CardId = 270, DropRate = 24, Set = 2 },
            new CardData { CardId = 276, DropRate = 16, Set = 2 },
            new CardData { CardId = 277, DropRate = 24, Set = 2 },
            new CardData { CardId = 279, DropRate = 24, Set = 2 },
            new CardData { CardId = 282, DropRate = 24, Set = 2 },
            new CardData { CardId = 283, DropRate = 24, Set = 2 },
            new CardData { CardId = 295, DropRate = 24, Set = 2 },
            new CardData { CardId = 296, DropRate = 16, Set = 2 },
            new CardData { CardId = 298, DropRate = 24, Set = 2 },
            new CardData { CardId = 300, DropRate = 24, Set = 2 },
            new CardData { CardId = 417, DropRate = 24, Set = 2 },
            new CardData { CardId = 431, DropRate = 16, Set = 2 },
            new CardData { CardId = 432, DropRate = 24, Set = 2 },
            new CardData { CardId = 446, DropRate = 24, Set = 2 },
            new CardData { CardId = 461, DropRate = 24, Set = 2 },
            new CardData { CardId = 463, DropRate = 24, Set = 2 },
            new CardData { CardId = 477, DropRate = 24, Set = 2 },
            new CardData { CardId = 478, DropRate = 24, Set = 2 },
            new CardData { CardId = 481, DropRate = 24, Set = 2 },
            new CardData { CardId = 489, DropRate = 24, Set = 2 },
            new CardData { CardId = 503, DropRate = 24, Set = 2 },
            new CardData { CardId = 505, DropRate = 24, Set = 2 },
            new CardData { CardId = 530, DropRate = 24, Set = 2 },
            new CardData { CardId = 534, DropRate = 24, Set = 2 },
            new CardData { CardId = 538, DropRate = 24, Set = 2 },
            new CardData { CardId = 539, DropRate = 24, Set = 2 },
            new CardData { CardId = 553, DropRate = 16, Set = 2 },
            new CardData { CardId = 559, DropRate = 24, Set = 2 },
            new CardData { CardId = 568, DropRate = 24, Set = 2 },
            new CardData { CardId = 569, DropRate = 24, Set = 2 },
            new CardData { CardId = 573, DropRate = 24, Set = 2 },
            new CardData { CardId = 580, DropRate = 16, Set = 2 },
            new CardData { CardId = 585, DropRate = 24, Set = 2 },
            new CardData { CardId = 588, DropRate = 16, Set = 2 },
            new CardData { CardId = 590, DropRate = 24, Set = 2 },
            new CardData { CardId = 592, DropRate = 24, Set = 2 },
            new CardData { CardId = 605, DropRate = 24, Set = 2 },
            new CardData { CardId = 610, DropRate = 24, Set = 2 },
            new CardData { CardId = 612, DropRate = 24, Set = 2 },
            new CardData { CardId = 629, DropRate = 16, Set = 2 },
            new CardData { CardId = 642, DropRate = 16, Set = 2 },
            new CardData { CardId = 646, DropRate = 16, Set = 2 },
            new CardData { CardId = 3, DropRate = 24, Set = 3 },
            new CardData { CardId = 10, DropRate = 24, Set = 3 },
            new CardData { CardId = 23, DropRate = 24, Set = 3 },
            new CardData { CardId = 25, DropRate = 24, Set = 3 },
            new CardData { CardId = 30, DropRate = 24, Set = 3 },
            new CardData { CardId = 34, DropRate = 24, Set = 3 },
            new CardData { CardId = 53, DropRate = 24, Set = 3 },
            new CardData { CardId = 76, DropRate = 24, Set = 3 },
            new CardData { CardId = 80, DropRate = 24, Set = 3 },
            new CardData { CardId = 109, DropRate = 24, Set = 3 },
            new CardData { CardId = 110, DropRate = 24, Set = 3 },
            new CardData { CardId = 112, DropRate = 24, Set = 3 },
            new CardData { CardId = 115, DropRate = 24, Set = 3 },
            new CardData { CardId = 119, DropRate = 24, Set = 3 },
            new CardData { CardId = 120, DropRate = 24, Set = 3 },
            new CardData { CardId = 121, DropRate = 24, Set = 3 },
            new CardData { CardId = 132, DropRate = 24, Set = 3 },
            new CardData { CardId = 146, DropRate = 24, Set = 3 },
            new CardData { CardId = 153, DropRate = 24, Set = 3 },
            new CardData { CardId = 164, DropRate = 24, Set = 3 },
            new CardData { CardId = 165, DropRate = 24, Set = 3 },
            new CardData { CardId = 171, DropRate = 24, Set = 3 },
            new CardData { CardId = 196, DropRate = 24, Set = 3 },
            new CardData { CardId = 201, DropRate = 24, Set = 3 },
            new CardData { CardId = 220, DropRate = 24, Set = 3 },
            new CardData { CardId = 221, DropRate = 24, Set = 3 },
            new CardData { CardId = 225, DropRate = 24, Set = 3 },
            new CardData { CardId = 233, DropRate = 16, Set = 3 },
            new CardData { CardId = 234, DropRate = 16, Set = 3 },
            new CardData { CardId = 236, DropRate = 24, Set = 3 },
            new CardData { CardId = 241, DropRate = 24, Set = 3 },
            new CardData { CardId = 246, DropRate = 24, Set = 3 },
            new CardData { CardId = 248, DropRate = 24, Set = 3 },
            new CardData { CardId = 251, DropRate = 24, Set = 3 },
            new CardData { CardId = 256, DropRate = 24, Set = 3 },
            new CardData { CardId = 258, DropRate = 24, Set = 3 },
            new CardData { CardId = 259, DropRate = 24, Set = 3 },
            new CardData { CardId = 262, DropRate = 24, Set = 3 },
            new CardData { CardId = 263, DropRate = 24, Set = 3 },
            new CardData { CardId = 272, DropRate = 24, Set = 3 },
            new CardData { CardId = 273, DropRate = 24, Set = 3 },
            new CardData { CardId = 274, DropRate = 24, Set = 3 },
            new CardData { CardId = 280, DropRate = 24, Set = 3 },
            new CardData { CardId = 290, DropRate = 24, Set = 3 },
            new CardData { CardId = 291, DropRate = 24, Set = 3 },
            new CardData { CardId = 293, DropRate = 24, Set = 3 },
            new CardData { CardId = 294, DropRate = 24, Set = 3 },
            new CardData { CardId = 381, DropRate = 24, Set = 3 },
            new CardData { CardId = 406, DropRate = 24, Set = 3 },
            new CardData { CardId = 414, DropRate = 24, Set = 3 },
            new CardData { CardId = 430, DropRate = 24, Set = 3 },
            new CardData { CardId = 435, DropRate = 24, Set = 3 },
            new CardData { CardId = 445, DropRate = 24, Set = 3 },
            new CardData { CardId = 450, DropRate = 24, Set = 3 },
            new CardData { CardId = 455, DropRate = 24, Set = 3 },
            new CardData { CardId = 457, DropRate = 24, Set = 3 },
            new CardData { CardId = 474, DropRate = 24, Set = 3 },
            new CardData { CardId = 480, DropRate = 24, Set = 3 },
            new CardData { CardId = 496, DropRate = 24, Set = 3 },
            new CardData { CardId = 502, DropRate = 24, Set = 3 },
            new CardData { CardId = 514, DropRate = 24, Set = 3 },
            new CardData { CardId = 543, DropRate = 24, Set = 3 },
            new CardData { CardId = 546, DropRate = 24, Set = 3 },
            new CardData { CardId = 550, DropRate = 24, Set = 3 },
            new CardData { CardId = 552, DropRate = 24, Set = 3 },
            new CardData { CardId = 560, DropRate = 24, Set = 3 },
            new CardData { CardId = 561, DropRate = 24, Set = 3 },
            new CardData { CardId = 566, DropRate = 24, Set = 3 },
            new CardData { CardId = 567, DropRate = 24, Set = 3 },
            new CardData { CardId = 570, DropRate = 24, Set = 3 },
            new CardData { CardId = 574, DropRate = 24, Set = 3 },
            new CardData { CardId = 576, DropRate = 24, Set = 3 },
            new CardData { CardId = 581, DropRate = 24, Set = 3 },
            new CardData { CardId = 583, DropRate = 24, Set = 3 },
            new CardData { CardId = 584, DropRate = 24, Set = 3 },
            new CardData { CardId = 599, DropRate = 24, Set = 3 },
            new CardData { CardId = 600, DropRate = 24, Set = 3 },
            new CardData { CardId = 601, DropRate = 24, Set = 3 },
            new CardData { CardId = 608, DropRate = 24, Set = 3 },
            new CardData { CardId = 615, DropRate = 24, Set = 3 },
            new CardData { CardId = 616, DropRate = 24, Set = 3 },
            new CardData { CardId = 620, DropRate = 24, Set = 3 },
            new CardData { CardId = 634, DropRate = 24, Set = 3 },
            new CardData { CardId = 643, DropRate = 24, Set = 3 },
            new CardData { CardId = 647, DropRate = 24, Set = 3 },
            new CardData { CardId = 649, DropRate = 24, Set = 3 },
            new CardData { CardId = 336, DropRate = 1024, Set = 4 },
            new CardData { CardId = 337, DropRate = 1024, Set = 4 },
            new CardData { CardId = 330, DropRate = 344, Set = 5 },
            new CardData { CardId = 331, DropRate = 344, Set = 5 },
            new CardData { CardId = 332, DropRate = 344, Set = 5 },
            new CardData { CardId = 333, DropRate = 344, Set = 5 },
            new CardData { CardId = 334, DropRate = 336, Set = 5 },
            new CardData { CardId = 335, DropRate = 336, Set = 5 },
            new CardData { CardId = 301, DropRate = 72, Set = 6 },
            new CardData { CardId = 302, DropRate = 72, Set = 6 },
            new CardData { CardId = 303, DropRate = 72, Set = 6 },
            new CardData { CardId = 304, DropRate = 72, Set = 6 },
            new CardData { CardId = 305, DropRate = 72, Set = 6 },
            new CardData { CardId = 306, DropRate = 72, Set = 6 },
            new CardData { CardId = 307, DropRate = 72, Set = 6 },
            new CardData { CardId = 308, DropRate = 72, Set = 6 },
            new CardData { CardId = 309, DropRate = 72, Set = 6 },
            new CardData { CardId = 310, DropRate = 72, Set = 6 },
            new CardData { CardId = 311, DropRate = 72, Set = 6 },
            new CardData { CardId = 312, DropRate = 72, Set = 6 },
            new CardData { CardId = 313, DropRate = 72, Set = 6 },
            new CardData { CardId = 314, DropRate = 72, Set = 6 },
            new CardData { CardId = 315, DropRate = 80, Set = 6 },
            new CardData { CardId = 316, DropRate = 72, Set = 6 },
            new CardData { CardId = 317, DropRate = 72, Set = 6 },
            new CardData { CardId = 319, DropRate = 72, Set = 6 },
            new CardData { CardId = 321, DropRate = 72, Set = 6 },
            new CardData { CardId = 322, DropRate = 72, Set = 6 },
            new CardData { CardId = 323, DropRate = 72, Set = 6 },
            new CardData { CardId = 324, DropRate = 72, Set = 6 },
            new CardData { CardId = 326, DropRate = 72, Set = 6 },
            new CardData { CardId = 327, DropRate = 72, Set = 6 },
            new CardData { CardId = 328, DropRate = 72, Set = 6 },
            new CardData { CardId = 652, DropRate = 80, Set = 6 },
            new CardData { CardId = 654, DropRate = 80, Set = 6 },
            new CardData { CardId = 659, DropRate = 80, Set = 6 },
        };

        public LoadDropRateTableUseCase()
        {
            starterDecks = new StarterDeck[Static.MAX_STARTER_DECKS_VANILLA];
        }

        public StarterDeck[] Execute()
        {
            InitializeDecks();

            Configurations.VanillaStarterDeck = starterDecks;
            Configurations.DropCardsVanillaStarterDeck = DroppedByDeck;

            return starterDecks;
        }

        public void ResetDropCardsVanillaStarterDeck()
        {
            Configurations.DropCardsVanillaStarterDeck = DroppedByDeck;
        }

        public int[] GetCards() {
            int[] Cards = new int[Static.MAX_CARDS];

            return Cards;
        }

        private void InitializeDecks()
        {
            for (int current = 0; current < Static.MAX_STARTER_DECKS_VANILLA; current++)
            {
                var NewStarterDeck = new StarterDeck {
                    Dropped = DroppedByDeck[current],
                    Cards = GetCardsWithRateDropBySet(current)
                };

                starterDecks[current] = NewStarterDeck;
            }
        }

        private int[] InitializateCards(int defaultValue)
        {
            int[] cards = new int[Static.MAX_CARDS];

            for (var i = 0; i < Static.MAX_CARDS; i++)
            {
                cards[i] = defaultValue;
            }
            return cards;
        }

        private int[] GetCardsWithRateDropBySet(int set)
        {
            int[] cards = InitializateCards(0);

            var setCards = CardSet.Where((card) => card.Set == set);
            foreach(var card in setCards)
            {
                cards[card.CardId - 1] = card.DropRate;
            }

            return cards;
        }
    }
}
