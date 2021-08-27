using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core.Enitites
{
    public class DocProperty
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Value
    {
        public string description { get; set; }
        public int value { get; set; }
    }

    public class Variant
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<Value> values { get; set; }
    }

    public class Table
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> columns { get; set; }
        public List<int> supress_columns { get; set; }
        public List<List<string>> data { get; set; }
    }

    public class AnswersEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public List<DocProperty> docProperties { get; set; }
        public List<Variant> variants { get; set; }
        public List<Table> tables { get; set; }
    }
}
