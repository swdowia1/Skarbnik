namespace Skarbnik.VM
{
    public class PodsumowanieVM
    {
        public List<WydatekKolumnaVM> Wydatki { get; set; } = new();

        public List<UczenPodsumowanieVM> Uczniowie { get; set; } = new();

        public decimal SumaWszystkichWydatkow =>
            Wydatki.Sum(w => w.Suma);

        public decimal SumaWszystkichZaleglosci =>
            Wydatki.Sum(w => w.SumaNiezaplacona);
    }
}
