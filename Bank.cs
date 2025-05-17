using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektArbeit
{
    // Bank Klasse
    internal class Bank
    {
        // Liste aller Banken, hier weitere Zweigstellen eintragen 
        internal static IEnumerable<Bank> AlleBanken()
        {
            List<Bank> banken = new List<Bank>
            {
                Bank.HauptZentrale
            };
            return banken;
        }

        // Attribute
        private string name;
        private string bic;
        private Adresse adresse;
        private List<Kunde> kunden;

        // Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Bic
        {
            get { return bic; }
            set { bic = value; }
        }
        public Adresse Adresse
        {
            get { return adresse; }
            set { adresse = value; }
        }
        public List<Kunde> Kunden
        {
            get { return kunden; }
            set { kunden = value; }
        }

        // Hauptzentrale
        public static Bank HauptZentrale { get; } = new Bank("Hauptzentrale", "PAYMOREXX", "Wucherweg", "2b", 30457, "Maushausen");

        // Konstruktor
        public Bank(string name, string bic, string strasse, string hsnr, int plz, string ort)
        {
            this.name = name;
            this.bic = bic;
            this.adresse = new Adresse(strasse, hsnr, plz, ort);
            this.kunden = new List<Kunde>();
        }

        public void KundenHinzufuegen(Kunde kunde)
        {
            kunden.Add(kunde);
        }

        // Zum Speichern in die CSV Datei
        public override string ToString()
        {
            return Name;
        }
    }
}
