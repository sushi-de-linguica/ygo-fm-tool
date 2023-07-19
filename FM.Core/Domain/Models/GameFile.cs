using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Core.Domain.Models
{
    public class GameFile
    {
        public int Offset { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
        public int NameSize { get; set; }
        public bool IsDirectory { get; set; }
    }
}
