//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Data.Entity;
using FamilyTreeProject.Data.EntityFramework.Configuration;

namespace FamilyTreeProject.Data.EntityFramework
{
    public class FamilyTreeContext : DbContext
    {
        public FamilyTreeContext(string connectionStringName) : base(connectionStringName)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TreeConfiguration());
            modelBuilder.Configurations.Add(new FamilyConfiguration());
            modelBuilder.Configurations.Add(new IndividualConfiguration());
            modelBuilder.Configurations.Add(new CitationConfiguration());
            modelBuilder.Configurations.Add(new FactConfiguration());
            modelBuilder.Configurations.Add(new MultimediaLinkConfiguration());
            modelBuilder.Configurations.Add(new NoteConfiguration());
            modelBuilder.Configurations.Add(new RepositoryConfiguration());
            modelBuilder.Configurations.Add(new SourceConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}