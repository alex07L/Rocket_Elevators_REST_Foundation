using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket_elevator_api
{
    public class Building
    {
        internal AppDb Db { get; set; }

        public int Id { get; set; }
        public String FullName { get; set; }
        public String CellPhone { get; set; }
        public String Email { get; set; }
        public String TechEmail { get; set; }
        public String TechName { get; set; }
        public String TechPhone { get; set; }
        public int Customer { get; set; }
        public int Address { get; set; }


        public Building() { }
        internal Building(AppDb db)
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
