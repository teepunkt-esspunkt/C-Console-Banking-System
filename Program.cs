namespace ProjektArbeit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                // Anzeigen der Auswahlmoeglichkeiten
                MenuAufruf();
                //Auswahl treffen
                MenuWahl();
            }
        }
        // Anzeige der Auswahlmoeglichkeiten, Hauptmenu
        internal static void MenuAufruf()
        {
            Console.WriteLine("");
            Console.WriteLine("(01) Privatkunde anlegen");
            Console.WriteLine("(02) Firmenkunde anlegen");
            Console.WriteLine("(03) Konto anlegen und Kundennummer zuordnen");
            Console.WriteLine("(04) Kunde mit Konten anzeigen (Auswahl durch Kundennummer)");
            Console.WriteLine("(05) Kunde mit Konten anzeigen (Auswahl durch Namen)");
            Console.WriteLine("(06) Konto anzeigen (Auswahl durch IBAN)");
            Console.WriteLine("(07) Alle Kunden unsortiert anzeigen");
            Console.WriteLine("(08) Alle Kunden sortiert nach aufsteigender Kundenummer anzeigen");
            Console.WriteLine("(09) Alle Konten unsortiert anzeigen");
            Console.WriteLine("(10) Geld auf Konto einzahlen");
            Console.WriteLine("(11) Geld von Konto abheben");
            Console.WriteLine("(12) Transaktionsliste absteigend sortiert nach Zeitstempel anzeigen");
            Console.WriteLine("(13) Transaktionsliste aufsteigend sortiert nach Zeitstempel speichern");
            Console.WriteLine("(14) Transaktionsliste einlesen");
            Console.WriteLine("(15) Beenden");
            Console.WriteLine("");
        }
        // Auswahl treffen im Hauptmenu
        internal static void MenuWahl()
        {
            int menueWahl;
            try
            {
                menueWahl = int.Parse(Console.ReadLine());

                switch (menueWahl)
                {
                    case 1:
                        Privatkunde.PrivatkundeAnlegen();
                        break;
                    case 2:
                        Firmenkunde.FirmenkundeAnlegen();
                        break;
                    case 3:
                        Konto.KontoAnlegen();
                        break;
                    case 4:
                        Kunde.KundenMitKontoAnzeigenAuswahl();
                        break;
                    case 5:
                        Kunde.KundenMitKontoAnzeigenAuswahl();
                        break;
                    case 6:
                        Konto.IbanSucheAnzeige();
                        break;
                    case 7:
                        Kunde.AlleKundenAnzeigen();
                        break;
                    case 8:
                        Kunde.AlleKundenAnzeigenSortieren();
                        break;
                    case 9:
                        Konto.AlleKonten();
                        break;
                    case 10:
                        Konto.EinzahlenAuswahl();
                        break;
                    case 11:
                        Konto.AuszahlenAuswahl();
                        break;
                    case 12:
                        Transaktion.TransaktionslisteAbsteigendAnzeigen();
                        break;
                    case 13:
                        Transaktion.TransaktionslisteSpeichernn();
                        break;
                    case 14:
                        Transaktion.TransaktionslisteImportieren();
                        break;
                    case 15:
                        Environment.Exit(0);
                        break;
                    default:
                        throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Ungueltige Auswahl! Bitte 1 - 15 waehlen");
            }
        }
    }
}