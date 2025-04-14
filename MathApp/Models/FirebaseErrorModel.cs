namespace MathApp.Models
{
    public class FirebaseErrorModel
    {
        public required Error error { get; set; }
    }
   
    public class Error
    {
        public required int code { get; set; }
        public required string message { get; set; }
        public required List<Error> errors { get; set; }
    }
}