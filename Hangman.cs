using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Jacob_sGames
{
    [Activity(Label = "Hangman")]
    public class Hangman : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.hangman);

            FindViewById<Button>(Resource.Id.btnAnimal).Click += (o, e) =>
            {
                Intent intent = new Intent(this, typeof(HangmanGame));
                intent.PutExtra("mode", 1);
                StartActivity(intent);
            };

            FindViewById<Button>(Resource.Id.btnColour).Click += (o, e) =>
            {
                Intent intent = new Intent(this, typeof(HangmanGame));
                intent.PutExtra("mode", 2);
                StartActivity(intent);
            };

            FindViewById<Button>(Resource.Id.btnMovies).Click += (o, e) =>
            {
                Intent intent = new Intent(this, typeof(HangmanGame));
                intent.PutExtra("mode", 3);
                StartActivity(intent);
            };

            FindViewById<Button>(Resource.Id.btnMusic).Click += (o, e) =>
            {
                Intent intent = new Intent(this, typeof(HangmanGame));
                intent.PutExtra("mode", 4);
                StartActivity(intent);
            };
        }
    }
}