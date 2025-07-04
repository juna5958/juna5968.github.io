Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Controls
Imports OpenSilver.Interop

' Define the Person class
Public Class Person
    Public Property Id As Integer
    Public Property Name As String
    Public Property Age As Integer
End Class

Partial Public Class MainPage
    Inherits Page
    Private persons As List(Of Person)
    Private Const LocalStorageKey As String = "persons_json"

    Public Sub New()
        Me.InitializeComponent()
        LoadPersonsFromLocalStorage()
        DataGrid1.ItemsSource = persons
    End Sub

    ' Create local JSON file (localStorage) and add 50 records
    Private Sub BtnCreateDb_Clicked(sender As Object, e As RoutedEventArgs)
        persons = New List(Of Person)()
        For i = 1 To 50
            persons.Add(New Person With {.Id = i, .Name = $"Person {i}", .Age = 20 + (i Mod 30)})
        Next
        SavePersonsToLocalStorage()
        DataGrid1.ItemsSource = Nothing
        DataGrid1.ItemsSource = persons
        MessageBox.Show("Local JSON DB created and 50 Person records added.", "Info")
    End Sub

    ' Read data and display in DataGrid
    Private Sub BtnReadData_Clicked(sender As Object, e As RoutedEventArgs)
        LoadPersonsFromLocalStorage()
        DataGrid1.ItemsSource = Nothing
        DataGrid1.ItemsSource = persons
    End Sub

    ' Save persons list to localStorage as JSON
    Private Sub SavePersonsToLocalStorage()
        Dim json = JsonSerializer.Serialize(persons)
        OpenSilver.Interop.ExecuteJavaScriptVoid($"window.localStorage.setItem('{LocalStorageKey}', `{json}`)")
    End Sub

    ' Load persons list from localStorage
    Private Sub LoadPersonsFromLocalStorage()
        Dim json = OpenSilver.Interop.ExecuteJavaScript($"window.localStorage.getItem('{LocalStorageKey}')")
        If json IsNot Nothing AndAlso json.ToString() <> "null" Then
            persons = JsonSerializer.Deserialize(Of List(Of Person))(json.ToString())
        Else
            'persons = New List(Of Person)()
            BtnCreateDb_Clicked(Nothing, Nothing) ' Create DB if not found
        End If
    End Sub

    ' DataGrid RowEditEnding event for update/add
    Private Sub DataGrid1_RowEditEnding(sender As Object, e As DataGridRowEditEndingEventArgs) Handles DataGrid1.RowEditEnding
        SavePersonsToLocalStorage()
    End Sub

    ' DataGrid SelectionChanged event for delete (manual delete button)
    Private Sub BtnDeleteSelected_Click(sender As Object, e As RoutedEventArgs)
        If DataGrid1.SelectedItem IsNot Nothing Then
            persons.Remove(CType(DataGrid1.SelectedItem, Person))
            SavePersonsToLocalStorage()
            DataGrid1.ItemsSource = Nothing
            DataGrid1.ItemsSource = persons
        End If
    End Sub

End Class