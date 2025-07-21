namespace MementoWeb.Models
{
    public class UndoHistory
    {
        private List<TextSnapshot> _history = new List<TextSnapshot>();

        // Save a snapshot in history
        public void SaveSnapshot(TextSnapshot snapshot)
        {
            _history.Add(snapshot);  // Store the snapshot for future undo
        }

        // Retrieve a snapshot by index
        public TextSnapshot GetSnapshot(int index)
        {
            return _history.ElementAtOrDefault(index);  // Get a saved snapshot
        }

        // Get all saved snapshots for displaying version history
        public List<TextSnapshot> GetAllSnapshots()
        {
            return _history;
        }

        public void RemoveSnapshot(int index)
{
    if (index >= 0 && index < _history.Count)
    {
        _history.RemoveAt(index);
    }
}

    }
}
