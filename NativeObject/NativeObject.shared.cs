using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;


namespace Namespace
{
    
    public static class NativeObject
    {

        // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/platform/platform-specifics/#consuming-the-platform-specific
        const string EffectName = "NativeObject.NativeObjectEffect";

        static void AttachEffect(Element element) // must take Element: https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Core/Element.cs
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

        static void DetachEffect(Element element)
        {
            Console.WriteLine("DetactEffect");
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

#if __IOS__

        public static UIKit.UIButton TestiOS()
        {
            Console.WriteLine("TestiOS");
            return new UIKit.UIButton();
        }

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

        static void OniOSObjectPropertyChanged(BindableObject element, object oldValue, object newValue)
        {
            Console.WriteLine("iOSObjectProperty changed");
            if(newValue != null)
            {
                AttachEffect((Element)element);
            }
            else
            {
                DetachEffect((Element)element);
            }
        }

        public static UIKit.UIView iOS(this IPlatformElementConfiguration<iOS, Element> config)
        {
            return GetiOS(config.Element);
        }

        // should it be possible to set native controls ?! ...
        public static IPlatformElementConfiguration<iOS, Element> SetiOS(this IPlatformElementConfiguration<iOS, Element> config, UIKit.UIView uiView)
        {
            SetiOS(config.Element, uiView);
            return config;
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
