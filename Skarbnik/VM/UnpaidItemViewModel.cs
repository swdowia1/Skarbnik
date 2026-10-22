namespace Skarbnik.VM
{
    public class UnpaidItemViewModel
    {
        public int UczenId { get; set; }
        public string Nazwisko { get; set; } = string.Empty;
        public int WydatekId { get; set; }
        public string NazwaWydatku { get; set; } = string.Empty;
        public decimal Kwota { get; set; }
    }
}
