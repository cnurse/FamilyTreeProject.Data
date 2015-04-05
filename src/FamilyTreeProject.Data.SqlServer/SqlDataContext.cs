#region Copyright

// Copyright 2011 - Charles Nurse

#endregion

using System;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Reflection;

using Keydance.Contracts;

namespace FamilyTreeProject.Data.SqlServer
{
    public class SqlDataContext : IDataContext, IDisposable
    {
        private Table<Individual> individuals;
        private Table<Family> families;
        private Table<Note> notes;

        private readonly DataContext db;

        #region Constructors

        /// <summary>
        ///   Constructs a new SqlDataContext using the specified connection
        /// </summary>
        /// <param name = "connection">
        ///   The name of a connection suitable for use with LINQ to SQL
        /// </param>
        public SqlDataContext(string connection)
        {
            db = new DataContext(connection, CreateMappingSource());
            Initialize();
        }

        /// <summary>
        ///   Constructs a new SqlDataContext using the specified delegate to
        ///   open an <see cref = "IDbConnection" /> to the SQL Server instance
        /// </summary>
        /// <param name = "connectionFactory">
        ///   A delegate which returns an <see cref = "IDbConnection" /> suitable for use with
        ///   LINQ to SQL.
        /// </param>
        public SqlDataContext(Func<IDbConnection> connectionFactory)
        {
            db = new DataContext(connectionFactory(), CreateMappingSource());
            Initialize();
        }

        /// <summary>
        ///   Constructs a new SqlDataContext using the specified connection
        /// </summary>
        /// <param name = "connection">
        ///   An <see cref = "IDbConnection" /> suitable for use with LINQ to SQL
        /// </param>
        public SqlDataContext(IDbConnection connection)
        {
            db = new DataContext(connection, CreateMappingSource());
            Initialize();
        }

        #endregion

        #region Private Methods

        private static MappingSource CreateMappingSource()
        {
            // Create an XML Mapping Source, using the mapping file from the embedded resources
            Stream mappingFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("FamilyTreeProject.Data.SqlServer.FamilyTreeMap.xml");
            return XmlMappingSource.FromStream(mappingFile);
        }

        private void Initialize()
        {
            individuals = db.GetTable<Individual>();
            families = db.GetTable<Family>();
            notes = db.GetTable<Note>();
        }

        #endregion

        #region IDataContext Implementation

        public IQueryable<Individual> Individuals 
        {
            get { return individuals; } 
        }

        public IQueryable<Family> Families
        {
            get { return families; }
        }

        public IQueryable<Note> Notes
        {
            get { return notes; }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void AddIndividual(Individual individual)
        {
            Requires.NotNull("individual", individual);

            individuals.InsertOnSubmit(individual);
        }

        public void DeleteIndividual(Individual individual)
        {
            Requires.NotNull("individual", individual);

            individuals.DeleteOnSubmit(individual);
        }

        public void UpdateIndividual(Individual individual)
        {
            Requires.NotNull("individual", individual);
        }

        public void AddFamily(Family family)
        {
            Requires.NotNull("family", family);

            families.InsertOnSubmit(family);
        }

        public void DeleteFamily(Family family)
        {
            Requires.NotNull("family", family);

            families.DeleteOnSubmit(family);
        }

        public void UpdateFamily(Family family)
        {
            Requires.NotNull("family", family);
        }

        public void AddNote(Note note)
        {
            Requires.NotNull("note", note);

            notes.InsertOnSubmit(note);
        }

        public void DeleteNote(Note note)
        {
            Requires.NotNull("note", note);

            notes.DeleteOnSubmit(note);
        }

        public void UpdateNote(Note note)
        {
            Requires.NotNull("note", note);
        }

        public void SubmitChanges()
        {
            db.SubmitChanges();
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            //DataContext does not need to be disposed
        }

        #endregion
    }
}