using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_2021
{
    public class CollectionOfContacts
    {
        public List<Contacts> Contacts { get; set; }


        public CollectionOfContacts()
        {
            Contacts = new List<Contacts>();
        }
    }
}
