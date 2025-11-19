
namespace DienMayLongQuyen.Api.Models
{
    public class ProductAttributeOption
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int AttributeOptionId { get; set; }
        public AttributeOption AttributeOption { get; set; }
    }


}