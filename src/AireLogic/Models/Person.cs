using System.ComponentModel.DataAnnotations;

namespace AireLogic.Models
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SelfLink { get; set; }

        [Key]
        public int Uuid { get; set; }
    }
}