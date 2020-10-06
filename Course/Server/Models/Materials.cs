using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class Materials
    {
        public Materials()
        {
            MaterialEmployees = new HashSet<MaterialEmployees>();
            VictimMaterials = new HashSet<VictimMaterials>();
            Documents = new HashSet<Documents>();
        }

        public int MaterialId { get; set; }
        public int NumberEk { get; set; }
        public string Story { get; set; }
        public DateTime? DateOfRegistration { get; set; }
        public DateTime? DateOfTerm { get; set; }
        public bool Extension { get; set; }
        public string Decision { get; set; }
        public bool ExecutedOrNotExecuted { get; set; }
        public string Perspective { get; set; }

        public virtual ICollection<MaterialEmployees> MaterialEmployees { get; set; }
        public virtual ICollection<VictimMaterials> VictimMaterials { get; set; }
        public virtual ICollection<Documents> Documents { get; set; }

        //public virtual ICollection<Victims> Victims { get; set; }
    }
}
