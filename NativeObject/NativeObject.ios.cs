using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("NativeObject")]
[assembly: ExportEffect(typeof(Namespace.IOS.NativeObjectEffect), "NativeObjectEffect")]
namespace Namespace.IOS
{
    class NativeObjectEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            Console.WriteLine("OnAattached");
        }

        protected override void OnDetached()
        {
            Console.WriteLine("OnDetached");
        }
    }
}
