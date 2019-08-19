using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Namespace;
[assembly: Dependency(typeof(FormsTestProject.iOS.Test))]
namespace FormsTestProject.iOS
{

    public class Test : ITest
    {
        async void ITest.Test(Element element)
        {
            Console.WriteLine("Test ");
            var label = (Label)element;
            var uiLabel = await label.On<Xamarin.Forms.PlatformConfiguration.iOS>().Get();

            // uiLabel.Text = "Changed the uilabel at runtime";
            Console.WriteLine("Test uiLabel: " + uiLabel);
        }
    }
}