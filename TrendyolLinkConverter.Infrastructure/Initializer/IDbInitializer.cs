using System;
using System.Collections.Generic;
using System.Text;

namespace TrendyolLinkConverter.Infrastructure.Initializer
{
    public interface IDbInitializer
    {
        void Initialize();


        void SeedData();
    }
}
