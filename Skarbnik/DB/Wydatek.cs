namespace Skarbnik.DB
{
    public class Wydatek : BaseEntity
    {
        public string Nazwa { get; set; } = string.Empty;

        public decimal Kwota { get; set; }



        public ICollection<UczenWydatek> UczenWydatki { get; set; }
            = new List<UczenWydatek>();
    }
}
