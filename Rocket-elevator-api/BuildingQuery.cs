using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class BuildingQuery
    {
        public AppDb Db { get; }
        public BuildingQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Building>> getAllInterventionBuilding()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT DISTINCT b.id, b.fullName, b.cellPhone, b.email,b.techEmail, b.techName, b.techPhone, b.customer_id, b.address_id FROM buildings b JOIN batteries b2 ON b2.building_id=b.id JOIN `columns` c ON b2.id=c.battery_id JOIN elevators e ON e.column_id=c.id WHERE b2.status_id=(SELECT s2.id FROM statuses s2 WHERE s2.name='intervention') OR c.status_id=(SELECT s2.id FROM statuses s2 WHERE s2.name='intervention') OR e.status_id=(SELECT s2.id FROM statuses s2 WHERE s2.name='intervention');";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Building>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Building>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Building(Db)
                    {
                        Id = reader.GetInt32("id"),
                        FullName = reader.GetString("fullName"),
                        CellPhone = reader.GetString("cellPhone"),
                        Email = reader.GetString("email"),
                        TechEmail = reader.GetString("techEmail"),
                        TechName = reader.GetString("techName"),
                        TechPhone = reader.GetString("techPhone"),
                        Customer = reader.GetInt32("customer_id"),
                        Address = reader.GetInt32("address_id"),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
