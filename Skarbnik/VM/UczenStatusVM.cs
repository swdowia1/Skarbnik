using Skarbnik.Enums;

namespace Skarbnik.VM
{
    public class UczenStatusVM
    {
        public int UczenId { get; set; }



        public string Nazwisko { get; set; } = string.Empty;
        public StanPlatnosc StanPlatnosc { get; set; }

        public DateTime? DataZaplaty { get; set; }
    }
}
