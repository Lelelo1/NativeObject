using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Namespace")]
[assembly: ExportEffect(typeof(Namespace.IOS.NativeObjectEffect), "NativeObjectEffect")]
namespace Namespace.IOS
{
    public class NativeObjectEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            Console.WriteLine("OnAttached, control is: " + Control);
            
        }

        protected override void OnDetached()
        {
            Console.WriteLine("OnDetached");
            
        }
        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            /* special implementation neccessary beacuse the native Control is not being ready at start/in mainpage contructor */
            if (Control != null && LoadedControl == null)
            {
                LoadedControl = Control;
                Console.WriteLine("OnElementPropertyChanged set LoadedControl to " + LoadedControl + " from Control " + Control);
                Loaded.Invoke();
            }
            
        }
        event OnLoaded Loaded;
        delegate void OnLoaded();

        public async void Set(TaskCompletionSource<UIKit.UIView> onLoaded)
        {

            if(Control != null)
            {
                onLoaded.SetResult(Control);

            }
            else
            {
                Console.WriteLine("else");

                var asyncEventListener = new AsyncEventListener(() =>
                {
                    Console.WriteLine("Loaded event " + LoadedControl + " and  " + Control);
                    Console.WriteLine("taskcomsource: " + onLoaded);
                    onLoaded.SetResult(LoadedControl);
                    Console.WriteLine("after set taskcomsource: " + onLoaded);
                });
                Loaded += asyncEventListener.Listen;

                await asyncEventListener.Successfully;

                Loaded -= asyncEventListener.Listen;
            }
        }

        UIKit.UIView LoadedControl { get; set; } = null;

        public class AsyncEventListener
        {
            public AsyncEventListener(Action action)
            {

                Successfully = new Task(action);
            }

            public void Listen()
            {
                if (!Successfully.IsCompleted)
                {
                    Successfully.RunSynchronously();
                }
            }

            public Task Successfully { get; }
        }
    }

}
