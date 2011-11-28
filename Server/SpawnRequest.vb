Public Class SpawnRequest

    Public Enum PaddlePosition
        left
        right
    End Enum

    Private _position As PaddlePosition
    Private _clientId As String
    Private _x As Double
    Private _y As Double
    Private _vx As Double
    Private _vy As Double

    Public Property Vx() As Double
        Get
            Return _vx
        End Get
        Set(ByVal value As Double)
            _vx = value
        End Set
    End Property

    Public Property Vy() As Double
        Get
            Return _vy
        End Get
        Set(ByVal value As Double)
            _vy = value
        End Set
    End Property

    Public Sub New(cmd As String)

        If Not String.IsNullOrEmpty(cmd) Then

            Dim s() As String = cmd.Split(",")

            For Each singleCmd As String In s

                Dim s1() As String = singleCmd.Split("=")

                Select Case s1(0).ToLower

                    Case "spawn"
                        Position = [Enum].Parse(GetType(PaddlePosition), s1(1))
                    Case "id"
                        ClientId = s1(1)
                    Case "x"
                        X = Double.Parse(s1(1))
                    Case "y"
                        Y = Double.Parse(s1(1))
                    Case "vx"
                        Vx = Double.Parse(s1(1))
                    Case "vy"
                        Vy = Double.Parse(s1(1))
                End Select

            Next

        End If

    End Sub

    Public Property Position() As PaddlePosition
        Get
            Return _position
        End Get
        Set(ByVal value As PaddlePosition)
            _position = value
        End Set
    End Property

    Public Property ClientId() As String
        Get
            Return _clientId
        End Get
        Set(ByVal value As String)
            _clientId = value
        End Set
    End Property

    Public Property X() As Double
        Get
            Return _x
        End Get
        Set(ByVal value As Double)
            _x = value
        End Set
    End Property

    Public Property Y() As Double
        Get
            Return _y
        End Get
        Set(ByVal value As Double)
            _y = value
        End Set
    End Property
End Class