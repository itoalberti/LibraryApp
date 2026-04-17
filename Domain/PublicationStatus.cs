using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Domain
{
    public enum PublicationStatus
    {
        Available = 1,
        Reserved = 2,
        Borrowed = 3,

        [Display(Name = "In Renovation")]
        InRenovation = 4,
    }
}
