//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using Naif.Core.Caching;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.Data.GEDCOM
{
    public class GEDCOMDataContext : IDataContext
    {
        private ICacheProvider _cache;
        private IGEDCOMStore _database;

        public GEDCOMDataContext(string path, ICacheProvider cache)
        {
            Requires.NotNullOrEmpty("path", path);
            Requires.NotNull("cache", cache);
            
            Initialize(new GEDCOMStore(path), cache);
        }

        public GEDCOMDataContext(IGEDCOMStore database, ICacheProvider cache)
        {
            Requires.NotNull("database", database);
            Requires.NotNull("cache", cache);

            Initialize(database, cache);
        }

        private void Initialize(IGEDCOMStore database, ICacheProvider cache)
        {
            _database = database;
            _cache = cache;
        }

        public void BeginTransaction()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Commit()
        {
            _database.SaveChangesAsync();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (typeof(T) == typeof(Individual))
            {
                return new GEDCOMIndividualRepository(_database, _cache)as IRepository<T>;
            }
            if (typeof(T) == typeof(Family))
            {
                return new GEDCOMFamilyRepository(_database, _cache)as IRepository<T>;
            }
            throw new NotImplementedException();
        }

        public void RollbackTransaction()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
