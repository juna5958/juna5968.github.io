﻿Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Windows
Imports System.Windows.Controls

Partial Public Class App
    Inherits Application
    Public Sub New()
        Me.InitializeComponent()
        Window.Current.Content = New MainPage()
    End Sub
End Class
