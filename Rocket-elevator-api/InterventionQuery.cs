using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class InterventionQuery
    {

        public AppDb Db { get; }
        public InterventionQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Intervention>> Async()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, author_id, customer_id, building_id, battery_id, column_id, elevator_id,employee_id, start_intervention, end_intervention, result, rapport, status FROM interventions WHERE start_intervention IS NULL AND status='pending';";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<Intervention> First(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT id, author_id, customer_id, building_id, battery_id, column_id, elevator_id,employee_id, start_intervention, end_intervention, result, rapport, status FROM interventions WHERE id=@id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Intervention>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Intervention>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Intervention(Db)
                    {
                        Id = saveInt(reader, "id"),
                        Author = saveInt(reader, "author_id"),
                        Customer = saveInt(reader, "customer_id"),
                        Building = saveInt(reader, "building_id"),
                        Battery = saveInt(reader, "battery_id"),
                        Column = saveInt(reader, "column_id"),
                        Elevator = saveInt(reader, "elevator_id"),
                        Employee = saveInt(reader, "employee_id"),
                        Start = saveDateTime(reader, "start_intervention"),
                        End = saveDateTime(reader, "end_intervention"),
                        Result = saveString(reader, "result"),
                        Rapport = saveString(reader, "rapport"),
                        Status = saveString(reader, "status")

                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        private int saveInt(DbDataReader reader ,String column)
        {
            if (!reader.IsDBNull(column))
            {
                return reader.GetInt32(column);
            }
            return 0;
        }

        private string saveString(DbDataReader reader, String column)
        {
            if (!reader.IsDBNull(column))
            {
                return reader.GetString(column);
            }
            return "";
        }

        private DateTime? saveDateTime(DbDataReader reader, String column)
        {
            if (!reader.IsDBNull(column))
            {
                return reader.GetDateTime(column);
            }
            return null;
        }
    }
}
