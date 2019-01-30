

Public Class Form1
    'Dim objects
    Const MinObjectWidth As Integer = 50

    Dim RowsNumberTextbox As New TextBox
    Dim ColumnsNumberTextbox As New TextBox

    Dim RowsNumberLabel As New Label
    Dim ColumnsNumberLabel As New Label

    Dim SubmitBoardSizeButton As New Button

    Dim Form1Objects As New List(Of myObjectInForm)

    Dim enableResize As Boolean = False


    'Done with dim


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initializeObjects()
        gatherGameInfoForm()



    End Sub

    Private Sub initializeObjects()
        Form1Objects.Add(New myObjectInForm(RowsNumberTextbox, 50, 50))
        Form1Objects.Add(New myObjectInForm(RowsNumberLabel, 500, 50))
        Form1Objects.Add(New myObjectInForm(ColumnsNumberLabel, 400, 50))
        Form1Objects.Add(New myObjectInForm(ColumnsNumberTextbox, 300, 50))
        Form1Objects.Add(New myObjectInForm(SubmitBoardSizeButton, 200, 50))
    End Sub

    Private Sub gatherGameInfoForm()
        createGameInfoObjects()



        positonGatherGameInfoObjects()

        showGatherGameInfoObjects()



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
        enableResize = True
    End Sub


    Private Function MeasureText(textSource As Object) As Integer
        Return TextRenderer.MeasureText(textSource.Text + "W", textSource.Font).Width
    End Function

    Private Sub createGameInfoObjects()
        RowsNumberLabel.Text = "RowsNumberLabel"
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
    Const MinObjectWidth As Integer = 50

    Public Sub New(addedObject As Object, addedPositionX As Integer, addedPositionY As Integer)
        myObject = addedObject
        myPosition = New Point(addedPositionX, addedPositionY)
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
        myObject.Location = myPosition

    End Sub

    Public Sub showObject()
        Form1.Controls.Add(myObject)
        myObject.Show()
    End Sub

End Class



