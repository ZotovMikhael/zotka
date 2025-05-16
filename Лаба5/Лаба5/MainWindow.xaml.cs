using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace лаба5
{
    public partial class MainWindow : Window
    {
        // Объявление команд
        public static RoutedCommand NewCommand = new RoutedCommand();
        public static RoutedCommand OpenCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();

        private string currentFilePath = null;
        private bool isUnsavedChanges = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeCommands();
            NoteTextBox.TextChanged += NoteTextBox_TextChanged;
        }

        private void InitializeCommands()
        {
            // Назначение горячих клавиш
            NewCommand.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        }

        private void NoteTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            isUnsavedChanges = true;
        }

        private void AddButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckUnsavedChanges())
            {
                currentFilePath = null;
                NoteTextBox.Clear();
                isUnsavedChanges = false;
            }
        }

        private void SaveButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFile();
        }

        private void OpenButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (!CheckUnsavedChanges()) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    currentFilePath = openFileDialog.FileName;
                    NoteTextBox.Text = File.ReadAllText(currentFilePath);
                    isUnsavedChanges = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CheckUnsavedChanges())
            {
                e.Cancel = true;
            }
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == true)
                {
                    currentFilePath = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            try
            {
                File.WriteAllText(currentFilePath, NoteTextBox.Text);
                isUnsavedChanges = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CheckUnsavedChanges()
        {
            if (isUnsavedChanges)
            {
                var result = MessageBox.Show("Сохранить изменения?", "Есть несохраненные изменения",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                    return !isUnsavedChanges;
                }
                else if (result == MessageBoxResult.No)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}