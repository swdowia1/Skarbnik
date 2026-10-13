namespace Skarbnik.VM
{
    public class UczenStatusVM
    {
        public int UczenId { get; set; }



        public string Nazwisko { get; set; } = string.Empty;

        public bool CzyPrzydzielono { get; set; }

        public int? PrzypisanieId { get; set; }

        public bool CzyZaplacono { get; set; }

        public DateTime? DataZaplaty { get; set; }
    }
}
