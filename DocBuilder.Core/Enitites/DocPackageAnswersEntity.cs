using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocBuilder.Core.Enitites
{
    public class GeneralDocProperty
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class DocProperty
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("value")]
        public int CopiesValue { get; set; }
    }

    public class Variant
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("values")]
        public List<Value> Values { get; set; }
    }

    public class Table
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("columns")]
        public List<string> Columns { get; set; }

        [JsonPropertyName("supress_columns")]
        public List<int> SupressColumns { get; set; }

        [JsonPropertyName("data")]
        public List<List<string>> Data { get; set; }
    }

    public class PackItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("docProperties")]
        public List<DocProperty> DocProperties { get; set; }

        [JsonPropertyName("variants")]
        public List<Variant> Variants { get; set; }

        [JsonPropertyName("tables")]
        public List<Table> Tables { get; set; }
    }

    public class DocPackageAnswersEntity
    {
        [JsonPropertyName("generalDocProperties")]
        public List<GeneralDocProperty> GeneralDocProperties { get; set; }

        [JsonPropertyName("packItems")]
        public List<PackItem> PackItems { get; set; }
    }
}
