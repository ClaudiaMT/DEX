﻿#pragma checksum "..\..\Add_cuvant.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "51F0FE4F84A3F56915ACAD4CFB52C749"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace DEX {
    
    
    /// <summary>
    /// Add_cuvant
    /// </summary>
    public partial class Add_cuvant : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\Add_cuvant.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DEX.Add_cuvant Add_cuvant_;
        
        #line default
        #line hidden
        
        
        #line 6 "..\..\Add_cuvant.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_cuvant_nou;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\Add_cuvant.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ok_add_word;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\Add_cuvant.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_cancel_add_word;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\Add_cuvant.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_cuvant_nou_descriere;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Add_cuvant.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox listbox_selectie_categorie;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DEX;component/add_cuvant.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Add_cuvant.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Add_cuvant_ = ((DEX.Add_cuvant)(target));
            return;
            case 2:
            this.txt_cuvant_nou = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.btn_ok_add_word = ((System.Windows.Controls.Button)(target));
            
            #line 8 "..\..\Add_cuvant.xaml"
            this.btn_ok_add_word.Click += new System.Windows.RoutedEventHandler(this.btn_ok_add_word_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_cancel_add_word = ((System.Windows.Controls.Button)(target));
            
            #line 9 "..\..\Add_cuvant.xaml"
            this.btn_cancel_add_word.Click += new System.Windows.RoutedEventHandler(this.btn_cancel_add_word_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txt_cuvant_nou_descriere = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.listbox_selectie_categorie = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

