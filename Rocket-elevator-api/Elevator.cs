using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class Elevator
    {
        internal AppDb Db { get; set; }

        public int Id { get; set; }
        public String Status { get; set; }
        public String SerialNumber { get; set; }
        public DateTime InspectionDate { get; set; }
        public DateTime InstallDate { get; set; }
        public String Certificat { get; set; }
        public String Information { get; set; }
        public String Note { get; set; }
        public String Type { get; set; }
        public int Column_id { get; set; }
        public int Category_id { get; set; }


        public Elevator() { }
        internal Elevator(AppDb db)
        {
            Db = db;
        }



        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE elevators e SET e.status_id = (SELECT s.id FROM statuses s WHERE s.name= @status) WHERE e.id = @id;";
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
