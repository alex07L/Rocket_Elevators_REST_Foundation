using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class ElevatorQuery
    {
        public AppDb Db { get; }
        public ElevatorQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Elevator> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT e.id, s.name, e.serialNumber, e.inspectionDate, e.installDate, e.certificat, e.information, e.note, t.name AS 'type', e.column_id, e.category_id FROM elevators e JOIN statuses s ON s.id=e.status_id JOIN types t ON t.id=e.type_id WHERE e.id = @id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Elevator>> Async()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT e.id, s.name, e.serialNumber, e.inspectionDate, e.installDate, e.certificat, e.information, e.note, t.name AS 'type', e.column_id, e.category_id FROM elevators e JOIN statuses s ON s.id=e.status_id JOIN types t ON t.id=e.type_id WHERE s.name !='active';";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Elevator>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Elevator>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Elevator(Db)
                    {
                        Id = reader.GetInt32("id"),
                        Status = reader.GetString("name"),
                        SerialNumber = reader.GetString("serialNumber"),
                        InspectionDate = reader.GetDateTime("inspectionDate"),
                        InstallDate = reader.GetDateTime("installDate"),
                        Certificat = reader.GetString("certificat"),
                        Information = reader.GetString("note"),
                        Type = reader.GetString("type"),
                        Column_id = reader.GetInt32("column_id"),
                        Category_id = reader.GetInt32("category_id")
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
