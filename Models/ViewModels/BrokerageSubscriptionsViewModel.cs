namespace Lab4.Models.ViewModels
{
    public class BrokerageSubscriptionsViewModel : BrokeragesViewModel
    {
        public string Title { get; set; }
        public bool IsMember { get; set; }
        public int ClientId { get; set; }
        public string BrokerageId { get; set; }

        public Client Client { get; set; }
        public Brokerage Brokerage { get; set; }
    }
}
