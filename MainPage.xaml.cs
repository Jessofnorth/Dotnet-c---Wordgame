using System.ComponentModel;
using System;
using System.Diagnostics;

namespace Wordgame;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    //
    #region UI
    //create string to contain the guesses
    public string Highlight
    {
        get => highlight;
        set
        {
            highlight = value;
            OnPropertyChanged();
        }
    }

    //create list for the letter buttons
    public List<Char> Letters
    {
        get => letters;

        set
        {
            letters = value;
            OnPropertyChanged();
        }
    }
    //property for message to player 
    public string Message
    {
        get => message;
        set
        {
            message = value;
            OnPropertyChanged();
        }
    }

    #endregion


    #region Words
    //the anser property, the word to guess
    string answer = "";

    //the property for the string with guessed chars and _
    private string highlight;

    //add the list of words for the game 
    List<string> words = new()
    {
        "lion",
        "hippopotamus",
        "giraffe",
        "elephant",
        "cat",
        "dog",
        "moose",
        "raindeer",
        "bear",
        "wolf",
        "penguin",
        "hamster"
    };

    //list with the guessed chars
    List<char> guesses = new();

    //the list for the letters list
    private List<char> letters = new();
    private string message;


    #endregion



    public MainPage()
    {
        InitializeComponent();

        //fill letters to list of letter buttons with add range method
        Letters.AddRange("abcdefghijklmnopqrstuvwxyz");

        //Set data binding context to this file
        BindingContext = this;

        //run the Word picker method for a word to guess
        WordPicker();

        //run the check guess method to match the answer to the guessed chars
        CheckGuess(answer, guesses);
    }

    #region Game
    //randomly chooses word from list 
    private void WordPicker()
    {
        //choose a random word in the list and set to answer property
        answer = words[new Random().Next(0, words.Count)];
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
    }

    //check if the complete answer is guessed and the game won
    private void GameWon()
    {
        //check if property, minus empty spaces, is equal to answer
        //if so the game is won
        if (Highlight.Replace(" ", "") == answer)
        {
            Message = "Great job! You won!";
        }
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


}


