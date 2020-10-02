Imports System
Imports JeffFerguson.Gepsio

Module Program
    Sub Main(args As String())
        LoadValidInstance()
        LoadInvalidInstance()
    End Sub

    Private Sub LoadValidInstance()
        Dim xbrlDoc = New XbrlDocument()
        xbrlDoc.Load("..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml")
        CheckValidity(xbrlDoc)
    End Sub

    Private Sub LoadInvalidInstance()
        Dim xbrlDoc = New XbrlDocument()
        xbrlDoc.Load("..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-10-FootnoteFromOutOfScope.xml")
        CheckValidity(xbrlDoc)
    End Sub

    Private Sub CheckValidity(xbrlDoc As XbrlDocument)
        If xbrlDoc.IsValid = True Then
            Console.WriteLine("Congratulations! This document is valid according to the XBRL specification.")
        Else
            Console.WriteLine("This document is invalid according to the XBRL specification.")
            For Each ValidationError In xbrlDoc.ValidationErrors
                Console.Write(ValidationError.Message)
            Next
        End If
    End Sub

End Module
