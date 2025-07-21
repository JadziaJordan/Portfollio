namespace MementoGame
{
    // Memento Class (Stores a Snapshot of the Text)
    public class TextSnapshot
    {
        public string TextContent { get; }  // Holds the text content at the time it was saved

        // Constructor: Takes a snapshot of the text content
        public TextSnapshot(string textContent)
        {
            TextContent = textContent;
        }
    }
}