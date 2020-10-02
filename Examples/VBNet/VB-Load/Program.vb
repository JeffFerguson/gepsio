Imports JeffFerguson.Gepsio

Module Program
    Sub Main(args As String())
        LoadFromLocalFile()
        LoadFromUrl()
    End Sub

    Private Sub LoadFromUrl()
        Dim xbrlDoc = New XbrlDocument()
        xbrlDoc.Load("http://xbrlsite.com/US-GAAP/BasicExample/2010-09-30/abc-20101231.xml")
    End Sub

    Private Sub LoadFromLocalFile()
        Dim xbrlDoc = New XbrlDocument()
        xbrlDoc.Load("..\..\..\..\..\..\JeffFerguson.Gepsio.Test\XBRL-CONF-2014-12-10\Common\300-instance\301-01-IdScopeValid.xml")
    End Sub

End Module
