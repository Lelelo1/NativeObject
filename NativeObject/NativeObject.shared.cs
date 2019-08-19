using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
namespace Namespace
{
    
    public static class NativeObject
    {
        public static void TestNetStandard()
        {
            Console.WriteLine("NetStandard");
        }
 
        // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/platform/platform-specifics/#consuming-the-platform-specific
        const string EffectName = "Namespace.NativeObjectEffect";
        

#if __IOS__

        public static UIKit.UIButton TestiOS()
        {
            Console.WriteLine("TestiOS");
            return new UIKit.UIButton();
        }
        /*
        static readonly BindableProperty iOSObjectProperty =
            BindableProperty.CreateAttached("iOS", typeof(UIKit.UIView),
                typeof(NativeObject), default(UIKit.UIView), propertyChanged: OniOSObjectPropertyChanged);

        static UIKit.UIView GetiOS(BindableObject element)
        {
            Console.WriteLine("GetiOS");
            return (UIKit.UIView)element.GetValue(iOSObjectProperty);
        }
        static void SetiOS(BindableObject element, UIKit.UIView uiView)
        {
            Console.WriteLine("SetiOS");
            element.SetValue(iOSObjectProperty, uiView);
        }
        */
        static void OniOSObjectPropertyChanged(BindableObject element, object oldValue, object newValue)
        {
            Console.WriteLine("iOSObjectProperty changed");
            if(newValue != null)
            {
                // AttachEffect((Element)element);
            }
            else
            {
               // DetachEffect((Element)element);
            }
        }
        

        static BindableProperty iOSControllerProperty =
            BindableProperty.CreateAttached("Controller",
                typeof(Controller), typeof(NativeObject), default(Controller),
                defaultValueCreator: (b) => new Controller(b));
        static Controller GetController(BindableObject b)
        {
            return (Controller)b.GetValue(iOSControllerProperty);
        }
        
        public static Task<UIKit.UIView> Get(this IPlatformElementConfiguration<iOS, VisualElement> config)
        {
            var controller = GetController(config.Element);
            Console.WriteLine("Got controller number: " + controller.GetNumber());
            var onLoaded = new TaskCompletionSource<UIKit.UIView>();
            controller.Wait(onLoaded);
            return onLoaded.Task;
        }
  
        /*
        // should it be possible to set native controls ?! ...
        public static IPlatformElementConfiguration<iOS, Element> SetiOS(this IPlatformElementConfiguration<iOS, Element> config, UIKit.UIView uiView)
        {
            SetiOS(config.Element, uiView);
            return config;
        }
        */

        class Controller
        {
            
            static int number = 0; // for testing
            public int GetNumber()
            {
                return number;
            }

            Element _element;
            public Controller(BindableObject b)
            {
                Console.WriteLine("controller received: " + b);
                // DetachEffect((Element)b);
                AttachEffect((Element)b);
                _element = (Element)b;
                number++;
            }

            public void AttachEffect(Element element) // must take Element: https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Core/Element.cs
            {
                Console.WriteLine("AttachEffect");
                IElementController controller = element;
                if (controller == null || controller.EffectIsAttached(EffectName))
                {
                    return;
                }
                element.Effects.Add(Effect.Resolve(EffectName));
                Console.WriteLine("Effect added");
            }

            public void DetachEffect(Element element)
            {
                Console.WriteLine("DetachEffect");
                IElementController controller = element;
                if (controller == null || !controller.EffectIsAttached(EffectName))
                {
                    return;
                }

                var toRemove = element.Effects.FirstOrDefault(e => e.ResolveId == Effect.Resolve(EffectName).ResolveId);
                if (toRemove != null)
                {
                    element.Effects.Remove(toRemove);
                    Console.WriteLine("Effect removed");
                }
            }
            public void Wait(TaskCompletionSource<UIKit.UIView> onLoaded)
            {
                
                var effect = _element.Effects.FirstOrDefault<Effect>((ef) => ef is Namespace.IOS.NativeObjectEffect);
                Console.WriteLine("effect: " + effect);
                if(effect == null)
                {
                    Console.WriteLine("No effect was attached on " + _element + " yet");
                    
                }
                else
                {
                    var nativeObjectEffect = (Namespace.IOS.NativeObjectEffect) effect;
                    nativeObjectEffect.Set(onLoaded);
                }
                

            }
        }

#else
#if __ANDROID__
        public static Android.Widget.Button TestAndroid()
        {
            Console.WriteLine("TestAndroid");
            return new Android.Widget.Button(null);
        }
        // public static readonly BindableProperty AndroidObjectProperty =
        // uwp etc...
#else
#endif
#endif

    }
    
}
