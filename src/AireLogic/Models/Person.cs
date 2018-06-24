using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AireLogic.Models
{
    [DataContract]
    public class Person
    {
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        [DataMember(Name = "self_link")]
        public string SelfLink { get; set; }

        [Key]
        [DataMember(Name = "id")]
        public int Uuid { get; set; }
    }
}