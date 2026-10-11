namespace Skarbnik.VM
{
    public class UczenPodsumowanieVM
    {
        public int Id { get; set; }
        public string Nazwisko { get; set; } = string.Empty;

        public List<KomorkaWydatkuVM> Wydatki { get; set; } = new();

        public decimal SumaNiezaplacona { get; set; }
    }
}
