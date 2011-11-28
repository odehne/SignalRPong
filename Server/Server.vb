
Imports SignalR
Imports System.Threading.Tasks

Public Class Server
    Inherits PersistentConnection

    Protected Overrides Function OnReceivedAsync(clientId As String, data As String) As Task
        Dim gr As New GameResponse With {.action = "unknown"}

        If data.StartsWith("hello") Then

            Dim pos As SpawnRequest.PaddlePosition = MyGames.AddPlayer(clientId)

            If Not MyGames.PlayersAreReady(clientId) Then
                gr = New GameResponse With {.cmd = "init", .action = "wait", .clientId = clientId, .position = pos}
            Else
                gr = New GameResponse With {.cmd = "init", .action = "go", .clientId = clientId, .position = pos}
            End If
        ElseIf data.StartsWith("spawn") Then
            Dim sr As New SpawnRequest(data)

            Dim p As Player = MyGames.GetOpponentPlayer(sr.ClientId)

            If Not p Is Nothing Then

                gr = New GameResponse With {.cmd = "respawn",
                                           .clientId = p.ClientId,
                                           .position = p.Position,
                                           .X = sr.X,
                                           .Y = sr.Y,
                                           .VX = sr.Vx,
                                           .VY = sr.Vy}
            End If

       
        ElseIf data.StartsWith("score") Then
            Dim sc As New ScoreRequest(data)

            Dim p As Player = MyGames.GetOpponentPlayer(sc.ClientId)

            p.Score += 1

            If Not p Is Nothing Then
                gr = New GameResponse With {.cmd = "newscore", .score = p.Score, .position = p.Position}
            End If

        End If

        If Not gr Is Nothing Then

            Return Connection.Broadcast(gr.ToJSON)

        End If

        Return Nothing
    End Function


    Protected Overrides Sub OnDisconnect(clientId As String)
        MyGames.RemovePlayer(clientId)
    End Sub


    Private Shared _myGames As Games
    Private Shared ReadOnly Property MyGames() As Games
        Get
            If _myGames Is Nothing Then
                _myGames = New Games
            End If
            Return _myGames
        End Get
    End Property

 
End Class