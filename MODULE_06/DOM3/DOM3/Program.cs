using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOM3
{
    public class Product : ICloneable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public object Clone()
        {
            return new Product(this.Name, this.Price, this.Quantity);
        }

        public override string ToString()
        {
            return $"{Name} - {Quantity} шт. по цене {Price:C}";
        }
    }

    public class Discount : ICloneable
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public Discount(string description, decimal amount)
        {
            Description = description;
            Amount = amount;
        }

        public object Clone()
        {
            return new Discount(this.Description, this.Amount);
        }

        public override string ToString()
        {
            return $"{Description}: {Amount:C}";
        }
    }

    public class Order : ICloneable
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public decimal DeliveryCost { get; set; }
        public Discount OrderDiscount { get; set; }
        public string PaymentMethod { get; set; }

        public Order(decimal deliveryCost, Discount discount, string paymentMethod)
        {
            DeliveryCost = deliveryCost;
            OrderDiscount = discount;
            PaymentMethod = paymentMethod;
        }

        public object Clone()
        {
            var clonedProducts = new List<Product>();
            foreach (var product in Products)
            {
                clonedProducts.Add((Product)product.Clone());
            }

            return new Order(this.DeliveryCost, (Discount)this.OrderDiscount.Clone(), this.PaymentMethod)
            {
                Products = clonedProducts
            };
        }

        public override string ToString()
        {
            string productList = string.Join("\n", Products);
            return $"Заказ:\nТовары:\n{productList}\nСтоимость доставки: {DeliveryCost:C}\nСкидка: {OrderDiscount}\nМетод оплаты: {PaymentMethod}";
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var discount = new Discount("Осенняя скидка", 10);
            var order = new Order(5, discount, "Кредитная карта");

            order.Products.Add(new Product("Телевизор", 500, 1));
            order.Products.Add(new Product("Кабель HDMI", 10, 2));

            Console.WriteLine("Оригинальный заказ:");
            Console.WriteLine(order);

            var clonedOrder = (Order)order.Clone();
            clonedOrder.Products[0].Quantity = 2;
            clonedOrder.PaymentMethod = "Наличные";

            Console.WriteLine("\nКлонированный заказ (с изменениями):");
            Console.WriteLine(clonedOrder);

            Console.WriteLine("\nОригинальный заказ после клонирования:");
            Console.WriteLine(order);
        }
    }
}