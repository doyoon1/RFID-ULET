Imports System.Data.OleDb
Imports System.IO.File
Imports System.IO.FileStream
Imports Microsoft.VisualBasic.Devices
Imports System.Net
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form1
    Dim Conn As OleDbConnection

    Private Sub Populate()
        sql = "Select RFID, NAME, LRN, GRADE, DEPARTMENT, IMAGE from StudentInfo"
        cmd = New OleDbCommand(sql, cn)
        Dim da As New OleDbDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)
        DBTABLE.DataSource = dt
        DBTABLE2.DataSource = dt
    End Sub

    Private Sub ButtonNew_Click(sender As Object, e As EventArgs) Handles ButtonNew.Click
        StudentInfoBindingSource.AddNew()
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        sql = "INSERT INTO [StudentInfo] (RFID, [NAME], LRN, [GRADE], DEPARTMENT) Values('" & TextBoxRFID.Text & "','" & TextBoxName.Text & "','" & TextBoxLRN.Text & "','" & TextBoxGrade.Text & "','" & TextBoxDepartment.Text & "')"
        cmd = New OleDbCommand(sql, cn)
        cmd.ExecuteNonQuery()
        MsgBox("Information Successfully Saved", MsgBoxStyle.Information, "Student Information")

        Call Populate()
    End Sub

    Private Sub ButtonRemove_Click(sender As Object, e As EventArgs) Handles ButtonRemove.Click
        If DBTABLE.SelectedRows.Count > 0 Then
            Dim selectedRowIndex As Integer = DBTABLE.SelectedRows(0).Index
            Dim rfidValue As String = DBTABLE.Rows(selectedRowIndex).Cells(0).Value.ToString()

            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Dim commandText As String = "DELETE FROM [StudentInfo] WHERE RFID = @RFID"
                cmd = New OleDbCommand(commandText, cn)
                cmd.Parameters.AddWithValue("@RFID", rfidValue)
                cmd.ExecuteNonQuery()
                MsgBox("Information Successfully Deleted", MsgBoxStyle.Information, "Student Information")
                Call Populate()
            End If
        Else
            MessageBox.Show("No record selected to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub ButtonBrowseImage_Click(sender As Object, e As EventArgs) Handles ButtonBrowseImage.Click
        OpenFileDialog1.ShowDialog()
        TextBoxImage.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub TextBoxImage_TextChanged(sender As Object, e As EventArgs) Handles TextBoxImage.TextChanged
        If (System.IO.File.Exists(TextBoxImage.Text)) Then
            PictureBoxImageInput.Image = Image.FromFile(TextBoxImage.Text)
        End If
        If TextBoxImage.Text = "" Then
            PictureBoxImageInput.Hide()
        Else
            PictureBoxImageInput.Show()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call connection()
        Call Populate()
    End Sub

    Private Sub ButtonConnection_Click(sender As Object, e As EventArgs) Handles ButtonConnection.Click
        PictureBoxSelect.Top = ButtonConnection.Top
        PanelUserData.Visible = False
        PanelRegistrationEditUserData.Visible = False
        PanelMasterlist.Visible = False
        PanelConnection.Visible = True
    End Sub

    Private Sub ButtonUserData_Click(sender As Object, e As EventArgs) Handles ButtonUserData.Click
        PictureBoxSelect.Top = ButtonUserData.Top
        PanelConnection.Visible = False
        PanelRegistrationEditUserData.Visible = False
        PanelMasterlist.Visible = False
        PanelUserData.Visible = True
    End Sub

    Private Sub ButtonRegistration_Click(sender As Object, e As EventArgs) Handles ButtonRegistration.Click
        PictureBoxSelect.Top = ButtonRegistration.Top
        PanelConnection.Visible = False
        PanelUserData.Visible = False
        PanelMasterlist.Visible = False
        PanelRegistrationEditUserData.Visible = True
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PictureBoxSelect.Top = ButtonRegistration.Top
        PanelConnection.Visible = False
        PanelUserData.Visible = False
        PanelRegistrationEditUserData.Visible = False
        PanelMasterlist.Visible = True
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        GroupBoxImage.Location = New Point((PanelUserData.Width / 2) - (GroupBoxImage.Width / 2), GroupBoxImage.Top)
    End Sub

    Private Sub PanelRegistrationEditUserData_Paint(sender As Object, e As PaintEventArgs) Handles PanelRegistrationEditUserData.Paint
        e.Graphics.DrawRectangle(New Pen(Color.LightGray, 2), PanelConnection.ClientRectangle)
    End Sub

    Private Sub PanelRegistrationEditUserData_Resize(sender As Object, e As EventArgs) Handles PanelRegistrationEditUserData.Resize
        PanelRegistrationEditUserData.Invalidate()
    End Sub

    Private Sub ButtonClearForm_Click(sender As Object, e As EventArgs) Handles ButtonClearForm.Click
        TextBoxRFID.Clear()
        TextBoxName.Clear()
        TextBoxLRN.Clear()
        TextBoxGrade.Clear()
        TextBoxDepartment.Clear()
        TextBoxImage.Clear()
    End Sub
    Private Sub DBTABLE_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DBTABLE.CellClick
        TextBoxRFID.Text = DBTABLE.Rows(e.RowIndex).Cells(0).Value.ToString
        TextBoxName.Text = DBTABLE.Rows(e.RowIndex).Cells(1).Value.ToString
        TextBoxLRN.Text = DBTABLE.Rows(e.RowIndex).Cells(2).Value.ToString
        TextBoxGrade.Text = DBTABLE.Rows(e.RowIndex).Cells(3).Value.ToString
        TextBoxDepartment.Text = DBTABLE.Rows(e.RowIndex).Cells(4).Value.ToString
    End Sub

    Private Sub DBTABLE2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DBTABLE2.CellClick
        TextBoxRFID2.Text = DBTABLE2.Rows(e.RowIndex).Cells(0).Value.ToString
        TextBoxName2.Text = DBTABLE2.Rows(e.RowIndex).Cells(1).Value.ToString
        TextBoxLRN2.Text = DBTABLE2.Rows(e.RowIndex).Cells(2).Value.ToString
        TextBoxGrade2.Text = DBTABLE2.Rows(e.RowIndex).Cells(3).Value.ToString
        TextBoxDepartment2.Text = DBTABLE2.Rows(e.RowIndex).Cells(4).Value.ToString
    End Sub

End Class


'Private Sub DBTABLE_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DBTABLE.CellContentClick
'    'TextBoxRFID.Text = DBTABLE.Rows(e.RowIndex).Cells(0).Value
'End Sub

'Private Sub TextBoxRFID_TextChanged(sender As Object, e As EventArgs) Handles TextBoxRFID.TextChanged
'    sql = "Select RFID,NAME,LRN,GRADE,DEPARTMENT from StudentInfo where [RFID]='" & TextBoxRFID.Text & "'"
'    cmd = New OleDbCommand(sql, cn)
'    dr = cmd.ExecuteReader
'    If dr.Read = True Then
'        TextBoxName.Text = dr("NAME")
'        TextBoxLRN.Text = dr("LRN")
'        TextBoxGrade.Text = dr("GRADE")
'        TextBoxDepartment.Text = dr("DEPARTMENT")
'    End If
'End Sub
