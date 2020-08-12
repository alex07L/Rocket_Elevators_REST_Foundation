using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class LeadQuery
    {
        public AppDb Db { get; }
        public LeadQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Lead>> getAllLead()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT DISTINCT l.id, l.fullName, l.entrepriseName, l.cellPhone, l.projectName, l.description, t.name FROM leads l JOIN customers c ON c.email!=l.email JOIN types t ON t.id=l.type_id WHERE DATEDIFF(l.created_at,CURDATE()) <= 30;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Lead>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Lead>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Lead(Db)
                    {
                        Id = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        EntrepriseName = reader.GetString(2),
                        CellPhone = reader.GetString(3),
                        ProjectName = reader.GetString(4),
                        Description = reader.GetString(5),
                        Type = reader.GetString(6)

                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
