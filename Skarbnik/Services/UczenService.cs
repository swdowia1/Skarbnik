using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;

namespace Skarbnik.Services
{
    public class UczenService
    {
        private readonly AppDbContext _context;

        public UczenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Uczen>> PobierzWszystkichAsync()
        {
            return await _context.Uczniowie
                .AsNoTracking()
                .OrderBy(uczen => uczen.Nazwisko)
                .ToListAsync();
        }

        public async Task<Uczen?> PobierzPoIdAsync(int uczenId)
        {
            return await _context.Uczniowie
                .AsNoTracking()
                .FirstOrDefaultAsync(uczen => uczen.Id == uczenId);
        }

        public async Task<bool> AktualizujAsync(Uczen daneUcznia)
        {
            var uczen = await _context.Uczniowie
                .FirstOrDefaultAsync(uczen => uczen.Id == daneUcznia.Id);

            if (uczen == null)
                return false;

            uczen.Nazwisko = daneUcznia.Nazwisko;
            uczen.Skrot = daneUcznia.Skrot;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Wydatek> DodajWydatekAsync(string nazwa, decimal kwota)
        {
            var wydatek = new Wydatek
            {
                Nazwa = nazwa.Trim(),
                Kwota = kwota
            };

            _context.Wydatki.Add(wydatek);
            await _context.SaveChangesAsync();

            return wydatek;
        }

        public async Task<bool> PrzypiszWydatekAsync(int uczenId, int wydatekId)
        {
            var uczenIstnieje = await _context.Uczniowie
                .AnyAsync(uczen => uczen.Id == uczenId);
            var wydatekIstnieje = await _context.Wydatki
                .AnyAsync(wydatek => wydatek.Id == wydatekId);

            if (!uczenIstnieje || !wydatekIstnieje)
                return false;

            var przypisanieIstnieje = await _context.UczenWydatki
                .AnyAsync(przypisanie =>
                    przypisanie.UczenId == uczenId &&
                    przypisanie.WydatekId == wydatekId);

            if (przypisanieIstnieje)
                return false;

            _context.UczenWydatki.Add(new UczenWydatek
            {
                UczenId = uczenId,
                WydatekId = wydatekId,
                CzyZaplacono = false,
                DataZaplaty = null
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UstawCzyZaplaconoAsync(
            int uczenId,
            int wydatekId,
            bool czyZaplacono)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(przypisanie =>
                    przypisanie.UczenId == uczenId &&
                    przypisanie.WydatekId == wydatekId);

            if (przypisanie == null)
                return false;

            przypisanie.CzyZaplacono = czyZaplacono;
            przypisanie.DataZaplaty = czyZaplacono
                ? DateTime.UtcNow
                : null;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
