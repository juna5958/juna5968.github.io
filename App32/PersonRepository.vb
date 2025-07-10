Imports System
Imports System.Collections.Generic
Imports System.Text.Json
Imports OpenSilver.Interop

Public Class PersonRepository
    Private Const LocalStorageKey As String = "persons_json"

    Public Function LoadPersons() As List(Of Person)
        Dim json = OpenSilver.Interop.ExecuteJavaScript($"window.localStorage.getItem('{LocalStorageKey}')")
        If json IsNot Nothing AndAlso json.ToString() <> "null" Then
            Return JsonSerializer.Deserialize(Of List(Of Person))(json.ToString())
        Else
            Return Nothing
        End If
    End Function

    Public Sub SavePersons(persons As List(Of Person))
        Dim json = JsonSerializer.Serialize(persons)
        OpenSilver.Interop.ExecuteJavaScriptVoid($"window.localStorage.setItem('{LocalStorageKey}', `{json}`)")
    End Sub

    Public Function CreateDefaultPersons() As List(Of Person)
        Dim persons = New List(Of Person)()
        For i = 1 To 50
            persons.Add(New Person With {.Id = i, .Name = $"Person {i}", .Age = 20 + (i Mod 30)})
        Next
        SavePersons(persons)
        Return persons
    End Function

    Public Sub DeletePerson(persons As List(Of Person), person As Person)
        persons.Remove(person)
        SavePersons(persons)
    End Sub
End Class
