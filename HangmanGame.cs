using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Android.Media;
using Android.Content.PM;

namespace Jacob_sGames
{
    [Activity(Label = "Hangman", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class HangmanGame : Activity
    {
        int wordChooser = 0;
        const int allowedWrong = 6;
        int wrongGuesses = 0;
        string word;
        string guess;
        string[] animals = new string[10] { "alligator", "chimpanzee", "crocodile", "hippopotamus", "elephant", "giraffe", "squirrel", "zebra", "kangaroo", "horse" };
        string[] colours = new string[10] { "cerulean", "pewter", "goldenrod", "vermillion", "fuschia", "indigo", "cyan", "violet", "saffron", "silver" };
        string[] movies = new string[10] { "The Dark Knight", "The Titanic", "Shrek", "The Avengers", "Spiderman", "The Lion King", "Finding Nemo", "Star Wars", "Lord of the Rings", "King Kong"};
        string[] musicians = new string[5] { "Usher", "Drake", "Cher", "Enya", "Eminem" };
        char[] guessedLetters = new char[27];
        int numLetters = 0;
        string guessedWord = new string('_', 0);
        TextView txtWord;
        Random rand;
        MediaPlayer player;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HangmanGameLayout);
            FindViewById<ImageView>(Resource.Id.imgMan).SetImageResource(Resource.Drawable.stage0);
            int mode = Intent.GetIntExtra("mode", 1);
            txtWord = FindViewById<TextView>(Resource.Id.txtDisplay);
            Button button = FindViewById<Button>(Resource.Id.btnGuess);
            button.Click += delegate {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                View view = layoutInflater.Inflate(Resource.Layout.userInputDialog, null);
                Android.Support.V7.App.AlertDialog.Builder alertbuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertbuilder.SetView(view);
                var userdata = view.FindViewById<EditText>(Resource.Id.editText);
                alertbuilder.SetCancelable(false)
                .SetPositiveButton("Submit", delegate
                {
                    guess = userdata.Text;
                    if(guess.ToUpper() == word.ToUpper())
                    {
                        DisableButtons();
                        string displayWord = "";
                        for(var i = 0; i < word.Length; i++)
                        {
                            displayWord += char.ToUpper(word[i]);
                            displayWord += " ";
                        }
                        FindViewById<TextView>(Resource.Id.txtDisplay).Text = displayWord;
                        Android.App.AlertDialog.Builder temp = new AlertDialog.Builder(this);
                        AlertDialog alert = temp.Create();
                        player = MediaPlayer.Create(this, Resource.Raw.celebrate);
                        player.Start();
                        alert.SetTitle("Correct! Congratulations!");
                        alert.SetMessage("Another Word?");
                        alert.SetButton("Yes", (c, ev) =>
                        {
                            SetNewWord(mode);
                            player.Stop();
                            for (var i = 0; i < guessedLetters.Length; i++)
                            {
                                guessedLetters[i] = '`';
                            }
                            guessedLetters[26] = ' ';
                            showWord(mode);
                            ResetButtons();
                            numLetters = 0;
                            guessedWord = " ";
                            FindViewById<ImageView>(Resource.Id.imgMan).SetImageResource(Resource.Drawable.stage0);
                        });
                        alert.SetButton2("No", (c, ev) =>
                        {
                            Intent intent = new Intent(this, typeof(MainActivity));
                            StartActivity(intent);
                            player.Stop();
                        });
                        alert.Show();
                    }
                    else
                    {
                        string message = "Incorrect";
                        Toast.MakeText(ApplicationContext, message, ToastLength.Short).Show();
                        wrongGuesses++;
                        if(wrongGuesses == allowedWrong)
                        {
                            DisableButtons();
                            Android.App.AlertDialog.Builder correctLetters = new AlertDialog.Builder(this);
                            AlertDialog alert = correctLetters.Create();
                            player = MediaPlayer.Create(this, Resource.Raw.failure);
                            player.Start();
                            alert.SetTitle("Uh oh! Out of guesses!");
                            alert.SetMessage("Another Word?");
                            alert.SetButton("Yes", (c, ev) =>
                            {
                                SetNewWord(mode);
                                for (var i = 0; i < guessedLetters.Length; i++)
                                {
                                    guessedLetters[i] = '`';
                                }
                                guessedLetters[26] = ' ';
                                showWord(mode);
                                ResetButtons();
                                player.Stop();
                                numLetters = 0;
                                guessedWord = " ";
                                wrongGuesses = 0;
                                FindViewById<ImageView>(Resource.Id.imgMan).SetImageResource(Resource.Drawable.stage0);

                            });
                            alert.SetButton2("No", (c, ev) =>
                            {
                                Intent intent = new Intent(this, typeof(MainActivity));
                                StartActivity(intent);
                                player.Stop();

                            });
                            alert.Show();
                        }
                        ImageView img = FindViewById<ImageView>(Resource.Id.imgMan);
                        switch (wrongGuesses)
                        {
                            case 0:
                                img.SetImageResource(Resource.Drawable.stage0);
                                break;
                            case 1:
                                img.SetImageResource(Resource.Drawable.stage1);
                                break;
                            case 2:
                                img.SetImageResource(Resource.Drawable.stage2);
                                break;
                            case 3:
                                img.SetImageResource(Resource.Drawable.stage3);
                                break;
                            case 4:
                                img.SetImageResource(Resource.Drawable.stage4);
                                break;
                            case 5:
                                img.SetImageResource(Resource.Drawable.stage5);
                                break;
                            case 6:
                                img.SetImageResource(Resource.Drawable.stage6);
                                break;
                        }

                    }
                })
                .SetNegativeButton("Cancel", delegate
                {
                    alertbuilder.Dispose();
                });
                Android.Support.V7.App.AlertDialog dialog = alertbuilder.Create();
                dialog.Show();
            };



            
            // Create your application here
            TextView txt;
            txt = FindViewById<TextView>(Resource.Id.mode);
            

            rand = new Random();
           
            switch (mode)
            {
                case 1:
                    txt.Text = "Animals";
                    wordChooser = rand.Next(0, 10);
                    word = animals[wordChooser];
                    break;
                case 2:
                    txt.Text = "Colours";
                    wordChooser = rand.Next(0, 10);
                    word = colours[wordChooser];
                    break;
                case 3:
                    txt.Text = "Movies";
                      wordChooser = rand.Next(0, 10);
                      word = movies[wordChooser];
                  
                    break;
                case 4:
                    txt.Text = "Musicians";
                    wordChooser = rand.Next(0, 5);
                    word = musicians[wordChooser];
                    break;
            }
            guessedLetters[26] = ' ';
           
            int length = word.Length;
            for (var i = 0; i < length; i++)
            {
                if(word[i] != ' ')
                {
                    guessedWord += "_ ";
                }
                else
                {
                    guessedWord += "  ";
                }
            }

            txtWord.Text = guessedWord;

            FindViewById<Button>(Resource.Id.btnTesting).Click += (o, e) =>
            {
                SetNewWord(mode);
                for (var i = 0; i < guessedLetters.Length; i++)
                {
                    guessedLetters[i] = '`';
                }
                guessedLetters[26] = ' ';
                showWord(mode);
                ResetButtons();
                numLetters = 0;
                guessedWord = " ";
                if (player != null)
                {
                    player.Stop();
                }
                FindViewById<ImageView>(Resource.Id.imgMan).SetImageResource(Resource.Drawable.stage0);
                wrongGuesses = 0;
            };

            FindViewById<Button>(Resource.Id.btnExit).Click += (o, e) =>
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                if(player != null)
                {
                    player.Stop();
                }
                
            };

            FindViewById<Button>(Resource.Id.aBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'A';
                numLetters++;
                FindViewById<Button>(Resource.Id.aBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.aBtn).Text = "-";
                showWord(mode);
                CheckGuess('A', mode);
            };

            FindViewById<Button>(Resource.Id.bBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'B';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.bBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.bBtn).Text = "-";
                CheckGuess('B', mode);
            };

            FindViewById<Button>(Resource.Id.cBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'C';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.cBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.cBtn).Text = "-";
                CheckGuess('C', mode);
            };

            FindViewById<Button>(Resource.Id.dBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'D';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.dBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.dBtn).Text = "-";
                CheckGuess('D', mode);
            };

            FindViewById<Button>(Resource.Id.eBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'E';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.eBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.eBtn).Text = "-";
                CheckGuess('E', mode);
            };

            FindViewById<Button>(Resource.Id.fBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'F';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.fBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.fBtn).Text = "-";
                CheckGuess('F', mode);
            };

            FindViewById<Button>(Resource.Id.gBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'G';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.gBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.gBtn).Text = "-";
                CheckGuess('G', mode);
            };

            FindViewById<Button>(Resource.Id.hBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'H';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.hBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.hBtn).Text = "-";
                CheckGuess('H', mode);
            };

            FindViewById<Button>(Resource.Id.iBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'I';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.iBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.iBtn).Text = "-";
                CheckGuess('I', mode);
            };

            FindViewById<Button>(Resource.Id.jBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'J';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.jBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.jBtn).Text = "-";
                CheckGuess('J', mode);
            };

            FindViewById<Button>(Resource.Id.kBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'K';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.kBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.kBtn).Text = "-";
                CheckGuess('K', mode);
            };

            FindViewById<Button>(Resource.Id.lBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'L';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.lBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.lBtn).Text = "-";
                CheckGuess('L', mode);
            };

            FindViewById<Button>(Resource.Id.mBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'M';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.mBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.mBtn).Text = "-";
                CheckGuess('M', mode);
            };

            FindViewById<Button>(Resource.Id.nBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'N';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.nBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.nBtn).Text = "-";
                CheckGuess('N', mode);
            };

            FindViewById<Button>(Resource.Id.oBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'O';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.oBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.oBtn).Text = "-";
                CheckGuess('O', mode);
            };

            FindViewById<Button>(Resource.Id.pBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'P';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.pBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.pBtn).Text = "-";
                CheckGuess('P', mode);
            };

            FindViewById<Button>(Resource.Id.qBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'Q';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.qBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.qBtn).Text = "-";
                CheckGuess('Q', mode);
            };

            FindViewById<Button>(Resource.Id.rBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'R';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.rBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.rBtn).Text = "-";
                CheckGuess('R', mode);
            };

            FindViewById<Button>(Resource.Id.sBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'S';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.sBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.sBtn).Text = "-";
                CheckGuess('S', mode);
            };

            FindViewById<Button>(Resource.Id.tBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'T';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.tBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.tBtn).Text = "-";
                CheckGuess('T', mode);
            };

            FindViewById<Button>(Resource.Id.uBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'U';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.uBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.uBtn).Text = "-";
                CheckGuess('U', mode);
            };

            FindViewById<Button>(Resource.Id.vBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'V';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.vBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.vBtn).Text = "-";
                CheckGuess('V', mode);
            };

            FindViewById<Button>(Resource.Id.wBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'W';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.wBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.wBtn).Text = "-";
                CheckGuess('W', mode);
            };

            FindViewById<Button>(Resource.Id.xBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'X';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.xBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.xBtn).Text = "-";
                CheckGuess('X', mode);
            };

            FindViewById<Button>(Resource.Id.yBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'Y';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.yBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.yBtn).Text = "-";
                CheckGuess('Y', mode);
            };

            FindViewById<Button>(Resource.Id.zBtn).Click += (o, e) =>
            {
                guessedLetters[numLetters] = 'Z';
                numLetters++;
                showWord(mode);
                FindViewById<Button>(Resource.Id.zBtn).Enabled = false;
                FindViewById<Button>(Resource.Id.zBtn).Text = "-";
                CheckGuess('Z', mode);
            };

            
        }

        public void SetNewWord(int mode)
        {
            switch (mode)
            {
                case 1:                 
                    wordChooser = rand.Next(0, 10);
                    word = animals[wordChooser];
                    break;
                case 2:
                    
                    wordChooser = rand.Next(0, 10);
                    word = colours[wordChooser];
                    break;
                case 3:
                   
                    wordChooser = rand.Next(0, 10);
                    word = movies[wordChooser];
                    break;
                case 4:
                   
                    wordChooser = rand.Next(0, 5);
                    word = musicians[wordChooser];
                    break;
            }
        }

        public void CheckGuess(char guess, int mode)
        {
            bool goodGuess = false;
            for(var i = 0; i < word.Length; i++)
            {
                if(char.ToUpper(guess) == char.ToUpper(word[i]))
                {
                    goodGuess = true;
                }
            }
            if(!goodGuess)
            {
                wrongGuesses++;
                   ImageView img = FindViewById<ImageView>(Resource.Id.imgMan);
                    switch(wrongGuesses)
                    {
                        case 0:
                            img.SetImageResource(Resource.Drawable.stage0);
                            break;
                        case 1:
                            img.SetImageResource(Resource.Drawable.stage1);
                            break;
                        case 2:
                            img.SetImageResource(Resource.Drawable.stage2);
                            break;
                        case 3:
                            img.SetImageResource(Resource.Drawable.stage3);
                            break;
                        case 4:
                            img.SetImageResource(Resource.Drawable.stage4);
                            break;
                        case 5:
                            img.SetImageResource(Resource.Drawable.stage5);
                            break;
                        case 6:
                            img.SetImageResource(Resource.Drawable.stage6);
                            break;
                    }
                       
                if (wrongGuesses == allowedWrong)
                {
                    DisableButtons();
                    Android.App.AlertDialog.Builder correctLetters = new AlertDialog.Builder(this);
                    AlertDialog alert = correctLetters.Create();
                    player = MediaPlayer.Create(this, Resource.Raw.failure);
                    player.Start();
                    alert.SetTitle("Uh oh! Out of guesses!");
                    alert.SetMessage("Another Word?");
                    alert.SetButton("Yes", (c, ev) =>
                    {
                        SetNewWord(mode);
                        for (var i = 0; i < guessedLetters.Length; i++)
                        {
                            guessedLetters[i] = '`';
                        }
                        guessedLetters[26] = ' ';
                        showWord(mode);
                        ResetButtons();
                        player.Stop();
                        numLetters = 0;
                        guessedWord = " ";
                        wrongGuesses = 0;
                        FindViewById<ImageView>(Resource.Id.imgMan).SetImageResource(Resource.Drawable.stage0);

                    });
                    alert.SetButton2("No", (c, ev) =>
                    {
                        Intent intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                        player.Stop();
                        
                    });
                    alert.Show();
                }
            }
        }
        public void showWord(int mode)
        {
            TextView txtWord;
            txtWord = FindViewById<TextView>(Resource.Id.txtDisplay);
            int length = 0;
            length = word.Length;

            int letters = 0;
            string guessedWord = new string('_', 0);
            string temp = new string(guessedLetters);
            int guessLength = temp.Length;
            bool found = false;

            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < guessLength; j++)
                {
                    if (char.ToUpper(temp[j]) == char.ToUpper(word[i]))
                    {
                        guessedWord += char.ToUpper(word[i]);
                        guessedWord += " ";
                        found = true;
                        letters++;
                    }
                }
                if (!found)
                {
                    guessedWord += "_ ";
                }
                found = false;
            }


            if (letters == length)
            {
                DisableButtons();
                Android.App.AlertDialog.Builder correctLetters = new AlertDialog.Builder(this);
                AlertDialog alert = correctLetters.Create();
                player = MediaPlayer.Create(this, Resource.Raw.celebrate);
                player.Start();
                alert.SetTitle("Correct! Congratulations!");
                alert.SetMessage("Another Word?");
                alert.SetButton("Yes", (c, ev) =>
                {
                    SetNewWord(mode);
                    for(var i = 0; i < guessedLetters.Length; i++)
                    {
                        guessedLetters[i] = '`';
                    }
                    guessedLetters[26] = ' ';
                    showWord(mode);
                    ResetButtons();
                    player.Stop();
                    numLetters = 0;
                    guessedWord = " ";
                    FindViewById<ImageView>(Resource.Id.imgMan).SetImageResource(Resource.Drawable.stage0);
                    wrongGuesses = 0;

                });
                alert.SetButton2("No", (c, ev) =>
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    player.Stop();
                });
                alert.Show();
            }

            txtWord.Text = guessedWord;
        }

        public void ResetButtons()
        {
            FindViewById<Button>(Resource.Id.aBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.aBtn).Text = "A";
            FindViewById<Button>(Resource.Id.bBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.bBtn).Text = "B";
            FindViewById<Button>(Resource.Id.cBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.cBtn).Text = "C";
            FindViewById<Button>(Resource.Id.dBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.dBtn).Text = "D";
            FindViewById<Button>(Resource.Id.eBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.eBtn).Text = "E";
            FindViewById<Button>(Resource.Id.fBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.fBtn).Text = "F";
            FindViewById<Button>(Resource.Id.gBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.gBtn).Text = "G";
            FindViewById<Button>(Resource.Id.hBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.hBtn).Text = "H";
            FindViewById<Button>(Resource.Id.iBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.iBtn).Text = "I";
            FindViewById<Button>(Resource.Id.jBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.jBtn).Text = "J";
            FindViewById<Button>(Resource.Id.kBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.kBtn).Text = "K";
            FindViewById<Button>(Resource.Id.lBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.lBtn).Text = "L";
            FindViewById<Button>(Resource.Id.mBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.mBtn).Text = "M";
            FindViewById<Button>(Resource.Id.nBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.nBtn).Text = "N";
            FindViewById<Button>(Resource.Id.oBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.oBtn).Text = "O";
            FindViewById<Button>(Resource.Id.pBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.pBtn).Text = "P";
            FindViewById<Button>(Resource.Id.qBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.qBtn).Text = "Q";
            FindViewById<Button>(Resource.Id.rBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.rBtn).Text = "R";
            FindViewById<Button>(Resource.Id.sBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.sBtn).Text = "S";
            FindViewById<Button>(Resource.Id.tBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.tBtn).Text = "T";
            FindViewById<Button>(Resource.Id.uBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.uBtn).Text = "U";
            FindViewById<Button>(Resource.Id.vBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.vBtn).Text = "V";
            FindViewById<Button>(Resource.Id.wBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.wBtn).Text = "W";
            FindViewById<Button>(Resource.Id.xBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.xBtn).Text = "X";
            FindViewById<Button>(Resource.Id.yBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.yBtn).Text = "Y";
            FindViewById<Button>(Resource.Id.zBtn).Enabled = true;
            FindViewById<Button>(Resource.Id.zBtn).Text = "Z";

        }

        public void DisableButtons()
        {
            FindViewById<Button>(Resource.Id.aBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.aBtn).Text = "-";
            FindViewById<Button>(Resource.Id.bBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.bBtn).Text = "-";
            FindViewById<Button>(Resource.Id.cBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.cBtn).Text = "-";
            FindViewById<Button>(Resource.Id.dBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.dBtn).Text = "-";
            FindViewById<Button>(Resource.Id.eBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.eBtn).Text = "-";
            FindViewById<Button>(Resource.Id.fBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.fBtn).Text = "-";
            FindViewById<Button>(Resource.Id.gBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.gBtn).Text = "-";
            FindViewById<Button>(Resource.Id.hBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.hBtn).Text = "-";
            FindViewById<Button>(Resource.Id.iBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.iBtn).Text = "-";
            FindViewById<Button>(Resource.Id.jBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.jBtn).Text = "-";
            FindViewById<Button>(Resource.Id.kBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.kBtn).Text = "-";
            FindViewById<Button>(Resource.Id.lBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.lBtn).Text = "-";
            FindViewById<Button>(Resource.Id.mBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.mBtn).Text = "-";
            FindViewById<Button>(Resource.Id.nBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.nBtn).Text = "-";
            FindViewById<Button>(Resource.Id.oBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.oBtn).Text = "-";
            FindViewById<Button>(Resource.Id.pBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.pBtn).Text = "-";
            FindViewById<Button>(Resource.Id.qBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.qBtn).Text = "-";
            FindViewById<Button>(Resource.Id.rBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.rBtn).Text = "-";
            FindViewById<Button>(Resource.Id.sBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.sBtn).Text = "-";
            FindViewById<Button>(Resource.Id.tBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.tBtn).Text = "-";
            FindViewById<Button>(Resource.Id.uBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.uBtn).Text = "-";
            FindViewById<Button>(Resource.Id.vBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.vBtn).Text = "-";
            FindViewById<Button>(Resource.Id.wBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.wBtn).Text = "-";
            FindViewById<Button>(Resource.Id.xBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.xBtn).Text = "-";
            FindViewById<Button>(Resource.Id.yBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.yBtn).Text = "-";
            FindViewById<Button>(Resource.Id.zBtn).Enabled = false;
            FindViewById<Button>(Resource.Id.zBtn).Text = "-";

        }
    }
}