namespace Skarbnik.VM
{
    public class WydatekKolumnaVM
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;

        public decimal Suma { get; set; }
        public decimal SumaNiezaplacona { get; set; }
    }
}
