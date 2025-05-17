using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjektArbeit
{
    // Kundenklasse
    internal abstract class Kunde
    {
        // Attribute
        private readonly int maxKonten = 10;
        private int kundennummer = 0;
        private static int neueKundennummer = 1;
        private string telefonnummer;
        private string email;
        private Adresse adresse;
        private List<Konto> konten;
        // Um BIC fuer die Kontoklasse zur Generierung der IBAN zugaenglich zu machen
        private Bank bank;

        // Properties
        public int Kundennummer
        {
            get { return kundennummer; }
        }
        public string Telefonnummer
        {
            get { return telefonnummer; }
            set { telefonnummer = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public Adresse Adresse
        {
            get { return adresse; }
            set { adresse = value; }
        }
        public List<Konto> Konten
        {
            get { return konten; } 
            set { konten = value; }
        }
        public Bank Bank
        { 
            get { return bank; }
            set { bank = value; }
        }
        public int MaxKonten
        {
            get { return maxKonten; }
        }
        

        // Konstruktor
        public Kunde(string telefonnummer, string email, Adresse adresse, int anzahlKonten, Bank bank = null)
        {
            this.kundennummer = neueKundennummer++;
            Telefonnummer = telefonnummer;
            Email = email;
            Adresse = adresse;
            // Konten erstellen
            Konten = new List<Konto>();
            if (anzahlKonten > 0)
            {
                for (int i = 0; i < anzahlKonten; i++)
                {
                    Konto.KontoAnlegen(this);
                }
            }
            // Wenn keine Zweigstelle eingegeben wurde soll der Hauptsitz genommen werden
            Bank = bank ?? Bank.HauptZentrale;
            // Kunde zur Liste der Bank hinzufuegen
            Bank.Kunden.Add(this);
        }


        // Methode fuer Menupunkt 4 und 5
        public static void KundenMitKontoAnzeigenAuswahl()
        {
           
            Console.Write("Bitte Namen oder Kundennummer eingeben: ");
            string eingabe = Console.ReadLine();
            if(int.TryParse(eingabe, out int kundennummer))
            {
                // Wenn die Eingabe eine Zahl war, nach Kundennummer suche
                KundenMitKontoAnzeigen(kundennummer);
            }
            else if(!string.IsNullOrEmpty(eingabe))
            {
                // Wenn die Eingabe ein String ist, nach namen suchen
                KundenMitKontoAnzeigen(eingabe);
            }
           else
            {
                Console.WriteLine("Ungueltige Eingabe.");
            }
            
        }
        // Methode zum anzeigen eines Kunden anhand der Kundennummer
        public static void KundenMitKontoAnzeigen(int kundennummer)
        {
            
            Kunde kunde = KundennummerSuche(kundennummer);
            if (kunde != null && kunde.Konten.Any())
            {
                
                Console.WriteLine($"Kunde mit der Kundennummer {kunde.Kundennummer} hat folgende Konten:");
                foreach (var konto in kunde.Konten)
                {
                    Console.WriteLine(konto.ToStringPlus());
                }
            }
            else
            {
                Console.WriteLine("Kunde nicht gefunden.");
            }
        }
        // Kundennummer Suche fuer das Hinzufuegen weiterer Konten zu einem bereits existierenden Kunden
        public static Kunde KundennummerSuche(int kundennummer)
        {
            foreach (var bank in Bank.AlleBanken())
            {
                Kunde treffer = bank.Kunden.FirstOrDefault(k => k.Kundennummer == kundennummer);
                if (treffer != null)
                {
                    return treffer;
                }
            }
            return null;
        }

        // Anzeige aller Konten eines Kunden
        public static void KundenMitKontoAnzeigen(string name)
        {
            List<Kunde> kundenListe = KundenNamenSuche(name);
            if(kundenListe.Count > 0)
            {
                foreach (var kunde in kundenListe)
                {
                    Console.WriteLine($"Kunde mit der Kundennummer {kunde.Kundennummer} hat folgende Konten:");
                    foreach (var konto in kunde.Konten)
                    {
                        Console.WriteLine(konto);
                    }
                }
            }
            else
            {
                Console.WriteLine("Kunde nicht gefunden oder keine Konten.");
            }
        }
        
        // Namenssuche
        public static List<Kunde> KundenNamenSuche(string name)
        {
            List<Kunde> trefferKunden = new List<Kunde>();

            foreach (var bank in Bank.AlleBanken())
            {
                // in Privatkunden suchen
                var privatkundenTreffer = bank.Kunden
                    .OfType<Privatkunde>()
                    .Where(k =>
                        k.Vorname.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                        k.Nachname.Contains(name, StringComparison.OrdinalIgnoreCase));

                trefferKunden.AddRange(privatkundenTreffer);

                // in Firmenkunden suchen
                var firmenkundenTreffer = bank.Kunden
                    .OfType<Firmenkunde>()
                    .Where(k =>
                        k.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

                trefferKunden.AddRange(firmenkundenTreffer);
            }
            return trefferKunden;
        }
        // Methode fuer Menupunkt 7
        public static void AlleKundenAnzeigen()
        {
            foreach (var bank in Bank.AlleBanken())
            {
                foreach (var kunde in bank.Kunden)
                {
                    Console.WriteLine(kunde.ToStringPlus());
                }
            }
        }
        // Methode fuer Menupunkt 8
        public static void AlleKundenAnzeigenSortieren()
        {
            List<Kunde> alleKunden = new List<Kunde>();
            int auswahl = 0;
            foreach (var bank in Bank.AlleBanken())
            {
                alleKunden.AddRange(bank.Kunden);
            }
            Console.WriteLine("Bitte Sortierung Auswaehlen. 1 fuer Kundennummer, 2 fuer Telefonnummer...");

            try
            {
                auswahl = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Ungueltige Auswahl");
            }
            switch (auswahl)
            {
                case 1:
                    alleKunden = alleKunden.OrderBy(k => k.Kundennummer).ToList();
                    break;
                case 2:
                    alleKunden = alleKunden.OrderBy(k => k.Telefonnummer).ToList();
                    break;
                default:
                    Console.WriteLine("Ungueltige Auswahl");
                    alleKunden = alleKunden.OrderBy(k => k.Kundennummer).ToList();
                    break;
            }            
            foreach (var kunde in alleKunden)
            {
                Console.WriteLine(kunde.ToStringPlus());
            }
        }
        
        // Methode fuer Ueberpruefugen (ausser fuer Geburtstag, PLZ, und Anzahl der Konten)
        public static string Pruefen(Regex regex, string aufforderung, string fehlermeldung)
        {
            while (true)
            {
                Console.Write(aufforderung);
                try
                {
                    string eingabe = Console.ReadLine();
                    if (regex.IsMatch(eingabe))
                    {
                        return eingabe;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(fehlermeldung);
                }
            }
        }
        // Pruefung der int variablen, Postleitzahl und Kontozanzahl
        public static int IntPruefen(string aufforderung, int min, int max, string fehlermeldung)
        {
            while (true)
            {
                Console.Write(aufforderung);
                try
                {
                    int eingabe = int.Parse(Console.ReadLine());

                    if (eingabe >= min && eingabe <= max)
                    {
                        return eingabe;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(fehlermeldung);
                }
            }
        }
        
        public virtual string ToStringPlus()
        {
            return "";
        }
    }
}
