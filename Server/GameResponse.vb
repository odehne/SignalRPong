Imports System.Web.Script.Serialization

Public Class GameResponse
    Public clientId As String
    Public cmd As String
    Public position As SpawnRequest.PaddlePosition
    Public action As String
    Public X As Double
    Public Y As Double
    Public VX As Double
    Public VY As Double
    Public score As Integer

    Public Function ToJSON() As String
        Dim serializer As New JavaScriptSerializer()
        Return (serializer.Serialize(Me))
    End Function

End Class