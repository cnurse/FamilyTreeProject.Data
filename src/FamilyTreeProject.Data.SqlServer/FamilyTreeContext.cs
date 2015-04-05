using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FamilyTreeProject.Data.SqlServer
{
    class FamilyTreeContext : DataContext
    {
        #region Constructors

		/// <summary>
        /// Constructs a new FamilyTreeContext using the specified connection
        /// </summary>
        /// <param name="connection">
        /// The name of a connection suitable for use with LINQ to SQL
        /// </param>
        public FamilyTreeContext(string connection)
            : base(connection, CreateMappingSource())
        {
        }

        /// <summary>
        /// Constructs a new FamilyTreeContext using the specified delegate to
        /// open an <see cref="IDbConnection"/> to the SQL Server instance
        /// </summary>
        /// <param name="connectionFactory">
        /// A delegate which returns an <see cref="IDbConnection"/> suitable for use with
        /// LINQ to SQL.
        /// </param>
        public FamilyTreeContext(Func<IDbConnection> connectionFactory)
            : base(connectionFactory(), CreateMappingSource())
        {
        }

        /// <summary>
        /// Constructs a new FamilyTreeContext using the specified connection
        /// </summary>
        /// <param name="connection">
        /// An <see cref="IDbConnection"/> suitable for use with LINQ to SQL
        /// </param>
        public FamilyTreeContext(IDbConnection connection)
            : base(connection, CreateMappingSource())
        {
        }

	    #endregion 

        #region Private Methods

        /// <summary>
        /// Creates the Xml Mapping Source that is used to map the property values and column names
        /// </summary>
        /// <returns>A Mapping Source object</returns>
        private static MappingSource CreateMappingSource()
        {
            // Create an XML Mapping Source, using the mapping file from the embedded resources
            Stream mappingFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("FamilyTreeProject.Data.SqlServer.FamilyTreeMap.xml");
            return XmlMappingSource.FromStream(mappingFile);
        }

        #endregion
    }
}
