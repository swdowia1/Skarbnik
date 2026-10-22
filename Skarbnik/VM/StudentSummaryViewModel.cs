namespace Skarbnik.VM
{
    public class StudentSummaryViewModel
    {
        public int Id { get; set; }
        public string Nazwisko { get; set; } = string.Empty;

        public List<ExpenseCellViewModel> Wydatki { get; set; } = new();

        public decimal SumaNiezaplacona { get; set; }
    }
}
