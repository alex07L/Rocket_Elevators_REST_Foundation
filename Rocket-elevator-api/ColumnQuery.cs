using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class ColumnQuery
    {
        public AppDb Db { get; }
        public ColumnQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Column> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT c.id, s.name FROM columns c JOIN statuses s ON c.status_id=s.id WHERE c.id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Column>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Column>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Column(Db)
                    {
                        Id = reader.GetInt32(0),
                        Status = reader.GetString(1)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
