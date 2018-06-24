using System;
using System.Runtime.Serialization;
using AireLogic.Models;

namespace AireLogic.Dto
{
    [DataContract]
    public class IssueUpdateDto
    {
        [DataMember(Name = "assignee")]
        public int Assignee { get; set; }

        [DataMember(Name = "short_description")]
        public string ShortDescription { get; set; }

        [DataMember(Name = "long_description")]
        public string LongDescription { get; set; }

        [DataMember(Name = "status")]
        public IssueStatus Status { get; set; }
    }
}