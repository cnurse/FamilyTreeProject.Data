//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.ComponentModel.DataAnnotations.Schema;
using FamilyTreeProject.Core;

namespace FamilyTreeProject.Data.EntityFramework.Configuration
{
    //public class IndividualConfiguration : EntityTypeConfiguration<Individual>
    //{
    //    public IndividualConfiguration()
    //    {
    //        ToTable("Individuals");
    //        Property(ind => ind.Id)
    //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
    //        Property(ind => ind.TreeId);
    //        Property(ind => ind.FirstName);
    //        Property(ind => ind.LastName);
    //        Property(ind => ind.Sex);
    //        Property(ind => ind.FatherId);
    //        Property(ind => ind.MotherId);

    //        Ignore(ind => ind.Name);
    //        Ignore(ind => ind.Father);
    //        Ignore(ind => ind.Mother);
    //        Ignore(ind => ind.Children);
    //        Ignore(ind => ind.Facts);
    //        Ignore(ind => ind.Multimedia);
    //        Ignore(ind => ind.Notes);
    //        Ignore(ind => ind.Spouses);
    //        Ignore(ind => ind.Citations);
    //    }
    //}
}
