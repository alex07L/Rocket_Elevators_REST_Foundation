using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class Battery
    {
        internal AppDb Db { get; set; }

        public int Id { get; set; }
        public String Status { get; set; }


        public Battery() { }
        internal Battery(AppDb db)
        {
            Db = db;
        }



        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE batteries b SET b.status_id = (SELECT s.id FROM statuses s WHERE s.name=@status) WHERE b.id = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }
        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@status",
                DbType = DbType.String,
                Value = Status,
            });
        }

    }
}
