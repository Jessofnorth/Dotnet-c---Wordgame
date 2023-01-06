using System.ComponentModel;
using System;
using System.Diagnostics;

namespace Wordgame;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    //
    #region UI
    public string Highlight
    {
        get => highlight;
        set
        {
            //get/set for the state of the guessed word, updates when the property changes
            //when a new char is guessed.
            highlight = value;
            OnPropertyChanged();
        }
    }

    //create list for the letter buttons
    public List<Char> Letters {
        get => letters;

        set
        {
            letters = value;
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
    private void WordPicker()
    {
        //choose a random word in the list and set to answer property
        answer = words[new Random().Next(0, words.Count)];
    }

    private void CheckGuess(string answer, List<char> guessedChars)
    {
        //get the random word, answer. Then check all the guessed chars in list, user input, to match word
        //if a letter in word is not guessed display a _ 
        var temp = answer.Select(x => (guessedChars.IndexOf(x) >= 0 ? x : '_'))
            .ToArray();
        Highlight = string.Join(' ', temp);

    }
    #endregion
}


