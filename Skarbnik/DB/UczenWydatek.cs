namespace Skarbnik.DB
{
    public class UczenWydatek : BaseEntity
    {
        public int UczenId { get; set; }

        public Uczen Uczen { get; set; } = null!;

        public int WydatekId { get; set; }

        public Wydatek Wydatek { get; set; } = null!;

  

        public bool CzyZaplacono { get; set; } = false;



        public DateTime? DataZaplaty { get; set; }
    }
}