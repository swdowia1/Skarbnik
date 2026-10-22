using Skarbnik.Enums;

namespace Skarbnik.VM
{
    public class StudentStatusViewModel
    {
        public int UczenId { get; set; }
        public int UczenWydatekId { get; set; }


        public string Nazwisko { get; set; } = string.Empty;
        public StanPlatnosc StanPlatnosc { get; set; }

        public DateTime? DataZaplaty { get; set; }
    }
}
