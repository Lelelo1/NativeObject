using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Namespace;
using CoreGraphics;

[assembly: Dependency(typeof(FormsTestProject.iOS.Test))]
namespace FormsTestProject.iOS
{

    public class Test : ITest
    {
        async void ITest.Test(Label element)
        {
            var uiLabel = (UILabel) await element.On<Xamarin.Forms.PlatformConfiguration.iOS>().iOSAsync();
            uiLabel.Text = "Changed Label via UILabel";
            CoreGraphics.CGSize cgSize = new CoreGraphics.CGSize();
            uiLabel.ShadowColor = UIColor.Blue;
            uiLabel.Layer.ShadowOffset = new CGSize(5, 5);
            uiLabel.Layer.ShadowOpacity = 1.0f;

        }



        async void ITest.Test(Button element)
        {
            var uiButton = (UIButton) await element.On<Xamarin.Forms.PlatformConfiguration.iOS>().iOSAsync();
            uiButton.SetTitle("changed Button via UIButton", UIControlState.Normal);

        }


        async void ITest.Test(StackLayout element)
        {
            var ui = await element.On<Xamarin.Forms.PlatformConfiguration.iOS>().iOSAsync();
            Console.WriteLine("ui: " + ui);
            ui.BackgroundColor = UIColor.Brown;

        }

        async void ITest.Test(Page element)
        {
            var p = await element.On<Xamarin.Forms.PlatformConfiguration.iOS>().iOSAsync();
            Console.WriteLine("p iz: " + p);
        }
    }
}