﻿using AngouriMath;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfMath;

namespace AngouriGamma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private Renderer renderer = new();

        private GUISimplifier simplifier;
        private async void OnComputationsRequested(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            if (OutputSimplified is null)
                return;
            var text = ((TextBox)sender).Text;
            if (text is "")
                return;
            Entity expr, simplified;
            try
            {
                expr = MathS.FromString(text);
                simplified = await simplifier.Simplify(expr);
            }
            catch (AngouriMath.Core.Exceptions.ParseException)
            {
                simplified = "Error";
            }
            OutputSimplified.Source = await renderer.Render(simplified.Latexise());
        }

        private async void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OutputInitial is null)
                return;
            var text = ((TextBox)sender).Text;
            if (text is "")
                return;
            var newSrc = await Task.Run(async () =>
            {
                try
                {
                    return await Render(MathS.FromString(text).Latexise().Replace(@"\implies", @"\Rightarrow"));
                }
                catch (AngouriMath.Core.Exceptions.ParseException)
                {
                    return null;
                }
            });
            if (newSrc is not null)
                OutputInitial.Source = newSrc;
        }

        
    }
}
