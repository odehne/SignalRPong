Public Class ScoreRequest

    Private _clientId As String

    Public Property ClientId() As String
        Get
            Return _clientId
        End Get
        Set (ByVal value As String)
            _clientId = value
        End Set
    End Property

    Public Sub New(ByVal data As String)

        If Not String.IsNullOrEmpty(data) Then

            Dim kv() As String = data.Split("=")

            Me.ClientId = kv(1)

        End If

    End Sub
End Class