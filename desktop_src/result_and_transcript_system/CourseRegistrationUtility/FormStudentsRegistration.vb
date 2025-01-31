﻿Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class FormStudentsRegistration
    Dim course_dept_idr As Integer = 1 '
    Dim session_idr As String = "2018/2019"
    Dim strSQL As String
    Dim strConnectionString As String
    Dim ref As Integer

    Dim tmpDS As DataSet

    Private Sub FormStudentsRegistration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized

        Me.BackColor = RGBColors.colorBlack2
        Me.dgvStudents.BackgroundColor = RGBColors.colorBlack2
        Me.dgvStudents.RowsDefaultCellStyle.BackColor = RGBColors.colorSilver
        Me.dgvStudents.RowsDefaultCellStyle.ForeColor = RGBColors.colorBlack

        Me.dgvImportCourses.BackgroundColor = RGBColors.colorBlack2
        Me.dgvImportCourses.RowsDefaultCellStyle.BackColor = RGBColors.colorSilver
        Me.dgvImportCourses.RowsDefaultCellStyle.ForeColor = RGBColors.colorBlack
    End Sub
    Private Sub FormStudentsRegistration_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        On Error Resume Next
        txtStudentMATNO.Text = ""
        If dictDepts.Count < 1 Then
            bgwLoad.RunWorkerAsync()
        Else
            ComboBoxDepartments.Items.Clear()
            For Each key In dictDepts.Keys
                ComboBoxDepartments.Items.Add(dictDepts(key))
            Next
            TextBoxDeptID.Text = dictDepts.Keys(0)
        End If
        BgWProcess.RunWorkerAsync(1)  'runs GetData()
    End Sub

    Public Sub GetData()
        Dim dDept As Integer = 1
        Try
            'mappDB.Dept = 1 'TODO
            If CInt(TextBoxDeptID.Text) > 0 Then
                dDept = CInt(TextBoxDeptID.Text) 'TODO
            Else

            End If
            tmpDS = mappDB.GetDataWhere(String.Format(STR_SQL_ALL_STUDENTS_IN_DEPT, dDept.ToString), "students")
            'TODO:
            If dictAllCourseCodeKeyAndCourseUnitVal.Count = 0 Then mappDB.getAllCourses()
            CheckedListBoxCourses.Items.Clear()
            For Each key In dictAllCourseCodeKeyAndCourseUnitVal.Keys
                CheckedListBoxCourses.Items.Add(key)
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Sub resizeDatagrids(dGrid As String)
        If dgvCourses.Rows.Count > 0 And dGrid = "Courses" Then

            For Each col As DataGridViewColumn In dgvCourses.Columns
                col.Width = 0
                col.Visible = False
                If col.HeaderText = "matno" Then
                    col.Width = 70
                    col.Visible = True
                ElseIf col.HeaderText = "CourseCode_1" Then
                    col.Width = 200
                    col.Visible = True
                ElseIf col.HeaderText = "CourseCode_2" Then
                    col.Width = 200
                    col.Visible = True
                End If
            Next
        ElseIf dgvStudents.Rows.Count > 0 And dGrid = "Students" Then

            For Each col As DataGridViewColumn In dgvStudents.Columns
                col.Width = 50
                If col.HeaderText = "matno" Then
                    col.Width = 100
                ElseIf col.HeaderText = "first_name" Then
                    col.Width = 150
                ElseIf col.HeaderText = "surname" Then
                    col.Width = 100
                End If
            Next
        End If
    End Sub
    Public Sub getRegisteredCoursesForStudent(dMATNO As String)
        Try
            Dim tmpDSCourses As DataSet
            mappDB.MATNO = dMATNO
            tmpDSCourses = mappDB.GetDataWhere(String.Format(STR_SQL_COURSES_REG_WHERE, dMATNO), "Courses") 'todo
            dgvCourses.DataSource = tmpDSCourses.Tables("Courses").DefaultView

            dgvCourses.Refresh()
            resizeDatagrids("courses")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtStudentMATNO_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStudentMATNO.TextChanged
        Dim strSearch As String = txtStudentMATNO.Text
        If txtStudentMATNO.Text.Length > 3 And dgvStudents.Rows.Count = 0 Then
            'Dim tmpDSStudents = mappDB.GetDataWhere(String.Format(STR_SQL_SEARCH_STUDENTS_BY_MATNO_SURNAME_FIRSTNAME, strSearch, strSearch, strSearch), "Students")
            'dgvStudents.DataSource = tmpDSStudents.Tables("Students").DefaultView
        ElseIf dgvStudents.Rows.Count > 0 Then
            'just filter
            On Error Resume Next
            If Not dgvStudents.SelectedRows(0).Cells("matno").Value = Nothing Then
                filterStudents(strSearch)
            End If
        End If
    End Sub


    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        btnReset.Enabled = False
        Me.ProgressBarBS.Value = 5
        TimerBS.Enabled = True
        TimerBS.Start()

        BgWProcess.RunWorkerAsync(1)  'runs GetData()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Public Sub CaptureCourses()
        If dgvCourses.Rows.Count < 1 Then Exit Sub
        dgvCourses.Rows(0).Selected = True
        Dim units As Integer
        Dim MATNO As String = dgvCourses.SelectedRows(0).Cells("matno").Value.ToString
        'Dim session As Double = CInt(dgvCourses.SelectedRows(0).Cells("session_idr").Value)
        'Dim fees As Double = CInt(dgvCourses.SelectedRows(0).Cells("Fees_Status").Value)


        Dim courseCodes1 As String() = (dgvCourses.SelectedRows(0).Cells("CourseCode_1").Value).ToString.Split(";")
        Dim courseCodes2 As String() = (dgvCourses.SelectedRows(0).Cells("CourseCode_2").Value).ToString.Split(";")
        Dim sumUnits As Integer = 0

        For Each c1 In courseCodes1
            If dictAllCourseCodeKeyAndCourseUnitVal.ContainsKey(c1) Then
                units = dictAllCourseCodeKeyAndCourseUnitVal(c1)
                sumUnits = sumUnits + units
            End If
        Next
        ''Capture Studennt TODO
        mappDB.MATNO = MATNO

        'Me.TextBoxCourseCode.Text = courseCode.ToString
        'Me.TextBoxSemester.Text = semester.ToString
        'Me.TextBoxUnits.Text = sumUnits.ToString
        Me.TextBoxTotalCredits.Text = sumUnits.ToString


    End Sub

    Sub filterReg(dMATNO As String)
        If dgvCourses.Rows.Count > 0 Then
            dgvCourses.DataSource.RowFilter = String.Format(STR_FILTER_REG_BY_MATNO, dMATNO)
        End If
    End Sub
    Sub filterStudents(dMATNO As String)
        If dgvStudents.Rows.Count > 0 Then
            dgvStudents.DataSource.RowFilter = String.Format(STR_FILTER_STUDENTS, dMATNO, dMATNO, dMATNO)
        End If
    End Sub
    Public Sub unregisterStudent()


    End Sub
    Public Sub registerStudent(all As Boolean)
        ButtonSaveBroadsheet_Click()
        dgvCourses.Update()
    End Sub
    Private Sub ButtonSaveBroadsheet_Click()
        Try
            Dim strSQL As String
            Dim dv As DataView = dgvCourses.DataSource
            Dim dtSource As DataTable
            Dim dtDestination As New DataTable
            Dim dSFtomDB As New DataSet ' = dv.ToTable
            dtSource = dv.ToTable

            'createBroadsheetTables()
            strSQL = "SELECT * FROM reg" ' WHERE session_idr={1}"

            Using xconn As New OleDb.OleDbConnection(ModuleGeneral.STR_connectionString32)
                Try
                    xconn.Open()
                Catch ex1 As Exception
                    xconn.ConnectionString = ModuleGeneral.STR_connectionString32
                    xconn.Open()
                End Try
                Dim adapter As New OleDb.OleDbDataAdapter(strSQL, xconn)
                'Dim insert As OleDb.OleDbCommand("INSERT INTO Broadsheet (matno) VALUES (@matno)", xconn)
                Dim builder As New OleDb.OleDbCommandBuilder(adapter)       'easy way for single table
                'Dim titleParam As New OleDb.OleDbParameter("@matno", Str)
                'cmd.Parameters.Add(titleParam)
                'adapter.InsertCommand = insert

                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey

                'fill it
                adapter.Fill(dSFtomDB)
                'put it in a datagrid view and all the manipulations can happen there, afterwards an update is used to save in database
                dgvImportCourses.DataSource = dSFtomDB.Tables(0).DefaultView
                'MsgBox("After fresh fill")
                'edit it
                dSFtomDB.Tables(0).Clear()
                adapter.Update(dSFtomDB)
                Dim dRow As DataRow
                Dim strColNames As String = ""
                Dim nExtraCols As Integer = 1
                'MsgBox("After empty db")
                'Note col85 and Col6 are for repeated courses hence Text datatype
                For i = 0 To dtSource.Rows.Count - 1
                    dRow = dSFtomDB.Tables(0).Rows.Add("MOCK00" & i.ToString) 'add mock row
                    For j = 0 To dSFtomDB.Tables(0).Columns.Count - 1 - nExtraCols      'Take as much as we have cols for to avoid errors
                        If j > dtSource.Rows.Count - 1 Then Exit For
                        dSFtomDB.Tables(0).Rows(i).Item(j) = dtSource.Rows(i).Item(j)   'update the row with data
                        'strColNames = strColNames & "," & dtSource.Columns(j).ColumnName
                    Next
                    'dRow.Item("ColNames") = strColNames
                Next

                dgvImportCourses.DataSource = dSFtomDB.Tables(0).DefaultView

                'MsgBox("After add to datatable")
                dgvImportCourses.Refresh()
                dgvImportCourses.EndEdit()
                ' MsgBox("After refresh")
                'save
                adapter.Update(dSFtomDB)
            End Using
        Catch ex As Exception
            MsgBox("Error occured, see log for details" & vbCrLf & ex.Message)
            logerror(ex.ToString)
        End Try
    End Sub
    Public Sub updatePix()
        'todo: get from ser doc
        Dim tmpileName As String = Application.StartupPath & "\photos\" & dgvStudents.SelectedRows(0).Cells("matno").Value & ".jpg"
        Dim tmpileName2 As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\photos\" & dgvStudents.SelectedRows(0).Cells("matno").Value & ".jpg"
        Dim tmpileName3 As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\photos\" & dgvStudents.SelectedRows(0).Cells("matno").Value & ".jpg"


        Try

            If My.Computer.FileSystem.FileExists(tmpileName) Then
                PictureBox1.Image = Image.FromFile(tmpileName)
            ElseIf My.Computer.FileSystem.FileExists(tmpileName2) Then
                PictureBox1.Image = Image.FromFile(tmpileName2)
            ElseIf My.Computer.FileSystem.FileExists(tmpileName3) Then
                PictureBox1.Image = Image.FromFile(tmpileName3)
            End If

        Catch ex As Exception

        End Try

    End Sub
    Public Function UpdateRecordWhere(s As String) As String
        Dim returnVal As String = ""
        Try
            Using con As New OleDb.OleDbConnection(STR_connectionString)
                strSQL = s
                Dim cmd As New OleDb.OleDbCommand(strSQL, con)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return returnVal
    End Function
    Public Function InsertRecord(s As String) As String
        Dim returnVal As String = ""
        Try
            Using con As New OleDb.OleDbConnection(STR_connectionString)
                strSQL = s
                Dim cmd As New OleDb.OleDbCommand(strSQL, con)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return returnVal
    End Function
    Private Sub ButtonUnregister_Click(sender As Object, e As EventArgs) Handles ButtonUnregister.Click
        unregisterStudent()
    End Sub

    Private Sub dgvStudents_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvStudents.CellContentDoubleClick
        getRegisteredCoursesForStudent(dgvStudents.Rows(0).Cells("matno").Value.ToString)
    End Sub

    Private Sub dgv_courses_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCourses.CellContentDoubleClick
        showCoursesList(e.ColumnIndex)
    End Sub

    Private Sub dgvStudents_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvStudents.RowsAdded
        On Error Resume Next
        For Each row As DataGridViewRow In Me.dgvStudents.Rows
            If row.Cells.Item("Fees_Status").Value.ToString = "no" Then
                row.DefaultCellStyle.BackColor = Color.White

            ElseIf row.Cells.Item("Fees_Status").Value.ToString = "yes" Then
                row.DefaultCellStyle.BackColor = Color.PaleVioletRed

            End If
        Next
    End Sub
    Private Sub dgvStudents_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvStudents.RowEnter
        On Error Resume Next
        If Not dgvStudents.SelectedRows(0).Cells("matno").Value = Nothing Then
            filterReg(dgvStudents.SelectedRows(0).Cells("matno").Value.ToString)
        End If
    End Sub

    Private Function computeCredits() As Integer
        Try
            'for each row
            'sum =sum + row(units)
            Return 0
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        PictureBox2.Visible = False
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        PictureBox2.Image = PictureBox1.Image
        PictureBox2.Visible = Not PictureBox2.Visible
    End Sub

    Private Sub FormStudentsRegistration_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        MainForm.doCloseForm()
    End Sub
    Private Sub TimerBS_Tick(sender As Object, e As EventArgs) Handles TimerBS.Tick
        If ProgressBarBS.Value < 97 Then ProgressBarBS.Value = (ProgressBarBS.Value + 3)
    End Sub


    Private Sub ButtonRegister_Click(sender As Object, e As EventArgs) Handles ButtonRegister.Click
        Dim dv As DataView
        Dim dt As DataTable
        dv = dgvCourses.DataSource
        dt = dv.ToTable
        If dgvCourses.Rows.Count > 0 Then
            If CInt(TextBoxSemester.Text) > 1 Then
                If dt.Rows(0).Item("CourseCode_2").Value.contains(TextBoxCourseCode.Text) Then
                    MsgBox("Already Registered")
                Else
                    dt.Rows(0).Item("CourseCode_2").Value = dt.Rows(0).Item("CourseCode_2").Value.ToString & ";" & TextBoxCourseCode.Text
                End If
            Else
                If dt.Rows(0).Item("CourseCode_1").Value.contains(TextBoxCourseCode.Text) Then
                    MsgBox("Already Registered")
                Else
                    dt.Rows(0).Item("CourseCode_1").Value = dt.Rows(0).Item("CourseCode_1").Value.ToString & ";" & TextBoxCourseCode.Text
                End If
            End If
        Else
            If CInt(TextBoxSemester.Text) > 1 Then
                dt.Rows.Add({mappDB.MATNO, "", TextBoxCourseCode.Text}) '2nd sem
            Else
                dt.Rows.Add({mappDB.MATNO, TextBoxCourseCode.Text, ""})
            End If
            dt.Rows(0).Item("session_idr").Value = TextBoxSession.Text

            dgvCourses.DataSource = dt.DefaultView
        End If
    End Sub

    Private Sub dgvCourses_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCourses.CellClick
        showCoursesList(e.ColumnIndex)
    End Sub

    Private Sub dgvCourses_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCourses.CellDoubleClick
        showCoursesList(e.ColumnIndex)
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        registerStudent(False)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        FormStudent.FormBorderStyle = FormBorderStyle.FixedDialog
        If FormStudent.ShowDialog() = DialogResult.OK Then

            GetData()
        End If
    End Sub
    Private Sub dgv_courses_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCourses.CellContentClick
        showCoursesList(e.ColumnIndex)
    End Sub
    Sub showCoursesList(dColumnIndex As Integer)
        Dim dLeft As Integer = 0
        If PanelCourses.Visible = True Then PanelCourses.Visible = False Else PanelCourses.Visible = True
        'CheckedListBoxCourses.Width = dgvCourses.Columns("CourseCode_1").Width + 10   'todo: calc the with as only once
        'dLeft = dgvCourses.Left
        ''For i = 0 To dColumnIndex ' dgv_courses.Columns.Count  'no need to calculate this bcos it will hide the listbox in the panel
        ''    dLeft = dLeft + dgvCourses.Columns(i).Width
        ''Next
        'If dColumnIndex > 0 Then
        '    dLeft = dLeft + dgvCourses.Columns(1).Width
        'Else
        '    CheckedListBoxCourses.Left = dLeft
        'End If

    End Sub


    Private Sub CheckedListBoxCourses_KeyPress(sender As Object, e As KeyPressEventArgs)

    End Sub

    Private Sub ButtonImportRegFERMA_Click(sender As Object, e As EventArgs) Handles ButtonImportRegFERMA.Click
        Dim FileOpenDialogBroadsheet As New OpenFileDialog()
        Dim resultFileName As String = ""
        Dim tmpDS As New DataSet
        Dim dt As DataTable
        Try


            If Not FileOpenDialogBroadsheet.ShowDialog = DialogResult.Cancel Then
                resultFileName = FileOpenDialogBroadsheet.FileName()
                objExcelFile.excelFileName = resultFileName
                tmpDS = objExcelFile.readResultFile()
                'Do some check
                dt = tmpDS.Tables(0)
                If dt.Rows(0).Item(0) = "MatNo" Then
                    For j = 0 To dt.Columns.Count - 1
                        dt.Columns(j).ColumnName = dt.Rows(0).Item(j).ToString
                    Next
                    dt.Rows(0).Delete()
                End If
                dgvImportCourses.DataSource = dt.DefaultView
                dgvImportCourses.Tag = "FERMA"
                dgvImportCourses.BringToFront()
            End If
        Catch ex As Exception
            logError(ex.ToString)
            MsgBox("Something went wrong!")
        End Try

    End Sub


    Private Sub ComboBoxDepartments_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxDepartments.SelectedIndexChanged
        Try
            course_dept_idr = mappDB.getDeptID(ComboBoxDepartments.SelectedItem.ToString)
            TextBoxDeptID.Text = course_dept_idr.ToString
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bgwLoad_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgwLoad.DoWork
        Select Case e.Argument
            Case 1
                dictDepts = combolistDict(STR_SQL_ALL_DEPARTMENTS_COMBO, "dept_id", "dept_name")
                dictSessions = combolistDict(STR_SQL_ALL_SESSIONS_COMBO, "session_id", "session_id")
                dictCourses = combolistDict(String.Format(STR_SQL_ALL_COURSES_ORDER, session_idr, course_dept_idr), "all_courses_1", "all_courses_1")
                'dictCoursesOrderFS = combolistDict(String.Format(STR_SQL_ALL_COURSES_ORDER, session_idr, course_dept_idr), "FS" & TextBoxLevel.Text, "FS" & TextBoxLevel.Text)
                'dictCoursesOrderSS = combolistDict(String.Format(STR_SQL_ALL_COURSES_ORDER, session_idr, course_dept_idr), "SS" & TextBoxLevel.Text & "L", "SS" & TextBoxLevel.Text & "L")
                dictAllCourses = combolistDict(STR_SQL_ALL_COURSES, "course_code", "course_code")
            Case Else
                dictDepts = combolistDict(STR_SQL_ALL_DEPARTMENTS_COMBO, "dept_id", "dept_name")
                dictSessions = combolistDict(STR_SQL_ALL_SESSIONS_COMBO, "session_id", "session_id")
                dictCourses = combolistDict(String.Format(STR_SQL_ALL_COURSES_ORDER, session_idr, course_dept_idr), "all_courses_1", "all_courses_1")
                'dictCoursesOrderFS = combolistDict(String.Format(STR_SQL_ALL_COURSES_ORDER, session_idr, course_dept_idr), "FS" & TextBoxLevel.Text, "FS" & TextBoxLevel.Text)
                'dictCoursesOrderSS = combolistDict(String.Format(STR_SQL_ALL_COURSES_ORDER, session_idr, course_dept_idr), "SS" & TextBoxLevel.Text & "L", "SS" & TextBoxLevel.Text & "L")
                dictAllCourses = combolistDict(STR_SQL_ALL_COURSES, "course_code", "course_code")

                GetData()
        End Select
    End Sub

    Private Sub bgwLoad_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bgwLoad.RunWorkerCompleted
        'for dictonaries
        ComboBoxDepartments.Items.Clear()
        For Each key In dictDepts.Keys
            ComboBoxDepartments.Items.Add(dictDepts(key))
        Next
        If ComboBoxDepartments.Items.Count > 0 Then ComboBoxDepartments.SelectedIndex = 0
        TextBoxDeptID.Text = dictDepts.Keys(0).ToString
        ComboBoxSessions.Items.Clear()
        For Each key In dictSessions.Keys
            ComboBoxSessions.Items.Add(dictSessions(key))
        Next

        If ComboBoxSessions.Items.Count > 0 Then ComboBoxSessions.SelectedIndex = 0
        ComboBoxCourseCode.Items.Clear()
        CheckedListBoxCourses.Items.Clear()
        For Each key In dictCourses.Keys
            ComboBoxCourseCode.Items.Add(dictCourses(key))
            CheckedListBoxCourses.Items.Add(dictCourses(key))
        Next
        ComboBoxCourseCode.SelectedIndex = 0


        dgvStudents.DataSource = tmpDS.Tables("students").DefaultView
        If dgvStudents.Rows.Count > 0 Then
            getRegisteredCoursesForStudent(dgvStudents.Rows(0).Cells("matno").Value.ToString)
            'resize cols
            resizeDatagrids("students")
        End If


        TimerBS.Stop()
        btnReset.Enabled = True
        Me.ProgressBarBS.Value = 100
    End Sub

    Private Sub ButtonSaveReg_Click(sender As Object, e As EventArgs) Handles ButtonSaveReg.Click
        'save to db
        If dgvImportCourses.Tag = "RTPS" Then

            MsgBox("Registration data will now be inserted into database")
            importReg()
            CaptureCourses()
            dgvImportCourses.SendToBack()
        ElseIf dgvImportCourses.Tag = "RTPS" Then
            MsgBox("A lot has to be done to convert from this format to the required dormat for Registration")
        End If
    End Sub
    Sub checkReg()
        Dim tmpDV As DataView
        Dim tmpDT As DataTable
        Dim snRow As Integer
        Dim emptyRow As Integer = 0
        Dim emptyRowFound As Boolean = False
        Dim rowCount As Integer = 0
        Dim colCount As Integer = 0
        Dim lstCols As New List(Of Integer)
        Dim listcolNames As New List(Of String)
        tmpDV = Me.dgvImportCourses.DataSource 'TODO: causes error if dirty
        tmpDT = tmpDV.ToTable()

        '# Detect header row
        rowCount = tmpDT.Rows.Count
        For i = 0 To rowCount - 1
            If tmpDT.Rows(i).ItemArray().Contains("matno") Or tmpDT.Rows(i).ItemArray().Contains("MAT") Then
                Debug.Print("ColumnHeader at row: " & i)
                snRow = i
                Exit For
            End If
        Next

        Dim dictColHeaders As New Dictionary(Of Integer, String)
        '# display header rows
        colCount = dgvImportCourses.Columns.Count
        For i = 0 To colCount - 1
            dgvImportCourses.Columns(i).HeaderText = dgvImportCourses.Item(i, snRow).Value.ToString
            dictColHeaders.Add(i, dgvImportCourses.Item(i, snRow).Value.ToString)
        Next

        '#change column names to match header text
        Try
            For i = 0 To colCount - 1
                If dictColHeaders.ContainsValue(dgvImportCourses.Columns(i).HeaderText) Then
                    dgvImportCourses.Columns(i).Name = dgvImportCourses.Rows(snRow).Cells(i).Value.ToString

                Else
                    'note column to delete
                    lstCols.Add(i)
                    listcolNames.Add(dgvImportCourses.Columns(i).Name)
                    'DataGridView1.Columns.RemoveAt(i)
                End If

            Next

            'delete noted useless columns
            Dim delColCount = 0
            For Each iName In listcolNames
                tmpDT.Columns.Remove(iName) 'TODO: its not removing, just moving to the end
                'DataGridView1.InvalidateColumn(iName)
                delColCount += 1 'update count of deleted cols
            Next

            'delete header rows
            tmpDT.AcceptChanges()

            For j = 0 To snRow
                tmpDT.Rows(0).Delete()
                tmpDT.AcceptChanges()
            Next

            rowCount = tmpDT.Rows.Count
            ''delete empty rows below
            'rowCount = tmpDT.Rows.Count
            'If emptyRow > 0 Then
            '    For j = emptyRow To rowCount - 1
            '        tmpDT.Rows(emptyRow).Delete()   'keep deleting the last row
            '    Next
            'End If
            dgvImportCourses.DataSource = Nothing
            dgvImportCourses.Refresh()
            dgvImportCourses.DataSource = tmpDT
            dgvImportCourses.Refresh()

        Catch ex As Exception
            MessageBox.Show("Result is not in the correct format, please correct ant try again")
            logError(ex.ToString)
            Exit Sub
        End Try
    End Sub
    Sub importReg()
        Try
            'Algorithm
            'Check database if result already exist to avoid duplicates
            'Write into Database
            'Get dataset from displayed datagrid
            'parse dataset record by record
            'insert record by record

            Dim dt As DataTable
            'Dim dv As DataView
            dgvImportCourses.EndEdit()
            dgvImportCourses.Update()
            If Not (IsDBNull(dgvImportCourses.DataSource) Or (dgvImportCourses.Rows.Count = 0)) Then
                dgvImportCourses.DataSource.AcceptChanges 'TODO: dataTable or dataView? lazy loading issues
                dt = dgvImportCourses.DataSource 'causes error if dirty
            Else
                MsgBox("Empty Registration Records")
                Exit Sub
            End If

            '#method 1 manual Insert or bulkInsert

            'If boolSession And boolDept And boolCourse Then
            mappDB.manualRegInsertDB(dt)
            'ElseIf dSession.Length > 10 Then
            'MsgBox("Import file with the right format")

            'End If
            MsgBox("Reg Uploaded into Database Successfully! Cross check what was uploaded below")
            'dgvImportCourses.Visible = True
            'strSQL = "SELECT * from Reg WHERE (session_idr = '{0}'  AND department_idr={1})"
            'dgvImportCourses.DataSource = mappDB.GetDataWhere(String.Format(strSQL, dSession, dDeptID)).Tables(0).DefaultView
            'dgvImportCourses.Top = dgvImportCourses.Top + dgvImportCourses.Height
            'dgvImportCourses.Refresh()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Function exportRegToFERMA(Optional internal As Boolean = True, Optional AccessFileName As String = "") As Boolean
        Try
            'Algorithm: 
            'Check database if result already exist to avoid duplicates
            'Write into Database;    'Get dataset from displayed datagrid;             'parse dataset record by record  'insert record by record
            Dim dt As New DataTable
            Dim dv As New DataView
            dgvImportCourses.EndEdit()
            dgvImportCourses.Update()
            If Not (IsDBNull(dgvImportCourses.DataSource) Or (dgvImportCourses.Rows.Count = 0)) Then
                If TypeOf (dgvImportCourses.DataSource) Is DataTable Then
                    ' dgvImportCourses.DataSource.AcceptChanges 'TODO: dataTable or dataView? lazy loading issues
                    dt = dgvImportCourses.DataSource 'causes error if dirty
                ElseIf TypeOf (dgvImportCourses.DataSource) Is DataView Then
                    dv = dgvImportCourses.DataSource
                    dt = dv.ToTable
                End If
            Else
                MsgBox("Empty Registration Records")
                Return False
                Exit Function
            End If


            '#method 1 manual Insert or bulkInsert
            If internal = True Then
                mappDB.manualRegExportToEmbeddedAccessSPD(dt)
            Else
                mappDB.manualRegExportToExternalAccess(dt, AccessFileName)
            End If


            MsgBox("Reg Uploaded into Database Successfully! Cross check what was uploaded below")

            Return True
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try

    End Function
    Private Sub dgvStudents_Click(sender As Object, e As EventArgs) Handles dgvStudents.Click
        If Not dgvStudents.SelectedRows(0).Cells("matno").Value = Nothing Then
            filterReg(dgvStudents.SelectedRows(0).Cells("matno").Value.ToString)
        End If
    End Sub



    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        Dim retFileName As String
        retFileName = objExcelFile.exportStudentsToExcelFile_NPOI(dgvStudents.DataSource, My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\ExportedStudents" & ComboBoxLevel.Text & ".xlsx")
        MsgBox("File exported to: " & retFileName)
    End Sub



    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
        'TODO: create a user control for these combo box. it should have events that are aware o dictionary storing sessions, new session added, auto load, auto update etc
        Try
            TextBoxSession.Text = ComboBoxSessions.SelectedItem.ToString

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ButtonImportFromAccess_Click(sender As Object, e As EventArgs) Handles ButtonImportFromAccess.Click
        Dim FileOpenDialogBroadsheet As New OpenFileDialog()
        Dim resultFileName As String = ""
        Dim dDept As String = "Computer Engineering"
        Dim dLevel As Integer = 100
        Dim dSession As String = "2018/2019"

        dDept = ComboBoxDepartments.Text
        dLevel = CInt(ComboBoxLevel.Text)
        dSession = ComboBoxSessions.Text
        If Not FileOpenDialogBroadsheet.ShowDialog = DialogResult.Cancel Then
            resultFileName = FileOpenDialogBroadsheet.FileName()

            objExcelFile.excelFileName = resultFileName

            'todo: PreviewReg(resultFileName,"ACCESS/EXCEL")

            Dim tmpDS As DataSet = mappDB.ImportFromAccess(resultFileName, String.Format(STR_SQL_ACCESS_IMPORT_REGISTERED_STUDENTS_INPUT_DEPT_LEVEL, dDept, dLevel)) 'objExcelFile.readResultFile()
            'TODO: reset datagrid
            dgvImportCourses.DataSource = tmpDS.Tables(0).DefaultView
            dgvImportCourses.BringToFront()
            For i = 0 To dgvImportCourses.Rows.Count - 1
                dgvImportCourses.Rows(i).Cells("session_idr").Value = dSession
                dgvImportCourses.Rows(i).Cells("status").Value = "successful"
                dgvImportCourses.Rows(i).Cells("student_othernames").Value = ""
                dgvImportCourses.Rows(i).Cells("dept_idr").Value = mappDB.getDeptID(ComboBoxDepartments.SelectedItem.ToString)


            Next
        End If

    End Sub

    Private Sub ButtonRegToExcel_Click(sender As Object, e As EventArgs) Handles ButtonRegToExcel.Click
        Dim retFileName As String
        retFileName = objExcelFile.exportRegToExcelFile_NPOI(dgvStudents.DataSource, My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\ExportedStudents" & ComboBoxLevel.Text & ".xlsx")
        MsgBox("File exported to: " & retFileName)
    End Sub

    Private Sub ButtonAccess_Click(sender As Object, e As EventArgs) Handles ButtonAccess.Click
        Dim FileOpenDialogBroadsheet As New OpenFileDialog()
        Dim resultFileName As String = ""
        Dim conStr As String

        If Not FileOpenDialogBroadsheet.ShowDialog = DialogResult.Cancel Then
            resultFileName = FileOpenDialogBroadsheet.FileName()
            conStr = buildConnectionStringFromAccessFile(resultFileName, True)



            If exportRegToFERMA(False, resultFileName) Then
                    MsgBox("Exported to FERMA")
                Else
                    MsgBox("Something went wrong, note that FERMA is an Access-based db used in University of Benin")
                End If


        Else
            MsgBox("The export will be done and save internally, You can export to excel afterwards")
            If exportRegToFERMA() Then
                MsgBox("Saved in database (FERMA Format. Yo can export to Excel later)")
            Else
                MsgBox("Something went wrong, note that FERMA is an Access-based db used in University of Benin")
            End If
        End If


    End Sub

    Private Sub ButtonImportRegRTPS_Click(sender As Object, e As EventArgs) Handles ButtonImportRegRTPS.Click
        Dim FileOpenDialogBroadsheet As New OpenFileDialog()
        Dim resultFileName As String = ""
        Dim tmpDS As New DataSet
        Dim dt As DataTable
        If Not FileOpenDialogBroadsheet.ShowDialog = DialogResult.Cancel Then
            resultFileName = FileOpenDialogBroadsheet.FileName()
            objExcelFile.excelFileName = resultFileName
            tmpDS = objExcelFile.readResultFile()
            'Do some check
            dt = tmpDS.Tables(0)
            If dt.Rows(0).Item(0) = "MatNo" Then
                For j = 0 To dt.Columns.Count - 1
                    dt.Columns(j).ColumnName = dt.Rows(0).Item(j).ToString
                Next
                dt.Rows(0).Delete()
            End If
            dgvImportCourses.DataSource = dt.DefaultView
            dgvImportCourses.Tag = "RTPS"
            checkReg()
            dgvImportCourses.BringToFront()
        End If
    End Sub

    Private Sub ComboBoxCourseCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxCourseCode.SelectedIndexChanged
        If Not ComboBoxCourseCode.SelectedItem = Nothing Then TextBoxCourseCode.Text = ComboBoxCourseCode.SelectedItem.ToString

    End Sub

    Private Sub ComboBoxSessions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxSessions.SelectedIndexChanged
        If Not ComboBoxSessions.SelectedItem = Nothing Then TextBoxSession.Text = ComboBoxSessions.SelectedItem.ToString

    End Sub

    Private Sub BgWProcess_DoWork(sender As Object, e As DoWorkEventArgs) Handles BgWProcess.DoWork

    End Sub

    Private Sub ComboBoxLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxLevel.SelectedIndexChanged
        'TODO: ComboBoxCourseCode.filter(level)
    End Sub

    Private Sub ButtonOKReg_Click(sender As Object, e As EventArgs) Handles ButtonOKReg.Click
        Dim strReg1 As String = ""
        Dim strReg2 As String = ""
        'Dim col As Collection
        'col = CheckedListBoxCourses.CheckedItems
        For Each col In CheckedListBoxCourses.CheckedItems
            If dictAllCourseCodeKeyAndCourseSemesterVal.ContainsKey(col) Then
                If dictAllCourseCodeKeyAndCourseSemesterVal(col) = 1 Then
                    If strReg1 = "" Then
                        strReg1 = col
                    Else
                        strReg1 = strReg1 & ";" & col
                    End If
                Else
                    If strReg2 = "" Then
                        strReg2 = col
                    Else
                        strReg2 = strReg2 & ";" & col
                    End If
                End If
            End If
        Next
        'TODO Sessions
        'dgvCourses.Rows.Add({dgvStudents.SelectedRows(0).Cells("matno").Value, strReg1, strReg2})

        If MsgBox("Are the courses corectly displayed" & vbCrLf & strReg1 & vbCrLf & vbCrLf & strReg2, MsgBoxStyle.YesNo) = vbYes Then
            PanelCourses.Visible = False
            If dgvCourses.SelectedRows.Count > 0 Then
                dgvCourses.SelectedRows(0).Cells("CourseCode_1").Value = strReg1
                dgvCourses.SelectedRows(0).Cells("CourseCode_2").Value = strReg2
            End If
        End If
    End Sub

    Private Sub dgvStudents_RowStateChanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles dgvStudents.RowStateChanged
        ' If dgvStudents.Rows.dirty
        If tmpDS.Tables("Students").Rows.Count > 0 And dgvStudents.SelectedRows.Count > 0 Then
            If dgvCourses.Rows.Count = 0 Then
                getRegisteredCoursesForStudent(dgvStudents.SelectedRows(0).Cells("matno").Value)
            Else
                filterReg(tmpDS.Tables("Students").Rows(0).Item("matno"))
                CaptureCourses()
            End If

            updatePix()
        End If
    End Sub

    Private Sub ButtonCancelReg_Click(sender As Object, e As EventArgs) Handles ButtonCancelReg.Click
        PanelCourses.Visible = False

        For Each itm In CheckedListBoxCourses.CheckedItems
           CheckedListBoxCourses.SetItemCheckState(itm, False)
        Next
    End Sub
End Class