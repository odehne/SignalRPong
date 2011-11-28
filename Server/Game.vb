Public Class Game

    Public Property Player1 As Player
    Public Property Player2 As Player

    Public Sub New()
        Player1 = New Player
        Player2 = New Player
    End Sub

    Public ReadOnly Property PlayersReady() As Boolean
        Get
            If Not String.IsNullOrEmpty(Player1.ClientId) AndAlso Not String.IsNullOrEmpty(Player2.ClientId) Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public Sub ResetClient(clientId As String)

        If Player1.ClientId = clientId Then
            Player1.ClientId = ""
            Player1.Score = 0
        End If

        If Player2.ClientId = clientId Then
            Player2.ClientId = ""
            Player2.Score = 0
        End If

    End Sub

End Class