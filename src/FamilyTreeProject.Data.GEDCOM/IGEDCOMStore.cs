//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System.Collections.Generic;

namespace FamilyTreeProject.Data.GEDCOM
{
    public interface IGEDCOMStore
    {
        List<Family> Families { get; }

        List<Individual> Individuals { get; }

        void AddFamily(Family family);

        void AddIndividual(Individual individual);

        void DeleteFamily(Family family);

        void DeleteIndividual(Individual individual);

        void SaveChangesAsync();

        void UpdateFamily(Family family);

        void UpdateIndividual(Individual individual);
    }
}
