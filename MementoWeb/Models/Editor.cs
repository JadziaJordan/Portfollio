namespace MementoWeb.Models
{
    public class Editor
    {
        public string TextContent { get; set; }

        // Method to save the current state (create a snapshot)
        public TextSnapshot CreateSnapshot()
        {
            return new TextSnapshot(TextContent);  // Save the current state
        }

        // Method to restore the editor's state from a snapshot
        public void RestoreState(TextSnapshot snapshot)
        {
            TextContent = snapshot.TextContent;  // Restore the saved state
        }
    }
}
