using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Coursework_2_2021
{
    public class CollectionOfUsers
    {

        public List<Person> people { get; set; }


        public CollectionOfUsers()
        {
            people = new List<Person>();
        }
    }
}
