using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using carXapp2.classes;

namespace carXapp2
{

    public class Data
    {
        public AppDataContext Database;
        
        public Data(string DbConnection)
        {
            Database = new AppDataContext(DbConnection);
        }

    }
}