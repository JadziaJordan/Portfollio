namespace MementoWeb.Models
{
    public class TextSnapshot
    //memento
    {
        public string TextContent { get; }

        public TextSnapshot(string textContent)
        {
            TextContent = textContent;  // Store the state of the text content
        }
    }
}
