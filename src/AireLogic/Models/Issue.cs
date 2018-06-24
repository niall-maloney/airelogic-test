using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AireLogic.Dto;

namespace AireLogic.Models
{
    public class Issue
    {
        public int Assignee { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateTimeOpened { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateTimeClosed { get; set; }

        public IssueStatus Status { get; set; }

        public string SelfLink { get; set; }

        [Key]
        public int Uuid { get; set; }
    }
}