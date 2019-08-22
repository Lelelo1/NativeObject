using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ResolutionGroupName("Namespace")]
[assembly: ExportEffect(typeof(Namespace.Droid.NativeObjectEffect), "NativeObjectEffect")]
namespace Namespace.Droid
{
    public class NativeObjectEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
 
        }

        protected override void OnDetached()
        {

        }
        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            /* special implementation neccessary beacuse the native Control is not being ready at start/in mainpage contructor */
            if (Control != null && LoadedControl == null)
            {
                LoadedControl = Control;
                // Console.WriteLine("OnElementPropertyChanged set LoadedControl to " + LoadedControl + " from Control " + Control);
                Loaded.Invoke();
            }

            // handling case with Xamarin.Forms.Layout // https://forums.xamarin.com/discussion/comment/386827#Comment_386827
            if (Container != null && LoadedControl == null)
            {
                LoadedControl = Container;
                // Console.WriteLine("OnElementPropertyChanged set LoadedControl to " + LoadedControl + " from Cotainer " + Container);
                Loaded.Invoke();
            }

            // no page equivalent in native

        }
        event OnLoaded Loaded;
        delegate void OnLoaded();

        public async void Set(TaskCompletionSource<Android.Views.View> onLoaded)
        {
            if (Element is Layout && Container != null)
            {
                // Console.WriteLine(Element + " is layout. returning " + Container);
                onLoaded.SetResult(Container);
            }
            else if (Control != null)
            {
                // Console.WriteLine("Assuming " + Element + " is Control returning: " + Control);
                onLoaded.SetResult(Control);
            }
            else
            {
                var asyncEventListener = new AsyncEventListener(() =>
                {
                    onLoaded.SetResult(LoadedControl);
                });
                Loaded += asyncEventListener.Listen;

                await asyncEventListener.Successfully;

                Loaded -= asyncEventListener.Listen;
            }
        }

         Android.Views.View LoadedControl { get; set; } = null;

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
