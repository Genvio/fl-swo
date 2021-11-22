Imports System.Data
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic

'This class converts a dataset to an HTML stream which can be used to display the dataset
'in MS Excel. The Convert method is overloaded three times as follows:
' 1) Default to the first table in the dataset.
' 2) Pass an index to tell which table in the dataset to use.
' 3) Pass a table name to tell which table in the dataset to use.
Public Class DataSetToExcel
    Public Shared Sub Convert(ByVal ds As DataSet, ByVal response As HttpResponse)
        'Clean up the response object.
        response.Clear()
        response.AddHeader("content-disposition", "attachment;filename=ExcelReport.xls")
        response.Charset = ""

        'Set the response MIME type for excel.
        response.ContentType = "application/vnd.ms-excel"

        'Create a string writer.
        Dim stringWrite As New System.IO.StringWriter

        'Create an HtmlTextWriter which uses the stringwriter.
        Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)

        'Instantiate a datagrid.
        Dim dg As New System.Web.UI.WebControls.DataGrid

        'Set the datagrid datasource to the dataset passed in.
        dg.DataSource = ds.Tables(0)

        'Bind the datagrid.
        dg.DataBind()
        dg.HeaderStyle.Font.Bold = True
        dg.HeaderStyle.Font.Size() = System.Web.UI.WebControls.FontUnit.Point(8)
        dg.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        dg.ItemStyle.Font.Size() = System.Web.UI.WebControls.FontUnit.Point(8)
        dg.ItemStyle.HorizontalAlign = HorizontalAlign.Center

        'Tell the datagrid to render itself to our HtmlTextWriter.
        dg.RenderControl(htmlWrite)

        'Output the HTML.
        response.Write(stringWrite.ToString)
        response.End()
    End Sub

    Public Shared Sub Convert(ByVal ds As DataSet, ByVal TableIndex As Integer, ByVal response As HttpResponse)
        'Check to see if a table exists at the passed in value.
        'If not the base method is called.
        If TableIndex > ds.Tables.Count - 1 Then
            Convert(ds, response)
        End If
        'Clean up the response object.
        response.Clear()
        response.AddHeader("content-disposition", "attachment;filename=ExcelReport.xls")
        response.Charset = ""

        'Set the response MIME type for excel.
        response.ContentType = "application/vnd.ms-excel"

        'Create a string writer.
        Dim stringWrite As New System.IO.StringWriter

        'Create an HtmlTextWriter which uses the stringwriter.
        Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)

        'Instantiate a datagrid.
        Dim dg As New System.Web.UI.WebControls.DataGrid

        'Set the datagrid datasource to the dataset passed in.
        dg.DataSource = ds.Tables(TableIndex)

        'Bind the datagrid.
        dg.DataBind()

        'Tell the datagrid to render itself to our HtmlTextWriter.
        dg.RenderControl(htmlWrite)

        'Output the HTML.
        response.Write(stringWrite.ToString)
        response.End()
    End Sub

    Public Shared Sub Convert(ByVal ds As DataSet, ByVal TableName As String, ByVal response As HttpResponse)
        'Check to see if the table name exists. If not, call the default method.
        If ds.Tables(TableName) Is Nothing Then
            Convert(ds, response)
        End If

        'Clean up the response object.
        response.Clear()
        response.AddHeader("content-disposition", "attachment;filename=ExcelReport.xls")
        response.Charset = ""

        'Set the response MIME type for excel.
        response.ContentType = "application/vnd.ms-excel"

        'Create a string writer.
        Dim stringWrite As New System.IO.StringWriter

        'Create an HtmlTextWriter which uses the stringwriter.
        Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)

        'Instantiate a datagrid.
        Dim dg As New System.Web.UI.WebControls.DataGrid

        'Set the datagrid datasource to the dataset passed in.
        dg.DataSource = ds.Tables(TableName)

        'Bind the datagrid.
        dg.DataBind()

        'Tell the datagrid to render itself to our HtmlTextWriter.
        dg.RenderControl(htmlWrite)

        'Output the HTML.
        response.Write(stringWrite.ToString)
        response.End()
    End Sub

    Public Shared Sub Convert(ByVal ds As DataSet, ByVal response As HttpResponse, ByVal strdestinationpath As String)
        'Clean up the response object.
        response.Clear()
        response.AddHeader("content-disposition", "attachment;filename=ExcelReport.xls")
        response.Charset = ""

        'Set the response MIME type for excel.
        response.ContentType = "application/vnd.ms-excel"

        'Create a string writer.
        Dim stringWrite As New System.IO.StringWriter

        'Create an HtmlTextWriter which uses the stringwriter.
        Dim htmlWrite As New System.Web.UI.HtmlTextWriter(stringWrite)

        'Instantiate a datagrid.
        Dim dg As New System.Web.UI.WebControls.DataGrid

        'Set the datagrid datasource to the dataset passed in.
        dg.DataSource = ds.Tables(0)

        'Bind the datagrid.
        dg.DataBind()
        dg.HeaderStyle.Font.Bold = True
        dg.HeaderStyle.Font.Size() = System.Web.UI.WebControls.FontUnit.Point(8)
        dg.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        dg.ItemStyle.Font.Size() = System.Web.UI.WebControls.FontUnit.Point(8)
        dg.ItemStyle.HorizontalAlign = HorizontalAlign.Center

        'Tell the datagrid to render itself to our HtmlTextWriter.
        dg.RenderControl(htmlWrite)

        'Output the HTML.
        Dim sw As System.IO.StreamWriter = System.IO.File.CreateText(strdestinationpath)
        sw.Write(stringWrite.ToString)
        sw.Close()
    End Sub
End Class