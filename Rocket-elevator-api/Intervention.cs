using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class Intervention
    {

        public int Id { get; set; }
        public int Author { get; set; }
        public int Customer { get; set; }
        public int Building { get; set; }
        public int Battery { get; set; }
        public int Column { get; set; }
        public int Elevator { get; set; }
        public int Employee { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public String Result { get; set; }
        public String Rapport { get; set; }
        public String Status { get; set; }


        internal AppDb Db { get; set; }

        public Intervention() { }
        internal Intervention(AppDb db)
        {
            Db = db;
        }

        public async Task UpdateStatus()
        {
            using var cmd = Db.Connection.CreateCommand();
            if (Status.ToLower().Equals("inprogress"))
            {
                cmd.CommandText = @"UPDATE interventions i SET i.status= @status, start_intervention=CURRENT_TIMESTAMP() WHERE i.id = @id;";
            }
            else if (Status.ToLower().Equals("complete"))
            {
                cmd.CommandText = @"UPDATE interventions i SET i.status= @status, end_intervention=CURRENT_TIMESTAMP() WHERE i.id = @id;";
            }
            
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
