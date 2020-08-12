using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class BatteryQuery
    {
        public AppDb Db { get; }
        public BatteryQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Battery> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT b.id, s.name FROM batteries b JOIN statuses s ON b.status_id=s.id WHERE b.id = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Battery>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Battery>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Battery(Db)
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
