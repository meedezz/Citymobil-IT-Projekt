using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Citymobil
{
    /// <summary>
    /// Die Fahrzeugklasse dient ausschließlich als Mapping/Schema für die Datenbank.
    /// Eine Erstellung von Objekten ist nur für die Insertation in die Datenbank vorgergesehen. -> keine Methoden, kein Konstruktor
    /// Die in SQLite.Net enthaltenen Attributoptionen setzen PK, Länge und andere Schemata fest.
    /// 
    /// Die Vereinigung von PKW und Transporter innerhalb einer Klasse und die daraus resultierende Unterscheidung bei jedem Vorgang war wegen des
    /// Datenbankmodells notwendig. Eine Generalisierung wie auf Entwurfs-Diagrammen konnte deswegen nicht stattfinden. So hat sich das Programm jedoch vereinfacht,
    /// da an bestimmten punkten im Programm trotzdem Unterscheidungen hätten vorgenommen werden müssen.
    /// </summary>
    public class Fahrzeug
    {
        // Basics
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(64)]
        public string kategorie { get; set; }

        public string modell { get; set; }

        public string hersteller { get; set; }

        public string kennzeichen { get; set; }

        public int zustand { get; set; }

        // PKW
        public int kindersitze { get; set; }

        public int automatik { get; set; }

        // Transporter
        public double nutzlast { get; set; }

        public double nutzvolumen { get; set; }
        
        // Dates -> extra DB
        public DateTime datetimeVon { get; set; }
        
        public DateTime datetimeBis { get; set; }

        // Mitglied -> extra DB
        //public Mitglied mitglied { get; set; }

        /*
         *  Von Vorteil wäre es, extra datenbanken für Buchungen und Mitglieder anzulegen. Dies würde in diesem Umfang des Programms,
         *  der hier notwendig ist, jedoch nicht von Bedeutung sein, zumal dies weder angezeigt, noch verwertet wird.
         *  Mitglied wird deswegen ausgeklammert, da hierfür ein Fremdschlüssel notwendig wäre. Übergeben wird ein Mitglied trotzdem.
         *  Lediglich für Logs und Übersichten wäre ein solcher Ausbau notwendig. Hierfür müssten kaum Änderungen übernommen werden und lediglich
         *  die Klasse "Database.cs" als Template für die verschiedenen Klassen umgeändert werden.
         */
    }
}