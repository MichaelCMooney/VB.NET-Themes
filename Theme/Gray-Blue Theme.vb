﻿Imports System.Drawing.Drawing2D

Module DesignFunctions
    Function ToBrush(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Brush
        Return ToBrush(Color.FromArgb(A, R, G, B))
    End Function
    Function ToBrush(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Brush
        Return ToBrush(Color.FromArgb(R, G, B))
    End Function
    Function ToBrush(ByVal A As Integer, ByVal C As Color) As Brush
        Return ToBrush(Color.FromArgb(A, C))
    End Function
    Function ToBrush(ByVal Pen As Pen) As Brush
        Return ToBrush(Pen.Color)
    End Function
    Function ToBrush(ByVal Color As Color) As Brush
        Return New SolidBrush(Color)
    End Function
    Function ToPen(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Pen
        Return ToPen(Color.FromArgb(A, R, G, B))
    End Function
    Function ToPen(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Pen
        Return ToPen(Color.FromArgb(R, G, B))
    End Function
    Function ToPen(ByVal A As Integer, ByVal C As Color) As Pen
        Return ToPen(Color.FromArgb(A, C))
    End Function
    Function ToPen(ByVal Color As Color) As Pen
        Return ToPen(New SolidBrush(Color))
    End Function
    Function ToPen(ByVal Brush As SolidBrush) As Pen
        Return New Pen(Brush)
    End Function

    Class CornerStyle
        Public TopLeft As Boolean
        Public TopRight As Boolean
        Public BottomLeft As Boolean
        Public BottomRight As Boolean
    End Class

    Public Function AdvRect(ByVal Rectangle As Rectangle, ByVal CornerStyle As CornerStyle, ByVal Curve As Integer) As GraphicsPath
        AdvRect = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2

        If CornerStyle.TopLeft Then
            AdvRect.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        Else
            AdvRect.AddLine(Rectangle.X, Rectangle.Y, Rectangle.X + ArcRectangleWidth, Rectangle.Y)
        End If

        If CornerStyle.TopRight Then
            AdvRect.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        Else
            AdvRect.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y, Rectangle.X + Rectangle.Width, Rectangle.Y + ArcRectangleWidth)
        End If

        If CornerStyle.BottomRight Then
            AdvRect.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        Else
            AdvRect.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height, Rectangle.X + Rectangle.Width - ArcRectangleWidth, Rectangle.Y + Rectangle.Height)
        End If

        If CornerStyle.BottomLeft Then
            AdvRect.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        Else
            AdvRect.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height, Rectangle.X, Rectangle.Y + Rectangle.Height - ArcRectangleWidth)
        End If

        AdvRect.CloseAllFigures()

        Return AdvRect
    End Function

    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        RoundRect = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        RoundRect.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        RoundRect.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        RoundRect.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        RoundRect.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        RoundRect.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, ArcRectangleWidth + Rectangle.Y))
        RoundRect.CloseAllFigures()
        Return RoundRect
    End Function

    Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
        Return RoundRect(New Rectangle(X, Y, Width, Height), Curve)
    End Function

    Class PillStyle
        Public Left As Boolean
        Public Right As Boolean
    End Class

    Public Function Pill(ByVal Rectangle As Rectangle, ByVal PillStyle As PillStyle) As GraphicsPath
        Pill = New GraphicsPath()

        If PillStyle.Left Then
            Pill.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Height, Rectangle.Height), -270, 180)
        Else
            Pill.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height, Rectangle.X, Rectangle.Y)
        End If

        If PillStyle.Right Then
            Pill.AddArc(New Rectangle(Rectangle.X + Rectangle.Width - Rectangle.Height, Rectangle.Y, Rectangle.Height, Rectangle.Height), -90, 180)
        Else
            Pill.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height)
        End If

        Pill.CloseAllFigures()

        Return Pill
    End Function

    Public Function Pill(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal PillStyle As PillStyle)
        Return Pill(New Rectangle(X, Y, Width, Height), PillStyle)
    End Function

End Module

Public Class GrayCombo
    Inherits ComboBox

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()

        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList
        ItemHeight = 20
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics

        G.Clear(Parent.BackColor)

        Dim BackgroundGradient As LinearGradientBrush = New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 1), Color.FromArgb(75, 130, 195), Color.FromArgb(40, 80, 135))
        G.FillPath(BackgroundGradient, RoundRect(0, 0, Width - 1, Height - 1, 3))
        G.DrawLine(ToPen(100, Color.White), New Point(2, 1), New Point(Width - 2, 1))
        G.DrawPath(Pens.Gray, RoundRect(0, 0, Width - 1, Height - 1, 3))
        G.DrawPath(Pens.Black, RoundRect(0, 0, Width - 1, Height - 2, 3))
        BackgroundGradient.Dispose()

        Dim TriangleRectangle As Rectangle = New Rectangle(Width - 16, Height / 2 - 2, 8, 4)

        Dim TrianglePoints As New List(Of Point)()
        TrianglePoints.Add(New Point(TriangleRectangle.X, TriangleRectangle.Y))
        TrianglePoints.Add(New Point(TriangleRectangle.X + TriangleRectangle.Width, TriangleRectangle.Y))
        TrianglePoints.Add(New Point(TriangleRectangle.X + TriangleRectangle.Width / 2, TriangleRectangle.Y + TriangleRectangle.Height))

        G.FillPolygon(Brushes.White, TrianglePoints.ToArray)

        Try
            If Items.Count > 0 Then
                If Not SelectedIndex = -1 Then
                    G.DrawString(Items(SelectedIndex), Font, Brushes.Black, New Rectangle(5, -1, Width - 20, Height), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                    G.DrawString(Items(SelectedIndex), Font, Brushes.White, New Rectangle(5, 0, Width - 20, Height), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                Else
                    G.DrawString(Items(0), Font, Brushes.Black, New Rectangle(5, -1, Width - 20, Height), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                    G.DrawString(Items(0), Font, Brushes.White, New Rectangle(5, 0, Width - 20, Height), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                End If
            End If
        Catch
            'Something went wrong, so we do nothing.
        End Try
    End Sub


    Sub ReplaceItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        Dim G As Graphics = e.Graphics

        Try
            G.FillRectangle(ToBrush(95, 95, 95), e.Bounds)
            G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, Brushes.LightGray, New Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), New StringFormat With {.LineAlignment = StringAlignment.Center})

            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                Dim BackgroundGradient As LinearGradientBrush = New LinearGradientBrush(New Point(e.Bounds.X, e.Bounds.Y), New Point(e.Bounds.X, e.Bounds.Y + e.Bounds.Height - 1), Color.FromArgb(75, 130, 195), Color.FromArgb(40, 80, 135))
                G.FillPath(BackgroundGradient, RoundRect(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1, 3))
                G.DrawLine(ToPen(100, Color.White), New Point(e.Bounds.X, e.Bounds.Y + 1), New Point(e.Bounds.Width - 1, e.Bounds.Y + 1))
                G.DrawPath(Pens.Gray, RoundRect(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1, 3))
                G.DrawPath(Pens.Black, RoundRect(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 2, 3))

                G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, Brushes.White, New Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), New StringFormat With {.LineAlignment = StringAlignment.Center})
            End If
        Catch
            'Uh oh
        End Try
    End Sub

    Protected Overrides Sub OnSelectedItemChanged(ByVal e As System.EventArgs)
        MyBase.OnSelectedItemChanged(e)

        Invalidate()
    End Sub
End Class


Class GrayButton
    Inherits Control

    Enum State
        None
        Over
        Down
    End Enum

    Private MouseState As State

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw, True)

        MouseState = State.None
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics

        G.Clear(Parent.BackColor)

        Dim BackgroundGradient As LinearGradientBrush = New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.Transparent, Color.Transparent)

        Select Case MouseState
            Case State.None
                BackgroundGradient.LinearColors = New Color() {Color.FromArgb(127, 127, 127), Color.FromArgb(93, 93, 93)}
            Case State.Over
                BackgroundGradient.LinearColors = New Color() {Color.FromArgb(75, 130, 195), Color.FromArgb(40, 80, 135)}
            Case State.Down
                BackgroundGradient.LinearColors = New Color() {Color.FromArgb(55, 110, 175), Color.FromArgb(40, 80, 135)}
        End Select

        G.FillPath(BackgroundGradient, RoundRect(0, 0, Width - 1, Height - 1, 3))
        G.DrawLine(ToPen(100, Color.White), New Point(2, 1), New Point(Width - 3, 1))
        G.DrawPath(Pens.Gray, RoundRect(0, 0, Width - 1, Height - 1, 3))
        G.DrawPath(Pens.Black, RoundRect(0, 0, Width - 1, Height - 2, 3))

        BackgroundGradient.Dispose()

        G.DrawString(Text, New Font(Font.Name, Font.Size, FontStyle.Bold), Brushes.Black, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
        G.DrawString(Text, New Font(Font.Name, Font.Size, FontStyle.Bold), Brushes.White, New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        MouseState = State.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        MouseState = State.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        MouseState = State.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        MouseState = State.Over : Invalidate()
    End Sub
End Class

Class GrayProgressBar
    Inherits Control

#Region " Properties "

    Private _minimum As Integer
    Public Property Minimum() As Integer
        Get
            Return _minimum
        End Get
        Set(ByVal value As Integer)
            _minimum = value

            If value > _maximum Then _maximum = value
            If value > _value Then _value = value

            Invalidate()
        End Set
    End Property

    Private _maximum As Integer
    Public Property Maximum() As Integer
        Get
            Return _maximum
        End Get
        Set(ByVal value As Integer)
            _maximum = value

            If _maximum < 1 Then _maximum = 1

            If value < _minimum Then _minimum = value
            If value < _value Then _value = value

            Invalidate()
        End Set
    End Property

    Event ValueChanged()
    Private _value As Integer
    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set(ByVal value As Integer)
            If value < _minimum Then
                _value = _minimum
            ElseIf value > _maximum Then
                _value = _maximum
            Else
                _value = value
            End If

            Invalidate()
            RaiseEvent ValueChanged()
        End Set
    End Property

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw, True)

        _maximum = 100
        _minimum = 0
        _value = 0
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics

        G.Clear(Parent.BackColor)

        G.FillPath(ToBrush(85, 85, 85), RoundRect(0, 0, Width - 1, Height - 1, 3))

        Dim BackgroundGradient As LinearGradientBrush = New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(75, 130, 195), Color.FromArgb(40, 80, 135))

        G.FillPath(BackgroundGradient, RoundRect(0, 1, (Width - 1) * _value / _maximum, Height - 3, 3))
        G.DrawPath(ToPen(150, Color.Black), RoundRect(0, 1, (Width - 1) * _value / _maximum, Height - 3, 3))

        If _value > 0 Then
            G.SmoothingMode = SmoothingMode.AntiAlias

            G.SetClip(RoundRect(0, 0, (Width - 1) * _value / _maximum - 1, Height - 1, 3))
            For i = 0 To (Width - 1) * _value / _maximum Step 25
                G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(35, Color.White)), 10), New Point(i, 0 - 5), New Point(i + 25, Height + 10))
            Next
            G.ResetClip()

            G.DrawLine(ToPen(100, Color.White), New Point(2, 1), New Point((Width - 3) * _value / _maximum - 3, 1))

            G.SmoothingMode = SmoothingMode.None
        End If

        G.DrawPath(Pens.Gray, RoundRect(0, 0, Width - 1, Height - 1, 3))
        G.DrawPath(Pens.Black, RoundRect(0, 0, Width - 1, Height - 2, 3))
    End Sub
End Class

Class GrayRadioButton
    Inherits Control

    Event CheckedChanged()
    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value

            Invalidate()
            RaiseEvent CheckedChanged()
        End Set
    End Property

    Enum State
        None
        Over
    End Enum

    Private MouseState As State

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw, True)

        Checked = False
        Size = New Size(15, 15)
        MouseState = State.None
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.AntiAlias

        If Checked Then
            G.FillEllipse(New LinearGradientBrush(New Point(Height / 2, 0), New Point(Height / 2, Height), Color.FromArgb(75, 130, 195), Color.FromArgb(40, 80, 135)), New Rectangle(0, 0, Height - 1, Height - 1))
            G.FillEllipse(ToBrush(150, Color.Black), New Rectangle(4, 4, Height - 9, Height - 9))
        Else
            If MouseState = State.Over Then
                G.FillEllipse(New LinearGradientBrush(New Point(Height / 2, 0), New Point(Height / 2, Height), Color.FromArgb(75, 130, 195), Color.FromArgb(40, 80, 135)), New Rectangle(0, 0, Height - 1, Height - 1))
            Else
                G.FillEllipse(New LinearGradientBrush(New Point(Height / 2, 0), New Point(Height / 2, Height), Color.FromArgb(127, 127, 127), Color.FromArgb(93, 93, 93)), New Rectangle(0, 0, Height - 1, Height - 1))
            End If
        End If

        G.DrawEllipse(ToPen(50, Color.White), New Rectangle(0, 1, Height - 1, Height - 2))
        G.DrawEllipse(Pens.Black, New Rectangle(0, 0, Height - 1, Height - 1))
    End Sub

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)

        If Not Checked Then Checked = True

        For Each ctl As Control In Parent.Controls
            If TypeOf ctl Is GrayRadioButton Then
                If ctl.Handle = Me.Handle Then Continue For
                If ctl.Enabled Then DirectCast(ctl, GrayRadioButton).Checked = False
            End If
        Next
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        MouseState = State.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        MouseState = State.None : Invalidate()
    End Sub
End Class

Class GrayContainer
    Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
         ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.UserPaint Or _
         ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics

        G.Clear(Parent.BackColor)

        G.SmoothingMode = SmoothingMode.AntiAlias

        G.FillPath(ToBrush(10, Color.Black), RoundRect(0, 0, Width - 1, Height - 2, 5))
        G.DrawPath(ToPen(50, Color.White), RoundRect(-1, -4, Width + 1, Height + 3, 5))
        G.DrawPath(ToPen(50, Color.Black), RoundRect(0, 0, Width - 1, Height - 2, 5))

        For i = 0 To 10
            G.DrawPath(ToPen(50 / (i + 1), Color.Black), RoundRect(i, i, Width - 1 - (i * 2), Height - 2 - (i * 2), 5))
        Next
    End Sub
End Class


Class GrayForm
    Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                ControlStyles.OptimizedDoubleBuffer Or _
                ControlStyles.UserPaint Or _
                ControlStyles.ResizeRedraw, True)

        Dock = DockStyle.Fill
        BackColor = Color.FromArgb(108, 108, 108)
        ForeColor = Color.White
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle() : SendToBack()
        Parent.FindForm.TransparencyKey = Color.Fuchsia
        Parent.FindForm.FormBorderStyle = FormBorderStyle.None
    End Sub

    Private _ico As Icon
    Public Property Icon() As Icon
        Get
            Return _ico
        End Get
        Set(ByVal value As Icon)
            _ico = value
            Invalidate()
        End Set
    End Property

#Region " Movement and Control Buttons"
    Private Cap As Boolean = False
    Private CapL As Point = Nothing

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If New Rectangle(0, 0, Width - 40, 25).Contains(e.Location) Then
            Cap = True : CapL = e.Location : End If
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e) : If Cap Then Parent.Location = MousePosition - CapL
        Invalidate()
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)
        If New Rectangle(Width - 20, 5, 10, 10).Contains(PointToClient(MousePosition)) Then
            Application.Exit()
        End If
    End Sub
#End Region

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e) : Dim G As Graphics = e.Graphics
        G.Clear(BackColor)

        'Border
        G.DrawRectangle(Pens.DimGray, New Rectangle(1, 1, Width - 3, Height - 3))
        G.DrawPath(Pens.Black, RoundRect(0, 0, Width - 1, Height - 1, 2))

        'Title
        G.FillRectangle(ToBrush(78, 78, 78), New Rectangle(1, 1, Width - 2, 19))
        G.FillRectangle(ToBrush(20, Color.White), New Rectangle(1, 1, Width - 2, 10))

        G.DrawLine(ToPen(150, Color.Black), New Point(1, 19), New Point(Width - 2, 19))
        G.DrawLine(ToPen(50, Color.White), New Point(1, 20), New Point(Width - 2, 20))

        'Title Icon
        Try
            G.DrawIcon(_ico, New Rectangle(3, 3, 16, 16))
        Catch : End Try

        'Title Text
        G.DrawString(Text, Font, ToBrush(120, Color.Black), New Rectangle(1, 0, Width - 2, 19), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        G.DrawString(Text, Font, ToBrush(ForeColor), New Rectangle(1, 1, Width - 2, 19), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        Try
            G.DrawRectangle(ToPen(Parent.FindForm.TransparencyKey), New Rectangle(0, 0, 1, 1))
            G.DrawRectangle(ToPen(Parent.FindForm.TransparencyKey), New Rectangle(Width - 1, 0, 1, 1))
            G.DrawRectangle(ToPen(Parent.FindForm.TransparencyKey), New Rectangle(0, Height - 1, 1, 1))
            G.DrawRectangle(ToPen(Parent.FindForm.TransparencyKey), New Rectangle(Width - 1, Height - 1, 1, 1))
        Catch : End Try

        G.SmoothingMode = SmoothingMode.AntiAlias
        If New Rectangle(Width - 20, 5, 10, 10).Contains(PointToClient(MousePosition)) Then
            G.DrawLine(New Pen(Brushes.White, 2), New Point(Width - 20, 7), New Point(Width - 13, 13))
            G.DrawLine(New Pen(Brushes.White, 2), New Point(Width - 20, 13), New Point(Width - 13, 7))
            If MouseButtons = Windows.Forms.MouseButtons.Left Then
                G.DrawLine(New Pen(Brushes.DarkGray, 2), New Point(Width - 20, 7), New Point(Width - 13, 13))
                G.DrawLine(New Pen(Brushes.DarkGray, 2), New Point(Width - 20, 13), New Point(Width - 13, 7))
            End If
        Else
            G.DrawLine(New Pen(Brushes.Gray, 2), New Point(Width - 20, 7), New Point(Width - 13, 13))
            G.DrawLine(New Pen(Brushes.Gray, 2), New Point(Width - 20, 13), New Point(Width - 13, 7))
        End If
    End Sub
End Class

Class GrayCheck
    Inherits Control

#Region " Properties "

    Event CheckChanged()
    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value

            Invalidate()
            RaiseEvent CheckChanged()
        End Set
    End Property

#End Region

    Enum State
        None
        Over
        Down
    End Enum

    Private MouseState As State

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                ControlStyles.OptimizedDoubleBuffer Or _
                ControlStyles.UserPaint Or _
                ControlStyles.ResizeRedraw, True)

        MouseState = State.None
        Size = New Size(15, 15)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics

        G.Clear(Parent.BackColor)

        Dim BackgroundGradient As LinearGradientBrush = New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.Transparent, Color.Transparent)


        Select Case MouseState
            Case State.None
                If Checked Then
                    BackgroundGradient.LinearColors = New Color() {Color.FromArgb(75, 130, 195), Color.FromArgb(40, 80, 135)}
                Else
                    BackgroundGradient.LinearColors = New Color() {Color.FromArgb(127, 127, 127), Color.FromArgb(93, 93, 93)}
                End If
            Case State.Over
                BackgroundGradient.LinearColors = New Color() {Color.FromArgb(75, 130, 195), Color.FromArgb(40, 80, 135)}
            Case State.Down
                BackgroundGradient.LinearColors = New Color() {Color.FromArgb(55, 110, 175), Color.FromArgb(40, 80, 135)}
        End Select

        G.FillPath(BackgroundGradient, RoundRect(0, 0, Width - 1, Height - 1, 2))

        G.SmoothingMode = SmoothingMode.AntiAlias

        If Checked Then
            Dim c As Rectangle = New Rectangle(3, 2, Width - 7, Height - 7)
            G.DrawLine(New Pen(Brushes.Black, 2), New Point(c.X, c.Y + c.Height / 2), New Point(c.X + c.Width / 2, c.Y + c.Height))
            G.DrawLine(New Pen(Brushes.Black, 2), New Point(c.X + c.Width / 2, c.Y + c.Height), New Point(c.X + c.Width, c.Y))
        End If

        G.SmoothingMode = SmoothingMode.None

        G.DrawLine(ToPen(100, Color.White), New Point(2, 1), New Point(Width - 3, 1))
        G.DrawPath(Pens.Gray, RoundRect(0, 0, Width - 1, Height - 1, 2))
        G.DrawPath(Pens.Black, RoundRect(0, 0, Width - 1, Height - 2, 2))

        BackgroundGradient.Dispose()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e) : MouseState = State.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e) : MouseState = State.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e) : MouseState = State.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : MouseState = State.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)

        Checked = Not Checked
    End Sub
End Class