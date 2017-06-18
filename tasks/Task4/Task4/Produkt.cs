using System;


namespace Task4
{
    /*Interface*/
    public interface Produkt
    {
        /// <summary>
        /// Gets the modell of this item.
        /// </summary>
        string GetDescription { get; }

        /// <summary>
        /// Gets the item's price in the specified currency.
        /// </summary>
        decimal Price { get; }
    }
}
