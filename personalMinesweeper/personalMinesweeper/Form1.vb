Public Class Form1
    'Dim objects
    Const MinObjectWidth As Integer = 50

    Dim RowsNumberTextbox As TextBox
    Dim ColumnsNumberTextbox As TextBox

    Dim RowsNumberLabel As Label
    Dim ColumnsNumberLabel As Label

    Dim SubmitBoardSizeButton As Button


    'Done with dim


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gatherGameInfoForm()


    End Sub


    Private Sub gatherGameInfoForm()
        createGameInfoObjects()



        positonGatherGameInfoObjects()

        showGatherGameInfoObjects()



    End Sub

    Private Sub positonGatherGameInfoObjects()

        'calculates width
        If ((MeasureText(RowsNumberTextbox)) > MinObjectWidth) Then
            RowsNumberTextbox.Width = (MeasureText(RowsNumberTextbox))
        Else
            RowsNumberTextbox.Width = MinObjectWidth
        End If
        'end width

        'set height
        'end height

        'calculate position
        RowsNumberTextbox.Location = New Point(Me.Width / 2, 50)



    End Sub

    Private Sub showGatherGameInfoObjects()
        Me.Controls.Add(RowsNumberTextbox)
        RowsNumberTextbox.Show()
    End Sub


    Private Function MeasureText(textSource As Object) As Integer
        Return TextRenderer.MeasureText(textSource.Text + "W", textSource.Font).Width
    End Function

    Private Sub createGameInfoObjects()
        RowsNumberTextbox = New TextBox
        AddHandler RowsNumberTextbox.TextChanged, AddressOf RowsNumberTextbox_TextChanged
    End Sub

    Private Sub RowsNumberTextbox_TextChanged()
        positonGatherGameInfoObjects()
    End Sub
End Class
