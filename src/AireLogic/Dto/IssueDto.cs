using System;
using System.Runtime.Serialization;
using AireLogic.Models;

namespace AireLogic.Dto
{
    [DataContract]
    public class IssueDto
    {
        [DataMember(Name = "assignee")]
        public int Assignee { get; set; }

        [DataMember(Name = "short_description")]
        public string ShortDescription { get; set; }

        [DataMember(Name = "long_description")]
        public string LongDescription { get; set; }

        [DataMember(Name = "opened")]
        public DateTime DateTimeOpened { get; set; }

        [DataMember(Name = "closed")]
        public DateTime? DateTimeClosed { get; set; }

        [DataMember(Name = "status")]
        public IssueStatus Status { get; set; }

        [DataMember(Name = "self_link")]
        public string SelfLink { get; set; }

        [DataMember(Name = "id")]
        public int Uuid { get; set; }
    }
}