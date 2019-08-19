using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Namespace;
namespace FormsTestProject
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            DependencyService.Get<ITest>().Test(label);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("effect iz: " + label.Effects[0]);
        }
    }
}
