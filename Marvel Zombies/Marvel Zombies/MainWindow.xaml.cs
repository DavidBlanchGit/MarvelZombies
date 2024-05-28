using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace Marvel_Zombies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HeroeData();
        }

        private List<Character> allHeroes;
        private List<Character> filteredHeroes;

        private void HeroeData()
        {
            JsonHeroProvider heroProvider = new JsonHeroProvider();
            allHeroes = heroProvider.pickAllHeroes("marvel_zombies.json");
            filteredHeroes = new List<Character>(allHeroes);
            DisplayHeroes(filteredHeroes);
        }

        private void DisplayHeroes(List<Character> heroes)
        {
            tablaGrid.RowDefinitions.Clear();
            tablaGrid.Children.Clear();

            for (int i = 0; i < heroes.Count; i++)
            {
                tablaGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var nombreTextBlock = new TextBlock { Text = heroes[i].Heroe, Margin = new Thickness(5) };
                var afiliacionTextBlock = new TextBlock { Text = heroes[i].Afiliacion, Margin = new Thickness(5) };
                var estadoTextBlock = new TextBlock { Text = heroes[i].Estado, Margin = new Thickness(5) };

                Grid.SetRow(nombreTextBlock, i);
                Grid.SetColumn(nombreTextBlock, 0);
                Grid.SetRow(afiliacionTextBlock, i);
                Grid.SetColumn(afiliacionTextBlock, 1);
                Grid.SetRow(estadoTextBlock, i);
                Grid.SetColumn(estadoTextBlock, 2);

                tablaGrid.Children.Add(nombreTextBlock);
                tablaGrid.Children.Add(afiliacionTextBlock);
                tablaGrid.Children.Add(estadoTextBlock);
            }
        }

        private void ApplyFilters()
        {
            String heroeFilter = TBox_hero.Text;
            String factionFilter = TBox_faction.Text;
            String stateFilter = CB_State.Text;

            if (stateFilter == "Estado")
            {
                stateFilter = "";
            }

            filteredHeroes = allHeroes
                .Where(h => (string.IsNullOrEmpty(heroeFilter) || h.Heroe.Contains(heroeFilter, StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrEmpty(factionFilter) || h.Afiliacion.Contains(factionFilter, StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrEmpty(stateFilter) || h.Estado.Contains(stateFilter, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            DisplayHeroes(filteredHeroes);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

    }

    public class Character
    {
        public string Heroe { get; set; }
        public string Afiliacion { get; set; }
        public string Estado { get; set; }
    }

    public class Affilation
    {
        public List<string> Afiliaciones { get; set; }
    }

    public class JsonHeroProvider
    {
        public List<Character> pickAllHeroes(string archive)
        {
            try
            {
                using (FileStream fs = File.OpenRead(archive))
                {
                    return JsonSerializer.DeserializeAsync<List<Character>>(fs).Result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo JSON: " + ex.Message);
                return new List<Character>();
            }
        }
    }
}