Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text.Json
Imports System.Windows
Imports System.Windows.Controls

' Define the Person class
Public Class Person
    Public Property Id As Integer
    Public Property Name As String
    Public Property Age As Integer
End Class

Partial Public Class MainPage
    Inherits Page
    Private persons As List(Of Person)
    Private repo As New PersonRepository()

    Public Sub New()
        Me.InitializeComponent()
        persons = repo.LoadPersons()
        If persons Is Nothing Then
            persons = repo.CreateDefaultPersons()
        End If
        DataGrid1.ItemsSource = persons
    End Sub

    ' Create local JSON file (localStorage) and add 50 records
    Private Sub BtnCreateDb_Clicked(sender As Object, e As RoutedEventArgs)
        persons = repo.CreateDefaultPersons()
        DataGrid1.ItemsSource = Nothing
        DataGrid1.ItemsSource = persons
        MessageBox.Show("Local JSON DB created and 50 Person records added.", "Info")
    End Sub

    ' Read data and display in DataGrid
    Private Sub BtnReadData_Clicked(sender As Object, e As RoutedEventArgs)
        persons = repo.LoadPersons()
        If persons Is Nothing Then
            persons = repo.CreateDefaultPersons()
        End If
        DataGrid1.ItemsSource = Nothing
        DataGrid1.ItemsSource = persons
    End Sub

    ' DataGrid RowEditEnding event for update/add
    Private Sub DataGrid1_RowEditEnding(sender As Object, e As DataGridRowEditEndingEventArgs) Handles DataGrid1.RowEditEnding
        repo.SavePersons(persons)
    End Sub

    ' DataGrid SelectionChanged event for delete (manual delete button)
    Private Sub BtnDeleteSelected_Click(sender As Object, e As RoutedEventArgs)
        If DataGrid1.SelectedItem IsNot Nothing Then
            repo.DeletePerson(persons, CType(DataGrid1.SelectedItem, Person))
            DataGrid1.ItemsSource = Nothing
            DataGrid1.ItemsSource = persons
        End If
    End Sub

    ' Add a new person from form
    Private Sub BtnAddPerson_Click(sender As Object, e As RoutedEventArgs)
        Dim name As String = TxtName.Text.Trim()
        Dim ageValue As Integer
        If String.IsNullOrWhiteSpace(name) Then
            MessageBox.Show("Please enter a name.", "Validation")
            Return
        End If
        If Not Integer.TryParse(TxtAge.Text.Trim(), ageValue) OrElse ageValue < 0 Then
            MessageBox.Show("Please enter a valid age.", "Validation")
            Return
        End If
        Dim nextId As Integer = 1
        If persons.Count > 0 Then
            nextId = persons.Max(Function(p) p.Id) + 1
        End If
        Dim newPerson As New Person With {.Id = nextId, .Name = name, .Age = ageValue}
        persons.Add(newPerson)
        repo.SavePersons(persons)
        DataGrid1.ItemsSource = Nothing
        DataGrid1.ItemsSource = persons
        TxtName.Text = ""
        TxtAge.Text = ""
    End Sub

End Class