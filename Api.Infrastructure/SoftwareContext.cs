using Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure
{
    public class SoftwareContext: DbContext
    {
        public SoftwareContext(DbContextOptions<SoftwareContext> options)
                  : base(options)
        {

        }

        public virtual DbSet<Rol> Rol { get; set; } 
    }
}
