﻿

Public Class Form1
    'Dim objects
    Const MinObjectWidth As Integer = 50
    Const MineSize As Integer = 20
    Dim game As Form

    Dim RowsNumberTextbox As New TextBox
    Dim ColumnsNumberTextbox As New TextBox

    Dim RowsNumberLabel As New Label
    Dim ColumnsNumberLabel As New Label

    Dim SubmitBoardSizeButton As New Button

    Public Form1Objects As New List(Of myObjectInForm)

    Dim enableResize As Boolean = False

    Dim originalSender As Object
    Dim originalEventArg As EventArgs

    Public numberOfRows As Integer
    Public numberOfColumns As Integer

    Public gameMapButtons(16, 16) As Button

    Public TerminateGameClose As Boolean = True


    'Done with dim


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        originalSender = sender
        originalEventArg = e
        FormBorderStyle = FormBorderStyle.Fixed3D
        Width = 450
        Height = 180
        initializeObjects()
        enableResize = True
        gatherGameInfoForm()


    End Sub

    Private Sub startForm()
        SaveToolStripMenuItem.Visible = False
        clearObjectsText()




    End Sub



    Private Sub clearObjectsText()
        Me.Show()
        If Not IsNothing(game) Then
            game.Controls.Remove(MenuStrip1)
            Controls.Add(MenuStrip1)
            TerminateGameClose = False
            game.Close()
            game.Dispose()
            game = Nothing
        End If
        For Each item In Form1Objects
            If TypeOf item.myObject Is TextBox Then
                item.myObject.Text = ""
            End If
        Next
    End Sub

    Private Sub initializeObjects()
        Form1Objects.Add(New myObjectInForm(RowsNumberLabel, New fraction(1, 8), New fraction(3, 16), "Number of Rows"))
        Form1Objects.Add(New myObjectInForm(RowsNumberTextbox, RowsNumberLabel, 120, 0))
        Form1Objects.Add(New myObjectInForm(ColumnsNumberLabel, RowsNumberLabel, 0, 30, "Number of Columns"))
        Form1Objects.Add(New myObjectInForm(ColumnsNumberTextbox, RowsNumberLabel, 120, 30))
        Form1Objects.Add(New myObjectInForm(SubmitBoardSizeButton, RowsNumberLabel, 90, 60, "Start Game"))
    End Sub

    Private Sub gatherGameInfoForm()
        createGameInfoObjects()



        positonGatherGameInfoObjects()

        showGatherGameInfoObjects()

        positonGatherGameInfoObjects()


    End Sub

    Private Sub positonGatherGameInfoObjects()

        For Each item In Form1Objects
            item.positonObject()
        Next



    End Sub

    Private Sub showGatherGameInfoObjects()
        For Each item In Form1Objects
            item.showObject()
        Next
    End Sub


    Private Function MeasureText(textSource As Object) As Integer
        Return TextRenderer.MeasureText(textSource.Text + "W", textSource.Font).Width
    End Function

    Private Sub createGameInfoObjects()
        AddHandler RowsNumberTextbox.TextChanged, AddressOf RowsNumberTextbox_TextChanged
        AddHandler ColumnsNumberTextbox.TextChanged, AddressOf ColumnsNumberTextbox_TextChanged
        AddHandler SubmitBoardSizeButton.Click, AddressOf SubmitBoardSizeButton_Click
    End Sub

    Private Function ValidateTextDimOfGame(validatingObject As Object) As Boolean
        Try
            If (CInt(validatingObject.Text) > 0) And (CInt(validatingObject.Text) < 31) Then
                Return True
            Else
                MsgBox("Please enter a valid number for the number of rows and columns")
                Return False
            End If
        Catch
            MsgBox("Please enter a valid number for the number of rows and columns")
            Return False
        End Try
    End Function



    Private Sub CreateGame()
        game = New Form
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        game.Name = "Minesweeper"
        AddHandler game.FormClosing, AddressOf game_FormClosing
        numberOfColumns = CInt(ColumnsNumberTextbox.Text)
        numberOfRows = CInt(RowsNumberTextbox.Text)
        Me.Controls.Remove(MenuStrip1)
        game.Controls.Add(MenuStrip1)
        SaveToolStripMenuItem.Visible = True
        CreateGameMap()

        'Me.Hide()
    End Sub

    Private Sub game_FormClosing()
        If TerminateGameClose Then
            Close()
        End If
        TerminateGameClose = True
    End Sub

    Private Sub CreateGameMap()
        ReDim gameMapButtons(numberOfRows, numberOfColumns)
        Dim rowIndex As Integer
        Dim columnIndex As Integer
        Dim mineSpacesPoints(2 * (numberOfColumns + numberOfRows)) As Point

        For rowIndex = 0 To numberOfRows - 1
            mineSpacesPoints(2 * rowIndex) = New Point(numberOfColumns * MineSize * (rowIndex Mod 2), rowIndex * MineSize + 24)
            mineSpacesPoints((2 * rowIndex) + 1) = New Point(numberOfColumns * MineSize * (rowIndex Mod 2), (rowIndex + 1) * MineSize + 24)
            'gameMapButtons(rowIndex, columnIndex) = New Button
            'gameMapButtons(rowIndex, columnIndex).Location = New Point(MineSize * columnIndex, (MineSize * rowIndex) + 24)
            'gameMapButtons(rowIndex, columnIndex).Height = MineSize
            'gameMapButtons(rowIndex, columnIndex).Width = MineSize
            'gameMapButtons(rowIndex, columnIndex).FlatStyle = FlatStyle.Flat
            'game.Controls.Add(gameMapButtons(rowIndex, columnIndex))
        Next
        For columnIndex = 0 To numberOfColumns - 1
            mineSpacesPoints(2 * (numberOfRows + columnIndex)) = New Point(columnIndex * MineSize, (numberOfRows * MineSize * (columnIndex Mod 2)) + 24)
            mineSpacesPoints((2 * (numberOfRows + columnIndex) + 1)) = New Point((columnIndex + 1) * MineSize, (numberOfRows * MineSize * (columnIndex Mod 2)) + 24)
            'gameMapButtons(rowIndex, columnIndex) = New Button
            'gameMapButtons(rowIndex, columnIndex).Location = New Point(MineSize * columnIndex, (MineSize * rowIndex) + 24)
            'gameMapButtons(rowIndex, columnIndex).Height = MineSize
            'gameMapButtons(rowIndex, columnIndex).Width = MineSize
            'gameMapButtons(rowIndex, columnIndex).FlatStyle = FlatStyle.Flat
            'game.Controls.Add(gameMapButtons(rowIndex, columnIndex))
        Next
        game.Width = 16 + (numberOfColumns * MineSize)
        game.Height = 62 + (numberOfRows * MineSize)
        game.Show()
        game.Location = New Point(0, 0)

        game.CreateGraphics.FillRectangle(Brushes.Red, 0, 24, MineSize * numberOfColumns, MineSize * numberOfRows)
        game.CreateGraphics.DrawLines(Pens.Black, mineSpacesPoints)
    End Sub


    Private Sub SubmitBoardSizeButton_Click()
        If ValidateTextDimOfGame(RowsNumberTextbox) Then
            If ValidateTextDimOfGame(ColumnsNumberTextbox) Then
                CreateGame()
            End If
        End If

    End Sub

    Private Sub RowsNumberTextbox_TextChanged()
        positonGatherGameInfoObjects()
    End Sub

    Private Sub ColumnsNumberTextbox_TextChanged()
        positonGatherGameInfoObjects()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If enableResize Then
            positonGatherGameInfoObjects()
        End If
        '
    End Sub

    Private Sub CustomToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CustomToolStripMenuItem.Click
        startForm()
    End Sub
End Class

Public Class myObjectInForm
    Public myObject As Object
    Public myPosition As Point
    Public relativePositionX As Decimal
    Public relativePositionY As Decimal
    Public myInitialText As String
    Const MinObjectWidth As Integer = 50
    Public relativeToObject As Boolean
    Public distanceFromObjectX As Integer
    Public distanceFromObjectY As Integer
    Public focusObject As Object
    Dim relativeObjectPositionX As Integer
    Dim relativeObjectPositionY As Integer


    Public Sub New(addedObject As Object, addedPositionX As Integer, addedPositionY As Integer) 'fixed position without initialized text
        myObject = addedObject
        myPosition = New Point(addedPositionX, addedPositionY)
        relativePositionX = 0
        relativePositionY = 0
        myInitialText = ""
        relativeToObject = False
    End Sub

    Public Sub New(addedObject As Object, addedPositionX As Integer, addedPositionY As Integer, addedInitialText As String) 'fixed position with initialized text
        myObject = addedObject
        myPosition = New Point(addedPositionX, addedPositionY)
        relativePositionX = 0
        relativePositionY = 0
        myInitialText = addedInitialText
        relativeToObject = False
    End Sub

    Public Sub New(addedObject As Object, basisObject As Object, addedPositionX As Integer, addedPositionY As Integer) 'fixed position relative to another object without initialized text
        myObject = addedObject
        relativeToObject = True
        focusObject = basisObject
        distanceFromObjectX = addedPositionX
        distanceFromObjectY = addedPositionY
        addedPositionX = 0
        addedPositionY = 0
        For Each item In Form1.Form1Objects
            If (item.myObject Is basisObject) Then
                addedPositionX = item.myPosition.X
                addedPositionY = item.myPosition.Y
            End If
        Next
        myPosition = New Point(addedPositionX + distanceFromObjectX, addedPositionY + distanceFromObjectY)
        relativePositionX = 0
        relativePositionY = 0
        myInitialText = ""
    End Sub

    Public Sub New(addedObject As Object, basisObject As Object, addedPositionX As Integer, addedPositionY As Integer, addedInitialText As String) 'fixed position relative to another object with initialized text
        myObject = addedObject
        relativeToObject = True
        focusObject = basisObject
        distanceFromObjectX = addedPositionX
        distanceFromObjectY = addedPositionY
        addedPositionX = 0
        addedPositionY = 0
        For Each item In Form1.Form1Objects
            If (item.myObject Is basisObject) Then
                addedPositionX = item.myPosition.X
                addedPositionY = item.myPosition.Y
            End If
        Next
        myPosition = New Point(addedPositionX + distanceFromObjectX, addedPositionY + distanceFromObjectY)
        relativePositionX = 0
        relativePositionY = 0
        myInitialText = addedInitialText
    End Sub

    Public Sub New(addedObject As Object, addedPositionX As fraction, addedPositionY As fraction) 'relative position without initialized text
        myObject = addedObject
        relativePositionX = addedPositionX.decimalRepresentation
        relativePositionY = addedPositionY.decimalRepresentation
        myPosition = New Point(Form1.Width * relativePositionX, Form1.Height * relativePositionY)
        myInitialText = ""
        relativeToObject = False
    End Sub


    Public Sub New(addedObject As Object, addedPositionX As fraction, addedPositionY As fraction, addedInitialText As String) 'relatve position with initialized text
        myObject = addedObject
        relativePositionX = addedPositionX.decimalRepresentation
        relativePositionY = addedPositionY.decimalRepresentation
        myPosition = New Point(Form1.Width * relativePositionX, Form1.Height * relativePositionY)
        myInitialText = addedInitialText
        relativeToObject = False
    End Sub

    Public Sub setObject(objectType As Object)
        myObject = objectType
    End Sub
    Public Sub setObject(objectPositionx As Integer, objectPositiony As Integer)
        myPosition = New Point(objectPositionx, objectPositiony)
    End Sub

    Private Function MeasureText() As Integer
        Return TextRenderer.MeasureText(myObject.Text + "W", myObject.Font).Width
    End Function

    Public Sub positonObject()

        'calculates width
        If (MeasureText() > MinObjectWidth) Then
            myObject.Width = (MeasureText())
        Else
            myObject.Width = MinObjectWidth
        End If
        'end width

        'set height
        'end height

        'calculate position
        If relativeToObject Then
            relativeObjectPositionX = 0
            relativeObjectPositionY = 0
            For Each item In Form1.Form1Objects
                If (item.myObject Is focusObject) Then
                    relativeObjectPositionX = item.myPosition.X
                    relativeObjectPositionY = item.myPosition.Y
                End If
            Next
            myPosition = New Point(relativeObjectPositionX + distanceFromObjectX, relativeObjectPositionY + distanceFromObjectY)
        Else
            If (relativePositionX <> 0) Or (relativePositionY <> 0) Then
                myPosition = New Point(Form1.Width * relativePositionX, Form1.Height * relativePositionY)
            End If
        End If



        myObject.Location = myPosition

    End Sub

    Public Sub showObject()
        Form1.Controls.Add(myObject)
        If myInitialText <> "" Then
            myObject.Text = myInitialText
        End If
        myObject.Show()
    End Sub

End Class


Public Class fraction
    Public numerator As Integer
    Public denomanator As Integer
    Public decimalRepresentation As Decimal

    Public Sub New(fractionNumerator As Integer, fractionDenomanator As Integer)
        numerator = fractionNumerator
        denomanator = fractionDenomanator
        decimalRepresentation = fractionNumerator / fractionDenomanator
    End Sub
End Class
