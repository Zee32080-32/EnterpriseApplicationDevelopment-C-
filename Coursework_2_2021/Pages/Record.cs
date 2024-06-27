using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework_2_2021.Pages
{
    public class Record
    {
        public string ID { get; set; }
        public string Content { get; set; }

        public Record(String id, String content = "")
        {
            id = ID;
            content = Content;
        }
    }
}
