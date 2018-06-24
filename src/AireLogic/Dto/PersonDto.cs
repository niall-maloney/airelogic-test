using System.Runtime.Serialization;
using AireLogic.Dto;

namespace AireLogic.Models
{
    [DataContract]
    public class PersonDto
    {
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        [DataMember(Name = "self_link")]
        public string SelfLink { get; set; }

        [DataMember(Name = "id")]
        public int Uuid { get; set; }
    }
}