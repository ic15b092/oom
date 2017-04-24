using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            Kamera X = new Kamera("Sony STC300IR", 3.0, 800);
            Kamera Z = new Kamera("Geovision BL1300", 1.3, 670);
            Console.WriteLine("Die Bezeichnung der X Kamera ist {0}.", X.Bezeichnung);
            Console.WriteLine("Die Aufloesung der Z Kamera ist {0}MP.", Z.Aufloesung);
            var neuerPreis = X.UpdatePreis(790);
            Console.WriteLine("Der Preis der X Kamera ist EUR{0}.", X.Preis);
        }
    }
}
public class Kamera
{
    /*Private Fields*/
    private string bezeichnung;
    private double aufloesung;
    private decimal preis;

    /*Properties*/
    public string Bezeichnung
    {
        get
        {
            return bezeichnung;
        }
        set
        {
            bezeichnung = value;
        }
    }
    public double Aufloesung
    {
        get
        {
            return aufloesung;
        }
        set
        {
            if (value < 1) throw new Exception("Die Kamera ist keine Megapixel Kamera");
            aufloesung = value;
        }
    }
    public decimal Preis
    {
        get
        {
            return preis;
        }
        set
        {
            if (value < 0) throw new Exception("Preis darf nicht negativ sein");
            preis = value;
        }
    }

    /*Konstruktor*/
    public Kamera(string newBezeichnung, double newAufloesung, double newPreis)
    {
        if (string.Compare(newBezeichnung, bezeichnung) == 0) throw new ArgumentOutOfRangeException("Bezeichnung muss vergeben werden");
        if (newAufloesung < 0) throw new ArgumentOutOfRangeException("Aufloesung kann nicht negativ sein");
        if (newAufloesung < 1) throw new ArgumentOutOfRangeException("Die Kamera ist keine Megapixel Kamera");
        if (newPreis < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
        bezeichnung = newBezeichnung;
        aufloesung = newAufloesung;
    }

    /*Methode*/
    public decimal UpdatePreis(decimal newPreis)
    {
        preis = newPreis;
        return preis;
    }
}