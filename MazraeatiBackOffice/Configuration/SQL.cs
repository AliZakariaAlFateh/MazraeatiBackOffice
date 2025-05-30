using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class SQL
    {
        private DbContext _dbContext;
        public SQL(DataContext dataContext)
        {
            _dbContext = dataContext;
        }
        public dynamic GetDynamicDefultValue(string type)
        {
            int? intNullable = null;
            DateTime? dateTimeNullable = null;
            decimal? decimalNullable = null;
            dynamic dynull = null;
            switch (type)
            {
                case "String":
                    return string.Empty;
                case "Int32":
                    return intNullable;
                case "Decimal":
                    return decimalNullable;
                case "DateTime":
                    return dateTimeNullable;
                default:
                    return dynull;
            }
        }
        public List<T> ExecQuery<T>(string qry)
        {
            var list = new List<dynamic>();
            dynamic dynull = null;
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = qry;
                command.CommandType = CommandType.Text;
                _dbContext.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                    foreach (IDataRecord record in reader)
                    {
                        var expando = new ExpandoObject() as IDictionary<string, dynamic>;
                        foreach (var name in names)
                            expando[name] = record[name] == DBNull.Value ? dynull : record[name];

                        list.Add(expando);
                    }
                }
                _dbContext.Database.CloseConnection();
            }
            return list as List<T>;
        }
        public List<T> ExecSQL<T>(string query)
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                _dbContext.Database.OpenConnection();
                List<T> list = new List<T>();
                using (var result = command.ExecuteReader())
                {
                    T obj = default(T);
                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                }
                _dbContext.Database.CloseConnection();
                return list;
            }
        }
    }
}
