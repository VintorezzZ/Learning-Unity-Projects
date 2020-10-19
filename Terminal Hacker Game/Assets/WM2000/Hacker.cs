using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hacker : MonoBehaviour
{
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
        switch (input)
        {
            case "1": 
                level = 1;
                password = "donkey";
                StartGame(level); 
                break;
            case "2":
                level = 2;
                password = "combo";
                StartGame(level); break;
            case "3":
                level = 3;
                StartGame(level); break;
            default:
                Terminal.WriteLine("Please, choose a valid level");
                break;
        }
    }

    private void StartGame(int level)
    {
        currenState = State.Password;
        Terminal.WriteLine("You have chosen level " + level);
        Terminal.WriteLine("Enter your password:");
    }

    private void CheckPassword(string input)
    {
        if (input == password) 
        {
            Terminal.WriteLine("Well done.");
        }
        else
        {
            Terminal.WriteLine("Wrong password.");
        }
    }

}
