using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektArbeit
{

    // Adressen Klasse
    internal class Adresse
    {
        // Eigenschaften
        private string strasse;
        private string hsnr;
        private int plz;
        private string ort;

        // Properties
        public string Strasse
        {
            get { return strasse; }
            set { strasse = value; }
        }
        public string Hsnr
        {
            get { return hsnr; }
            set { hsnr = value; }
        }
        public int Plz
        {
            get { return plz; }
            set { plz = value; }
        }
        public string Ort
        {
            get { return ort; }
            set { ort = value; }
        }

        // Konstruktor
        public Adresse(string strasse, string hsnr, int plz, string ort)
        {
            this.strasse = strasse;
            this.hsnr = hsnr;
            this.plz = plz;
            this.ort = ort;
        }

        public override string ToString()
        {
            return $"{strasse}, {hsnr}, {plz:D5}, {ort}";
        }
        public string ToStringPlus()
        {
            return $"| {strasse, 12} |{hsnr, 4} |{plz:D5}| {ort, 8}";
        }

    }

}
