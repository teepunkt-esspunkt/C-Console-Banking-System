using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjektArbeit
{
    // Firmenkunde Klasse, erbt von Kunde
    internal class Firmenkunde : Kunde
    {
        // Attribute
        private string name;
        private Ansprechpartner ansprechpartner;

        // Properties
        public string Name
        {
            get { return name; } 
            set { name = value; }
        }
        public Ansprechpartner Ansprechpartner
        {
            get { return ansprechpartner; }
            set { ansprechpartner = value; }
        }
        // Konstruktor
        public Firmenkunde
            (string name, string telefonnummer, string email, 
            Adresse adresse, int anzahlKonten, Bank bank, Ansprechpartner ansprechpartner) 
            : base(telefonnummer, email, adresse, anzahlKonten, bank)
        {
            Name = name;
            Ansprechpartner = ansprechpartner;
        }
        // Methode zum Firmenkunde anlegen
        public static Firmenkunde FirmenkundeAnlegen()
        {
            //Regex Pattern fuer Firmenname (mindestens ein alphbetisches Zeichen, Zahlen sind erlaubt)
            string patternFirma = @"^(?=.*[A-Za-zÖÄÜöäü])[-\s'A-Za-zÖÄÜöäü0-9]{1,}$";
            Regex regexFirma = new Regex(patternFirma);
            //Regex Pattern fuer Ort (wie Name)
            string patternName = @"^[A-Za-zÖÄÜöäü]{2,}(?:[-\s'][A-Za-zÖÄÜöäü]+)?$";
            Regex regexName = new Regex(patternName);
            // Regex Pattern fuer Telefonnummer (ein weiteres Leerzeichen, / oder - wird erlaubt fuer Firmendurchwahlen)
            string patternTeleFirma = @"^[1-9](?:\D*\d){5,}[\/\\-]?\d*[\/\\-]?\d*$";
            Regex regexTeleFirma = new Regex(patternTeleFirma);
            // Regex Pattern fuer EMail Adresse
            string patternMail = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regexMail = new Regex(patternMail);
            //Regex Pattern fuer Strasse (nur 1 Zeichen mehr als name)
            string patternStrasse = @"^[A-Za-zÖÄÜöäü]{3,}(?:[-\s'][A-Za-zÖÄÜöäü]+)?$";
            Regex regexStrasse = new Regex(patternStrasse);
            //Regex Pattern fuer Hausnummer (nur 1 Zeichen mehr als name)
            string patternHsnr = @"^(?:[1-9]\d{0,2}|1\d{3})[ \/-]?[A-Za-z]{0,3}$";
            Regex regexHsnr = new Regex(patternHsnr);

            string name = Pruefen(regexFirma, "Bitte Firmennamen eingeben: ", "Mindestens 1 alphabetisches Zeichen oder Zahl.");
            string telefonnummer = Pruefen(regexTeleFirma, "Bitte Telefonnummer eingeben: (+49) ", "Mindestens 6 Zahlen, Leerzeichen und maximal zwei Leerzeichen, / oder - sind erlaubt.");
            string email = Pruefen(regexMail, "Bitte E-Mail Adresse eingeben: ", "Bitte gueltige E-Mail Adresse eingeben.");
            string strasse = Pruefen(regexStrasse, "Bitte Strasse eingeben: ", "Strasse darf nur aus alphabetischen Zeichen bestehen, und muss mindestens 3 Zeichen lang sein.");
            string hsnr = Pruefen(regexHsnr, "Bitte Hausnummer eingeben: ", "Muss aus einer Zahl gefolgt von 0-3 Buchstaben bestehen. Ein Leerzeichen, ein / oder - stehen ist erlaubt.");
            int plz = IntPruefen("Bitte Postleitzahl eingeben: ", 1067, 99998, "Bitte eine gueltige Postleitzahl zwischen 1067 und 99998 eingeben.");
            string ort = Pruefen(regexName, "Bitte Ort eingeben: ", "Bitte einen gueltigen Ort eingeben. Mindestens 2 alphabetische Zeichen.");
            
            // Ansprechpartner erstellen
            string ansprechpartnerVorname = Pruefen(regexName, "Bitte Vorname des Ansprechpartners eingeben: ", "Vorname darf nur aus alphabetischen und mindestens 2 Zeichen bestehen.");
            string ansprechpartnerNachname = Pruefen(regexName, "Bitte Nachname des Ansprechpartners eingeben: ", "Nachname darf nur aus alphabetischen und mindestens 2 Zeichen bestehen.");
            string ansprechpartnerTelefonnummer = Pruefen(regexTeleFirma, "Bitte Telefonnummer eingeben: (+49) ", "Mindestens 6 Zahlen, Leerzeichen und maximal zwei Leerzeichen, / oder - sind erlaubt.");

            int anzahlKonten = IntPruefen("Bitte Anzahl der gewuenschten Konten eingeben: ", 1, 10, "Bitte zwischen 1 und 10 waehlen.");
            // Rueckgabe des erstellen Firmenkunden
            Firmenkunde fk1 = new Firmenkunde(name, telefonnummer, email, new Adresse(strasse, hsnr, plz, ort), anzahlKonten, Bank.HauptZentrale, new Ansprechpartner(ansprechpartnerVorname, ansprechpartnerNachname, ansprechpartnerTelefonnummer));

            Console.WriteLine(fk1.ToString());
            return fk1;
        }

        // Zum speichern in eine csv Datei, Pfad wird uebernommen (letztes Kettenglied..)
        // genau wie PrivatkundenSpeichern aus der Privatkunde klasse
        public static void FirmenkundenSpeichern(string ordnerPfad)
        {
            string speicherPfad = Path.Combine(ordnerPfad, "Firmenkundenliste.csv");
            try
            {
                using (StreamWriter writer = new StreamWriter(speicherPfad))
                {
                    writer.WriteLine("name,telefonnummer,email,strasse,hausnummer,plz,ort,bank,VornameAnsprechpartner,NachnameAnsprechpartner,TelefonnummerAnsprechpartner");
                    foreach (var bank in Bank.AlleBanken())
                    {
                        foreach (Firmenkunde kunde in bank.Kunden.OfType<Firmenkunde>())
                        {
                            if (kunde is Firmenkunde firmenkunde)
                            {
                                writer.WriteLine($"{firmenkunde.Name},{firmenkunde.Telefonnummer},{firmenkunde.Email},{firmenkunde.Adresse.Strasse},{firmenkunde.Adresse.Hsnr},{firmenkunde.Adresse.Plz},{firmenkunde.Adresse.Ort},{firmenkunde.Bank.ToString()},{firmenkunde.Konten.Count},{firmenkunde.Bank},{firmenkunde.Ansprechpartner.Vorname},{firmenkunde.Ansprechpartner.Nachname},{firmenkunde.Ansprechpartner.Telefonnummer}");
                            }
                        }
                    }
                }
                Console.WriteLine($"Firmenkundenliste wurde gespeichert in {speicherPfad}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim speichern der Firmenkunden.{ex.Message}");
            }
        }
        // Firmenkunden aus csv importieren, wird von KontenImportieren ausgeloest
        public static void FirmenkundenImportieren(string ordnerPfad)
        {
            string standardPfad = Path.Combine(ordnerPfad, "Firmenkundenliste.csv");
            try
            {
                using (StreamReader reader = new StreamReader(standardPfad))
                {
                    string ersteZeile = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string zeile = reader.ReadLine();
                        var werte = zeile.Split(',');
                        string name = werte[0];
                        string telefonnummer = werte[1];
                        string email = werte[2];
                        string strasse = werte[3];
                        string hsnr = werte[4];
                        int plz = int.Parse(werte[5]);
                        string ort = werte[6];
                        string bankName = werte[7];
                        string vorname = werte[8];
                        string nachname = werte[9];
                        string telefonAp = werte[10];
                      //Finden der Bank durch den gespeicherten Namen
                        Bank bank = Bank.AlleBanken().FirstOrDefault(b => b.Name == bankName);
                      
                        Firmenkunde fk = new Firmenkunde(name, telefonnummer, email, new Adresse(strasse, hsnr, plz, ort), 0, bank, new Ansprechpartner(vorname, nachname, telefonAp));
                        Console.WriteLine($"Firmenkunde importiert mit der Kundennummer: {fk.Kundennummer}");

                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Fehler beim Laden der Firmenkunden");
            }
        }
        public override string ToString()
        {
            return $"{Name}, {base.Kundennummer}, {base.Telefonnummer}, {base.Email}, {base.Adresse}, {base.Konten.Count}, {Ansprechpartner}";
        }
        public override string ToStringPlus()
        {
            return $"|{base.Kundennummer, 3}|{Name, 8}|{base.Telefonnummer, 10}|{base.Email, 12}{base.Adresse.ToStringPlus()}|{base.Konten.Count, 3}{Ansprechpartner.ToStringPlus()}";
        }
    }
}
