using System.ComponentModel.DataAnnotations;

namespace StockScreener.Data
{
    public class Stock
    {
        [Key]
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
