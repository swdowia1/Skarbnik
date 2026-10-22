namespace Skarbnik.VM
{
    public class ExpenseColumnViewModel
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public decimal Kwota { get; set; }
        public decimal Suma { get; set; }
        public decimal SumaNiezaplacona { get; set; }
    }
}
