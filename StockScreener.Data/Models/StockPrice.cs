﻿using System.ComponentModel.DataAnnotations;

namespace StockScreener.Data
{
    public class StockPrice
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
