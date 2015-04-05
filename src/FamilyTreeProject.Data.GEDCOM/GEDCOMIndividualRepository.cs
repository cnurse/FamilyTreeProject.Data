//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using Naif.Core.Caching;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.Data.GEDCOM
{
    public class GEDCOMIndividualRepository : RepositoryBase<Individual>
    {
        private readonly IGEDCOMStore _database;

        public GEDCOMIndividualRepository(IGEDCOMStore database, ICacheProvider cache) : base(cache)
        {
            Requires.NotNull("database", database);

            _database = database;
        }

        protected override void AddInternal(Individual individual)
        {
            Requires.NotNull("individual", individual);

            _database.AddIndividual(individual);
        }

        protected override void DeleteInternal(Individual individual)
        {
            Requires.NotNull("individual", individual);

            _database.DeleteIndividual(individual);
        }

        protected override IEnumerable<Individual> GetAllInternal()
        {
            return _database.Individuals;
        }

        protected override Individual GetByIdInternal<TProperty>(TProperty id)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        protected override IEnumerable<Individual> GetByPropertyInternal<TProperty>(string propertyName, TProperty propertyValue)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        protected override void UpdateInternal(Individual individual)
        {
            Requires.NotNull("individual", individual);

            _database.UpdateIndividual(individual);
        }
    }
}
