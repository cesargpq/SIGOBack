using Api.Core.Entities;
using Api.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly SoftwareContext _context;

        public RolRepository(SoftwareContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<Rol>> GetRol()
        {
            return await _context.Rol.ToListAsync();
        }
    }
}
