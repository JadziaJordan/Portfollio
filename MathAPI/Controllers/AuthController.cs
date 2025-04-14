using Firebase.Auth;
using MathAPI.Models;
using MathAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MathAPI.Controllers
{
    // Route all requests to /api/Auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        // Firebase authentication provider
        FirebaseAuthProvider auth;

        // Constructor that sets up the Firebase provider using an API key from environment variables
        public AuthController()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig(Environment.GetEnvironmentVariable("FirebaseMathApp")));
        }

        // Endpoint for registering a new user
        [HttpPost("Register")]
        public async Task<IActionResult> Register(LoginModel login)
        {
            try
            {
                // Create a new Firebase user account with email and password
                await auth.CreateUserWithEmailAndPasswordAsync(login.Email, login.Password);

                // Immediately sign in the new user
                var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);
                string currentUserId = fbAuthLink.User.LocalId;

                if (currentUserId != null)
                {
                    // Return the Firebase user ID in a custom AuthResponse object
                    return Ok(new AuthResponse(currentUserId));
                }
            }
            catch (FirebaseAuthException ex)
            {
                // Handle Firebase-specific errors and return the code/message
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseErrorModel>(ex.ResponseData);
                return Unauthorized(firebaseEx.error.code + " - " + firebaseEx.error.message);
            }
            catch (Exception ex)
            {
                // Handle other types of errors (e.g., network issues)
                return Unauthorized(ex.Message);
            }

            return View(); // fallback (should rarely be reached)
        }

        // Endpoint for logging in an existing user
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                // Attempt to authenticate the user using Firebase
                var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);
                string currentUserId = fbAuthLink.User.LocalId;

                if (currentUserId != null)
                {
                    // Return the user ID on successful login
                    return Ok(new AuthResponse(currentUserId));
                }
            }
            catch (FirebaseAuthException ex)
            {
                // Deserialize the error, log it to auth_errors.log, and return a readable error
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseErrorModel>(ex.ResponseData);

                // Log detailed info using your custom logger
                AuthLogger.Instance.LogError(
                    firebaseEx.error.message 
                    + " - User: " + login.Email 
                    + " - IP: " + HttpContext.Connection.RemoteIpAddress 
                    + " - Browser: " + Request.Headers.UserAgent
                );

                return Unauthorized(firebaseEx.error.code + " - " + firebaseEx.error.message);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                return Unauthorized(ex.Message);
            }

            return View(); // fallback (should rarely be reached)
        }

        // Dummy logout endpoint – doesn't do anything here since Firebase handles session client-side
        [HttpPost("Logout")]
        public IActionResult LogOut()
        {
            return Ok(); // A placeholder for potential future enhancements
        }
    }
}
