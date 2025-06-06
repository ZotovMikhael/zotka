using System.Text.Json;
using KursovayaServer.Models;
using Microsoft.Extensions.Logging;

namespace KursovayaServer.Services;

public class DataService
{
    private readonly string _dataFile = "notes.json";
    private List<Note> _notes;
    private readonly ILogger<DataService> _logger;
    private readonly object _lockObject = new object();

    public DataService(ILogger<DataService> logger)
    {
        _logger = logger;
        _notes = LoadData();
    }

    private List<Note> LoadData()
    {
        lock (_lockObject)
        {
            try
            {
                if (File.Exists(_dataFile))
                {
                    _logger.LogInformation("Loading notes from file: {FilePath}", _dataFile);
                    var json = File.ReadAllText(_dataFile);
                    var notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
                    _logger.LogInformation("Loaded {Count} notes", notes.Count);
                    return notes;
                }
                _logger.LogInformation("Notes file not found, creating new list");
                return new List<Note>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading notes from file");
                return new List<Note>();
            }
        }
    }

    private void SaveData()
    {
        lock (_lockObject)
        {
            try
            {
                _logger.LogInformation("Saving {Count} notes to file", _notes.Count);
                var json = JsonSerializer.Serialize(_notes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_dataFile, json);
                _logger.LogInformation("Notes saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving notes to file");
                throw; // Пробрасываем исключение дальше, чтобы контроллер мог обработать ошибку
            }
        }
    }

    public List<Note> GetUserNotes(string userId)
    {
        _logger.LogInformation("Getting notes for user: {UserId}", userId);
        var notes = _notes.Where(n => n.UserId == userId).ToList();
        _logger.LogInformation("Found {Count} notes for user {UserId}", notes.Count, userId);
        return notes;
    }

    public Note AddNote(Note note)
    {
        _logger.LogInformation("Adding new note for user {UserId}", note.UserId);
        note.Id = _notes.Count > 0 ? _notes.Max(n => n.Id) + 1 : 1;
        _notes.Add(note);
        SaveData();
        _logger.LogInformation("Note {Id} added successfully", note.Id);
        return note;
    }

    public bool UpdateNote(Note note)
    {
        _logger.LogInformation("Updating note {Id}", note.Id);
        var existing = _notes.FirstOrDefault(n => n.Id == note.Id);
        if (existing == null)
        {
            _logger.LogWarning("Note {Id} not found", note.Id);
            return false;
        }

        existing.Title = note.Title;
        existing.Content = note.Content;
        SaveData();
        _logger.LogInformation("Note {Id} updated successfully", note.Id);
        return true;
    }

    public bool DeleteNote(int id)
    {
        _logger.LogInformation("Deleting note {Id}", id);
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note == null)
        {
            _logger.LogWarning("Note {Id} not found", id);
            return false;
        }

        _notes.Remove(note);
        SaveData();
        _logger.LogInformation("Note {Id} deleted successfully", id);
        return true;
    }
} 