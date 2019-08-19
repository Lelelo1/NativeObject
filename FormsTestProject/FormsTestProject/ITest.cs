using System;
using System.Collections.Generic;
using System.Text;

namespace FormsTestProject
{
    public interface ITest
    {
        void Test(Xamarin.Forms.Label element); // can't use On with Element
        void Test(Xamarin.Forms.Button element);
    }
}
