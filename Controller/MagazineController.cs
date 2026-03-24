using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using LibraryApp.Domain;
using LibraryApp.Repository;

namespace LibraryApp.Controller
{
    public class MagazineController
    {
        private readonly IPublicationRepository _magazineRepo;

        public MagazineController(IPublicationRepository magazineRepo) =>
            _magazineRepo = magazineRepo;

        public Magazine CreateMagazine(Magazine magazine)
        {
            if (magazine is null)
                throw new ArgumentNullException($"🚫 {nameof(magazine)} 🚫");
            return (Magazine)_magazineRepo.Create(magazine);
        }
    }
}
