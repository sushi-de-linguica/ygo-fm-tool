using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FM.Core.Application;

namespace FM.Core.Domain.Models
{
    public class StarterDeck
    {
        public int Dropped { get; set; }
        public int[] Cards { get; set; } = new int[Static.MAX_CARDS];
    }
}
