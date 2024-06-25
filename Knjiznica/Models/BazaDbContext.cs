using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace Knjiznica.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]

    public class BazaDbContext : DbContext
    {

        public DbSet<Knjiga> PopisKnjiga { get; set; }
        public DbSet<TipKnjige> PopisTipovaKnjiga { get; set; }
        public DbSet<Korisnik> PopisKorisnika { get; set; }
        public DbSet<Ovlast> PopisOvlasti { get; set; }

    }
}