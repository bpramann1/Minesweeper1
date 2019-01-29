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
        RowsNumberTextbox = New TextBox


        positonGatherGameInfoObjects()
        showGatherGameInfoObjects()




    End Sub

    Private Sub positonGatherGameInfoObjects()

        'calculates width
        If ((RowsNumberTextbox.Font.Height * RowsNumberTextbox.TextLength) > MinObjectWidth) Then
            RowsNumberTextbox.Width = (RowsNumberTextbox.Font.Height * RowsNumberTextbox.TextLength)
        Else
            RowsNumberTextbox.Width = MinObjectWidth
        End If
        RowsNumberTextbox.Width = MinObjectWidth
        'end width

        'set height
        'end height

        'calculate position
        RowsNumberTextbox.Location = New Point(50, 50)



    End Sub

    Private Sub showGatherGameInfoObjects()
        Me.Controls.Add(RowsNumberTextbox)
        RowsNumberTextbox.Show()
    End Sub

End Class
