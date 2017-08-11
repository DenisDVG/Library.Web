using Library.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataEF
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("ApplicationDefaultConnection")
        {

        }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Brochure> Brochures { get; set; }
        public virtual DbSet<Magazine> Magazines { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<PublicationInPublisihngHouse> PublicationInPublisihngHouses { get; set; }
        public virtual DbSet<PublishingHouse> PublishingHouses { get; set; }

    }
}
