using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;
using Windows.Storage;
using Windows.ApplicationModel;

namespace Citymobil
{
    /// <summary>
    /// Diese Datenbankklasse beherrscht die grundlegenden SQL-Queries, basierend auf SQLite.NET-PCL
    /// und archivirt die für das Programm notwendigen Fahrzeuge.
    /// Umfang (TODO):
    ///     - DB erstellen
    ///     - Alle Einträge ausgeben (Liste)
    ///     - Löschene ines einzelnen Fahrzeugs
    ///     - Ein Fahrzeug ändern
    ///     - Ein Fahrzeug hinzufügen
    ///     - (Tracing erweitern für Tests/Fehlermeldungen)
    /// </summary>
    internal static class Database
    {
        // Speicherort der Datenbank-Datei
        private static string dbPath = string.Empty;
        private static string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(dbPath))
                {
                    dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite"); // Pfad = Projektmappe/Projekt/
                }

                return dbPath;
            }
        }

        
        private static SQLiteConnection DbConnection
        {
            get
            {
                // Verbindung aufbauen
                return new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            }
        }

        public static void CreateDatabase()
        {
            // Neue Verbindung
            using (var db = DbConnection)
            {
                // Tracing
                db.TraceListener = new DebugTraceListener();

                // Tabelle erstellen, basierend auf der Fahrzeugklasse, wennd iese noch nicht erstellt wurde.                
                var c = db.CreateTable<Fahrzeug>();
                var info = db.GetMapping(typeof(Fahrzeug));

                // Einträge erstellen
                // Vorsicht: Einträge werden zu Beginn des Programmaufrufs mit den hier festgelegten Werten überschrieben.
                // Eine Funktion zum manuellen hinzufügen von daten innerhalb des Programms wären notwendig, umd Daten beizubehalten.
                // -> kein Problem -> Nutzeroberfläche erstellen -> sprengt Umfang des Programms -> Schlecht für die Vorführung
                // Der erste Eintrag muss mit der Id=1 starten, da für das Hinzufügen/Ändern eines Eintrags die Id=0 benötigt wird. s.u.

                Fahrzeug fahrzeug = new Fahrzeug();
                fahrzeug.Id = 8;
                fahrzeug.kategorie = "pkw";
                fahrzeug.hersteller = "Toyota";
                fahrzeug.modell = "Yaris";
                fahrzeug.kennzeichen = "SIG-XS-101";
                fahrzeug.zustand = 0;
                fahrzeug.kindersitze = 1;
                fahrzeug.automatik = 1;
                fahrzeug.nutzlast = 0;
                fahrzeug.nutzvolumen = 0;
                fahrzeug.datetimeVon = new DateTime();
                fahrzeug.datetimeBis = new DateTime();
                var i = db.InsertOrReplace(fahrzeug);

                fahrzeug = new Fahrzeug();
                fahrzeug.Id = 1;
                fahrzeug.kategorie = "pkw";
                fahrzeug.hersteller = "Opel";
                fahrzeug.modell = "Corsa";
                fahrzeug.kennzeichen = "SIG-UT-666";
                fahrzeug.zustand = 0;
                fahrzeug.kindersitze = 2;
                fahrzeug.automatik = 0;
                fahrzeug.nutzlast = 0;
                fahrzeug.nutzvolumen = 0;
                fahrzeug.datetimeVon = new DateTime();
                fahrzeug.datetimeBis = new DateTime();
                i = db.InsertOrReplace(fahrzeug);

                fahrzeug = new Fahrzeug();
                fahrzeug.Id = 2;
                fahrzeug.kategorie = "pkw";
                fahrzeug.hersteller = "Audi";
                fahrzeug.modell = "A4";
                fahrzeug.kennzeichen = "SIG-ZY-1432";
                fahrzeug.zustand = 1;
                fahrzeug.kindersitze = 0;
                fahrzeug.automatik = 0;
                fahrzeug.nutzlast = 0;
                fahrzeug.nutzvolumen = 0;
                fahrzeug.datetimeVon = new DateTime(2016, 5, 14);
                fahrzeug.datetimeBis = new DateTime(2016, 6, 2);
                i = db.InsertOrReplace(fahrzeug);

                fahrzeug = new Fahrzeug();
                fahrzeug.Id = 3;
                fahrzeug.kategorie = "trans";
                fahrzeug.hersteller = "Mercedes Benz";
                fahrzeug.modell = "Sprinter";
                fahrzeug.kennzeichen = "SIG-PO-123";
                fahrzeug.zustand = 0;
                fahrzeug.kindersitze = 0;
                fahrzeug.automatik = 0;
                fahrzeug.nutzlast = 600;
                fahrzeug.nutzvolumen = 8;
                fahrzeug.datetimeVon = new DateTime();
                fahrzeug.datetimeBis = new DateTime();
                i = db.InsertOrReplace(fahrzeug);

                fahrzeug = new Fahrzeug();
                fahrzeug.Id = 4;
                fahrzeug.kategorie = "trans";
                fahrzeug.hersteller = "Peugeot";
                fahrzeug.modell = "Partner";
                fahrzeug.kennzeichen = "SIG-A-9988";
                fahrzeug.zustand = 0;
                fahrzeug.kindersitze = 0;
                fahrzeug.automatik = 0;
                fahrzeug.nutzlast = 700;
                fahrzeug.nutzvolumen = 10;
                fahrzeug.datetimeVon = new DateTime();
                fahrzeug.datetimeBis = new DateTime();
                i = db.InsertOrReplace(fahrzeug);

                fahrzeug = new Fahrzeug();
                fahrzeug.Id = 5;
                fahrzeug.kategorie = "pkw";
                fahrzeug.hersteller = "Renault";
                fahrzeug.modell = "Twingo";
                fahrzeug.kennzeichen = "SIG-AP-&%!";
                fahrzeug.zustand = 0;
                fahrzeug.kindersitze = 0;
                fahrzeug.automatik = 1;
                fahrzeug.nutzlast = 0;
                fahrzeug.nutzvolumen = 0;
                fahrzeug.datetimeVon = new DateTime();
                fahrzeug.datetimeBis = new DateTime();
                i = db.InsertOrReplace(fahrzeug);

                fahrzeug = new Fahrzeug();
                fahrzeug.Id = 6;
                fahrzeug.kategorie = "pkw";
                fahrzeug.hersteller = "VW";
                fahrzeug.modell = "Polo";
                fahrzeug.kennzeichen = "SIG-Z-481";
                fahrzeug.zustand = 0;
                fahrzeug.kindersitze = 0;
                fahrzeug.automatik = 0;
                fahrzeug.nutzlast = 0;
                fahrzeug.nutzvolumen = 0;
                fahrzeug.datetimeVon = new DateTime();
                fahrzeug.datetimeBis = new DateTime();
                i = db.InsertOrReplace(fahrzeug);

                fahrzeug = new Fahrzeug();
                fahrzeug.Id = 7;
                fahrzeug.kategorie = "pkw";
                fahrzeug.hersteller = "VW";
                fahrzeug.modell = "Golf";
                fahrzeug.kennzeichen = "SIG-GT-303";
                fahrzeug.zustand = 0;
                fahrzeug.kindersitze = 1;
                fahrzeug.automatik = 0;
                fahrzeug.nutzlast = 0;
                fahrzeug.nutzvolumen = 0;
                fahrzeug.datetimeVon = new DateTime();
                fahrzeug.datetimeBis = new DateTime();
                i = db.InsertOrReplace(fahrzeug);

            }
        }

        public static void DeleteFahrzeug(int Id)
        {
            // Neue Verbindung
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                // Tracing
                db.TraceListener = new DebugTraceListener();
                
                // SQL
                db.Execute("DELETE FROM Fahrzeug WHERE Id = ?", Id);
            }
        }

        public static List<Fahrzeug> GetAllFahrzeuge()
        {
            List<Fahrzeug> fahrzeuge;

            // Neue Verbindung
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                // Tracing
                db.TraceListener = new DebugTraceListener();

                // Schiebt alle Einträge in die Liste, die ausgegeben wird.
                // Neu: die LINQ from-Klausel, anstatt konventionelle foreach-Schleifen 
                // -> keine Iteration -> einfach in die Liste zu speichern -> SQL-ähnlich mit Bedingungen (siehe nächste Methode)
                fahrzeuge = (from f in db.Table<Fahrzeug>() select f).ToList();
            }

            return fahrzeuge;
        }

        public static Fahrzeug GetFahrzeugById(int Id)
        {
            // Neue Verbindung
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                // Tracing
                db.TraceListener = new DebugTraceListener();

                // LINQ-Statement zur bedingten Ausgabe der Einträge
                Fahrzeug fahrzeug = (from f in db.Table<Fahrzeug>() where f.Id == Id select f).FirstOrDefault();
                return fahrzeug;
            }
        }

        public static void SaveFahrzeug(Fahrzeug fahrzeug)
        {
            // Neue Verbindung
            using (var db = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath))
            {
                // Tracing
                db.TraceListener = new DebugTraceListener();

                // Id = 0 wird hier als Platzhalter und Bedingung zur Insertation eines neuen Eintrags verwendet. Somit kann die Methode für beide Zwecke dienen
                // und sie kann dynamisch und automatisiert verwenden werden.
                if (fahrzeug.Id == 0)
                {
                    // Neuer Eintrag
                    db.Insert(fahrzeug);
                }
                else
                {
                    // Update Eintrag
                    db.Update(fahrzeug);
                }
            }
        }

    }
}
