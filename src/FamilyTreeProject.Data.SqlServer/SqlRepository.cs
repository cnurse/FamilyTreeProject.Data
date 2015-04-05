using System;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Reflection;

using Keydance.Collections;
using Keydance.Contracts;
using Keydance.Data;

namespace FamilyTreeProject.Data.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlRepository<T> : IDisposable, IRepository<T> where T : class
    {
        #region Private Members

        private FamilyTreeContext db;

        /// <summary>
        /// Gets the table provided by the type T and returns for querying
        /// </summary>
        private Table<T> Table
        {
            get { return db.GetTable<T>(); }
        }

        #endregion

        #region Constructors

		/// <summary>
        /// Constructs a new SqlRepository using the specified connection
        /// </summary>
        /// <param name="connection">
        /// The name of a connection suitable for use with LINQ to SQL
        /// </param>
        public SqlRepository(string connection)
        {
            db = new FamilyTreeContext(connection);
        }

        /// <summary>
        /// Constructs a new SqlRepository using the specified delegate to
        /// open an <see cref="IDbConnection"/> to the SQL Server instance
        /// </summary>
        /// <param name="connectionFactory">
        /// A delegate which returns an <see cref="IDbConnection"/> suitable for use with
        /// LINQ to SQL.
        /// </param>
        public SqlRepository(Func<IDbConnection> connectionFactory)
        {
            db = new FamilyTreeContext(connectionFactory());
        }

        /// <summary>
        /// Constructs a new SqlRepository using the specified connection
        /// </summary>
        /// <param name="connection">
        /// An <see cref="IDbConnection"/> suitable for use with LINQ to SQL
        /// </param>
        public SqlRepository(IDbConnection connection)
        {
            db = new FamilyTreeContext(connection);
        }

	    #endregion 

        #region IRepository<T> Members

        /// <summary>
        /// Adds an item to the database
        /// </summary>
        /// <param name="item">The item to add</param>
        public void Add(T item)
        {
            Requires.NotNull("item", item);
 
            Table.InsertOnSubmit(item);
            db.SubmitChanges();
        }

        /// <summary>
        /// Deletes an item from the database
        /// </summary>
        /// <param name="item">The item to delete</param>
        public void Delete(T item)
        {
            Requires.NotNull("item", item);
            
            Table.DeleteOnSubmit(item);
            db.SubmitChanges();
        }

        /// <summary>
        /// Deletes items from the database
        /// </summary>
        /// <param name="expression">An expression that identifes the items to delete</param>
        public void Delete(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            foreach (T entity in Find(expression))
                Delete(entity);
        }

        /// <summary>
        /// Finds an item using a passed-in expression lambda
        /// </summary>
        public IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return Table.Where(expression);
        }

        /// <summary>
        /// Returns all T records in the repository
        /// </summary>
        public IQueryable<T> GetAll()
        {
            return Table;
        }

        /// <summary>
        /// Returns a PagedList of items
        /// </summary>
        /// <param name="pageIndex">zero-based index to be used for lookup</param>
        /// <param name="pageSize">the size of the paged items</param>
        /// <returns></returns>
        public PagedList<T> GetPaged(int pageIndex, int pageSize)
        {
            return new PagedList<T>(Table, pageIndex, pageSize);
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="item"></param>
        public void Update(T item)
        {
            Requires.NotNull("item", item);

            db.SubmitChanges();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes of this repository and the underlying database connection
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }

        #endregion
    }
}



