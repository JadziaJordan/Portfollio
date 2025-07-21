using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MementoWeb.Models;


namespace MementoWeb.Controllers;

public class HomeController : Controller
{
    private static Editor _editor = new Editor();
        private static UndoHistory _undoHistory = new UndoHistory();

        // Display the text editor page
        public ActionResult Index()
        {
            ViewBag.History = _undoHistory.GetAllSnapshots();  // Pass version history to view
            return View(_editor);
        }


        // Handle saving the current text state
        [HttpPost]
        public ActionResult SaveState(string textContent)
        {
            _editor.TextContent = textContent;  // Update the editor with the new content

            // Save the state by creating a snapshot
            TextSnapshot snapshot = _editor.CreateSnapshot();
            _undoHistory.SaveSnapshot(snapshot);  // Store the snapshot in history

            return RedirectToAction("Index");  // Redirect to the editor view
        }

        // Handle undo functionality (restore to the previous state)
      public ActionResult Undo()
{
    int lastIndex = _undoHistory.GetAllSnapshots().Count - 1;
    if (lastIndex >= 0)
    {
        var lastSnapshot = _undoHistory.GetSnapshot(lastIndex);
        if (lastSnapshot != null)
        {
            _editor.RestoreState(lastSnapshot);
            _undoHistory.RemoveSnapshot(lastIndex); // Remove the last snapshot
        }
    }

    return RedirectToAction("Index");
}


        // Display the version history (list of previous snapshots)
       
    }

