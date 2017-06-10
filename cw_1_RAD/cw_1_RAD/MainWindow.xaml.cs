using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cw_1_RAD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //  v_      - variables
        //  p_      - property
        //  sm_     - standard methods
        //  xe_     - XAML elements
        //  em_     - event methods

        //linkver: http://newidols.ru

        //==================================================================================================================
        //============================================== VARIABLES =========================================================
        //==================================================================================================================

        /// <summary>
        /// Przechowuje storyboard dla animacji menu funkcyjnego
        /// </summary>
        Storyboard v_storyBoardBox;

        /// <summary>
        /// Przechowuje storyboard dla animacji koloru elementow paska funkcyjnego (FunctionBar)
        /// </summary>
        Storyboard v_storyboardFunctionBarColor;

        /// <summary>
        /// Przechowuje storyboard dla animacji poszczegolnych kart
        /// </summary>
        Storyboard v_storyBoardTab;

        /// <summary>
        /// Przechowuje animacje menu funkcyjnego
        /// </summary>
        DoubleAnimation v_animationBox;

        /// <summary>
        /// Przechowuje animacje zmiany kolory dla elementow paska funkcyjnego
        /// </summary>
        ColorAnimation v_animationFunctionBarColor;

        /// <summary>
        /// Przechowuje animacje dla zwijania/rozwijania kart
        /// </summary>
        DoubleAnimation v_animationTab;

        /// <summary>
        /// Zawiera napis ktory pojawia sie na przycisku maksymalizacji
        /// </summary>
        string v_maximize_text = "Maksymalizuj";

        string v_textEnterNumber = "Wprowadź numer karty kredytowej do walidacji.";
        string v_textNumberInvalid = "Wprowadzony numer jest niepoprawny.";
        string v_textAmericanExpres = "Podany numer karty jest poprawny - American Express.";
        string v_textDinersClub = "Podany numer karty jest poprawny - Diners Club.";
        string v_textDiscover = "Podany numer karty jest poprawny - Disover.";
        string v_textJCB = "Podany numer karty jest poprawny - JCB.";
        string v_textMasterCard = "Podany numer karty jest poprawny - Master Card.";
        string v_textVisa = "Podany numer karty jest poprawny - Visa.";
        string v_textFileSygnature = "### Plik wygenerowany przez aplikacje: CreditCard Manager. ###";


        Regex v_regexVisa;
        Regex v_regexMasterCard;
        Regex v_regexAmericanExpres;
        Regex v_regexDinersClub;
        Regex v_regexDiscover;
        Regex v_regexJCB;


        //==================================================================================================================
        //=============================================== PROPERTY =========================================================
        //==================================================================================================================

        /// <summary>
        /// Czy przycisk zamknij (prawy gorny rog) zostal kilkniety - sluzy dla sprawdzania animacji
        /// </summary>
        public bool p_isCloseClicked { get; private set; } = false;

        /// <summary>
        /// Czy okno jest zmaksymalizowane
        /// </summary>
        public bool p_isMaximized { get; private set; } = false;

        /// <summary>
        /// Czy DropDown menu przycisku Help (prawy gorny rog) jest aktywne
        /// </summary>
        public bool p_isDropDownHelpActived { get; set; } = false;

        /// <summary>
        /// Czy karta sprawdzania numerow (po lewej) jest aktywna
        /// </summary>
        public bool p_isTabCheckOpen { get; set; } = false;

        /// <summary>
        /// Czy karta z listami numerow (po prawej) jest aktywna
        /// </summary>
        public bool p_isTabListOpen { get; set; } = false;

        /// <summary>
        /// Czy karta z podpowiedziami (dolna) jest aktywna
        /// </summary>
        public bool p_isTabTIPPOpen { get; set; } = false;

        //==================================================================================================================
        //======================================= CONSTRUCTOR / DESTRUCTOR =================================================
        //==================================================================================================================

        public MainWindow()
        {
            InitializeComponent();

            ((INotifyCollectionChanged)xe_ListTab_ListBox.Items).CollectionChanged +=
        xe_ListTab_ListBox_OnCollectionChange;

            xe_TIPPbar_Grid.Height = 0;
            xe_CheckTab_Grid.Width = 0;
            xe_ListTab_Grid.Width = 0;

            v_storyBoardBox = new Storyboard();
            v_storyboardFunctionBarColor = new Storyboard();
            v_storyBoardTab = new Storyboard();

            v_animationBox = new DoubleAnimation();
            v_animationFunctionBarColor = new ColorAnimation();
            v_animationTab = new DoubleAnimation();

            v_storyBoardBox.Children.Add(v_animationBox);
            v_storyboardFunctionBarColor.Children.Add(v_animationFunctionBarColor);
            v_storyBoardTab.Children.Add(v_animationTab);


            v_animationBox.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            v_animationFunctionBarColor.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            v_animationTab.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            CircleEase ease = new CircleEase();
            ease.EasingMode = EasingMode.EaseInOut;
            v_animationTab.EasingFunction = ease;


            Storyboard.SetTargetProperty(v_animationBox, new PropertyPath(WidthProperty));
            Storyboard.SetTargetProperty(v_animationFunctionBarColor, new PropertyPath("(Panel.Background).(SolidColorBrush.Color)"));

            xe_CheckTab_TEXTBOX.Text = v_textEnterNumber;

            v_regexAmericanExpres = new Regex("^3[47][0-9]{13}$");
            v_regexDinersClub = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
            v_regexDiscover = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
            v_regexJCB = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");
            v_regexMasterCard = new Regex("^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$");
            v_regexVisa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");

            xe_CheckTab_TEXTBOX.TextChanged += em_CheckTab_TEXTBOX_OnTextChange;
        }

       



        //==================================================================================================================
        //============================================= STD METHODS ========================================================
        //==================================================================================================================



        /// <summary>
        /// Animuje menu funkcyjne (prawy gorny rog) defaultowo po najechaniu na nie kursorem.
        /// </summary>
        /// <param name="obiekt">Wskaz obiekt ktory ma byc animowany</param>
        /// <param name="text">Podaj tekst jaki ma zastapic symbol</param>
        /// <param name="kolor">Podaj kolor tla animowanego obiektu</param>
        /// <param name="nowyRozmiar">Podaj dlugosc docelowa przy ktorej konczy sie animacja</param>
        private void sm_functionMenuAnimation(TextBlock obiekt, string text, Brush kolor, double nowyRozmiar = 24)
        {
            Storyboard.SetTargetName(v_animationBox, obiekt.Name); // na jakim obiekcie wykonac?
            //v_animationBox.From = obiekt.Width; //CZY TO MUSI BYC??
            v_animationBox.To = nowyRozmiar;
            obiekt.Text = text;
            obiekt.Background = kolor;
            v_storyBoardBox.Begin(this);
            
        }

        private void sm_functionBarColorAnimation(Grid obiekt, Color color)
        {
            Storyboard.SetTargetName(v_animationFunctionBarColor, obiekt.Name);
            //v_animationFunctionBarColor.From = obiekt.Background;//obiekt.Background;
            v_animationFunctionBarColor.To = color;
            v_storyboardFunctionBarColor.Begin(this);
        }

        private void sm_tabAnimation(Grid tab, double targetSize, PropertyPath propertyPath)
        {
            
            Storyboard.SetTargetName(v_animationTab, tab.Name);
            v_animationTab.To = targetSize;
            Storyboard.SetTargetProperty(v_animationTab, propertyPath);
            v_storyBoardTab.Begin(this);
            
        }

        //==================================================================================================================
        //============================================= EVENT METHODS ======================================================
        //==================================================================================================================

        private void em_textBlock_Close_upperRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!p_isDropDownHelpActived)
            {
                p_isCloseClicked = true;
                if (MessageBox.Show("Czy na pewno chcesz zamknąć aplikacje 'CreditCard Manager'?", "Prosba o potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) Application.Current.Shutdown();
                else
                {
                    p_isCloseClicked = false;
                    sm_functionMenuAnimation(xe_textBlock_Close_upperRight, "X", xe_Grid_GornaBelka.Background);
                } 
            }
        }

        private void em_textBlock_Close_upperRight_MouseEnter(object sender, MouseEventArgs e)
        {
            if(!p_isDropDownHelpActived)
            { 
                sm_functionMenuAnimation(xe_textBlock_Close_upperRight, "Zamknij", Brushes.PaleVioletRed, 80);
            }
        }

        private void em_textBlock_Close_upperRight_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!p_isCloseClicked)sm_functionMenuAnimation(xe_textBlock_Close_upperRight, "X", xe_Grid_GornaBelka.Background, 24);
        }

        private void em_textBlock_Maximize_upperRight_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!p_isDropDownHelpActived)
            {
                sm_functionMenuAnimation(xe_textBlock_Maximize_upperRight, v_maximize_text, Brushes.LightBlue, 110); 
            }
        }

        private void em_textBlock_Maximize_upperRight_MouseLeave(object sender, MouseEventArgs e)
        {
            sm_functionMenuAnimation(xe_textBlock_Maximize_upperRight, "[  ]", xe_Grid_GornaBelka.Background);
        }

        private void em_textBlock_Maximize_upperRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!p_isDropDownHelpActived)
            {
                if (!p_isMaximized)
                {
                    WindowState = WindowState.Maximized;
                    p_isMaximized = true;
                    v_maximize_text = "Przywroc w dol";
                }
                else
                {
                    WindowState = WindowState.Normal;
                    p_isMaximized = false;
                    v_maximize_text = "Maksymalizuj";
                }
                
            }
        }

        private void em_textBlock_Minimize_upperRight_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!p_isDropDownHelpActived)
            {
                sm_functionMenuAnimation(xe_textBlock_Minimize_upperRight, "Minimalizuj", Brushes.LightGreen, 100); 
            }
        }

        private void em_textBlock_Minimize_upperRight_MouseLeave(object sender, MouseEventArgs e)
        {
            sm_functionMenuAnimation(xe_textBlock_Minimize_upperRight, "__", xe_Grid_GornaBelka.Background);
        }

        private void em_textBlock_Minimize_upperRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!p_isDropDownHelpActived)
            {
                WindowState = WindowState.Minimized; 
            }
        }

        private void em_textBlock_Help_upperRight_MouseEnter(object sender, MouseEventArgs e)
        {
            sm_functionMenuAnimation(xe_textBlock_Help_upperRight, "Pomoc", Brushes.Orange, 70);
            xe_textBlock_Help_upperRight.FontSize = (double)new FontSizeConverter().ConvertFrom("12pt");
        }

        private void em_textBlock_Help_upperRight_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!p_isDropDownHelpActived)
            { 
            sm_functionMenuAnimation(xe_textBlock_Help_upperRight, "?", xe_Grid_GornaBelka.Background);
            xe_textBlock_Help_upperRight.FontSize = (double)new FontSizeConverter().ConvertFrom("14pt");
            }
        }

        private void em_textBlock_Settings_upperRight_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!p_isDropDownHelpActived)
            {
                sm_functionMenuAnimation(xe_textBlock_Settings_upperRight, "Ustawienia", Brushes.LightGray, 80);
                xe_textBlock_Settings_upperRight.FontSize = (double)new FontSizeConverter().ConvertFrom("12pt"); 
            }
        }

        private void em_textBlock_Settings_upperRight_MouseLeave(object sender, MouseEventArgs e)
        {
            sm_functionMenuAnimation(xe_textBlock_Settings_upperRight, "*", xe_Grid_GornaBelka.Background);
            xe_textBlock_Settings_upperRight.FontSize = (double)new FontSizeConverter().ConvertFrom("18pt");
        }

        private void em_textBlock_Help_upperRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!p_isDropDownHelpActived) 
            {
                xe_DropDown_Help.Height = 48;
                p_isDropDownHelpActived = true;
            }
            else
            {
                xe_DropDown_Help.Height = 0;
                p_isDropDownHelpActived = false;
                
            }
            
        }

        private void em_textBlock_DDMenu_About_MouseEnter(object sender, MouseEventArgs e)
        {
            xe_textBlock_DDMenu_About.Background = Brushes.LightBlue;
        }

        private void em_textBlock_DDMenu_About_MouseLeave(object sender, MouseEventArgs e)
        {
            xe_textBlock_DDMenu_About.Background = xe_Grid_GornaBelka.Background;

        }

        private void em_Window_main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source != xe_textBlock_DDMenu_About && e.Source != xe_textBlock_DDMenu_help && e.Source != xe_textBlock_Help_upperRight)
            {
                p_isDropDownHelpActived = false;
                xe_DropDown_Help.Height = 0;
                sm_functionMenuAnimation(xe_textBlock_Help_upperRight, "?", xe_Grid_GornaBelka.Background);
            }
        }

        private void em_DDMenu_Help_MouseEnter(object sender, MouseEventArgs e)
        {
            xe_textBlock_DDMenu_help.Background = Brushes.LightBlue;
            
        }

        private void em_DDMenu_Help_MouseLeave(object sender, MouseEventArgs e)
        {
            xe_textBlock_DDMenu_help.Background = xe_Grid_GornaBelka.Background;
        }

        private void em_FunctionBar_Check_OnMouseEnter(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_Check, Colors.RoyalBlue);
        }

        private void em_FunctionBar_Check_OnMouseLeave(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_Check, Colors.LightGray);
        }

        private void em_GornaBelka_OnClick(object sender, MouseButtonEventArgs e)
        {
            if (!p_isCloseClicked)
            {
                try
                {
                    base.OnMouseLeftButtonDown(e);
                    DragMove();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void em_FunctionBar_List_OnMouseEnter(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_List, Colors.RoyalBlue);
        }

        private void em_FunctionBar_List_OnMouseLeave(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_List, Colors.LightGray);
        }

        private void em_FunctionBar_TIPP_OnMouseEnter(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_TIPP, Colors.RoyalBlue);
        }

        private void em_FunctionBar_TIPP_OnMouseLeave(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_TIPP, Colors.LightGray);
        }

        private void em_FunctionBar_LoadSingle_OnMouseEnter(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_LoadSingle, Colors.RoyalBlue);
        }

        private void em_FunctionBar_LoadSingle_OnMouseLeave(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_LoadSingle, Colors.LightGray);
        }

        private void em_FunctionBar_LoadList_OnMouseEnter(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_LoadList, Colors.RoyalBlue);
        }

        private void emFunctionBar_LoadList_OnMouseEnter(object sender, MouseEventArgs e)
        {
            sm_functionBarColorAnimation(xe_FunctionBar_Grid_LoadList, Colors.LightGray);
        }

        private void em_Window_OnResize(object sender, SizeChangedEventArgs e)
        {
            
            if (p_isTabCheckOpen) 
            {
                
                sm_tabAnimation(xe_CheckTab_Grid, Width / 2, new PropertyPath(WidthProperty));
                if (p_isTabTIPPOpen) 
                {
                    
                    sm_tabAnimation(xe_CheckTab_Grid, Height - 212, new PropertyPath(HeightProperty));
                }
                else 
                {
                    
                    sm_tabAnimation(xe_CheckTab_Grid, Height - 82, new PropertyPath(HeightProperty));
                }
                
            }

            if (p_isTabListOpen)
            {

                sm_tabAnimation(xe_ListTab_Grid, Width / 2, new PropertyPath(WidthProperty));
                if (p_isTabTIPPOpen)
                {
                    //xe_ListTab_Grid.Height = Height - 212;
                    sm_tabAnimation(xe_ListTab_Grid, Height - 212, new PropertyPath(HeightProperty));
                }
                else
                {
                    //xe_ListTab_Grid.Height = Height - 82;
                    sm_tabAnimation(xe_ListTab_Grid, Height - 82, new PropertyPath(HeightProperty));
                }
                
            }
            
        }

        private void em_FunctionBar_Check_OnClick(object sender, MouseButtonEventArgs e)
        {
            if(p_isTabCheckOpen)
            {
                sm_tabAnimation(xe_CheckTab_Grid, 0, new PropertyPath(WidthProperty));
                p_isTabCheckOpen = false;
            }
            else
            {
                if (p_isTabTIPPOpen)
                {
                    //xe_CheckTab_Grid.Height = Height - 212;
                    sm_tabAnimation(xe_CheckTab_Grid, Height - 212, new PropertyPath(HeightProperty));
                    sm_tabAnimation(xe_CheckTab_Grid, Width/2, new PropertyPath(WidthProperty));
                    p_isTabCheckOpen = true; 
                }
                else
                {
                   // xe_CheckTab_Grid.Height = Height - 82;
                    sm_tabAnimation(xe_CheckTab_Grid, Height - 82, new PropertyPath(HeightProperty));
                    sm_tabAnimation(xe_CheckTab_Grid, Width/2, new PropertyPath(WidthProperty));
                    p_isTabCheckOpen = true;
                }
            }
        }

        private void em_FunctionBar_List_OnClick(object sender, MouseButtonEventArgs e)
        {
            if (p_isTabListOpen)
            {
                sm_tabAnimation(xe_ListTab_Grid, 0, new PropertyPath(WidthProperty));
                p_isTabListOpen = false;
            }
            else
            {
                if (p_isTabTIPPOpen)
                {
                    //xe_ListTab_Grid.Height = Height - 212;
                    sm_tabAnimation(xe_ListTab_Grid, Height - 212, new PropertyPath(HeightProperty));
                    sm_tabAnimation(xe_ListTab_Grid, Width / 2, new PropertyPath(WidthProperty));
                    p_isTabListOpen = true;
                }
                else
                {
                    //xe_ListTab_Grid.Height = Height - 82;
                    sm_tabAnimation(xe_ListTab_Grid, Width / 2, new PropertyPath(WidthProperty));
                    sm_tabAnimation(xe_ListTab_Grid, Height - 82, new PropertyPath(HeightProperty));
                    p_isTabListOpen = true;
                }
            }
        }

        private void em_FunctionBar_TIPP_OnClick(object sender, MouseButtonEventArgs e)
        {

            if (p_isTabTIPPOpen)
            {
                if (p_isTabListOpen || p_isTabCheckOpen)
                {
                    sm_tabAnimation(xe_TIPPbar_Grid, 0, new PropertyPath(HeightProperty));
                    sm_tabAnimation(xe_CheckTab_Grid, Height - 82, new PropertyPath(HeightProperty));
                    sm_tabAnimation(xe_ListTab_Grid, Height - 82, new PropertyPath(HeightProperty));
                }
                else
                {
                    sm_tabAnimation(xe_TIPPbar_Grid, 0, new PropertyPath(HeightProperty));
                    
                }
                p_isTabTIPPOpen = false;
            }
            else
            {
                if (p_isTabListOpen || p_isTabCheckOpen)
                {
                    sm_tabAnimation(xe_TIPPbar_Grid, 130, new PropertyPath(HeightProperty));
                    sm_tabAnimation(xe_CheckTab_Grid, Height - 212, new PropertyPath(HeightProperty));
                    sm_tabAnimation(xe_ListTab_Grid, Height - 212, new PropertyPath(HeightProperty));
                }
                else
                {
                    sm_tabAnimation(xe_TIPPbar_Grid, 130, new PropertyPath(HeightProperty));

                }
                p_isTabTIPPOpen = true;
            }
        }

        private void em_CheckTab_ButtonApply_OnClick(object sender, RoutedEventArgs e)
        {

            if(!xe_ListTab_ListBox.Items.Contains(xe_CheckTab_TEXTBOX.Text))
            {
                xe_ListTab_ListBox.Items.Add(xe_CheckTab_TEXTBOX.Text);
            }
            else
            {
                MessageBox.Show("Wprowadzony numer jest już na liśćie.", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
                
        }

        private void em_CheckTab_TEXTBOX_OnTextChange(object sender, TextChangedEventArgs e)
        {
            

            //REGEX
            if (v_regexAmericanExpres.IsMatch(xe_CheckTab_TEXTBOX.Text))
            {
                xe_CheckTab_TEXTBOX.Background = Brushes.LightGreen;
                xe_CheckTab_TextBlock_Output.Foreground = Brushes.Green;
                xe_CheckTab_TextBlock_Output.Text = v_textAmericanExpres;
                xe_CheckTab_Button_Apply.IsEnabled = true;
                xe_CheckTab_Button_Save.IsEnabled = true;
            }
            else if(v_regexDinersClub.IsMatch(xe_CheckTab_TEXTBOX.Text))
            {
                xe_CheckTab_TEXTBOX.Background = Brushes.LightGreen;
                xe_CheckTab_TextBlock_Output.Foreground = Brushes.Green;
                xe_CheckTab_TextBlock_Output.Text = v_textDinersClub;
                xe_CheckTab_Button_Apply.IsEnabled = true;
                xe_CheckTab_Button_Save.IsEnabled = true;
            }
            else if (v_regexDiscover.IsMatch(xe_CheckTab_TEXTBOX.Text))
            {
                xe_CheckTab_TEXTBOX.Background = Brushes.LightGreen;
                xe_CheckTab_TextBlock_Output.Foreground = Brushes.Green;
                xe_CheckTab_TextBlock_Output.Text = v_textDiscover;
                xe_CheckTab_Button_Apply.IsEnabled = true;
                xe_CheckTab_Button_Save.IsEnabled = true;
            }
            else if (v_regexJCB.IsMatch(xe_CheckTab_TEXTBOX.Text))
            {
                xe_CheckTab_TEXTBOX.Background = Brushes.LightGreen;
                xe_CheckTab_TextBlock_Output.Foreground = Brushes.Green;
                xe_CheckTab_TextBlock_Output.Text = v_textJCB;
                xe_CheckTab_Button_Apply.IsEnabled = true;
                xe_CheckTab_Button_Save.IsEnabled = true;
            }
            else if (v_regexMasterCard.IsMatch(xe_CheckTab_TEXTBOX.Text))
            {
                xe_CheckTab_TEXTBOX.Background = Brushes.LightGreen;
                xe_CheckTab_TextBlock_Output.Foreground = Brushes.Green;
                xe_CheckTab_TextBlock_Output.Text = v_textMasterCard;
                xe_CheckTab_Button_Apply.IsEnabled = true;
                xe_CheckTab_Button_Save.IsEnabled = true;
            }
            else if (v_regexVisa.IsMatch(xe_CheckTab_TEXTBOX.Text))
            {
                xe_CheckTab_TEXTBOX.Background = Brushes.LightGreen;
                xe_CheckTab_TextBlock_Output.Foreground = Brushes.Green;
                xe_CheckTab_TextBlock_Output.Text = v_textVisa;
                xe_CheckTab_Button_Apply.IsEnabled = true;
                xe_CheckTab_Button_Save.IsEnabled = true;
            }
            else
            {
                xe_CheckTab_TEXTBOX.Background = Brushes.Red;
                xe_CheckTab_TextBlock_Output.Foreground = Brushes.Red;
                xe_CheckTab_TextBlock_Output.Text = v_textNumberInvalid;
                xe_CheckTab_Button_Apply.IsEnabled = false;
                xe_CheckTab_Button_Save.IsEnabled = false;
            }

            //CLEAR
            if (xe_CheckTab_TEXTBOX.Text != v_textEnterNumber && xe_CheckTab_TEXTBOX.Text != "")
            {
                xe_CheckTab_Button_Clear.IsEnabled = true;
            }
            else
            {
                xe_CheckTab_Button_Clear.IsEnabled = false;
                xe_CheckTab_TEXTBOX.Background = Brushes.White;
                xe_CheckTab_TextBlock_Output.Foreground = Brushes.Black;
                xe_CheckTab_TextBlock_Output.Text = v_textEnterNumber;
            }

        }

        private void em_CheckTab_ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            xe_CheckTab_TEXTBOX.Text = v_textEnterNumber;
        }

        private void em_CheckTab_TEXTBOX_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (xe_CheckTab_TEXTBOX.Text == "") xe_CheckTab_TEXTBOX.Text = v_textEnterNumber;
        }

        private void TEST(object sender, RoutedEventArgs e) //TEXTBOX OOnFocus
        {
            if(xe_CheckTab_TEXTBOX.Text == v_textEnterNumber)xe_CheckTab_TEXTBOX.Text = "";
        }

        private void em_ListTab_UsunButton_OnClick(object sender, RoutedEventArgs e)
        {
            xe_ListTab_ListBox.Items.Remove(xe_ListTab_ListBox.SelectedItem);
        }

        private void em_ListTab_ListBox_OnSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if(xe_ListTab_ListBox.SelectedItem == null)
            {
                xe_ListTab_Button_Usun.IsEnabled = false;
            }
            else
            {
                xe_ListTab_Button_Usun.IsEnabled = true;
            }
        }

        private void em_ListTab_Clear_OnClick(object sender, RoutedEventArgs e)
        {

            if (xe_ListTab_ListBox.Items.Count > 0)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć wszystkie wpisy na liście?", "Potwierdź", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    xe_ListTab_ListBox.Items.Clear();
                } 
            }
            else
            {
                MessageBox.Show("Lista jest już pusta.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void xe_ListTab_ListBox_OnCollectionChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (xe_ListTab_ListBox.Items.Count > 0) xe_ListTab_Button_Save.IsEnabled = true;
            else xe_ListTab_Button_Save.IsEnabled = false;
        }

        private void em_CheckTab_TEXTBOX_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && xe_CheckTab_Button_Apply.IsEnabled == true)
            {
                em_CheckTab_ButtonApply_OnClick(null, null);
            }
        }

        private void em_CheckTab_Save_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = "MyCreditCardNumber"; // Default file name
            saveDialog.DefaultExt = ".txt"; // Default file extension
            saveDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            string output = xe_CheckTab_TEXTBOX.Text + Environment.NewLine + v_textFileSygnature;
            if (saveDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveDialog.FileName, output);
            }
        }

        private void em_ListTab_ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = "MyCreditCardNumberList"; // Default file name
            saveDialog.DefaultExt = ".txt"; // Default file extension
            saveDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
                                                               //string output = xe_CheckTab_TEXTBOX.Text + Environment.NewLine + "### Plik wygenerowany przez aplikacje: CreditCard Manager. ###";
            string output = "";
            foreach (string item in xe_ListTab_ListBox.Items)
            {
                output += (item + Environment.NewLine);
            }
            output += Environment.NewLine + v_textFileSygnature;
            if (saveDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveDialog.FileName, output);
            }
        }

        private void em_FunctionBar_LoadSingle_OnClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //xe_CheckTab_TEXTBOX.Text = File.ReadAllText(openFileDialog.FileName);
                
                StreamReader reader = new StreamReader(openFileDialog.FileName);
                
                xe_CheckTab_TEXTBOX.Text = reader.ReadLine(); //zczytujemy pierwsza linie
                reader.Close(); //za niezamykanie plikow mozna trafic do piekla ;(
            }
                

        }

        private void em_FunctionBar_LoadList_OnClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if(openDialog.ShowDialog() == true)
            {
                string line = "Hello World :-)";
                StreamReader reader = new StreamReader(openDialog.FileName);
                while (line != null)
                {
                    line = reader.ReadLine();
                    if(line != v_textFileSygnature && line != "")xe_ListTab_ListBox.Items.Add(line);
                }

                reader.Close();
            }
        }

        private void em_GornaBelka_Settings_OnClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Aplikacja nie posiada żadnych opcji do ustawienia", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void em_GornaBelka_About_OnClick(object sender, MouseButtonEventArgs e)
        {
            p_isDropDownHelpActived = false;
            xe_DropDown_Help.Height = 0;
            sm_functionMenuAnimation(xe_textBlock_Help_upperRight, "?", xe_Grid_GornaBelka.Background);

            About about = new About();
            about.ShowDialog();
        }

        private void em_GornaBelka_Help_OnClick(object sender, MouseButtonEventArgs e)
        {
            p_isDropDownHelpActived = false;
            xe_DropDown_Help.Height = 0;
            sm_functionMenuAnimation(xe_textBlock_Help_upperRight, "?", xe_Grid_GornaBelka.Background);

            MessageBox.Show("Aby uzyskać pomoc kliknij niebieką ikonę z literą 'i' po lewej stronie, zaraz poniżej paska tytułu.","Podpowiedź",MessageBoxButton.OK,MessageBoxImage.Information);
        }















        //==================================================================================================================
        //============================================= UNCATEGORIZED ======================================================
        //==================================================================================================================


    }
}

