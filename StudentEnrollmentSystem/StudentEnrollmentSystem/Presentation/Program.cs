using System;
using System.Threading.Tasks;
using Data; // Ensure this is included to initialize the database
using Presentation; // Ensure this is included for MainMenu

namespace StudentEnrollmentSystem
{
    class Program
    {
        static async Task Main(string[] args) // Use async Task instead of void
        {
            Database.Initialize(); // Ensures tables are created
            await MainMenu.ShowAsync(); // Await the async ShowAsync method
        }
    }
}
