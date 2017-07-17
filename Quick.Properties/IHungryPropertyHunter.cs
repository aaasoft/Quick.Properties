using System;
using System.Collections.Generic;

namespace Quick.Properties
{
    public interface IHungryPropertyHunter
    {
        void Hunt(IDictionary<String, String> properties);
    }
}
