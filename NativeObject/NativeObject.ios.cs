using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Namespace")]
[assembly: ExportEffect(typeof(Namespace.iOS.NativeObjectEffect), "NativeObjectEffect")]
namespace Namespace.iOS
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
            /*
            Console.WriteLine("Container: " + this.Container);
            Console.WriteLine("Control: " + this.Control);
            Console.WriteLine("Element: " + this.Element);
            */
            //Console.WriteLine("OnElementPropertyChanged set LoadedControl to " + LoadedControl + " from Control " + Control + " Element: " + Element);
            
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
                // Console.WriteLine("OnElementPropertyChanged set LoadedControl to " + LoadedControl + " from Container " + Container);
                Loaded.Invoke();
            }

            // no page equivalent in native
            
        }
        event OnLoaded Loaded;
        delegate void OnLoaded();

        public async void Set(TaskCompletionSource<UIKit.UIView> onLoaded)
        {
            if(Element is Layout && Container != null)
            {
                onLoaded.SetResult(Container);
            }
            else if(Control != null)
            {
                onLoaded.SetResult(Control);
            }
            else
            {
                var asyncEventListener = new AsyncEventListener(() =>
                {
                    /*
                    Console.WriteLine("Loaded event " + LoadedControl + " and  " + Control);
                    Console.WriteLine("taskcomsource: " + onLoaded);
                    */
                    onLoaded.SetResult(LoadedControl);

                    // Console.WriteLine("after set taskcomsource: " + onLoaded);
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
/* Can't get access to the page "Control" 
 * 2019-08-21 11:54:40.001897+0200 FormsTestProject.iOS[55653:9625752] Set Continer: <Xamarin_Forms_Platform_iOS_PageContainer: 0x7fca5de264d0; frame = (0 0; 375 667); autoresize = W+H; gestureRecognizers = <NSArray: 0x600003d3dc20>; layer = <CALayer: 0x6000032f68c0>>
2019-08-21 11:54:40.002308+0200 FormsTestProject.iOS[55653:9625752] Set Control:
2019-08-21 11:54:40.002714+0200 FormsTestProject.iOS[55653:9625752] Set Element: FormsTestProject.MainPage

    */