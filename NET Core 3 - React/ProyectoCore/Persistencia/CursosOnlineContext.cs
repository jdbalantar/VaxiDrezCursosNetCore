using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia
{
    public class CursosOnlineContext : DbContext
    {
        public CursosOnlineContext(DbContextOptions<CursosOnlineContext> options) : base(options)
        {

        }
    }
}
