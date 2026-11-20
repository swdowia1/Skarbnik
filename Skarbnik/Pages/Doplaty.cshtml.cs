using Microsoft.AspNetCore.Mvc.RazorPages;
using Skarbnik.Services;
using Skarbnik.VM;

namespace Skarbnik.Pages
{
    public class DoplatyModel : PageModel
    {
        private readonly UczenService _uczenService;

        public DoplatyModel(UczenService uczenService)
        {
            _uczenService = uczenService;
        }

        public List<StudentTopUpViewModel> Uczniowie { get; private set; } = new();

        public decimal SumaDoplat => Uczniowie.Sum(uczen => uczen.KwotaDoDoplaty);

        public async Task OnGetAsync()
        {
            var uczniowie = await _uczenService.PobierzWszystkichAsync();
            var niezaplacone = await _uczenService.PobierzNiezaplaconeAsync();

            Uczniowie = uczniowie
                .Select(uczen => new StudentTopUpViewModel
                {
                    UczenId = uczen.Id,
                    Nazwisko = uczen.Nazwisko,
                    KwotaDoDoplaty = niezaplacone
                        .Where(pozycja => pozycja.UczenId == uczen.Id)
                        .Sum(pozycja => pozycja.Kwota)
                })
                .ToList();
        }
    }
}
