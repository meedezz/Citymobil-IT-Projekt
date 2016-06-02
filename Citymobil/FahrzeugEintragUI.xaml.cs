using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Citymobil
{
    /// <summary>
    /// Dieses userControl stellt die Einträge in der Auflistung dar und wurde für einfache Wiederverwendung implementiert.
    /// Das Benutzersteuerelement ist auch dafür verantwortlich, ein Fahrzeug zu buchen.
    /// </summary>
    public sealed partial class FahrzeugEintragUI : UserControl
    {
        private Fahrzeug fahrzeug;
        private DateTime von;
        private DateTime bis;
        private Mitglied mitglied;

        public FahrzeugEintragUI()
        {

        }

        public FahrzeugEintragUI(string bezeichnung, string kennzeichen, string att1, string att2, bool button, string kategorie, int zustand, Fahrzeug f, DateTime von, DateTime bis, Mitglied m)
        {
            this.InitializeComponent();

            // Ist der Eintrag in der Fahrzeugbestandsliste? Wenn ja, zeige den Button für das Buchen nicht an.
            if (button)
            {
                buchenButton.Visibility = Visibility.Visible;
            }
            else
            {
                buchenButton.Visibility = Visibility.Collapsed;
            }

            // Beschriftungen des Elements
            textBezeichnung.Text = bezeichnung;
            textKennzeichen.Text = kennzeichen;
            textAtt1.Text = att1;
            textAtt2.Text = att2;

            // Kategoriebedingte Darstellungen
            // Unterschiedliche Farben für PKW/Transporter
            if (kategorie.Equals("pkw"))
            {
                gridBack.Background = new SolidColorBrush(Color.FromArgb(255, 0, 160, 132));

                if(att2 == "1")
                {
                    textAtt2.Text = "|  Automatik";
                }
                else
                {
                    textAtt2.Text = "";
                }
            }
            else
            {
                gridBack.Background = new SolidColorBrush(Color.FromArgb(255, 0, 129, 107));
            }

            // Andere farbe für gebuchtes Fahrzeug in Bestandsliste
            if (zustand == 1)
            {
                gridBack.Background = new SolidColorBrush(Color.FromArgb(255, 170, 110, 100));
            }

            // Attributzuweisungen
            fahrzeug = f;
            this.von = von;
            this.bis = bis;
            mitglied = m;
        }

        // Event zur nachrichtenvermittlung an MainPage.xaml.cs
        // BuchungFertig schickt Nahricht bei Button-Click (unten) an den EventHandler in MainPage.xaml.cs
        public event EventHandler BuchungFertig;

        private void buchenButton_Click(object sender, RoutedEventArgs e)
        {
            // Setzt Werte, die für eine Buchung geändert werden müssen
            fahrzeug.zustand = 1;
            fahrzeug.datetimeVon = von;
            fahrzeug.datetimeBis = bis;
            //fahrzeug.mitglied = mitglied;
            Database.SaveFahrzeug(fahrzeug);

            // Event
            if (BuchungFertig != null)
            {
                BuchungFertig(this, new EventArgs());
            }
        }
    }
}
