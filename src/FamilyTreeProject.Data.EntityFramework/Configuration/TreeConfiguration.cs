//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FamilyTreeProject.Data.EntityFramework.Configuration
{
    public class TreeConfiguration : EntityTypeConfiguration<Tree>
    {
        public TreeConfiguration()
        {
            ToTable("Trees");
            Property(t => t.TreeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Name);
            Property(t => t.OwnerId);

            Ignore(t => t.HomeIndividualId);
        }
    }
}
