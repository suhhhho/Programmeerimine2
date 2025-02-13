namespace KooliProjekt.Data
{
    public class Invoice
    {
        public int Id { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
    }
}
