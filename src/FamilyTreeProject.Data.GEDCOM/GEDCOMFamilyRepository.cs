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
    public class GEDCOMFamilyRepository : RepositoryBase<Family>
    {
        private readonly IGEDCOMStore _database;

        public GEDCOMFamilyRepository(IGEDCOMStore database, ICacheProvider cache) : base(cache)
        {
            Requires.NotNull("database", database);

            _database = database;
        }

        protected override void AddInternal(Family family)
        {
            Requires.NotNull("family", family);

            _database.AddFamily(family);
        }

        protected override void DeleteInternal(Family family)
        {
            Requires.NotNull("family", family);

            _database.DeleteFamily(family);
        }

        protected override IEnumerable<Family> GetAllInternal()
        {
            return _database.Families;
        }

        protected override Family GetByIdInternal<TProperty>(TProperty id)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        protected override IEnumerable<Family> GetByPropertyInternal<TProperty>(string propertyName, TProperty propertyValue)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        protected override void UpdateInternal(Family family)
        {
            Requires.NotNull("family", family);

            _database.UpdateFamily(family);
        }
    }
}
