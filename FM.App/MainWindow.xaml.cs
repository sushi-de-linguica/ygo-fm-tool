using FM.Core.Application;
using FM.Core.Domain.Models;
using FM.Core.Usecases;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
using static FM.Core.Domain.Enums.CardEnums;

namespace FM.App
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<RuleItem> RuleItems { get; set; }
        public RuleItem SelectedRuleItem { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            RuleItems = new List<RuleItem>();

            DataContext = this;
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
            RuleItem Item = new RuleItem();
            
            Item.CardsFrom = new int[0];
            Item.CardType = null;
            Item.MonsterType = null;

            int MinQuantity;
            int MaxQuantity;
            EMonsterCardType MonsterCardType;

            bool ParsedMinQuantity = Int32.TryParse(MinQuantityTextBox.Text, out MinQuantity);
            bool ParsedMaxQuantity = Int32.TryParse(MaxQuantityTextBox.Text, out MaxQuantity);
            
            Item.MinQuantity = ParsedMinQuantity ? MinQuantity : 0;
            Item.MaxQuantity = ParsedMaxQuantity ? MaxQuantity : 0;
            Item.CardType = CardTypeComboBox.Text;
            Item.MonsterType = null;

            bool IsMonsterCardType = Item.CardType == "Monster";
            if (IsMonsterCardType)
            {
                int MinAttack;
                int MaxAttack;
                int MinDefense;
                int MaxDefense;

                bool ParsedMinAttack = Int32.TryParse(MinAttackTextBox.Text, out MinAttack);
                bool ParsedMaxAttack = Int32.TryParse(MaxAttackTextBox.Text, out MaxAttack);
                bool ParsedMinDefense = Int32.TryParse(MinDefenseTextBox.Text, out MinDefense);
                bool ParsedMaxDefense = Int32.TryParse(MaxDefenseTextBox.Text, out MaxDefense);
                Enum.TryParse(MonsterTypeComboBox.Text, out MonsterCardType);

                Item.MinAttack = ParsedMinAttack ? MinAttack : 0;
                Item.MaxAttack = ParsedMaxAttack ? MaxAttack : 0;
                Item.MinDefense = ParsedMinDefense ? MinDefense : 0;
                Item.MaxDefense = ParsedMaxDefense ? MaxDefense : 0;
                Item.MonsterType = MonsterCardType;

                Item.MonsterTypeIndex = CardTypeComboBox.SelectedIndex;
            }

            Item.DropsCardsFromIndex = GetCardsFromComboBox.SelectedIndex;
            Item.CardTypeIndex = CardTypeComboBox.SelectedIndex;

            Item.CardsFrom = HandleGetCardsFrom(GetCardsFromComboBox.Text);

            if (Item.DropsCardsFromIndex == 9)
            {
                Item.Custom = CustomCardsTextBox.Text;
            }

            if (SelectedRuleItem != null )
            {
                SelectedRuleItem = null;
                ClearAddRulesFields();
                return;
            }

            HandleAddToRulesList(Item);
            ClearAddRulesFields();
        }

        private void EditRuleItem(RuleItem Rule)
        {
            ClearAddRulesFields();
            CardTypeComboBox.SelectedIndex = Rule.CardTypeIndex;
            MinQuantityTextBox.Text = Rule.MinQuantity.ToString();
            MaxQuantityTextBox.Text = Rule.MaxQuantity.ToString();

            if (Rule.CardTypeIndex == 0)
            {
                MonsterTypeComboBox.SelectedIndex = Rule.MonsterTypeIndex;
                MinAttackTextBox.Text = Rule.MinAttack.ToString();
                MaxAttackTextBox.Text = Rule.MaxAttack.ToString();
                MinDefenseTextBox.Text = Rule.MinDefense.ToString();
                MaxDefenseTextBox.Text = Rule.MaxDefense.ToString();
            }

            HandleSwitchOptionsStep1FromCardType(Rule.CardType.ToString());
            GetCardsFromComboBox.SelectedIndex = Rule.DropsCardsFromIndex;
            TabControl.SelectedIndex = 0;
        }

        private void HandleSwitchOptionsStep1FromCardType(string type)
        {
            type = type.ToLower();
            switch (type)
            {
                case "monster":
                    AllowOnlyMonsterGetCardsFromOptions();
                    PanelMonsterAttributes.Visibility = Visibility.Visible;
                    PanelMonsterType.Visibility = Visibility.Visible;
                    return;
                case "equip":
                    AllowOnlyEquipsGetCardsFromOptions();
                    break;
                case "fields":
                    AllowOnlyFieldsGetCardsFromOptions();
                    break;
                case "trap":
                    AllowOnlyAllCardsAndSetsGetCardsFromOptions();
                    break;
                case "magic":
                    AllowAllExceptMonstersGetCardsFromOptions();
                    break;

            }

            PanelMonsterAttributes.Visibility = Visibility.Hidden;
            PanelMonsterType.Visibility = Visibility.Hidden;
        }

        private void CardTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItemContent = ((ListBoxItem)CardTypeComboBox.SelectedItem).Content;
            if (SelectedItemContent == null)
            {
                return;
            }

            string type = SelectedItemContent.ToString().ToLower();
            HandleSwitchOptionsStep1FromCardType(type);
        }

        private void AllowAllGetCardsFromOptions()
        {
            GetCardsFromOptionAllCards.IsEnabled = true;
            GetCardsFromOptionAllSets.IsEnabled = true;

            GetCardsFromOptionSet1Monsters.IsEnabled = true;
            GetCardsFromOptionSet2Monsters.IsEnabled = true;
            GetCardsFromOptionSet3Monsters.IsEnabled = true;
            GetCardsFromOptionSet4Monsters.IsEnabled = true;
            GetCardsFromOptionSet5Disarm.IsEnabled = true;
            GetCardsFromOptionSet6Fields.IsEnabled = true;
            GetCardsFromOptionSet7Equips.IsEnabled = true;

            GetCardsFromOptionAllCards.IsSelected = true;
        }
        private void AllowAllExceptMonstersGetCardsFromOptions()
        {
            GetCardsFromOptionAllCards.IsEnabled = true;
            GetCardsFromOptionAllSets.IsEnabled = true;

            GetCardsFromOptionSet1Monsters.IsEnabled = false;
            GetCardsFromOptionSet2Monsters.IsEnabled = false;
            GetCardsFromOptionSet3Monsters.IsEnabled = false;
            GetCardsFromOptionSet4Monsters.IsEnabled = false;
            GetCardsFromOptionSet5Disarm.IsEnabled = true;
            GetCardsFromOptionSet6Fields.IsEnabled = true;
            GetCardsFromOptionSet7Equips.IsEnabled = true;

            GetCardsFromOptionAllCards.IsSelected = true;
        }

        private void AllowOnlyAllCardsAndSetsGetCardsFromOptions()
        {
            GetCardsFromOptionAllCards.IsEnabled = true;
            GetCardsFromOptionAllSets.IsEnabled = true;

            GetCardsFromOptionSet1Monsters.IsEnabled = false;
            GetCardsFromOptionSet2Monsters.IsEnabled = false;
            GetCardsFromOptionSet3Monsters.IsEnabled = false;
            GetCardsFromOptionSet4Monsters.IsEnabled = false;
            GetCardsFromOptionSet5Disarm.IsEnabled = false;
            GetCardsFromOptionSet6Fields.IsEnabled = false;
            GetCardsFromOptionSet7Equips.IsEnabled = false;

            GetCardsFromOptionAllCards.IsSelected = true;
        }

        private void AllowOnlyEquipsGetCardsFromOptions()
        {
            GetCardsFromOptionAllCards.IsEnabled = true;
            GetCardsFromOptionAllSets.IsEnabled = true;

            GetCardsFromOptionSet1Monsters.IsEnabled = false;
            GetCardsFromOptionSet2Monsters.IsEnabled = false;
            GetCardsFromOptionSet3Monsters.IsEnabled = false;
            GetCardsFromOptionSet4Monsters.IsEnabled = false;
            GetCardsFromOptionSet5Disarm.IsEnabled = false;
            GetCardsFromOptionSet6Fields.IsEnabled = false;
            GetCardsFromOptionSet7Equips.IsEnabled = true;

            GetCardsFromOptionAllCards.IsSelected = true;
        }

        private void AllowOnlyFieldsGetCardsFromOptions()
        {
            GetCardsFromOptionAllCards.IsEnabled = true;
            GetCardsFromOptionAllSets.IsEnabled = true;

            GetCardsFromOptionSet1Monsters.IsEnabled = false;
            GetCardsFromOptionSet2Monsters.IsEnabled = false;
            GetCardsFromOptionSet3Monsters.IsEnabled = false;
            GetCardsFromOptionSet4Monsters.IsEnabled = false;
            GetCardsFromOptionSet5Disarm.IsEnabled = false;
            GetCardsFromOptionSet6Fields.IsEnabled = true;
            GetCardsFromOptionSet7Equips.IsEnabled = false;

            GetCardsFromOptionAllCards.IsSelected = true;
        }

        private void AllowOnlyDisarmsGetCardsFromOptions()
        {
            GetCardsFromOptionAllCards.IsEnabled = true;
            GetCardsFromOptionAllSets.IsEnabled = true;

            GetCardsFromOptionSet1Monsters.IsEnabled = false;
            GetCardsFromOptionSet2Monsters.IsEnabled = false;
            GetCardsFromOptionSet3Monsters.IsEnabled = false;
            GetCardsFromOptionSet4Monsters.IsEnabled = false;
            GetCardsFromOptionSet5Disarm.IsEnabled = true;
            GetCardsFromOptionSet6Fields.IsEnabled = false;
            GetCardsFromOptionSet7Equips.IsEnabled = false;

            GetCardsFromOptionAllCards.IsSelected = true;
        }

        private void AllowOnlyMonsterGetCardsFromOptions()
        {
            GetCardsFromOptionAllCards.IsEnabled = true;
            GetCardsFromOptionAllSets.IsEnabled = true;

            GetCardsFromOptionSet1Monsters.IsEnabled = true;
            GetCardsFromOptionSet2Monsters.IsEnabled = true;
            GetCardsFromOptionSet3Monsters.IsEnabled = true;
            GetCardsFromOptionSet4Monsters.IsEnabled = true;
            GetCardsFromOptionSet5Disarm.IsEnabled = false;
            GetCardsFromOptionSet6Fields.IsEnabled = false;
            GetCardsFromOptionSet7Equips.IsEnabled = false;

            GetCardsFromOptionAllCards.IsSelected = true;
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
            AllowOnlyMonsterGetCardsFromOptions();
        }

        #region CUSTOM FUNCTIONS

        private void HandleAddToRulesList(RuleItem Item)
        {
            Configurations.Rules.Add(Item);
            UpdateDataGridFromGlobal();
        }

        private void UpdateDataGridFromGlobal()
        {
            RuleItems.Clear();

            foreach(var Rule in Configurations.Rules)
            {
                RuleItems.Add(Rule);
            }
        }

        private int[] GetCardIdsArrayFromVanillaByStarterDeck(StarterDeck starterDeck)
        {
            var itens = new List<int>();
            for (var index = 0; index < starterDeck.Cards.Length; index++)
            {
                bool IsValidDrop = starterDeck.Cards[index] != 0;
                if (IsValidDrop)
                {
                    itens.Add(index + 1);
                }
            }
            return itens.ToArray();
        }

        private void PopulateHandleGetCardsFromByDeck(ref List<int> itens, StarterDeck Deck) {
            int[] Cards = GetCardIdsArrayFromVanillaByStarterDeck(Deck);
            if (Cards.Length > 0)
            {
                foreach(var Card in Cards)
                {
                    itens.Add((int)Card);
                }
            }
        }

        private int[] HandleGetCardsFrom(string option)
        {
            var itens = new List<int>();

            switch (option)
            {
                case "Custom":
                    break;
                case "All Cards":
                    for(var index = 1; index <= Static.MAX_CARDS; index++)
                    {
                        itens.Add(index);
                    }
                    break;
                case "Initial Deck (all sets)":
                    foreach(var Deck in Configurations.VanillaStarterDeck)
                    {
                        PopulateHandleGetCardsFromByDeck(ref itens, Deck);
                    }
                    break;
                case "Initial Deck (set 1 - monsters)":
                    PopulateHandleGetCardsFromByDeck(ref itens, Configurations.VanillaStarterDeck[0]);
                    break;
                case "Initial Deck (set 2 - monsters)":
                    PopulateHandleGetCardsFromByDeck(ref itens, Configurations.VanillaStarterDeck[1]);
                    break;
                case "Initial Deck (set 3 - monsters)":
                    PopulateHandleGetCardsFromByDeck(ref itens, Configurations.VanillaStarterDeck[2]);
                    break;
                case "Initial Deck (set 4 - monsters)":
                    PopulateHandleGetCardsFromByDeck(ref itens, Configurations.VanillaStarterDeck[3]);
                    break;
                case "Initial Deck (set 5 - DH/Geki)":
                    PopulateHandleGetCardsFromByDeck(ref itens, Configurations.VanillaStarterDeck[4]);
                    break;
                case "Initial Deck (set 6 - fields)":
                    PopulateHandleGetCardsFromByDeck(ref itens, Configurations.VanillaStarterDeck[5]);
                    break;
                case "Initial Deck (set 7 - equips)":
                    PopulateHandleGetCardsFromByDeck(ref itens, Configurations.VanillaStarterDeck[6]);
                    break;
            }

            return itens.ToArray();
        }

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


        private void RulesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RulesDataGrid.SelectedItems.Count == 0)
            {
                return;
            }

            var SelectedItem = (RuleItem)RulesDataGrid.SelectedItems[0];
            if (SelectedItem == null) {
                return;
            }

            SelectedRuleItem = SelectedItem;
        }

        private void RuleStepEditSelectedRule_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRuleItem != null)
            {
                EditRuleItem(SelectedRuleItem);
            }
        }
    }
}
