using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

namespace Citymobil
{
    /// <summary>
    /// Die MainPage ist der Blickpunkt des programms.
    /// Hier werden UI-Elemente dargestellt und Datenbank (Database.cs) - Aufrufe ausgeführt.
    /// Die Seite existiert zusammen mit der XAML-Datei. Sie wurde nicht nach dem MVVC-Prinzip untergliedert und verschoben,
    /// da dies für den Umfang des programms so ausreicht und das Programm nur aus dieser einen Seite besteht.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Mitglied user = new Mitglied("Pascal", "Michler");

        public MainPage()
        {
            this.InitializeComponent();

            // Nachdem UI-Komponenten initialisiert wurden:

            /* Database Setup */
            Database.CreateDatabase();

            // Bestand schon zu Anfang laden
            bestandAnzeigen();

            // Datum bereits setzen, um NullExceptions zu vermeiden und den Nutzer auf die Eintragung hinzuweisen
            // (-> TODO: Bei Datum vor aktuellem datum eine Fehlermeldung bringen)
            dateVon.Date = DateTime.Today;
            dateBis.Date = DateTime.Today;
            
            mitgliedButton.Label = user.vorname + " " + user.nachname;            
        }

        // Verschiedene Views für PKW/Transporter
        private void comboKategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (gridPKW != null && gridTransporter != null)
            {
                if (comboKategorie.SelectedIndex == 0)
                {
                    gridPKW.Visibility = Visibility.Visible;
                    gridTransporter.Visibility = Visibility.Collapsed;
                }
                if (comboKategorie.SelectedIndex == 1)
                {
                    gridPKW.Visibility = Visibility.Collapsed;
                    gridTransporter.Visibility = Visibility.Visible;
                }
            }
            
        }        

        // Suchen-Button-Klick-Event
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            filterAnzeigen();
        }

        // Event aus FahrzeugEintragUI (BuchungFertig)
        public void Eventhandler_BuchungFertig(object sender, EventArgs e)
        {
            filterAnzeigen();
        }

        // Logik des Filters und der Suche
        // Logik kann in Database.cs mithilfe LINQ/SQL-Queries vereinfacht werden.
        private void filterAnzeigen()
        {
            stackFilter.Children.Clear();
            // Alle Fahrzeuge aus Datenbank laden
            List<Fahrzeug> fahrzeuge = Database.GetAllFahrzeuge();

            // Datum wird aus den UI-Komponenten für die Weiterverarbeitung vorbereitet
            string[] dv = dateVon.Date.ToString().Split(' ');
            string[] dvs = dv[0].Split('.');
            string[] db = dateBis.Date.ToString().Split(' ');
            string[] dbs = db[0].Split('.');

            // year, month, day, hour, minute, second
            DateTime von = new DateTime(int.Parse(dvs[2]), int.Parse(dvs[1]), int.Parse(dvs[0]));
            DateTime bis = new DateTime(int.Parse(dbs[2]), int.Parse(dbs[1]), int.Parse(dbs[0]));

            // UserControl zur Wiederverwendung und Event-Funktionalität
            // Muss jedes mal, nachdem "fe" neue initialisiert wurde, neu mit dem Eventhandler verbunden werden 
            // -> besseres MVVC schafft Abhilfe
            FahrzeugEintragUI fe = new FahrzeugEintragUI();

            // Logik
            foreach (var f in fahrzeuge)
            {
                if (f.zustand == 0) // zustand = 0 -> nicht gebucht
                {
                    // PKW
                    if (comboKategorie.SelectedIndex == 0 && f.kategorie == "pkw")
                    {
                        // Kindersitze
                        if (comboKinder.SelectedIndex == f.kindersitze)
                        {
                            // Automatik
                            if (checkAutomatik.IsChecked == true && f.automatik == 1)
                            {
                                stackFilter.Children.Add(fe = new FahrzeugEintragUI(f.hersteller + " " + f.modell, f.kennzeichen, f.kindersitze + " Kindersitze", f.automatik.ToString(), true, "pkw", f.zustand, f, von, bis, user));
                                fe.BuchungFertig += new EventHandler(Eventhandler_BuchungFertig);
                            }
                            if (checkAutomatik.IsChecked == false && f.automatik == 0)
                            {
                                stackFilter.Children.Add(fe = new FahrzeugEintragUI(f.hersteller + " " + f.modell, f.kennzeichen, f.kindersitze + " Kindersitze", f.automatik.ToString(), true, "pkw", f.zustand, f, von, bis, user));
                                fe.BuchungFertig += new EventHandler(Eventhandler_BuchungFertig);
                            }
                        }

                    }

                    // Transporter
                    if (comboKategorie.SelectedIndex == 1 && f.kategorie == "trans")
                    {
                        // Verschiedene Möglichkeiten, um auch mit leeren Inputs suchen zu können (keine Fehler zu bekommen)

                        double t;

                        if (Double.TryParse(inputNutzvolumen.Text, out t) && t == f.nutzvolumen && Double.TryParse(inputNutzlast.Text, out t) && t == f.nutzlast)
                        {
                            stackFilter.Children.Add(fe = new FahrzeugEintragUI(f.hersteller + " " + f.modell, f.kennzeichen, f.nutzlast + "kg  |", f.nutzvolumen + "m³", true, "trans", f.zustand, f, von, bis, user));
                            fe.BuchungFertig += new EventHandler(Eventhandler_BuchungFertig);
                        }

                        if (String.IsNullOrWhiteSpace(inputNutzvolumen.Text) && Double.TryParse(inputNutzlast.Text, out t) && t == f.nutzlast)
                        {
                            stackFilter.Children.Add(fe = new FahrzeugEintragUI(f.hersteller + " " + f.modell, f.kennzeichen, f.nutzlast + "kg  |", f.nutzvolumen + "m³", true, "trans", f.zustand, f, von, bis, user));
                            fe.BuchungFertig += new EventHandler(Eventhandler_BuchungFertig);
                        }

                        if (Double.TryParse(inputNutzvolumen.Text, out t) && t == f.nutzvolumen && String.IsNullOrWhiteSpace(inputNutzlast.Text))
                        {
                            stackFilter.Children.Add(fe = new FahrzeugEintragUI(f.hersteller + " " + f.modell, f.kennzeichen, f.nutzlast + "kg  |", f.nutzvolumen + "m³", true, "trans", f.zustand, f, von, bis, user));
                            fe.BuchungFertig += new EventHandler(Eventhandler_BuchungFertig);
                        }

                        if (String.IsNullOrWhiteSpace(inputNutzlast.Text) && String.IsNullOrWhiteSpace(inputNutzvolumen.Text))
                        {
                            stackFilter.Children.Add(fe = new FahrzeugEintragUI(f.hersteller + " " + f.modell, f.kennzeichen, f.nutzlast + "kg  |", f.nutzvolumen + "m³", true, "trans", f.zustand, f, von, bis, user));
                            fe.BuchungFertig += new EventHandler(Eventhandler_BuchungFertig);
                        }

                    }
                }
            }
        }

        // Bestand/Alle Fahrzeuge anzeigen und zwischen PKW/Transporter unterscheiden
        private void bestandAnzeigen()
        {
            stackBestand.Children.Clear();

            // Alle fahrzeuge aus Datenbank laden
            List<Fahrzeug> fahrzeuge = Database.GetAllFahrzeuge();

            foreach (var f in fahrzeuge)
            {
                switch (f.kategorie)
                {
                    case "pkw":
                        stackBestand.Children.Add(new FahrzeugEintragUI(f.hersteller + " " + f.modell, f.kennzeichen, f.kindersitze + " Kindersitze", f.automatik.ToString(), false, "pkw", f.zustand, f, new DateTime(), new DateTime(), user));
                        break;

                    case "trans":
                        stackBestand.Children.Add(new FahrzeugEintragUI(f.hersteller + " " + f.modell, f.kennzeichen, f.nutzlast + "kg  |", f.nutzvolumen + "m³", false, "trans", f.zustand, f, new DateTime(), new DateTime(), user));
                        break;
                }

            }
        }

        // Bei Wischen/navigieren zur Bestands-Seite wird dieser neue geladen
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int p = myPivot.SelectedIndex;
            if(p == 1)
            {
                bestandAnzeigen();
            }
        }
        
    }
}
