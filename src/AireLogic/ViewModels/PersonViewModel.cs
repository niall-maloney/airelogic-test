using System.ComponentModel.DataAnnotations;

namespace AireLogic.ViewModels
{
    public class PersonViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Uuid { get; set; }
    }
}