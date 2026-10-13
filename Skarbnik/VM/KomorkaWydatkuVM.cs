namespace Skarbnik.VM
{
    public class KomorkaWydatkuVM
    {
        public int Id { get; set; }
        public decimal? Kwota { get; set; }
        public bool CzyZaplacono { get; set; }
        public int UczenId { get; set; }
        public string Nazwa { get; set; }
        public int WydatekId { get; set; }
    }
}
