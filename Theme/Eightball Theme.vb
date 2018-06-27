﻿Imports System.Drawing.Drawing2D, System.ComponentModel

Module Drawing

    Public Function RoundRect(ByVal rect As Rectangle, ByVal slope As Integer) As GraphicsPath
        Dim gp As GraphicsPath = New GraphicsPath()
        Dim arcWidth As Integer = slope * 2
        gp.AddArc(New Rectangle(rect.X, rect.Y, arcWidth, arcWidth), -180, 90)
        gp.AddArc(New Rectangle(rect.Width - arcWidth + rect.X, rect.Y, arcWidth, arcWidth), -90, 90)
        gp.AddArc(New Rectangle(rect.Width - arcWidth + rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 0, 90)
        gp.AddArc(New Rectangle(rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 90, 90)
        gp.CloseAllFigures()
        Return gp
    End Function

    Public Function TopRoundRect(ByVal rect As Rectangle, ByVal slope As Integer) As GraphicsPath
        Dim gp As GraphicsPath = New GraphicsPath
        Dim arcWidth As Integer = slope * 2
        gp.AddArc(New Rectangle(rect.X, rect.Y, arcWidth, arcWidth), -180, 90)
        gp.AddArc(New Rectangle(rect.Width - arcWidth + rect.X, rect.Y, arcWidth, arcWidth), -90, 90)
        gp.AddLine(New Point(rect.X + rect.Width, rect.Y + arcWidth), New Point(rect.X + rect.Width, rect.Y + rect.Height))
        gp.AddLine(New Point(rect.X + rect.Width, rect.Y + rect.Height), New Point(rect.X, rect.Y + rect.Height))
        gp.AddLine(New Point(rect.X, rect.Y + rect.Height), New Point(rect.X, rect.Y + arcWidth))
        gp.CloseAllFigures()
        Return gp
    End Function

    Public Function LeftRoundRect(ByVal rect As Rectangle, ByVal slope As Integer) As GraphicsPath
        Dim gp As GraphicsPath = New GraphicsPath()
        Dim arcWidth As Integer = slope * 2
        gp.AddArc(New Rectangle(rect.X, rect.Y, arcWidth, arcWidth), -180, 90)
        gp.AddLine(New Point(rect.X + arcWidth, rect.Y), New Point(rect.Width, rect.Y))
        gp.AddLine(New Point(rect.X + rect.Width, rect.Y), New Point(rect.X + rect.Width, rect.Y + rect.Height))
        gp.AddLine(New Point(rect.X + rect.Width, rect.Y + rect.Height), New Point(rect.X + arcWidth, rect.Y + rect.Height))
        gp.AddArc(New Rectangle(rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 90, 90)
        gp.CloseAllFigures()
        Return gp
    End Function

    Public Function TabControlRect(ByVal rect As Rectangle, ByVal slope As Integer) As GraphicsPath
        Dim gp As GraphicsPath = New GraphicsPath()
        Dim arcWidth As Integer = slope * 2
        gp.AddLine(New Point(rect.X, rect.Y), New Point(rect.X, rect.Y))
        gp.AddArc(New Rectangle(rect.Width - arcWidth + rect.X, rect.Y, arcWidth, arcWidth), -90, 90)
        gp.AddArc(New Rectangle(rect.Width - arcWidth + rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 0, 90)
        gp.AddArc(New Rectangle(rect.X, rect.Height - arcWidth + rect.Y, arcWidth, arcWidth), 90, 90)
        gp.CloseAllFigures()
        Return gp
    End Function

    Public Sub ShadowedString(ByVal g As Graphics, ByVal s As String, ByVal font As Font, ByVal brush As Brush, ByVal pos As Point)
        g.DrawString(s, font, Brushes.Black, New Point(pos.X + 1, pos.Y + 1))
        g.DrawString(s, font, brush, pos)
    End Sub

    Public Function getSmallerRect(ByVal rect As Rectangle, ByVal value As Integer) As Rectangle
        Return New Rectangle(rect.X + value, rect.Y + value, rect.Width - (value * 2), rect.Height - (value * 2))
    End Function

    Public Function AlterColor(ByVal original As Color, Optional ByVal amount As Integer = -20) As Color
        Dim c As Color = original, a As Integer = amount
        Dim r, g, b As Integer
        If c.R + a < 0 Then
            r = 0
        ElseIf c.R + a > 255 Then
            r = 255
        Else
            r = c.R + a
        End If
        If c.G + a < 0 Then
            g = 0
        ElseIf c.G + a > 255 Then
            g = 255
        Else
            g = c.G + a
        End If
        If c.B + a < 0 Then
            b = 0
        ElseIf c.B + a > 255 Then
            b = 255
        Else
            b = c.B + a
        End If
        Return Color.FromArgb(r, g, b)
    End Function

End Module

Class EightBallContainer
    Inherits ContainerControl

    Private moveHeight As Integer = 39
    Private formCanMove As Boolean = False
    Private mouseX, mouseY As Integer
    Private overExit, overMin As Boolean

    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Private _icon As Icon
    Public Property Icon() As Icon
        Get
            Return _icon
        End Get
        Set(ByVal value As Icon)
            _icon = value
            Invalidate()
        End Set
    End Property

    Private _showIcon As Boolean
    Public Property ShowIcon() As Boolean
        Get
            Return _showIcon
        End Get
        Set(ByVal value As Boolean)
            _showIcon = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        Dock = DockStyle.Fill
        Font = New Font("Segoe UI", 9)
        BackColor = Color.FromArgb(90, 90, 90)
        ShowIcon = True
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        If FindForm.TransparencyKey = Nothing Then FindForm.TransparencyKey = Color.Fuchsia
        FindForm.FormBorderStyle = FormBorderStyle.None
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.HighQuality
        g.Clear(FindForm.TransparencyKey)

        'Entire Form
        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim slope As Integer = 8
        Dim mainPath As GraphicsPath = TopRoundRect(mainRect, slope)
        g.FillPath(New SolidBrush(Color.FromArgb(60, 60, 60)), mainPath)

        'Title Gradient
        Dim titleTopRect As New Rectangle(0, 0, Width - 1, moveHeight / 1.75)
        Dim titleTopPath As GraphicsPath = TopRoundRect(titleTopRect, slope)
        Dim titleTopBrush As New LinearGradientBrush(New Rectangle(titleTopRect.X, titleTopRect.Y, titleTopRect.Width, titleTopRect.Height + 2), Color.FromArgb(100, 100, 100), Color.FromArgb(60, 60, 60), 90.0F)
        g.FillPath(titleTopBrush, titleTopPath)

        'Inner Form Area
        Dim innerRect As New Rectangle(6, moveHeight, Width - 13, Height - moveHeight - 8)
        g.FillRectangle(New SolidBrush(BackColor), innerRect)
        g.DrawRectangle(Pens.Black, innerRect)

        'Title and Icon
        Dim textY As Integer = (moveHeight / 2) - (g.MeasureString(Text, Font).Height / 2) + 1
        If _showIcon And _icon IsNot Nothing Then
            g.DrawIcon(_icon, New Rectangle(8, 6, moveHeight - 11, moveHeight - 11))
            ShadowedString(g, Text, Font, Brushes.White, New Point(8 + moveHeight - 11 + 4, textY))
        Else
            ShadowedString(g, Text, Font, Brushes.White, New Point(8, textY))
        End If

        'Control Box Exit
        Dim exitRect As New Rectangle(Width - 29, 8, 22, 22)
        Dim exitPath As GraphicsPath = RoundRect(exitRect, 3)
        Dim exitBrush As New LinearGradientBrush(exitRect, Color.FromArgb(105, 105, 105), Color.FromArgb(75, 75, 75), 90.0F)
        g.FillPath(exitBrush, exitPath)
        If overExit Then g.FillPath(New SolidBrush(Color.FromArgb(15, Color.White)), exitPath)
        g.DrawPath(New Pen(Color.FromArgb(40, 40, 40)), exitPath)
        g.DrawString("r", New Font("Marlett", 10), Brushes.LightGray, New Point(Width - 26, 13))

        'Control Box Minimize
        Dim minRect As New Rectangle(Width - 55, 8, 22, 22)
        Dim minPath As GraphicsPath = RoundRect(minRect, 3)
        Dim minBrush As New LinearGradientBrush(minRect, Color.FromArgb(105, 105, 105), Color.FromArgb(75, 75, 75), 90.0F)
        g.FillPath(minBrush, minPath)
        If overMin Then g.FillPath(New SolidBrush(Color.FromArgb(15, Color.White)), minPath)
        g.DrawPath(New Pen(Color.FromArgb(40, 40, 40)), minPath)
        g.DrawString("0", New Font("Marlett", 13), Brushes.LightGray, New Point(Width - 53, 10))

        'Borders
        g.DrawPath(Pens.DimGray, TopRoundRect(New Rectangle(mainRect.X + 1, mainRect.Y, mainRect.Width - 2, mainRect.Height), slope))
        g.DrawPath(Pens.DimGray, TopRoundRect(New Rectangle(mainRect.X, mainRect.Y + 1, mainRect.Width, mainRect.Height - 2), slope))
        g.SmoothingMode = SmoothingMode.None
        g.DrawPath(New Pen(Color.FromArgb(40, 40, 40)), mainPath)

    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)

        If formCanMove = True Then
            FindForm.Location = MousePosition - New Point(mouseX, mouseY)
        End If

        If e.Y > 8 AndAlso e.Y < 30 Then
            If e.X > Width - 29 AndAlso e.X < Width - 7 Then overExit = True Else overExit = False
            If e.X > Width - 55 AndAlso e.X < Width - 33 Then overMin = True Else overMin = False
        Else
            overExit = False
            overMin = False
        End If

        Invalidate()

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        mouseX = e.X
        mouseY = e.Y

        If e.Y <= moveHeight AndAlso overExit = False AndAlso overMin = False Then formCanMove = True

        If overExit Then
            FindForm.Close()
        ElseIf overMin Then
            FindForm.WindowState = FormWindowState.Minimized
        Else
            Focus()
        End If

    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        formCanMove = False
    End Sub

End Class

Class EightBallButton
    Inherits Control

    Enum MouseState
        None
        Over
        Down
    End Enum

    Private State As MouseState

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        Font = New Font("Segoe UI", 9)
        BackColor = Color.Gray
        ForeColor = Color.WhiteSmoke
        Size = New Size(120, 40)
        Cursor = Cursors.Hand
        State = MouseState.None
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.Clear(Parent.BackColor)
        g.SmoothingMode = SmoothingMode.HighQuality

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = RoundRect(mainRect, 6)

        Dim bgBrush As New LinearGradientBrush(mainRect, BackColor, Color.FromArgb(40, 40, 40), 90.0F)
        g.FillPath(bgBrush, mainPath)
        If State = MouseState.Over Then
            g.FillPath(New SolidBrush(Color.FromArgb(15, Color.White)), mainPath)
        ElseIf State = MouseState.Down Then
            g.FillPath(New SolidBrush(Color.FromArgb(25, Color.White)), mainPath)
        End If

        Dim textX, textY As Integer
        textX = ((Width - 1) / 2) - (g.MeasureString(Text, Font).Width / 2)
        textY = ((Height - 1) / 2) - (g.MeasureString(Text, Font).Height / 2)
        ShadowedString(g, Text, Font, New SolidBrush(ForeColor), New Point(textX, textY))

        g.DrawPath(New Pen(Color.FromArgb(30, 30, 30)), mainPath)

    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub

End Class

Class EightBallProgressbar
    Inherits Control

    Private _barColor As Color
    Public Property BarColor() As Color
        Get
            Return _barColor
        End Get
        Set(ByVal value As Color)
            _barColor = value
            Invalidate()
        End Set
    End Property

    Private _maximum As Integer = 100
    Public Property Maximum() As Integer
        Get
            Return _maximum
        End Get
        Set(ByVal v As Integer)
            If v < 1 Then v = 1
            If v < _value Then _value = v
            _maximum = v
            Invalidate()
        End Set
    End Property

    Private _value As Integer
    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set(ByVal v As Integer)
            If v > _maximum Then v = _maximum
            _value = v
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        Size = New Size(200, 26)
        BackColor = Color.FromArgb(80, 80, 80)
        BarColor = Color.Gray
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g As Graphics = e.Graphics
        g.Clear(Parent.BackColor)
        g.SmoothingMode = SmoothingMode.HighQuality

        Dim slope As Integer = 6

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = RoundRect(mainRect, slope)
        Dim bgBrush As New LinearGradientBrush(mainRect, BackColor, Color.FromArgb(25, 25, 25), 90.0F)
        g.FillPath(bgBrush, mainPath)

        Dim percent As Single = (_value / _maximum) * 100
        If percent > 2.75 Then
            Dim barRect As New Rectangle(0, 0, CInt((Width / _maximum) * _value) - 1, Height - 1)
            Dim barPath As GraphicsPath = RoundRect(barRect, slope)
            Dim barBrush As New LinearGradientBrush(barRect, BarColor, Color.FromArgb(45, 45, 45), 90.0F)
            g.FillPath(barBrush, barPath)
        End If

        g.DrawPath(New Pen(Color.FromArgb(30, 30, 30)), mainPath)

    End Sub

End Class

<DefaultEvent("TextChanged")> Class EightBallTextBox
    Inherits Control

    Private _MaxLength As Integer = 32767
    Public Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If Base IsNot Nothing Then
                Base.MaxLength = value
            End If
        End Set
    End Property

    Private _ReadOnly As Boolean
    Public Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If Base IsNot Nothing Then
                Base.ReadOnly = value
            End If
        End Set
    End Property

    Private _UseSystemPasswordChar As Boolean
    Public Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If Base IsNot Nothing Then
                Base.UseSystemPasswordChar = value
            End If
        End Set
    End Property

    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If Base IsNot Nothing Then
                Base.Text = value
            End If
        End Set
    End Property

    Public Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If Base IsNot Nothing Then
                Base.Font = value
                Base.Location = New Point(3, 5)
                Base.Width = Width - 6
            End If
        End Set
    End Property

    Private _image As Image
    Public Property Image() As Image
        Get
            Return _image
        End Get
        Set(ByVal value As Image)
            _image = value
            If _image IsNot Nothing Then
                Base.Location = New Point(33, 5)
                Base.Width = Width - 38
            Else
                Base.Location = New Point(5, 5)
                Base.Width = Width - 10
            End If
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(Base) Then
            Controls.Add(Base)
        End If
    End Sub

    Private Base As TextBox

    Sub New()

        Font = New Font("Arial", 9)
        ForeColor = Color.DimGray
        Cursor = Cursors.IBeam

        Base = New TextBox
        Base.Font = Font
        Base.Text = Text
        Base.ForeColor = ForeColor
        Base.MaxLength = _MaxLength
        Base.ReadOnly = _ReadOnly
        Base.BackColor = Color.Gainsboro
        Base.UseSystemPasswordChar = _UseSystemPasswordChar
        Base.BorderStyle = BorderStyle.None
        Base.Location = New Point(5, 5)
        Base.Width = Width - 10

        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Size = New Size(Size.Width, Base.Height + 12)

        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 3

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = RoundRect(mainRect, slope)
        G.FillPath(Brushes.Gainsboro, mainPath)
        G.DrawPath(New Pen(Color.FromArgb(55, 55, 55)), mainPath)

        If _image IsNot Nothing Then
            Dim imageArea As New GraphicsPath
            imageArea.AddArc(0, 0, slope * 2, slope * 2, -90, -90)
            imageArea.AddLine(New Point(0, slope), New Point(0, Height - slope - 1))
            imageArea.AddArc(0, Height - (slope * 2) - 1, slope * 2, slope * 2, -180, -90)
            imageArea.AddLine(New Point(slope * 2, Height - 1), New Point(28, Height - 1))
            imageArea.AddLine(New Point(28, Height - 1), New Point(28, 0))
            imageArea.AddLine(New Point(28, 0), New Point(slope * 2, 0))
            imageArea.CloseAllFigures()
            G.FillPath(Brushes.Gainsboro, imageArea)
            G.DrawPath(New Pen(Color.FromArgb(55, 55, 55)), imageArea)
            G.DrawImage(_image, 7, 5, 16, 16)
        End If

    End Sub

    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        Text = Base.Text
    End Sub

    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            Base.SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    <Category("Options")> _
    Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If Base IsNot Nothing Then
                Base.TextAlign = value
            End If
        End Set
    End Property

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        If _image IsNot Nothing Then
            Base.Location = New Point(33, 6)
            Base.Width = Width - 38
        Else
            Base.Location = New Point(6, 6)
            Base.Width = Width - 12
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        Base.SelectionStart = Base.TextLength
        Base.Focus()
    End Sub

End Class

Class EightBallLabel
    Inherits Label

    Sub New()
        BackColor = Color.Transparent
        ForeColor = Color.WhiteSmoke
        Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        Focus()
    End Sub

End Class

Class EightBallGroupBox
    Inherits ContainerControl

    Private _titleAlign As HorizontalAlignment
    Public Property TitleAlignment() As HorizontalAlignment
        Get
            Return _titleAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _titleAlign = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.ContainerControl, True)
        Size = New Size(300, 140)
        BackColor = Color.FromArgb(70, 70, 70)
        ForeColor = Color.WhiteSmoke
        Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 6

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = RoundRect(mainRect, slope)
        G.FillPath(New SolidBrush(BackColor), mainPath)

        Dim titleRect As New Rectangle(0, 0, Width - 1, 26)
        Dim titlePath As GraphicsPath = TopRoundRect(titleRect, slope)
        Dim titleBrush As New LinearGradientBrush(titleRect, Color.FromArgb(100, 100, 100), BackColor, 90.0F)
        G.FillPath(titleBrush, titlePath)

        Dim textX As Integer
        If _titleAlign = HorizontalAlignment.Left Then
            textX = 5
        ElseIf _titleAlign = HorizontalAlignment.Center Then
            textX = ((Me.Width - 1) / 2) - (G.MeasureString(Text, Font).Width / 2)
        Else
            textX = Me.Width - 5 - G.MeasureString(Text, Font).Width - 1
        End If
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(textX, 5))

        G.DrawPath(Pens.Black, mainPath)
        G.DrawLine(Pens.Black, New Point(0, 26), New Point(Width - 1, 26))

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        Focus()
    End Sub

End Class

Class EightBallTabControl
    Inherits TabControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        Size = New Size(400, 200)
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(35, 145)
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim FontColor As New Color
        Dim borderPen As New Pen(Color.FromArgb(55, 55, 55))

        Dim mainAreaRect As New Rectangle(ItemSize.Height + 2, 2, Width - 1 - ItemSize.Height - 2, Height - 3)
        Dim mainAreaPath As GraphicsPath = TabControlRect(mainAreaRect, 4)
        G.FillPath(New SolidBrush(Color.FromArgb(100, 100, 100)), mainAreaPath)
        G.DrawPath(borderPen, mainAreaPath)

        For i = 0 To TabCount - 1

            If i = SelectedIndex Then

                Dim mainRect As Rectangle = GetTabRect(i)
                Dim mainPath As GraphicsPath = LeftRoundRect(mainRect, 6)

                G.FillPath(New SolidBrush(Color.FromArgb(100, 100, 100)), mainPath)
                G.DrawPath(borderPen, mainPath)

                Dim orbRect As New Rectangle(mainRect.X + 12, mainRect.Y + (mainRect.Height / 2) - 8, 16, 16)
                G.FillEllipse(Brushes.SteelBlue, orbRect)
                G.FillEllipse(New LinearGradientBrush(orbRect, Color.FromArgb(30, Color.White), Color.FromArgb(10, Color.Black), 115.0F), orbRect)

                '// Color of out side of circle below
                G.DrawEllipse(New Pen(Color.FromArgb(40, 105, 145)), orbRect)
                '// Color of out side of circle above

                G.SmoothingMode = SmoothingMode.None
                G.DrawLine(New Pen(Color.FromArgb(100, 100, 100)), New Point(mainRect.X + mainRect.Width, mainRect.Y + 1), New Point(mainRect.X + mainRect.Width, mainRect.Y + mainRect.Height - 1))
                G.SmoothingMode = SmoothingMode.HighQuality

                FontColor = Color.White

                Dim titleX As Integer = (mainRect.Location.X + 28 + 8)
                Dim titleY As Integer = (mainRect.Location.Y + mainRect.Height / 2) - (G.MeasureString(TabPages(i).Text, Font).Height / 2)
                G.DrawString(TabPages(i).Text, Font, New SolidBrush(FontColor), New Point(titleX, titleY))

            Else

                Dim tabRect As Rectangle = GetTabRect(i)
                Dim mainRect As New Rectangle(tabRect.X + 6, tabRect.Y, tabRect.Width - 6, tabRect.Height)
                Dim mainPath As GraphicsPath = LeftRoundRect(mainRect, 6)

                G.FillPath(New SolidBrush(Color.FromArgb(75, 75, 75)), mainPath)
                G.DrawPath(borderPen, mainPath)

                Dim orbRect As New Rectangle(mainRect.X + 12, mainRect.Y + (mainRect.Height / 2) - 8, 16, 16)
                G.FillEllipse(Brushes.Gray, orbRect)
                G.DrawEllipse(Pens.DimGray, orbRect)

                FontColor = Color.Silver

                Dim titleX As Integer = (mainRect.Location.X + 28 + 8)
                Dim titleY As Integer = (mainRect.Location.Y + mainRect.Height / 2) - (G.MeasureString(TabPages(i).Text, Font).Height / 2)
                G.DrawString(TabPages(i).Text, Font, New SolidBrush(FontColor), New Point(titleX, titleY))

            End If

            Try : TabPages(i).BackColor = Color.FromArgb(100, 100, 100) : Catch : End Try

        Next

    End Sub

End Class

Class EightBallComboBox
    Inherits ComboBox

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        Font = New Font("Arial", 9)
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()

        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList
        DoubleBuffered = True
        ItemHeight = 20

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 4

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = RoundRect(mainRect, slope)
        Dim bgLGB As New LinearGradientBrush(mainRect, Color.FromArgb(85, 85, 85), Color.FromArgb(60, 60, 60), 90.0F)
        G.FillPath(bgLGB, mainPath)
        G.DrawPath(New Pen(Color.FromArgb(55, 55, 55)), mainPath)

        Dim triangle As Point() = New Point() {New Point(Width - 14, 16), New Point(Width - 17, 10), New Point(Width - 11, 10)}
        G.FillPolygon(Brushes.DarkGray, triangle)
        G.DrawLine(New Pen(Color.FromArgb(55, 55, 55)), New Point(Width - 25, 1), New Point(Width - 25, Height - 2))

        Try
            If Items.Count > 0 Then
                If Not SelectedIndex = -1 Then
                    Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Items(SelectedIndex), Font).Height / 2)
                    G.DrawString(Items(SelectedIndex), Font, Brushes.WhiteSmoke, New Point(6, textY))
                Else
                    Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Items(0), Font).Height / 2)
                    G.DrawString(Items(0), Font, Brushes.WhiteSmoke, New Point(6, textY))
                End If
            End If
        Catch : End Try

    End Sub

    Sub replaceItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim rect As New Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width + 1, e.Bounds.Height + 1)

        Try

            G.FillRectangle(New SolidBrush(Parent.BackColor), e.Bounds)
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                Dim bgBrush As New LinearGradientBrush(rect, Color.FromArgb(170, 170, 170), Color.FromArgb(140, 140, 140), 90.0F)
                G.FillRectangle(bgBrush, rect)
                G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, Brushes.White, New Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height))
                G.DrawRectangle(Pens.Silver, rect)
            Else
                Dim bgLGB As New LinearGradientBrush(rect, Color.FromArgb(135, 135, 135), Color.FromArgb(110, 110, 110), 90.0F)
                G.FillRectangle(bgLGB, rect)
                G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, Brushes.DarkGray, New Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height))
                G.DrawRectangle(Pens.Silver, rect)
            End If

        Catch : End Try

    End Sub

    Protected Overrides Sub OnSelectedItemChanged(ByVal e As System.EventArgs)
        MyBase.OnSelectedItemChanged(e)
        Invalidate()
    End Sub

End Class

<DefaultEvent("CheckedChanged")> Class EightBallCheckBox
    Inherits Control

    Event CheckedChanged(ByVal sender As Object)

    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        Size = New Size(150, 20)
        ForeColor = Color.WhiteSmoke
        Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Size = New Size(Size.Width, 20)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 4

        Dim boxRect As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim boxPath As GraphicsPath = RoundRect(boxRect, slope)
        Dim bgBrush As New LinearGradientBrush(boxRect, Color.FromArgb(120, 120, 120), Color.FromArgb(100, 100, 100), 90.0F)
        G.FillPath(bgBrush, boxPath)
        G.DrawPath(New Pen(Color.FromArgb(50, 50, 50)), boxPath)

        Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Text, Font).Height / 2) + 1
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point((Height - 1) + 4, textY))

        If _checked Then
            Dim checkFont As New Font("Marlett", 13)
            G.DrawString("b", checkFont, New SolidBrush(ForeColor), New Point(0, 2))
        End If

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        Focus()

        If _checked Then
            _checked = False
        Else
            _checked = True
        End If

        RaiseEvent CheckedChanged(Me)
        Invalidate()

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Class EightBallRadioButton
    Inherits Control

    Event CheckedChanged(ByVal sender As Object)

    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        Size = New Size(140, 20)
        ForeColor = Color.WhiteSmoke
        Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim circleRect As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim bgBrush As New LinearGradientBrush(circleRect, Color.FromArgb(120, 120, 120), Color.FromArgb(100, 100, 100), 90.0F)
        G.FillEllipse(bgBrush, circleRect)
        G.DrawEllipse(New Pen(Color.FromArgb(50, 50, 50)), circleRect)

        Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Text, Font).Height / 2) + 1
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point((Height - 1) + 4, textY))

        If _checked Then
            Dim checkedRect As New Rectangle(5, 5, Height - 11, Height - 11)
            Dim checkedBrush As New LinearGradientBrush(checkedRect, Color.LightGray, Color.Gray, 90.0F)
            G.FillEllipse(checkedBrush, checkedRect)
            G.DrawEllipse(New Pen(Color.FromArgb(70, 70, 70)), checkedRect)
        End If

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is EightBallRadioButton Then
                DirectCast(C, EightBallRadioButton).Checked = False
            End If
        Next

        If _checked Then
            _checked = False
        Else
            _checked = True
        End If

        RaiseEvent CheckedChanged(Me)
        Invalidate()

    End Sub

End Class