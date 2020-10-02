using Course.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Victim> Victims { get; set; }
    }
}
