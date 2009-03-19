﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Windows.Data;

namespace System.Windows.Controls
{
    /// <summary>
    /// Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column that can 
    /// bind to a property in the grid's data source.
    /// </summary>
    /// <QualityBand>Mature</QualityBand>
    public abstract class DataGridBoundColumn : DataGridColumn
    {
        #region Constants

        #endregion Constants

        #region Data

        private Binding _binding;
        private Style _elementStyle; // 
        private Style _editingElementStyle; // 

        #endregion Data

        #region Dependency Properties
        /* 


























































































*/

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the binding that associates the column with a property in the data source.
        /// </summary>
        public virtual Binding Binding
        {
            get
            {
                return this._binding;
            }
            set
            {
                if (this._binding != value)
                {
                    if (this.OwningGrid != null && !this.OwningGrid.CommitEdit(DataGridEditingUnit.Row, true /*exitEditing*/))
                    {
                        // Edited value couldn't be committed, so we force a CancelEdit
                        this.OwningGrid.CancelEdit(DataGridEditingUnit.Row, false /*raiseEvents*/);
                    }

                    this._binding = value;

                    if (this._binding != null)
                    {
                        // Force the TwoWay binding mode if there is a Path present.  TwoWay binding requires a Path.
                        if (!String.IsNullOrEmpty(this._binding.Path.Path))
                        {
                            this._binding.Mode = BindingMode.TwoWay;
                        }

                        if (this._binding.Converter == null)
                        {
                            this._binding.Converter = new DataGridValueConverter();
                        }

                        // Setup the binding for validation
                        this._binding.ValidatesOnExceptions = true;
                        this._binding.NotifyOnValidationError = true;
                        this._binding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;

                        // Apply the new Binding to existing rows in the DataGrid
                        if (this.OwningGrid != null)
                        {
                            // 


                            this.OwningGrid.OnColumnBindingChanged(this);
                        }
                    }
                    ResetBindingPaths();

                    //
                }
            }
        }

        // 



        public Style EditingElementStyle
        {
            get
            {
                return _editingElementStyle;
            }
            set
            {
                if (_editingElementStyle != value)
                {
                    _editingElementStyle = value;
                    // We choose not to update the elements already editing in the Grid here.  They
                    // will get the EditingElementStyle next time they go into edit mode
                }
            }
        }

        // 




        public Style ElementStyle
        {
            get
            {
                return _elementStyle;
            }
            set
            {
                if (_elementStyle != value)
                {
                    _elementStyle = value;
                    if (this.OwningGrid != null)
                    {
                        this.OwningGrid.OnColumnElementStyleChanged(this);
                    }
                }
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal DependencyProperty BindingTarget { get; set; }

        #endregion Internal Properties

        #region Internal Methods

        internal override List<string> CreateBindingPaths()
        {
            if (this.Binding != null && this.Binding.Path != null)
            {
                return new List<string>() { this.Binding.Path.Path };
            }
            return base.CreateBindingPaths();
        }

        internal override List<BindingInfo> CreateBindings(FrameworkElement element, object dataItem, bool twoWay)
        {
            BindingInfo bindingData = new BindingInfo();
            if (twoWay && this.BindingTarget != null)
            {
                bindingData.BindingExpression = element.GetBindingExpression(this.BindingTarget);
                if (bindingData.BindingExpression != null)
                {
                    bindingData.BindingTarget = this.BindingTarget;
                    bindingData.Element = element;
                    return new List<BindingInfo> { bindingData };
                }
            }
            foreach (DependencyProperty bindingTarget in element.GetDependencyProperties(false))
            {
                bindingData.BindingExpression = element.GetBindingExpression(bindingTarget);
                if (bindingData.BindingExpression != null
                    && bindingData.BindingExpression.ParentBinding == this.Binding)
                {
                    this.BindingTarget = bindingTarget;
                    bindingData.BindingTarget = this.BindingTarget;
                    bindingData.Element = element;
                    return new List<BindingInfo> { bindingData };
                }
            }
            return base.CreateBindings(element, dataItem, twoWay);
        }

        #endregion Internal Methods
    }
}