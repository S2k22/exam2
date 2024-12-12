using Data;
using Presentation;

class Program
{
    static void Main(string[] args)
    {
        Database.Initialize(); // Ensures tables are created
        MainMenu.Show();       // Starts the Main Menu
    }
}