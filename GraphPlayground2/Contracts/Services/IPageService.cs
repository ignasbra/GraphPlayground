using System;
using System.Windows.Controls;

namespace GraphPlayground2.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);

        Page GetPage(string key);
    }
}
