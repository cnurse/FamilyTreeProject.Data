//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using FamilyTreeProject.Contracts;
using FamilyTreeProject.Core;

namespace FamilyTreeProject.Data.GEDCOM
{
    public class GEDCOMUnitOfWork : IUnitOfWork
    {
        private IGEDCOMStore _database;

        public GEDCOMUnitOfWork(string path)
        {
            Requires.NotNullOrEmpty("path", path);
            
            Initialize(new GEDCOMStore(path));
        }

        public GEDCOMUnitOfWork(IGEDCOMStore database)
        {
            Requires.NotNull("database", database);

            Initialize(database);
        }

        private void Initialize(IGEDCOMStore database)
        {
            _database = database;
        }

        public void BeginTransaction()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Commit()
        {
            _database.SaveChanges();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (typeof(T) == typeof(Individual))
            {
                return new GEDCOMIndividualRepository(_database) as IRepository<T>;
            }
            if (typeof(T) == typeof(Family))
            {
                return new GEDCOMFamilyRepository(_database) as IRepository<T>;
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
