using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hacker : MonoBehaviour
{
    const string menuHint = "Type 'menu' to return";
    string[] level_1_passwords = { "books", "shelf", "aisle", "password", "font", "borrow" };
    string[] level_2_passwords = { "prisoner", "handcuffs", "holster", "uniform", "arrest" };
    string[] level_3_passwords = { "space", "rocket", "eath", "solar", "sun", "star" };

    int level;
    string password;
    enum State
    {
        MainMenu, 
        Password, 
        Win
    }

    State currenState;

    void Start()
    {
        ShowMainMenu();
    }
    void ShowMainMenu()
    {
        currenState = State.MainMenu;
        Terminal.ClearScreen();
        Terminal.WriteLine("What would you like to hack into?");
        Terminal.WriteLine("Press 1 for the local library");
        Terminal.WriteLine("Press 2 for the police station");
        Terminal.WriteLine("Press 3 for NASA");
        Terminal.WriteLine("Enter your selection: ");
    }

    void OnUserInput(string input)     // Usefull thing!
    {
        if (input == "menu")
        {
            ShowMainMenu();
        }
        else if (currenState == State.MainMenu)
        {
            RunMainMenu(input);
        }
        else if (currenState == State.Password)
        {
            CheckPassword(input);
        }
     
    }
    
    private void RunMainMenu(string input)
    {
        bool isValidNumber = (input == "1" || input == "2" || input == "3");  // Cool thing!
        if (isValidNumber)
        {
            level = int.Parse(input);
            AskForPassword();
        }
        else
        {
            Terminal.WriteLine("Please, choose a valid level");
        }
    }

    private void AskForPassword()
    {
        currenState = State.Password;
        Terminal.ClearScreen();
        SetRandomPassword();
        Terminal.WriteLine("Enter your password, hint: " + password.Anagram());
        Terminal.WriteLine(menuHint);
    }

    private void SetRandomPassword()
    {
        switch (level)
        {
            case 1:
                password = level_1_passwords[Random.Range(0, level_1_passwords.Length)];
                break;
            case 2:
                password = level_2_passwords[Random.Range(0, level_1_passwords.Length)];
                break;
            case 3:
                password = level_3_passwords[Random.Range(0, level_1_passwords.Length)];
                break;
            default:
                Terminal.WriteLine("Invalid level number");
                break;
        }
    }

    private void CheckPassword(string input)
    {
        if (input == password)
        {
            DisplayWinScreen();
        }
        else
        {
            AskForPassword();
        }
    }

    private void DisplayWinScreen()
    {
        currenState = State.Win;
        Terminal.ClearScreen();
        ShowLevelReward();
        Terminal.WriteLine(menuHint);
    }

    private void ShowLevelReward()
    {
        switch (level)
        {
            case 1:
                Terminal.WriteLine("Have a book...");
                Terminal.WriteLine(@"

    _______
   /      //
  /      //
 /_____ //
(______(/
"
                );
                break;
            case 2:
                Terminal.WriteLine("You got a prison key");
                break;
            case 3:
                Terminal.WriteLine(@"
NASA

");
                Terminal.WriteLine("Welcome to NASA's internal system!");
                break;
            default:
                Debug.LogError("Invalid level");
                break;
        }
        
    }
}
