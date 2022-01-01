using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace IvRain.Views
{
    public static class WindowUtil
    {
        public static void ResizeMainWindow(int width, int height)
            => (Application.Current as App)!.MainWindow.ResizeWindow(width, height);
    }
}
