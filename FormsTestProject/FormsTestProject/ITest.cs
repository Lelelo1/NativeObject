using System;
using System.Collections.Generic;
using System.Text;

namespace FormsTestProject
{
    public interface ITest
    {
        void Test(Xamarin.Forms.Element element); // can't use On with Element
    }
}
