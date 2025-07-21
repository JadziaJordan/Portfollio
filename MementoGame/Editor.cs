namespace MementoGame
{
    // Originator Class (Text Editor - Can Save and Restore Its State)
    public class Editor
    {
        public string TextContent { get; set; }  // The text content of the editor

        // Save the current state into a TextSnapshot (like saving the current version of text)
        public TextSnapshot SaveState()
        {
            Console.WriteLine($" Saving Text: {TextContent}");
            return new TextSnapshot(TextContent); //Calls other class // Create and return a snapshot of the current text
        }

        // Restore the state from a given TextSnapshot (like loading a previous version of the text)
        public void RestoreState(TextSnapshot snapshot)
        {
            TextContent = snapshot.TextContent;  // Set the editor's text back to the snapshot's content
            Console.WriteLine($" Restoring Text: {TextContent}");
        }
    }
}
