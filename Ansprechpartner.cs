using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektArbeit
{
    // Ansprechpartner Klasse
    internal class Ansprechpartner
    {
        // Attribute
        private string vorname;
        private string nachname;
        private string telefonnummer;

        // Properties
        public string Vorname
        { get { return vorname; } set {  vorname = value; } }
        public string Nachname
        { get { return nachname; } set { nachname = value; } }
        public string Telefonnummer
        {  get { return telefonnummer; } set {  telefonnummer = value; } }

        // Konstruktor
        public Ansprechpartner(string vorname, string nachname, string telefonnummer)
        {
            Vorname = vorname;
            Nachname = nachname;
            Telefonnummer = telefonnummer;
        }

        public override string ToString()
        {
            return $"{Vorname}, {Nachname}, {Telefonnummer}";
        }
        public string ToStringPlus()
        {
            return $"|{Vorname, 8}|{Nachname, 8}|{Telefonnummer, 8}";
        }
    }
}
