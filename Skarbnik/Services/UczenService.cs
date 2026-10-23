using Microsoft.EntityFrameworkCore;
using Skarbnik.DB;
using Skarbnik.Enums;
using Skarbnik.VM;

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

        public async Task<bool> PrzypiszWydatekAsync(
            int uczenId,
            int wydatekId,
            bool czyZaplacono = false)
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
                CzyZaplacono = czyZaplacono,
                DataZaplaty = czyZaplacono ? DateTime.UtcNow : null
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> PrzypiszWszystkichUczniowAsync(int wydatekId)
        {
            if (!await _context.Wydatki.AnyAsync(wydatek => wydatek.Id == wydatekId))
                return null;

            var przypisaniUczniowieIds = await _context.UczenWydatki
                .Where(przypisanie => przypisanie.WydatekId == wydatekId)
                .Select(przypisanie => przypisanie.UczenId)
                .ToListAsync();
            var przypisaniUczniowie = przypisaniUczniowieIds.ToHashSet();
            var nowePrzypisania = await _context.Uczniowie
                .Where(uczen => !przypisaniUczniowie.Contains(uczen.Id))
                .Select(uczen => new UczenWydatek
                {
                    WydatekId = wydatekId,
                    UczenId = uczen.Id,
                    CzyZaplacono = false
                })
                .ToListAsync();

            if (nowePrzypisania.Count > 0)
            {
                _context.UczenWydatki.AddRange(nowePrzypisania);
                await _context.SaveChangesAsync();
            }

            return nowePrzypisania.Count;
        }

        public async Task<bool> UsunWydatekAsync(int wydatekId)
        {
            var wydatek = await _context.Wydatki
                .FirstOrDefaultAsync(w => w.Id == wydatekId);
            if (wydatek == null)
                return false;

            var przypisania = await _context.UczenWydatki
                .Where(przypisanie => przypisanie.WydatekId == wydatekId)
                .ToListAsync();
            _context.UczenWydatki.RemoveRange(przypisania);
            _context.Wydatki.Remove(wydatek);
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

        public Task<List<Wydatek>> PobierzWszystkieWydatkiAsync()
        {
            return _context.Wydatki.AsNoTracking().OrderBy(w => w.Nazwa).ToListAsync();
        }

        public Task<List<UczenWydatek>> PobierzWszystkiePrzypisaniaAsync()
        {
            return _context.UczenWydatki.AsNoTracking().Include(uw => uw.Wydatek).ToListAsync();
        }

        public Task<Wydatek?> PobierzWydatekAsync(int wydatekId, bool tylkoDoOdczytu = true)
        {
            var query = _context.Wydatki.AsQueryable();
            if (tylkoDoOdczytu)
                query = query.AsNoTracking();

            return query.FirstOrDefaultAsync(wydatek => wydatek.Id == wydatekId);
        }

        public async Task<bool> AktualizujWydatekAsync(int id, string nazwa, decimal kwota)
        {
            var wydatek = await PobierzWydatekAsync(id, tylkoDoOdczytu: false);
            if (wydatek == null)
                return false;

            wydatek.Nazwa = nazwa.Trim();
            wydatek.Kwota = kwota;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Wydatek> DodajWydatekDlaWszystkichAsync(string nazwa, decimal kwota)
        {
            var uczniowieIds = await _context.Uczniowie
                .Select(uczen => uczen.Id)
                .ToListAsync();
            var wydatek = new Wydatek { Nazwa = nazwa.Trim(), Kwota = kwota };
            _context.Wydatki.Add(wydatek);

            foreach (var uczenId in uczniowieIds)
            {
                _context.UczenWydatki.Add(new UczenWydatek
                {
                    UczenId = uczenId,
                    Wydatek = wydatek,
                    CzyZaplacono = false
                });
            }

            await _context.SaveChangesAsync();
            return wydatek;
        }

        public Task<List<UnpaidItemViewModel>> PobierzNiezaplaconeAsync()
        {
            return _context.UczenWydatki
                .AsNoTracking()
                .Where(uw => !uw.CzyZaplacono)
                .OrderBy(uw => uw.Uczen.Nazwisko)
                .ThenBy(uw => uw.Wydatek.Nazwa)
                .ThenBy(uw => uw.UczenId)
                .ThenBy(uw => uw.WydatekId)
                .Select(uw => new UnpaidItemViewModel
                {
                    UczenId = uw.UczenId,
                    Nazwisko = uw.Uczen.Nazwisko,
                    WydatekId = uw.WydatekId,
                    NazwaWydatku = uw.Wydatek.Nazwa,
                    Kwota = uw.Wydatek.Kwota
                })
                .ToListAsync();
        }

        public Task<Uczen?> PobierzUczniaAsync(int uczenId, bool tylkoDoOdczytu = true)
        {
            var query = _context.Uczniowie.AsQueryable();
            if (tylkoDoOdczytu)
                query = query.AsNoTracking();
            return query.FirstOrDefaultAsync(uczen => uczen.Id == uczenId);
        }

        public Task<List<UczenWydatek>> PobierzWydatkiUczniaAsync(int uczenId)
        {
            return _context.UczenWydatki
                .Include(uw => uw.Wydatek)
                .Where(uw => uw.UczenId == uczenId)
                .OrderBy(uw => uw.Wydatek.Nazwa)
                .ToListAsync();
        }

        public async Task<List<IdName>> PobierzNieprzypisaneWydatkiAsync(int uczenId)
        {
            var przypisaneIds = await _context.UczenWydatki
                .Where(uw => uw.UczenId == uczenId)
                .Select(uw => uw.WydatekId)
                .ToListAsync();

            return await _context.Wydatki
                .Where(w => !przypisaneIds.Contains(w.Id))
                .OrderBy(w => w.Nazwa)
                .Select(w => new IdName { Id = w.Id, Name = w.Nazwa })
                .ToListAsync();
        }

        public Task<List<StudentStatusViewModel>> PobierzStatusyUczniowWydatkuAsync(int wydatekId)
        {
            return (
                from u in _context.Uczniowie.AsNoTracking()
                join uw in _context.UczenWydatki.AsNoTracking()
                    .Where(x => x.WydatekId == wydatekId)
                    on u.Id equals uw.UczenId into przypisaniaUcznia
                from przypisanie in przypisaniaUcznia.DefaultIfEmpty()
                orderby u.Id
                select new StudentStatusViewModel
                {
                    UczenId = u.Id,
                    Nazwisko = u.Nazwisko,
                    UczenWydatekId = przypisanie == null ? 0 : przypisanie.Id,
                    StanPlatnosc = przypisanie == null
                        ? StanPlatnosc.Brak
                        : przypisanie.CzyZaplacono
                            ? StanPlatnosc.Zaplacono
                            : StanPlatnosc.DoZaplaty,
                    DataZaplaty = przypisanie == null ? (DateTime?)null : przypisanie.DataZaplaty
                })
                .ToListAsync();
        }

        public Task<UczenWydatek?> PobierzPrzypisanieAsync(int id, bool tylkoDoOdczytu = true)
        {
            var query = _context.UczenWydatki
                .Include(uw => uw.Uczen)
                .Include(uw => uw.Wydatek)
                .AsQueryable();
            if (tylkoDoOdczytu)
                query = query.AsNoTracking();
            return query.FirstOrDefaultAsync(uw => uw.Id == id);
        }

        public Task<UczenWydatek?> PobierzPrzypisanieDlaUczniaIWydatkuAsync(
            int uczenId,
            int wydatekId)
        {
            return _context.UczenWydatki
                .AsNoTracking()
                .FirstOrDefaultAsync(uw => uw.UczenId == uczenId && uw.WydatekId == wydatekId);
        }

        public async Task<bool> UsunPrzypisanieAsync(int id)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(uw => uw.Id == id);
            if (przypisanie == null)
                return false;

            _context.UczenWydatki.Remove(przypisanie);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UstawCzyZaplaconoAsync(int przypisanieId, bool czyZaplacono)
        {
            var przypisanie = await _context.UczenWydatki
                .FirstOrDefaultAsync(uw => uw.Id == przypisanieId);
            if (przypisanie == null)
                return false;

            przypisanie.CzyZaplacono = czyZaplacono;
            przypisanie.DataZaplaty = czyZaplacono ? DateTime.UtcNow : null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
