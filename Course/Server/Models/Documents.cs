using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Documents
    {
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public byte[] DocumentData { get; set; }

        public int MaterialsMaterialId { get; set; }
        //public virtual Materials Materials { get; set; }

    }
}
