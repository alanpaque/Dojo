using System.Data.Entity;

namespace Dojo.Models
{ 

    public class Context : DbContext
    {
        public System.Data.Entity.DbSet<BO.Samourai> Samourais {get; set;}

        public System.Data.Entity.DbSet<BO.Arme> Armes {get; set;}
    }
}
