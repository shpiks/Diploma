using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class MaterialEmployees
    {
        public int EmployeeEmployeeId { get; set; }
        public int MaterialMaterialId { get; set; }

        public virtual Employees EmployeeEmployee { get; set; }
        public virtual Materials MaterialMaterial { get; set; }
    }
}
