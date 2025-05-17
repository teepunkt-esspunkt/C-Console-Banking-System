using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjektArbeit
{
    // Privatkunden Klasse, erbt von Kunde
    internal class Privatkunde : Kunde
    {
        // Attribute
        private string vorname;
        private string nachname;
        private DateTime geburtsdatum;

        // Mindestalter für Kontoerstellung
        private static readonly int Mindestalter = 18;

        // Properties
        public string Vorname
        {
            get { return vorname; }
            set { vorname = value; }
        }
        public string Nachname
        {
            get { return nachname; }
            set { nachname = value; }
        }
        public DateTime Geburtsdatum
        {
            get { return geburtsdatum; }
        }

        // Konstruktor
        public Privatkunde
            (string vorname, string nachname, DateTime geburtsdatum, 
            string telefonnummer, string email, Adresse adresse, 
            int anzahlKonten, Bank bank) 
            : base(telefonnummer, email, adresse, anzahlKonten, bank)
        {
            Vorname = vorname;
            Nachname = nachname;
            this.geburtsdatum = geburtsdatum;
            
        }

        
        // Methode zum Privatkunden anlegen
        public static Privatkunde PrivatkundeAnlegen()
        {
            // Regexe's
            //Regex Pattern fuer Vor- und Nachname
            string patternName = @"^[A-Za-zÖÄÜöäü]{2,}(?:[-\s'][A-Za-zÖÄÜöäü]+)?$";
            Regex regexName = new Regex(patternName);
            // Regex Pattern fuer Telefonnummer
            string patternTelePriv = @"^[1-9](?:\D*\d){5,}[\/\\-]?\d*$";
            Regex regexTelePriv = new Regex(patternTelePriv);
            // Regex Pattern fuer EMail Adresse
            string patternMail = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regexMail = new Regex(patternMail);
            //Regex Pattern fuer Strasse (nur 1 Zeichen mehr als name)
            string patternStrasse = @"^[A-Za-zÖÄÜöäü]{3,}(?:[-\s'][A-Za-zÖÄÜöäü]+)*$";
            Regex regexStrasse = new Regex(patternStrasse);
            //Regex Pattern fuer Hausnummer (nur 1 Zeichen mehr als name)
            string patternHsnr = @"^(?:[1-9]\d{0,2}|1\d{3})[ \/-]?[A-Za-z]{0,3}$";
            Regex regexHsnr = new Regex(patternHsnr);

            // Usereingaben ueberpruefen
            string vorname = Pruefen(regexName, "Bitte Vornamen eingeben: ", "Vorname darf nur aus alphabetischen und mindestens 2 Zeichen bestehen.");
            string nachname = Pruefen(regexName, "Bitte Nachnamen eingeben: ", "Nachname darf nur aus alphabetischen und mindestens 2 Zeichen bestehen.");
            DateTime geburtsdatum = GeburtsdatumPruefen("Bitte Geburtsdatum z.B. im Format \"JJJJ.MM.TT\" oder \"TT.MM.JJ\" eingeben: "); // Hier sind die Fehlermeldungen in der Methode, da es verschiedene sein können
            string telefonnummer = Pruefen(regexTelePriv, "Bitte Telefonnummer eingeben: (+49) ", "Mindestens 6 Zahlen, Leerzeichen und maximal ein / oder ein - sind erlaubt.");
            string email = Pruefen(regexMail, "Bitte E-Mail Adresse eingeben: ", "Bitte gueltige E-Mail Adresse eingeben.");
            string strasse = Pruefen(regexStrasse, "Bitte Strasse eingeben: ", "Strasse darf nur aus alphabetischen Zeichen bestehen, und muss mindestens 3 Zeichen lang sein.");
            string hsnr = Pruefen(regexHsnr, "Bitte Hausnummer eingeben: ", "Muss aus einer Zahl gefolgt von 0-3 Buchstaben bestehen. Ein Leerzeichen, ein / oder - stehen ist erlaubt.");
            int plz = IntPruefen("Bitte Postleitzahl eingeben: ", 1067, 99998, "Bitte eine gueltige Postleitzahl zwischen 1067 und 99998 eingeben.");
            string ort = Pruefen(regexName, "Bitte Ort eingeben: ", "Bitte einen gueltigen Ort eingeben. Mindestens 2 alphabetische Zeichen.");
            int anzahlKonten = IntPruefen("Bitte Anzahl der gewuenschten Konten eingeben: ", 1, 10, "Bitte zwischen 1 und 10 waehlen."); 
            // Rueckgabe des erstellten Privatkundenobjekts
            Privatkunde pk1 = new Privatkunde(vorname, nachname, geburtsdatum, telefonnummer, email, new Adresse(strasse, hsnr, plz, ort), anzahlKonten, Bank.HauptZentrale);
            Console.WriteLine(pk1.ToStringPlus());

            return pk1;
        }

        // Ueberpruefung fuer das Geburtsdatum, Datum darf nicht in der Zukunft liegen und Mindestalter muss eingehalten werden
        public static DateTime GeburtsdatumPruefen(string aufforderung)
        {
            while (true)
            {
                Console.Write(aufforderung);
                try
                {
                    DateTime eingabeDatum = DateTime.Parse(Console.ReadLine());
                    // Pruefen ob Geburtsdatum in der Zukunft liegt und nicht ueber 100 Jahre in der Vergangenheit liegt
                    // Format wird automatisch uebernommen, das heist auch 12.12.99 waehre gueltig
                    if (DateTime.Now > eingabeDatum && (DateTime.Now.Year - eingabeDatum.Year) < 100)
                    {
                        // Alter festlegen
                        int alter = DateTime.Now.Year - eingabeDatum.Year;
                        // Ein Jahr abziehen falls der Geburtstag dieses Jahr erst noch kommt
                        if (DateTime.Now < eingabeDatum.AddYears(alter))
                            alter--;
                        // Altersueberpruefung
                        if (alter >= Mindestalter)
                        {
                            return eingabeDatum;
                        }
                        else
                        {
                            throw new ArgumentException($"Mindestalter: {Mindestalter}");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Datum darf nicht in der Zukunft liegen und nicht ueber 100 Jahre in der Vergangenheit");
                    }
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Ungueltige Eingabe");
                }
            }
        }
        // Speichern in eine CSV Datei, Pfad wird uebernommen (letztes Kettenglied..)
        public static void PrivatkundenSpeichern(string ordnerPfad)
        {
            string speicherPfad = Path.Combine(ordnerPfad, "Privatkundenliste.csv");
            try
            {
                using (StreamWriter writer = new StreamWriter(speicherPfad))
                {
                    //erste Zeile der csv Datei
                    writer.WriteLine("vorname,nachname,geburtsdatum,telefonnummer,email,strasse,hsnr,plz,ort,bank");
                    foreach (var bank in Bank.AlleBanken())
                    {
                        // nur die Privatkunden
                        foreach (Privatkunde kunde in bank.Kunden.OfType<Privatkunde>())
                        {
                            // Datum formatieren
                            string formatiertesDatum = kunde.Geburtsdatum.ToString("yyyy-MM-dd");
                            //schreiben in die csv datei
                            writer.WriteLine($"{kunde.Vorname},{kunde.Nachname},{formatiertesDatum},{kunde.Telefonnummer},{kunde.Email},{kunde.Adresse.Strasse},{kunde.Adresse.Hsnr},{kunde.Adresse.Plz},{kunde.Adresse.Ort},{kunde.Bank.ToString()}");
                        }
                    }
                }
                Console.WriteLine($"Privatkundenliste wurde gespeichert in {speicherPfad}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim speichern der Privatkunden.{ex.Message}");
            }
        }

        // Privatkunden aus csv importieren, wird von KontenImportieren ausgeloest
        public static void PrivatkundenImportieren(string ordnerPfad)
        {
            string standardPfad = Path.Combine(ordnerPfad, "Privatkundenliste.csv");
            try
            {
                using(StreamReader reader = new StreamReader(standardPfad))
                {
                    string ersteZeile = reader.ReadLine();
                    while(!reader.EndOfStream)
                    {
                        string zeile = reader.ReadLine();
                        var werte = zeile.Split(',');
                        string vorname = werte[0];
                        string nachname = werte[1];
                        DateTime geburtsdatum = DateTime.ParseExact(werte[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        string telefonnummer = werte[3];
                        string email = werte[4];
                        string strasse = werte[5];
                        string hsnr = werte[6];
                        int plz = int.Parse(werte[7]);
                        string ort = werte[8];
                        string bankName = werte[9];
                        Bank bank = Bank.AlleBanken().FirstOrDefault(b => b.Name == bankName);
                        Privatkunde pk = new Privatkunde(vorname, nachname, geburtsdatum, telefonnummer, email, new Adresse(strasse, hsnr, plz, ort), 0, bank);
                        //bank.Kunden.Add(pk);
                        Console.WriteLine($"Privatkunde importiert mit der Kundennummer: {pk.Kundennummer}");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Fehler beim Laden der Privatkunden");
            }
        }
        public override string ToString()
        {
            return $"{Vorname}, {Nachname}, {Geburtsdatum}, {base.Kundennummer}, {base.Telefonnummer}, {base.Email}, {base.Adresse}, {base.Konten.Count}";
        }
        public override string ToStringPlus()
        {
            return $"|{base.Kundennummer, 3}|{Vorname, 8}|{Nachname, 12}|{Geburtsdatum.ToString("yyyy.MM.dd"), 15}|{base.Telefonnummer, 12}|{base.Email, 10}{base.Adresse.ToStringPlus()}|{base.Konten.Count, 3}|";
        }
    }
  
}
