using System.Runtime.Serialization;

namespace AireLogic.Dto
{
    [DataContract]
    public class PersonCreateDto
    {
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }
    }
}