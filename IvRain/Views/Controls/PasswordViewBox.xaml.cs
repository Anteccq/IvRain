using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IvRain.Views.Controls
{
    public sealed partial class PasswordViewBox : UserControl
    {
        private string _invisiblePassword = null;
        private bool _passwordVisible = false;

        public new double FontSize
        {
            get => PasswordText.FontSize;
            set => PasswordText.FontSize = value;
        }

        public string Password
        {
            get => RealPasswordText;
            set
            {
                RealPasswordText = value;
                var isFirstRender = _invisiblePassword == null;
                _invisiblePassword = RealPasswordText.Select(x => '*').Aggregate(new StringBuilder(), (sb, x) => sb.Append(x)).ToString();
                if(isFirstRender) this.PasswordText.Text = _invisiblePassword;
            }
        }

        public string RealPasswordText { get; private set; }

        public PasswordViewBox()
        {
            this.InitializeComponent();

            this.Loaded += (_, _) =>
            {
                this.PasswordText.Text = _invisiblePassword;
                this.PasswordShowToggle.IsChecked = false;
            };

            this.PasswordShowToggle.Click += (_, _) =>
            {
                _passwordVisible = PasswordShowToggle.IsChecked == true;
                PasswordText.Text = _passwordVisible
                    ? RealPasswordText
                    : _invisiblePassword;
            };
        }
    }
}
