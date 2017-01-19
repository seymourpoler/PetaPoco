// <copyright file="TableInfo.cs" company="PetaPoco - CollaboratingPlatypus">
//      Apache License, Version 2.0 https://github.com/CollaboratingPlatypus/PetaPoco/blob/master/LICENSE.txt
// </copyright>
// <author>PetaPoco - CollaboratingPlatypus</author>
// <date>2015/12/05</date>

using System;
using System.Linq;

namespace PetaPoco
{
    /// <summary>
    ///     Use by IMapper to override table bindings for an object
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        ///     The database table name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     The name of the primary key column of the table
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        ///     True if the primary key column is an auto-incrementing
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        ///     The name of the sequence used for auto-incrementing Oracle primary key fields
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        ///     Creates and populates a TableInfo from the attributes of a POCO
        /// </summary>
        /// <param name="t">The POCO type</param>
        /// <returns>A TableInfo instance</returns>
        public static TableInfo FromPoco(Type t)
        {
            TableInfo tableInfo = new TableInfo();

            // Get the table name
            var a = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            tableInfo.TableName = a.Length == 0 ? t.Name : (a[0] as TableNameAttribute).Value;

            // Get the primary key
            a = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            tableInfo.PrimaryKey = a.Length == 0 ? null : (a[0] as PrimaryKeyAttribute).Value;
            tableInfo.SequenceName = a.Length == 0 ? null : (a[0] as PrimaryKeyAttribute).SequenceName;
            tableInfo.AutoIncrement = a.Length == 0 ? false : (a[0] as PrimaryKeyAttribute).AutoIncrement;

            if (string.IsNullOrEmpty(tableInfo.PrimaryKey))
            {
                var prop = t.GetProperties()
                    .FirstOrDefault(p => (p.Name.Equals("id", StringComparison.OrdinalIgnoreCase)) ||
                                         (p.Name.Equals(t.Name + "id", StringComparison.OrdinalIgnoreCase)) ||
                                         (p.Name.Equals(t.Name + "_id", StringComparison.OrdinalIgnoreCase)));

                if (prop != null)
                {
                    tableInfo.PrimaryKey = prop.Name;
                    tableInfo.AutoIncrement = prop.PropertyType.IsValueType;
                }
            }
            return tableInfo;
        }
    }
}