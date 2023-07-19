using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Domain.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Attribute { get; set; }
        public int Level { get; set; }
        public int Type { get; set; }
        public int GuardianStar1 { get; set; }
        public int GuardianStar2 { get; set; }
    }
}
