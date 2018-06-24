using System.Runtime.Serialization;

namespace AireLogic.Models
{
    [DataContract]
    public class SuccessResult
    {
        [DataMember(Name = "results")]
        public object[] Results { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}