using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Citymobil
{
    /// <summary>
    /// Die Mitgliedklasse dient zur Übersicht und Logging der Buchungen und zum Verwalten der Fahrzeuge.
    /// Sie wird im jetzigen Entwicklungsstand des Programms nicht funktionsfähig verwendet und stellt einen Dummy dar. -> Erklärung in Fahrzeugklasse
    /// </summary>
    public class Mitglied
    {
        public Mitglied()
        {

        }

        public Mitglied(string vorname, string nachname)
        {
            this.vorname = vorname;
            this.nachname = nachname;
        }

        public string vorname { get; set; }

        public string nachname { get; set; }
    }
}