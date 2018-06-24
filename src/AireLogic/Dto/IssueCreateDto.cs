using System;
using System.Runtime.Serialization;

namespace AireLogic.Dto
{
    [DataContract]
    public class IssueCreateDto
    {
        [DataMember(Name = "short_description")]
        public string ShortDescription { get; set; }

        [DataMember(Name = "long_description")]
        public string LongDescription { get; set; }
    }
}