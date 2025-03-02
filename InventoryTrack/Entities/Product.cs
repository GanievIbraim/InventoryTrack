namespace InventoryTrack.Entities
{
    public class Product : BaseEntity
    {
        public decimal? Price { get; set; }
        public int? Count { get; set; }
    }
}
