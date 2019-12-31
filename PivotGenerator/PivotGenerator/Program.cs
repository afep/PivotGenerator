//using Syncfusion.XlsIO;
using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;


/*
  Se instala paquete nugget Syncfusion.XlsIO.ClientProfile.nupkg
*/
namespace PivotGenerator
{
    class Program
    {
        //static void Main(string[] args)
        //{

        //    using (ExcelEngine excelEngine = new ExcelEngine())
        //    {
        //        IApplication application = excelEngine.Excel;
        //        application.DefaultVersion = ExcelVersion.Excel2013;

        //        //The new workbook will have 5 worksheets
        //        IWorkbook workbook = application.Workbooks.Create(5);
        //        //Creating a Sheet
        //        IWorksheet sheet = workbook.Worksheets.Create();
        //        //Creating a Sheet with name “Sample”
        //        IWorksheet namedSheet = workbook.Worksheets.Create("Sample");

        //        workbook.SaveAs("Output.xlsx");
        //    }
        //}

        static void Main(string[] args)
        {
            oracleTest();
            //createFile();


        }

        public static void oracleTest()
        {
            OracleConnection conn = DBUtils.GetDBConnection();

            Console.WriteLine("Get Connection: " + conn);
            try
            {
                conn.Open();

                Console.WriteLine(conn.ConnectionString, "Successful Connection");

                GetDate(conn);
                
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("## ERROR: " + ex.Message);
                Console.Read();
                return;
            }

            Console.WriteLine("Connection successful!");

        }

        public static void GetDate(OracleConnection conn)
        {
            string output ="";
            string sqlQuery = "select sysdate from dual";
            OracleCommand command;
            OracleDataReader dataReader;
            command = new OracleCommand(sqlQuery, conn);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                output = output + dataReader.GetValue(0);
            }
            Console.WriteLine("Salida: " +output);


        }

        public static void createFile()
        {
            Excel.Application objApp;
            Excel.Workbook objBook;
            Excel.Sheets objSheets;
            //Excel.Workbooks objBooks;

            objApp = new Excel.Application();
            try
            {

                //objBooks = objApp.Workbooks;
                objBook = objApp.Workbooks.Add(Missing.Value);
                objSheets = objBook.Worksheets;

                Excel.Worksheet sheet1 = (Excel.Worksheet)objSheets[1];
                sheet1.Name = "PivotData";

                sheet1.Cells[1, 1] = "Dato 1";
                sheet1.Cells[1, 2] = "Dato 2";
                sheet1.Cells[2, 1] = "1";
                sheet1.Cells[2, 2] = "2";
                sheet1.Cells[3, 1] = "3";
                sheet1.Cells[3, 2] = "4";

                //Crear hoja cache
                //CREATE A PIVOT CACHE BASED ON THE EXPORTED DATA
                Excel.PivotCache pivotCache = objBook.PivotCaches().Add(Excel.XlPivotTableSourceType.xlDatabase, sheet1.UsedRange);

                Console.WriteLine(pivotCache.SourceData.ToString());

                //WORKSHEET FOR NEW PIVOT TABLE
                var sheet2 = (Excel.Worksheet)objSheets.Application.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //Excel.Worksheet sheet2 = (Excel.Worksheet)objSheets[2];
                sheet2.Name = "PivotTable";

                //PIVOT TABLE BASED ON THE PIVOTCACHE OF EXPORTED DATA
                Excel.PivotTables pivotTables = (Excel.PivotTables)sheet2.PivotTables(Missing.Value);
                Excel.PivotTable pivotTable = pivotTables.Add(pivotCache, objApp.ActiveCell, "PivotTable1", Missing.Value, Missing.Value);

                //Poner los campos
                var pivotFields = (Microsoft.Office.Interop.Excel.PivotFields)pivotTable.PivotFields();
                var dato1 = (Microsoft.Office.Interop.Excel.PivotField)pivotFields.Item("Dato 1");
                dato1.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;
                var dato2 = (Microsoft.Office.Interop.Excel.PivotField)pivotFields.Item("Dato 2");
                dato2.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;


                pivotTable.SmallGrid = false;
                pivotTable.TableStyle = "PivotStyleLight1";

                /*Console.ReadLine();*/

                //Almacenar Pivote 
                objApp.DisplayAlerts = false;
                //Se elimina la hoja de datos
                ((Excel.Worksheet)objSheets["PivotData"]).Delete();
                objBook.SaveAs("prueba", Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                objApp.DisplayAlerts = true;
                objApp.Quit();
            }
            catch (Exception e)
            {
                objApp.Quit();
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
