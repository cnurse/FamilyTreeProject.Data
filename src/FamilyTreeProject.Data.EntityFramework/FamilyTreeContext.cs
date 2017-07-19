//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using Microsoft.EntityFrameworkCore;

namespace FamilyTreeProject.Data.EntityFramework
{
    public class FamilyTreeContext : DbContext
    {
        public FamilyTreeContext() { }

        public FamilyTreeContext(DbContextOptions<FamilyTreeContext> options) : base(options) { }

        public DbSet<Tree> Trees { get; set; }

        public DbSet<Individual> Individuals { get; set; }

        public DbSet<Family> Familys { get; set; }

        public DbSet<Citation> Citations { get; set; }

        public DbSet<Fact> Facts { get; set; }

        public DbSet<MultimediaLink> MultimediaLinks { get; set; }

        public DbSet<Note> Note { get; set; }

        public DbSet<Source> Sources { get; set; }

        public DbSet<Repository> Repositorys { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Configurations.Add(new TreeConfiguration());
            //modelBuilder.Configurations.Add(new FamilyConfiguration());
            //modelBuilder.Configurations.Add(new IndividualConfiguration());
            //modelBuilder.Configurations.Add(new CitationConfiguration());
            //modelBuilder.Configurations.Add(new FactConfiguration());
            //modelBuilder.Configurations.Add(new MultimediaLinkConfiguration());
            //modelBuilder.Configurations.Add(new NoteConfiguration());
            //modelBuilder.Configurations.Add(new RepositoryConfiguration());
            //modelBuilder.Configurations.Add(new SourceConfiguration());



            base.OnModelCreating(modelBuilder);
        }
    }
}