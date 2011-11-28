Public Class Games
    Inherits List(Of Game)


    Public Function PlayersAreReady(clientId As String) As Boolean
        Dim g As Game = GetGame(clientId)

        If Not g Is Nothing Then

            Return g.PlayersReady

        End If

        Return False
    End Function

    Public Sub RemovePlayer(clientId As String)

        Dim g As Game = GetGame(clientId)

        If Not g Is Nothing Then

            g.ResetClient(clientId)

        End If


    End Sub

    Public Function AddPlayer(clientId As String) As SpawnRequest.PaddlePosition

        For Each g In Me

            If String.IsNullOrEmpty(g.Player1.ClientId) Then
                g.Player1 = New Player With {.ClientId = clientId, .Position = SpawnRequest.PaddlePosition.left, .Score = 0}
                Return g.Player1.Position
            ElseIf String.IsNullOrEmpty(g.Player2.ClientId) Then
                g.Player2 = New Player With {.ClientId = clientId, .Position = SpawnRequest.PaddlePosition.right, .Score = 0}
                Return g.Player2.Position
            End If

        Next

        'All slots are filled, we need a new game
        Dim g1 As New Game
        g1.Player1 = New Player With {.ClientId = clientId, .Position = SpawnRequest.PaddlePosition.left, .Score = 0}
        Add(g1)

        Return g1.Player1.Position

    End Function

    Public Function GetOpponentPlayer(clientId As String) As Player
        Dim g As Game = GetGame(clientId)

        If Not g Is Nothing Then
            If g.Player1.ClientId = clientId Then
                Return g.Player2
            Else
                Return g.Player1
            End If
        End If

        Return Nothing
    End Function

    Public Function GetGame(ByVal clientId As String) As Game
        For Each g In Me

            If Not g.Player1 Is Nothing AndAlso g.Player1.ClientId = clientId Then
                Return g
            End If

            If Not g.Player2 Is Nothing AndAlso g.Player2.ClientId = clientId Then
                Return g
            End If

        Next

        Return Nothing
    End Function

End Class
