VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 1  'vbDataSource
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "vmAddressBook"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Public vmAddresses As ADODB.Recordset

Private Sub Class_GetDataMember(DataMember As String, Data As Object)
    Set Data = vmAddresses
End Sub

Private Sub Class_Initialize()
    Dim fld As ADODB.Field
    Dim strRow As String
    Dim strField As String
    Dim intPos As Integer
    Set vmAddresses = New ADODB.Recordset
    With vmAddresses
        .Fields.Append "AddressID", adVarChar, 5, adFldRowID
        .Fields.Append "NickName", adVarChar, 20, adFldUpdatable
        .Fields.Append "EmailAddress", adVarChar, 100, adFldUpdatable
        .CursorType = adOpenKeyset
        .LockType = adLockOptimistic
        .Open
    End With
    Open "C:\Program Files\Virtual Morse\AddressBook.txt" For Input As #1
    Do Until EOF(1)
        Line Input #1, strRow
        With vmAddresses
            .AddNew
            For Each fld In .Fields
                'find tab delimiter
                If InStr(strRow, Chr(9)) <> 0 Then
                    intPos = InStr(strRow, Chr(9))
                    strField = Left(strRow, intPos - 1)
                Else
                    strField = strRow
                End If
                'strip off quotation marks
                If Left(strField, 1) = Chr(34) Then
                    strField = Left(strField, Len(strField) - 1)
                    strField = Right(strField, Len(strField) - 1)
                End If
                fld.Value = strField
                'strip off field value text from text row
                strRow = Right(strRow, Len(strRow) - intPos)
                intPos = 0
            Next
            .Update
            .MoveFirst
        End With
    Loop
    Close
End Sub

Public Sub WriteToFile()
    Dim fld As ADODB.Field
    Dim strRow As String
    Dim strField As String
    Open "C:\Program Files\Virtual Morse\AddressBook.txt" For Output As #1
    With vmAddresses
        .MoveFirst
        Do While Not .EOF
            For Each fld In .Fields
                'add quotes
                If Len(fld.Value) > 0 Then
                    strField = Chr(34) & fld.Value & Chr(34)
                Else
                    strField = ""
                End If
                'add tab
                strRow = strRow & strField & Chr(9)
            Next
            'strip off end tab character
            strRow = Left(strRow, Len(strRow) - 1)
            Print #1, strRow
            strRow = ""
        .MoveNext
        Loop
    End With
    Close
End Sub

