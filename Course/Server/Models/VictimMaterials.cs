using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class VictimMaterials
    {
        public int MaterialMaterialId { get; set; }
        public int VictimVictimId { get; set; }

        public virtual Materials MaterialMaterial { get; set; }
        public virtual Victims VictimVictim { get; set; }
    }
}
