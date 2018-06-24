using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AireLogic.Dto;

namespace AireLogic.Models
{
    [DataContract]
    public class Issue
    {
        [DataMember(Name = "assignee")]
        public int Assignee { get; set; }

        [DataMember(Name = "short_description")]
        public string ShortDescription { get; set; }

        [DataMember(Name = "long_description")]
        public string LongDescription { get; set; }

        [Column(TypeName = "datetime")]
        [DataMember(Name = "opened")]
        public DateTime DateTimeOpened { get; set; }

        [Column(TypeName = "datetime2")]
        [DataMember(Name = "closed")]
        public DateTime? DateTimeClosed { get; set; }

        [DataMember(Name = "status")]
        public IssueStatus Status { get; set; }

        [DataMember(Name = "self_link")]
        public string SelfLink { get; set; }

        [Key]
        [DataMember(Name = "id")]
        public int Uuid { get; set; }
    }
}