namespace Lab4.Models.ViewModels
{
    public class AdsViewModel : Advertisement
    {
        public Brokerage Brokerage { get; set; }
        public IEnumerable<Advertisement> Advertisements { get; set; }

    }
}
