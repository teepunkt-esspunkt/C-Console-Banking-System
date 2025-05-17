using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace ProjektArbeit
{
    internal class Transaktion
    {
        //Attribute
        private string transIban;
        private DateTime zeitstempel;
        private string transaktionsart;
        private string beschreibungstext;
        private decimal betrag;

        //Properties
        public string TransIban
        {
            get { return transIban; }
            set { transIban = value; }
        }
        public DateTime Zeitstempel
        {
            get { return zeitstempel; }
            set { zeitstempel = value; }
        }
        public string Transaktionsart
        {
            get { return transaktionsart; }
            set { transaktionsart = value; }
        }
        public string Beschreibungstext
        {
            get { return beschreibungstext; }
            set { beschreibungstext = value; }
        }
        public decimal Betrag
        {
            get { return betrag; }
            set { betrag = value; }
        }

        // Konstruktor
        public Transaktion(string transIban, DateTime zeitstempel, string transaktionsart, string beschreibungstext, decimal betrag)
        {
            TransIban = transIban;
            Zeitstempel = zeitstempel;
            Transaktionsart = transaktionsart;
            Beschreibungstext = beschreibungstext;
            Betrag = betrag;
        }

        // Methode fuer Menupunkt 12
        public static void TransaktionslisteAbsteigendAnzeigen()
        {
            Console.WriteLine("Transaktionsliste absteigend sortiert nach Zeitstempel:");
            Console.WriteLine($"|{"IBAN", 23}|{"Zeitstempel",25}|{"Transaktionsart",15}|{"Beschreibungstext",18}|{"Betrag",8}");

            foreach (var bank in Bank.AlleBanken())
            {
                foreach (var kunde in bank.Kunden)
                {
                    foreach (var konto in kunde.Konten)
                    {
                        foreach (var transaktion in konto.Transaktionen.OrderByDescending(t => t.Zeitstempel))
                        {
                            Console.WriteLine(transaktion.ToStringPlus());
                        }
                    }
                }
            }
        }

        // Transaktionsliste in csv Speichern, loest auch das Speichern der anderen KontoKlassen aus
        public static void TransaktionslisteSpeichernn()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            string standardPfad = Path.Combine(desktop, "Transaktionsliste.csv");
            Console.WriteLine("Bitte Dateipfad angeben. Bei keiner Eingabe wird auf den Desktop gespeichert.");
            string pfadEingabe = Console.ReadLine();
            string dateiPfad = string.IsNullOrEmpty(pfadEingabe) ? standardPfad : pfadEingabe;
            string ordnerPfad = Path.GetDirectoryName(dateiPfad);
            if (!string.IsNullOrEmpty(ordnerPfad) && !Directory.Exists(ordnerPfad))
            {
                Directory.CreateDirectory(ordnerPfad);
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(dateiPfad))
                {
                    writer.WriteLine("IBAN,Zeitstempel,Transaktionsart,Beschreibungstext,Betrag");

                    foreach (var bank in Bank.AlleBanken())
                    {
                        foreach (var kunde in bank.Kunden)
                        {
                            foreach (var konto in kunde.Konten)
                            {
                                foreach (var transaktion in konto.Transaktionen.OrderBy(t => t.Zeitstempel))
                                {
                                    string formatiertesDatum = transaktion.Zeitstempel.ToString("yyyy-MM-ddTHH:mm:sszzz");
                                    writer.WriteLine($"{transaktion.TransIban},{formatiertesDatum},{transaktion.Transaktionsart},{transaktion.Beschreibungstext},{transaktion.Betrag.ToString("N2")}");
                                }
                            }
                        }
                    }
                }
                Konto.KontenSpeichern(ordnerPfad);
                Console.WriteLine($"Transaktionsliste wurde gespeichert in {dateiPfad}");
            }
            catch (Exception)
            {
                Console.WriteLine("Fehler beim Speichern der Transaktionsliste.");
            }
        }

        // Menupunkt 14, loest das importieren der Kontenliste aus
        public static void TransaktionslisteImportieren()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string standardPfad = Path.Combine(desktop, "Transaktionsliste.csv");
            Console.Write("Nichts eingeben um von Desktop zu lesen. Dateipfad: ");
            string pfadEingabe = Console.ReadLine();
            string dateiPfad = string.IsNullOrEmpty(pfadEingabe) ? standardPfad : pfadEingabe;
            string ordnerPfad = Path.GetDirectoryName(dateiPfad);

            Konto.KontenImportieren(ordnerPfad);

            try
            {
                using (StreamReader reader = new StreamReader(dateiPfad))
                {
                    string ersteZeile = reader.ReadLine();
                   
                    while (!reader.EndOfStream)
                    {
                        string zeile = reader.ReadLine();
                        var werte = zeile.Split(',');

                        string iban = werte[0];
                        DateTime zeitstempel = DateTime.ParseExact(werte[1], "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
                        string transaktionsart = werte[2];
                        string beschreibungstext = werte[3];
                        decimal betrag = decimal.Parse(werte[4], CultureInfo.InvariantCulture);

                        Transaktion transaktion = new Transaktion(iban, zeitstempel, transaktionsart, beschreibungstext, betrag);
                        
                        Konto.IbanSuche(iban).Transaktionen.Add(transaktion);
                    }
                    Console.WriteLine("Transaktionsliste wurde erfolgreich importiert");
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Import nicht erfolgreich");
            }
        }
    public override string ToString()
        {
            return $"{TransIban}, {Zeitstempel}, {Transaktionsart}, {Beschreibungstext}, {Betrag}";
        }
        public string ToStringPlus()
        {
            return $"|{TransIban, 23}|{Zeitstempel, 25}|{Transaktionsart, 15}|{Beschreibungstext, 18}|{Betrag.ToString("N2"), 8}";
        }
    }
}
