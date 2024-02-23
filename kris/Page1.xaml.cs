using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

public class Note
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}

public class JsonSerializer
{
    public static void Serialize(List<Note> notes, string filePath)
    {
        string json = JsonConvert.SerializeObject(notes);
        File.WriteAllText(filePath, json);
    }

    public static List<Note> Deserialize(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Note>>(json);
        }
        return new List<Note>();
    }
}

public partial class MainWindow : Window
{
    private List<Note> notes;
    private string filePath = "notes.json";

    public MainWindow()
    {
        InitializeComponent();
        notes = JsonSerializer.Deserialize(filePath);
        UpdateNotesList();
    }

    private void UpdateNotesList()
    {
        notesList.ItemsSource = null;
        notesList.ItemsSource = notes;
    }

    private void addNoteButton_Click(object sender, RoutedEventArgs e)
    {
        Note newNote = new Note
        {
            Title = titleTextBox.Text,
            Description = descriptionTextBox.Text,
            DueDate = datePicker.SelectedDate ?? DateTime.Now
        };
        notes.Add(newNote);
        JsonSerializer.Serialize(notes, filePath);
        UpdateNotesList();
    }

    private void editNoteButton_Click(object sender, RoutedEventArgs e)
    {
        if (notesList.SelectedItem is Note selectedNote)
        {
            selectedNote.Title = titleTextBox.Text;
            selectedNote.Description = descriptionTextBox.Text;
            selectedNote.DueDate = datePicker.SelectedDate ?? DateTime.Now;
            JsonSerializer.Serialize(notes, filePath);
            UpdateNotesList();
        }
    }

    private void deleteNoteButton_Click(object sender, RoutedEventArgs e)
    {
        if (notesList.SelectedItem is Note selectedNote)
        {
            notes.Remove(selectedNote);
            JsonSerializer.Serialize(notes, filePath);
            UpdateNotesList();
        }
    }

    private void notesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (notesList.SelectedItem is Note selectedNote)
        {
            titleTextBox.Text = selectedNote.Title;
            descriptionTextBox.Text = selectedNote.Description;
            datePicker.SelectedDate = selectedNote.DueDate;
        }
    }
}
