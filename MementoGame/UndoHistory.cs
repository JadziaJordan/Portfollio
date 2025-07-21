namespace MementoGame
{
    // Caretaker Class (Keeps Track of Saved States)
    public class UndoHistory
    {
        private readonly List<TextSnapshot> _savedStates = new List<TextSnapshot>();  // A list to store the snapshots (history)

        // Save a snapshot (store the current state)
        public void SaveSnapshot(TextSnapshot snapshot)
        {
            _savedStates.Add(snapshot);  // Add the snapshot to history
        }

        // Retrieve a previous snapshot by index (undo action)
        public TextSnapshot GetSnapshot(int index)
        {
            return _savedStates[index];  // Return the snapshot at the given index
        }
    }
}
