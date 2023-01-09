using System.ComponentModel;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Wordgame.Models;

namespace Wordgame;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    
    #region UI
    //onpropertycanged notifys that the property has ben updated
    //get/set string to contain the guesses
    public string Highlight
    {
        get => highlight;
        set
        {
            highlight = value;
            OnPropertyChanged();
        }
    }

    //get/set list for the letter buttons
    public List<Char> Letters
    {
        get => letters;

        set
        {
            letters = value;
            OnPropertyChanged();
        }
    }
    //get/set for message to player 
    public string Message
    {
        get => message;
        set
        {
            message = value;
            OnPropertyChanged();
        }
    }
    //get/set for the display of number of mistakes left 
    public string Status
    {
        get => status;
        set
        {
            status = value;
            OnPropertyChanged();
        }
    }

    //get/set for the imag name based on number of mistakes  
    public string Image
    {
        get => image;
        set
        {
            image = value;
            OnPropertyChanged();
        }
    }
    #endregion


    #region Words
    //the anser property, the word to guess
    string answer = "";
    //the property for the string with guessed chars and _
    private string highlight;
    //create list of strings to save word list into
    private List<string> words = new();
    //list with the guessed chars
    List<char> guesses = new();
    //the list for the letters list
    private List<char> letters = new();
    //property for displaying messages
    private string message;
    //proprety for counting wrong guesses and max wrong guesses
    int wrongGuess = 0;
    int maxWrongGuess = 10;
    //property for status of game, number of wrong guesses
    private string status;
    //propertry of image nam
    private string image = "zero.png";
    //list of model FileWords
    private List<FileWords> fileWords = new();

    #endregion


    //constructor
    public MainPage()
    {
        InitializeComponent();

        //fill letters to list of letter buttons with add range method
        Letters.AddRange("abcdefghijklmnopqrstuvwxyz");

        //Set data binding context to this file
        BindingContext = this;

        //get words from file
        getWords();

        //run the Word picker method for a word to guess
        WordPicker();

        //run the check guess method to match the answer to the guessed chars
        CheckGuess(answer, guesses);
    }

    #region Game
    
    public void getWords()
    {
        //get words from file if it exists, if it dosent the app crashes
        if (File.Exists(@"words.json") == true)
        {
            //read the content of file to string
            string jsonString = File.ReadAllText(@"words.json");
            //deserialise into list with FileWordsmodel as base
            fileWords = JsonSerializer.Deserialize<List<FileWords>>(jsonString); 
            //loop to add each animal to new string list so the game can handle the data
            foreach(var x in fileWords)
            {
                words.Add(x.animal);
            }
        };
    }
    //randomly chooses word from list 
    private void WordPicker()
    {
        //choose a random word in the list and set to answer property
        answer = words[new Random().Next(0, fileWords.Count)];
    }

    //check if the guessed char(s) is in answer
    private void CheckGuess(string answer, List<char> guessedChars)
    {
        //get the random word, answer. Then check all the guessed chars in list, user input, to match word
        //if a letter in word is not guessed display a _ 
        var temp = answer.Select(x => (guessedChars.IndexOf(x) >= 0 ? x : '_'))
            .ToArray();
        Highlight = string.Join(' ', temp);

    }

    //method that processes guess from btn
    private void ProcessGuess(char letter)
    {
        //check if letter has been selected before.
        // -1 indicates it has not 
        if (guesses.IndexOf(letter) == -1)
        {
            guesses.Add(letter);
        }
        //check if letter is a match to answer word
        if (answer.IndexOf(letter) >= 0)
        {
            CheckGuess(answer, guesses);
            GameWon();
        }
        //if letter is not a match to answer
        else if (answer.IndexOf(letter) == -1)
        {
            //add to wrong guesses counter, update status, check if game is lost, change image
            wrongGuess++;
            StatusUpdate();
            GameLost();
            SetImage();
        }
    }
    //set the name of the image based on number of mistakes.
    //since Apple divices dont support namnes with numbers, useing a switch case.
    private void SetImage()
    {
        switch(wrongGuess)
        {
            case 1:
                Image = "one.png";
                break;

            case 2:
                Image = "two.png";
                break;

            case 3:
                Image = "three.png";
                break;

            case 4:
                Image = "four.png";
                break;

            case 5:
                Image = "five.png";
                break;

            case 6:
                Image = "six.png";
                break;

            case 7:
                Image = "seven.png";
                break;

            case 8:
                Image = "eight.png";
                break;

            case 9:
                Image = "nine.png";
                break;

            case 10:
                Image = "ten.png";
                break;
        }
    }


    //check if the complete answer is guessed and the game won
    private void GameWon()
    {
        //check if property, minus empty spaces, is equal to answer
        //if so the game is won
        if (Highlight.Replace(" ", "") == answer)
        {
            Message = "Great job! You won!";
            DisableBtns();
        }
    }
    //check if game is lost, disavle btns and prin message 
    private void GameLost()
    {//if player has guesse the max number of times they lose
        if(wrongGuess == maxWrongGuess)
        {
            Message = "You lost!";
            DisableBtns();
        }
    }
    //disable btns when los/won
    private void DisableBtns()
    {
        foreach (var child in LetterBtns.Children)
        {
            var btn = child as Button;
            if(btn != null)
            {
                btn.IsEnabled = false;
            }
        }
    }
    //enable btns when reset
    private void EnableBtns()
    {
        foreach (var child in LetterBtns.Children)
        {
            var btn = child as Button;
            if (btn != null)
            {
                btn.IsEnabled = true;
            }
        }
    }


    //method for checking number of guesses that are left and displaying to player
    public void StatusUpdate()
    {
        Status = $"Mistakes: {wrongGuess} of {maxWrongGuess}";
    }

    #endregion

    //eventhandler for the letter buttons 
    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        //get the event from pressed button, sender, and save to variable
        var btn = sender as Button;
        //if the btn is not null
        if (btn != null)
        {
            //get the letter and save to variable 
            var letter = btn.Text;
            //disable button
            btn.IsEnabled = false;
            //call function to process guess
            ProcessGuess(letter[0]);
        }
    }
    //when reset btn clicked reset game with new word 
    void Reset_Clicked(System.Object sender, System.EventArgs e)
    {
        wrongGuess = 0;
        guesses = new();
        Image = "zero.png";
        WordPicker();
        CheckGuess(answer, guesses);
        Message = "";
        StatusUpdate();
        EnableBtns();
    }

}


