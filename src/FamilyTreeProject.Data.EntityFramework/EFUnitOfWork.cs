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

namespace FamilyTreeProject.Data.EntityFramework
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private FamilyTreeContext _db;
        private ICacheProvider _cache;

        public EFUnitOfWork(string connectionStringName, ICacheProvider cache)
        {
            Requires.NotNullOrEmpty("connectionStringName", connectionStringName);
            Requires.NotNull(cache);

            Initialize(new FamilyTreeContext(connectionStringName), cache);
        }

        public EFUnitOfWork(FamilyTreeContext db, ICacheProvider cache)
        {
            Requires.NotNull(db);
            Requires.NotNull(cache);

            Initialize(db, cache);
        }

        private void Initialize(FamilyTreeContext db, ICacheProvider cache)
        {
            _db = db;
            _cache = cache;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new EFRepository<T>(_db, _cache);
        }
    }
}
