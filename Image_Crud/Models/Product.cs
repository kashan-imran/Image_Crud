using System;
using System.Collections.Generic;

namespace Image_Crud.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public string Image { get; set; } = null!;
    }
}
