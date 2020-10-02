using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class Employees
    {
        public Employees()
        {
            MaterialEmployees = new HashSet<MaterialEmployees>();
        }

        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Rank { get; set; }
        public int NumberMaterialsOnPerformance { get; set; }
        public int NumberMaterialsPerformed { get; set; }
        public string Position { get; set; }

        public virtual ICollection<MaterialEmployees> MaterialEmployees { get; set; }
    }
}
