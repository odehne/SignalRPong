Public Class Player

    Private _clientId As String
    Private _position As SpawnRequest.PaddlePosition
    Private _score As Integer

    Public Sub New()

    End Sub

    Public Sub New(clientId As String, data As String)

        Me.ClientId = clientId
        Me.Score = 0

        If Not String.IsNullOrEmpty(data) Then

            Dim cmds() As String = data.Split(",")

            For Each cmd As String In cmds

                If cmd.ToLower = "position" Then

                    Dim kv() As String = cmd.Split("=")

                    Me.Position = [Enum].Parse(GetType(SpawnRequest.PaddlePosition), kv(1))

                End If

            Next

        End If

    End Sub

    Public Property ClientId() As String
        Get
            Return _clientId
        End Get
        Set(ByVal value As String)
            _clientId = value
        End Set
    End Property

    Public Property Position() As SpawnRequest.PaddlePosition
        Get
            Return _position
        End Get
        Set(ByVal value As SpawnRequest.PaddlePosition)
            _position = value
        End Set
    End Property

    Public Property Score() As Integer
        Get
            Return _score
        End Get
        Set(ByVal value As Integer)
            _score = value
        End Set
    End Property
End Class