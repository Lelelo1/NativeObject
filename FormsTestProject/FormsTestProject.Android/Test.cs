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
using Xamarin.Forms;
using Namespace;
namespace FormsTestProject.Droid
{
    public class Test : ITest
    {
        async void ITest.Test(Label element)
        {
            var editText = (Android.Widget.EditText) await element.On<Xamarin.Forms.PlatformConfiguration.Android>().AndroidAsync();
            editText.Text = "I was changed nativly";

        }


        async void ITest.Test(Xamarin.Forms.Button element)
        {
            var androidButton = (Android.Widget.Button) await element.On<Xamarin.Forms.PlatformConfiguration.Android>().AndroidAsync());
            androidButton.Text = "I was changed nativly";
            androidButton.SetBackgroundColor(Android.Graphics.Color.Bisque);

        }

        async void ITest.Test(StackLayout element)
        {
            var view = await element.On<Xamarin.Forms.PlatformConfiguration.Android>().AndroidAsync();
            Console.WriteLine("view: " + view);
            view.SetBackgroundColor(Android.Graphics.Color.Brown);

        }

        async void ITest.Test(Page element)
        {
            var p = await element.On<Xamarin.Forms.PlatformConfiguration.Android>().AndroidAsync();
            Console.WriteLine("p iz: " + p);
        }


    }
}