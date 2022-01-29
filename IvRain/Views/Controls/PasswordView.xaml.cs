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
using System.Windows;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using IvRain.Models;
using Clipboard = System.Windows.Clipboard;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IvRain.Views.Controls
{
    public sealed partial class PasswordView : UserControl
    {
        public string Password => PasswordViewBox.RealPasswordText;
        public string SiteName => TextBlock.Text;
        public event Action PasswordChanged;
        public PasswordView()
        {
            this.InitializeComponent();

            Loaded += (_, _) =>
            {
                UpdatePasswordTexts( ((Block)DataContext)?.Password ?? "");
                TextBlock.Text = ((Block)DataContext)?.SiteName ?? "";
            };

            DataContextChanged += (_, _) =>
            {
                UpdatePasswordTexts( ((Block)DataContext)?.Password ?? "");
                TextBlock.Text = ((Block)DataContext)?.SiteName ?? "";
            };

            CopyButton.Click += (_, _) => Clipboard.SetText(Password);

            SubmitButton.Click += (_, _) =>
            {
                ((Block)DataContext).Password = PasswordEditText.Text;
                UpdatePasswordTexts(PasswordEditText.Text);
                PasswordChanged?.Invoke();
            };
        }

        private void UpdatePasswordTexts(string text)
        {
            PasswordEditText.Text = text;
            PasswordViewBox.Password = text;
        }
    }
}
