using FM.Core.Application;
using FM.Core.Domain.Models;
using FM.Core.Usecases;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FM.App
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadCharacterTableButton_Click(object sender, RoutedEventArgs e)
        {
            var table_path = @"./CharacterTable.txt";

            if (!File.Exists(table_path))
            {
                var dialog = new OpenFileDialog
                {
                    Title = "CharacterTable file",
                    Filter = "CharacterTable.txt|CharacterTable.txt"
                };

                if (dialog.ShowDialog() == false)
                {
                    return;
                }

                table_path = dialog.FileName;
            }

            LoadCharacterTableUseCase LoadCharacterTable = new LoadCharacterTableUseCase(table_path);
            var success = LoadCharacterTable.Execute();

            if (!success)
            {
                MessageBox.Show(
                    "Provided CharacterTable.txt is incorrect or incomplete!",
                    "Error reading CharacterTable.txt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            LoadCharacterTableButton.IsEnabled = false;
            MessageBox.Show("Success load character table", "Success");
        }

        private void LoadRomFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Location of Yu-Gi-Oh! Forbidden Memories NTSC CUE File",
                Filter = "*.cue | *.cue"
            };

            if (dialog.ShowDialog() == true)
            {
                LoadBinaryFileUseCase LoadBinaryFile = new LoadBinaryFileUseCase(dialog.FileName);
                LoadBinaryFile.Execute();


                LoadCardsDataUseCase LoadCardsData = new LoadCardsDataUseCase();
                LoadCardsData.Execute();

                LoadRomFileButton.IsEnabled = false;

                MessageBox.Show(
                    "Extracting game data complete.",
                    "Extracting data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                TabControl.Visibility = Visibility.Visible;
            }
        }

        private void WriteChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Configurations.UsedIso)
            {
                // HandleApplySixDecks();
                HandleApplyFirstDeckOnly();

                Configurations.UsedIso = true;

                MessageBox.Show(
                    "Success",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            // FiltersStackPanel.Children.Add(stackPanel);
        }

        private void AddRuleToInitialDeckButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAddRulesFields();
        }

        private void CardTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItemContent = ((ListBoxItem)CardTypeComboBox.SelectedItem).Content;
            if (SelectedItemContent == null)
            {
                return;
            }

            string type = SelectedItemContent.ToString();
            bool IsEnableMonsterAttributesPanel = type == "Monster";
            PanelMonsterAttributes.Visibility = IsEnableMonsterAttributesPanel ? Visibility.Visible : Visibility.Collapsed;
            PanelMonsterType.Visibility = IsEnableMonsterAttributesPanel ? Visibility.Visible : Visibility.Hidden;
        }

        private void GetCardsFromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItemContent = ((ListBoxItem)GetCardsFromComboBox.SelectedItem).Content;
            if (SelectedItemContent == null)
            {
                return;
            }

            string Value = SelectedItemContent.ToString();

            bool IsEnableCustomCardsTextBox = Value == "Custom";
            CustomCardsTextBox.Visibility = IsEnableCustomCardsTextBox ? Visibility.Visible : Visibility.Collapsed;
            CustomCardsTextBox.IsEnabled = IsEnableCustomCardsTextBox;
        }

        private void initialDeck1MinusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 0;
            var value = Int32.Parse(initialDeck1Quantity.Text) - 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck1PlusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 0;
            var value = Int32.Parse(initialDeck1Quantity.Text) + 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck2MinusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 1;
            var value = Int32.Parse(initialDeck2Quantity.Text) - 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck2PlusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 1;
            var value = Int32.Parse(initialDeck2Quantity.Text) + 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck3PlusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 2;
            var value = Int32.Parse(initialDeck3Quantity.Text) + 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck4PlusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 3;
            var value = Int32.Parse(initialDeck4Quantity.Text) + 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck5PlusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 4;
            var value = Int32.Parse(initialDeck5Quantity.Text) + 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck6PlusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 5;
            var value = Int32.Parse(initialDeck6Quantity.Text) + 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck7PlusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 6;
            var value = Int32.Parse(initialDeck7Quantity.Text) + 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck3MinusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 2;
            var value = Int32.Parse(initialDeck3Quantity.Text) - 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck4MinusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 3;
            var value = Int32.Parse(initialDeck4Quantity.Text) - 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck5MinusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 4;
            var value = Int32.Parse(initialDeck5Quantity.Text) - 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck6MinusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 5;
            var value = Int32.Parse(initialDeck6Quantity.Text) - 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void initialDeck7MinusButton_Click(object sender, RoutedEventArgs e)
        {
            int position = 6;
            var value = Int32.Parse(initialDeck7Quantity.Text) - 1;

            HandleUpdateValueOfDeckDrops(value, position);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            LoadDropRateTableUseCase LoadDropRateTable = new LoadDropRateTableUseCase();
            LoadDropRateTable.Execute();

            AutoLoadCharacterTable();
            UpdateInitialDeckFinishStepValues();
        }

        #region CUSTOM FUNCTIONS

        private int[] initCardsArray(int defaultValue)
        {
            int[] array = new int[Static.MAX_CARDS];

            for (var index = 0; index < Static.MAX_CARDS; index++)
            {
                array[index] = defaultValue;
            }

            return array;
        }

        private void HandleApplySixDecks()
        {
            int MAX_DECKS = 6;
            CustomDeckUseCase.EnableSixDecks(Configurations.WaPath);
            Configurations.StarterDecks = new StarterDeck[MAX_DECKS];

            for (var index = 0; index < MAX_DECKS; index++)
            {
                Configurations.StarterDecks[index] = new StarterDeck
                {
                    Dropped = 0,
                    Cards = initCardsArray(0)
                };
            }

            for (int current = 0; current < Static.MAX_CARDS; current++)
            {
                ushort value = current <= 39 ? (ushort)1 : (ushort)0;
                Configurations.StarterDecks[0].Cards[current] = value;
            }

            CustomDeckUseCase.SetInitialDeck(Configurations.StarterDecks);

            WritePatchedImageUseCase writePatchedImage = new WritePatchedImageUseCase(Configurations.IsoPath);
            writePatchedImage.Execute();
        }

        private void HandleApplyFirstDeckOnly()
        {
            int MAX_DECKS = Static.MAX_STARTER_DECKS;
            CustomDeckUseCase.EnableFirstDeckOnly(Configurations.WaPath);
            Configurations.StarterDecks = new StarterDeck[MAX_DECKS];

            for (var index = 0; index < MAX_DECKS; index++)
            {
                Configurations.StarterDecks[index] = new StarterDeck
                {
                    Dropped = 0,
                    Cards = initCardsArray(0)
                };
            }

            for (int current = 0; current < Static.MAX_CARDS; current++)
            {
                ushort value = current <= 39 ? (ushort)1 : (ushort)0;
                Configurations.StarterDecks[0].Cards[current] = value;
            }

            CustomDeckUseCase.SetInitialDeck(Configurations.StarterDecks);

            WritePatchedImageUseCase writePatchedImage = new WritePatchedImageUseCase(Configurations.IsoPath);
            writePatchedImage.Execute();
        }

        private void ClearAddRulesFields()
        {
            MonsterTypeComboBox.SelectedItem = "";
            CardTypeComboBox.SelectedItem = "";
            MinQuantityTextBox.Clear();
            MaxQuantityTextBox.Clear();
            MinAttackTextBox.Clear();
            MaxAttackTextBox.Clear();
            MinDefenseTextBox.Clear();
            MaxDefenseTextBox.Clear();

            MinQuantityTextBox.Text = "1";
            MaxQuantityTextBox.Text = "1";

            MinAttackTextBox.Text = "0";
            MaxAttackTextBox.Text = "0";
            MinDefenseTextBox.Text = "0";
            MaxDefenseTextBox.Text = "0";
        }

        private void UpdateInitialDeckFinishStepValues()
        {
            initialDeck1Quantity.Text = Configurations.DropCardsVanillaStarterDeck[0].ToString();
            initialDeck2Quantity.Text = Configurations.DropCardsVanillaStarterDeck[1].ToString();
            initialDeck3Quantity.Text = Configurations.DropCardsVanillaStarterDeck[2].ToString();
            initialDeck4Quantity.Text = Configurations.DropCardsVanillaStarterDeck[3].ToString();
            initialDeck5Quantity.Text = Configurations.DropCardsVanillaStarterDeck[4].ToString();
            initialDeck6Quantity.Text = Configurations.DropCardsVanillaStarterDeck[5].ToString();
            initialDeck7Quantity.Text = Configurations.DropCardsVanillaStarterDeck[6].ToString();
        }

        private void HandleUpdateValueOfDeckDrops(int newValue, int position)
        {
            if (newValue >= 0)
            {
                Configurations.DropCardsVanillaStarterDeck[position] = newValue;
                UpdateInitialDeckFinishStepValues();
            }
        }
        private void AutoLoadCharacterTable()
        {
            var table_path = @"./CharacterTable.txt";
            if (File.Exists(table_path))
            {
                LoadCharacterTableUseCase LoadCharacterTable = new LoadCharacterTableUseCase(table_path);
                var success = LoadCharacterTable.Execute();
                if (success)
                {
                    LoadCharacterTableButton.IsEnabled = false;
                }
            }

        }

        #endregion
    }
}
