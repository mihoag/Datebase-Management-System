﻿#pragma checksum "..\..\..\..\..\View\EmployeeSide\showMedicine.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "39066D20159823354B72F8EE31BB48F9F872BC0C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HospitalManagement.View.EmployeeSide;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace HospitalManagement.View.EmployeeSide {
    
    
    /// <summary>
    /// showMedicine
    /// </summary>
    public partial class showMedicine : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\..\View\EmployeeSide\showMedicine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox keyWord;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\View\EmployeeSide\showMedicine.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ComboboxMedicine;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/HospitalManagement;component/view/employeeside/showmedicine.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\EmployeeSide\showMedicine.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\..\..\..\View\EmployeeSide\showMedicine.xaml"
            ((HospitalManagement.View.EmployeeSide.showMedicine)(target)).Loaded += new System.Windows.RoutedEventHandler(this.MainLoad);
            
            #line default
            #line hidden
            return;
            case 2:
            this.keyWord = ((System.Windows.Controls.TextBox)(target));
            
            #line 15 "..\..\..\..\..\View\EmployeeSide\showMedicine.xaml"
            this.keyWord.KeyDown += new System.Windows.Input.KeyEventHandler(this.search);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ComboboxMedicine = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            
            #line 27 "..\..\..\..\..\View\EmployeeSide\showMedicine.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.back);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

