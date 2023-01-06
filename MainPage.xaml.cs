using System.ComponentModel;

namespace Wordgame;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    //add the list of words for the game 
    #region Words
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
    #endregion 

    public MainPage()
	{
		InitializeComponent();
	}

}


