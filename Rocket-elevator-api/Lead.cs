using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class Lead
    {
        internal AppDb Db { get; set; }

        public int Id { get; set; }
        public String FullName { get; set; }
        public String EntrepriseName { get; set; }
        public String CellPhone { get; set; }
        public String ProjectName { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }


        public Lead() { }
        internal Lead(AppDb db)
        {
            Db = db;
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

    }
}
