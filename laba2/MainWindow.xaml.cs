using System;
using System.Data; ///для вычисления матем. выражения
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel; ///Работа с коллекциями

namespace HelloWorldWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> HistoryEntries { get; } = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement el in MainRoot.Children) { 
            if(el is Button)
                {
                    ((Button)el).Click += Button_Click; /// подписка на событие - при клике выполняется метод Button_Click
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = (string)((Button)e.OriginalSource).Content;
            if (str == "C") {
                textLabel.Text = "";
            }
            else if (str == "H")
            {
                
            }
            else if (str == "=")
            {
                string expression = textLabel.Text;
                string comma = textLabel.Text.Replace(",",".");
                string result = new DataTable().Compute(comma, null).ToString();
                textLabel.Text = result.Replace(".", ",");
                HistoryEntries.Add($"{expression} = {result.Replace(".", ",")}");
            }
            else 
                textLabel.Text += str;
        }
        private void history_Click(object sender, RoutedEventArgs e)
        {
            Hist1 histo = new Hist1();
            histo.DataContext = this;
            histo.Show();
        }

    }
}
