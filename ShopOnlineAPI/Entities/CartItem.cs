using System.ComponentModel.DataAnnotations;

namespace ShopOnlineAPI.Entities
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Qty { get; set; }
    }
}
