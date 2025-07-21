using System;

namespace MementoGame
{
    class Program
    {
        static void Main()
        {
            // Create an instance of the Text Editor (Originator)
            var editor = new Editor();

            // Create an instance of the Undo History (Caretaker)
            var history = new UndoHistory();

            // 1️⃣ Set Initial Text & Save it (Creating the first snapshot)
            editor.TextContent = "Version 1";  
            history.SaveSnapshot(editor.SaveState());  

            Console.WriteLine($" Current Text 1: {editor.TextContent}");


            // 2️⃣ Change Text & Save Again (Create another snapshot)
            editor.TextContent = "Version 2";  // Change the text
            history.SaveSnapshot(editor.SaveState());  // Save "Version 2"
            Console.WriteLine($" Current Text: {editor.TextContent}");

            // 3️⃣ Modify Text Again (without saving it yet)
            editor.TextContent = "Version 3";  // Change to "Version 3"

            // Display Current Text (Should be "Version 3")
            Console.WriteLine($" Current Text: {editor.TextContent}");

            // ⏪ Undo to "Version 2" (Restore from history)
            editor.RestoreState(history.GetSnapshot(1));  // Restores "Version 2"

             Console.WriteLine($" Current Text: {editor.TextContent}");

            // ⏪ Undo to "Version 1" (Restore from history)
           editor.RestoreState(history.GetSnapshot(0));  // Restores "Version 1"

           

            // Pause to view the result
            Console.ReadLine();
        }
    }
}
