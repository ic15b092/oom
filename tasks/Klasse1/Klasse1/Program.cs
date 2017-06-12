using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasse1
{
    class Book
    {
        private string title;
        private decimal price;

        public Book(string newTitle, decimal newPrice)
        {
            if (newTitle == null || newTitle.Length == 0)
                throw new ArgumentException("Leerer Titel");
            title = newTitle;
            setPrice(newPrice);
        }
        public Book(string newTitle) : this(newTitle, 0) { }
        public Book() : this() { } 
        public string getTitle() { return title; }
        public decimal getPrice() => price; 
        public void setPrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Negativer Preis");
            price = newPrice;
        }
        public string Title
        {
            get
            {
                return title;
            }
        }
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Negativer Preis");
                price = value;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Book a = new Book("Mein Erstes Buch", 10);
            Book b = a;
            
            Console.WriteLine($"Title:{a.getTitle()} Price:{a.getPrice()}");
        }
    }
}
