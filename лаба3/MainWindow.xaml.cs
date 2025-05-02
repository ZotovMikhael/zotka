using System;
using System.Collections.Generic;
using System.IO;
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

namespace лаба3
{
    public partial class MainWindow : Window
    {
        private const string SaveFilePath = "notes.txt"; // Правильное объявление пути
        private List<string> notes = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();
        }

        private void LoadNotes()
        {
            // Используем SaveFilePath вместо file
            if (File.Exists(SaveFilePath))
            {
                notes = new List<string>(File.ReadAllLines(SaveFilePath));
                NotesListBox.ItemsSource = notes;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Используем SaveFilePath вместо file
            File.WriteAllLines(SaveFilePath, notes);
        }

        // Остальные методы без изменений
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NoteTextBox.Text))
            {
                notes.Add(NoteTextBox.Text);
                NotesListBox.Items.Refresh();
                NoteTextBox.Clear();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(NoteTextBox.Text))
            {
                notes[NotesListBox.SelectedIndex] = NoteTextBox.Text;
                NotesListBox.Items.Refresh();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedIndex != -1)
            {
                notes.RemoveAt(NotesListBox.SelectedIndex);
                NotesListBox.Items.Refresh();
                NoteTextBox.Clear();
            }
        }

        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotesListBox.SelectedIndex != -1)
            {
                NoteTextBox.Text = notes[NotesListBox.SelectedIndex];
            }
        }
    }
}
