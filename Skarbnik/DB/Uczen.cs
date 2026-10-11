namespace Skarbnik.DB
{
    public class Uczen : BaseEntity
    {
        public string Nazwisko { get; set; } = string.Empty;

        public string Skrot { get; set; } = string.Empty;

        public ICollection<UczenWydatek> UczenWydatki { get; set; }
            = new List<UczenWydatek>();
    }
}
