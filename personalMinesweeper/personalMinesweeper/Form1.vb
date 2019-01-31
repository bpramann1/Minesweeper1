

Public Class Form1
    'Dim objects
    Const MinObjectWidth As Integer = 50

    Dim RowsNumberTextbox As New TextBox
    Dim ColumnsNumberTextbox As New TextBox

    Dim RowsNumberLabel As New Label
    Dim ColumnsNumberLabel As New Label

    Dim SubmitBoardSizeButton As New Button

    Public Form1Objects As New List(Of myObjectInForm)

    Dim enableResize As Boolean = False


    'Done with dim


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initializeObjects()
        enableResize = True
        gatherGameInfoForm()



    End Sub

    Private Sub initializeObjects()
        Form1Objects.Add(New myObjectInForm(RowsNumberLabel, New fraction(1, 3), New fraction(1, 8), "Number of Rows"))
        Form1Objects.Add(New myObjectInForm(RowsNumberTextbox, RowsNumberLabel, 120, 0))
        Form1Objects.Add(New myObjectInForm(ColumnsNumberLabel, RowsNumberLabel, 0, 30, "Number of Columns"))
        Form1Objects.Add(New myObjectInForm(ColumnsNumberTextbox, RowsNumberLabel, 120, 30))
        Form1Objects.Add(New myObjectInForm(SubmitBoardSizeButton, RowsNumberLabel, 60, 60, "Start Game"))
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
    End Sub

    Private Sub RowsNumberTextbox_TextChanged()
        positonGatherGameInfoObjects()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If enableResize Then
            positonGatherGameInfoObjects()
        End If
        '
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
