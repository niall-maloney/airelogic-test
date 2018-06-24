using System;
using System.Runtime.Serialization;
using AireLogic.Models;

namespace AireLogic.ViewModels
{
    [DataContract]
    public class IssueViewModel
    {
        public int Assignee { get; set; }

        public string AssigneeName { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public DateTime DateTimeOpened { get; set; }

        public IssueStatus Status { get; set; }

        public int Uuid { get; set; }
    }
}