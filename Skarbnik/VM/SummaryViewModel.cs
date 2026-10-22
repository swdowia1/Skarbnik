namespace Skarbnik.VM
{
    public class SummaryViewModel
    {
        public List<ExpenseColumnViewModel> Wydatki { get; set; } = new();

        public List<StudentSummaryViewModel> Uczniowie { get; set; } = new();

        public decimal SumaWszystkichWydatkow =>
            Wydatki.Sum(w => w.Suma);

        public decimal SumaWszystkichZaleglosci =>
            Wydatki.Sum(w => w.SumaNiezaplacona);
    }
}
