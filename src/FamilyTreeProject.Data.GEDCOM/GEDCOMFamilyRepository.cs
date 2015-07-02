//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Linq;
using System.Linq.Expressions;
using Naif.Core.Collections;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.Data.GEDCOM
{
    public class GEDCOMFamilyRepository : ILinqRepository<Family>
    {
        private readonly IGEDCOMStore _database;

        public GEDCOMFamilyRepository(IGEDCOMStore database)
        {
            Requires.NotNull(database);

            _database = database;
        }

        public void Add(Family item)
        {
            Requires.NotNull(item);

            _database.AddFamily(item);
        }

        public void Delete(Family item)
        {
            Requires.NotNull(item);

            _database.DeleteFamily(item);
        }

        public IQueryable<Family> Find(Expression<Func<Family, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IPagedList<Family> Find(int pageIndex, int pageSize, Expression<Func<Family, bool>> predicate)
        {
            return GetAll().Where(predicate).InPagesOf(pageSize).GetPage(pageIndex);
        }

        public IQueryable<Family> GetAll()
        {
            return _database.Families.AsQueryable();
        }

        public IPagedList<Family> GetPage(int pageIndex, int pageSize)
        {
            return GetAll().InPagesOf(pageSize).GetPage(pageIndex);
        }

        public void Update(Family item)
        {
            Requires.NotNull(item);

            _database.UpdateFamily(item);
        }
    }
}
