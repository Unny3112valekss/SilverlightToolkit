﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;

namespace Microsoft.Windows.Controls.Theming
{
    /// <summary>
    /// Implicitly applies the Shiny Red theme to all of its descendent
    /// FrameworkElements.
    /// </summary>
    /// <remarks>
    /// The theme is applied using ImplicitStyleManager.
    /// </remarks>
    /// <QualityBand>Preview</QualityBand>
    public partial class ShinyRedTheme : Theme
    {
        /// <summary>
        /// Initializes a new instance of the ShinyRedTheme class.
        /// </summary>
        public ShinyRedTheme()
            : base(typeof(ShinyRedTheme).Assembly, "Microsoft.Windows.Controls.Theming.Theme.xaml")
        {
        }
    }
}
