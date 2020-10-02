using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class Victims
    {
        public Victims()
        {
            VictimMaterials = new HashSet<VictimMaterials>();
        }

        public int VictimId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Flat { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Corps { get; set; }

        public virtual ICollection<VictimMaterials> VictimMaterials { get; set; }
    }
}
