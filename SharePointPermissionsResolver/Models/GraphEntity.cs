using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace SharePointPermissionsResolver.Models
{
    [DataContract]
    [Serializable]
    public class GraphEntityWrapper
    {
        [DataMember(Name = "value")]
        public List<GraphEntity> Value { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GraphEntity
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "fields")]
        public object? Fields { get; set; }
    }
}

