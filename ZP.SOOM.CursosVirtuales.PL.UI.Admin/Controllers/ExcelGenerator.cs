using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using GemBox.Spreadsheet;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;

using ZP.SOOM.CursosVirtuales.BL.BC;
using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.PL.UI.Admin.Models;
using ZP.SOOM.CursosVirtuales.Util;
using System.Runtime.InteropServices;

namespace ZP.SOOM.DesempenoExpress.PL.UI.Controllers
{
    public class ExcelGenerator
    {

        internal void GenerarReporterBaseTrabajadores(IEnumerable<IDictionary<string, object>> lstTrabajador, HttpSessionStateBase Session, string FechaActual)
        {
            try
            {
                Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] = 0;
                Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.TotalProgress] = lstTrabajador.Count() + 1;
                Task.Run(() => TaskGenerarReporteUsuarios(lstTrabajador.ToList(), Session, FechaActual));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void TaskGenerarReporteUsuarios(List<IDictionary<string, object>> lstTrabajador, HttpSessionStateBase Session, string FechaActual)
        {
            Application excelApp = null;
            Range range = null;
            Workbooks workbooks = null;
            Workbook workbook = null;
            Sheets worksheets = null;
            Worksheet workSheet = null;
            try
            {
                excelApp = new Application
                {
                    Visible = false
                };
                workbooks = excelApp.Workbooks;
                object misValue = Missing.Value;
                workbook = workbooks.Add(misValue);
                worksheets = workbook.Sheets;
                //workSheet = worksheets.Add();

                workSheet = excelApp.ActiveSheet;
                excelApp.Visible = false;
                excelApp.Interactive = false;

                int fila = 1;

                workSheet.Columns[1].EntireColumn.NumberFormat = "@";
                workSheet.Columns[2].EntireColumn.NumberFormat = "@";
                workSheet.Columns[18].EntireColumn.NumberFormat = "@";

                //Datos del trabajador
                workSheet.Cells[1, 1] = "CÓDIGO *";
                workSheet.Cells[1, 2] = "DNI / C.E *";
                workSheet.Cells[1, 3] = "NOMBRES *";
                workSheet.Cells[1, 4] = "PRIMER APELLIDO *";
                workSheet.Cells[1, 5] = "SEGUNDO APELLIDO *";
                workSheet.Cells[1, 6] = "NOMBRE COMPLETO";
                workSheet.Cells[1, 7] = "FECHA DE NACIMIENTO";
                workSheet.Cells[1, 8] = "GÉNERO";
                workSheet.Cells[1, 9] = "CORREO ELECTRÓNICO *";
                workSheet.Cells[1, 10] = "CELULAR";
                workSheet.Cells[1, 11] = "SEDE";
                workSheet.Cells[1, 12] = "PROVINCIA";
                workSheet.Cells[1, 13] = "REGIÓN";
                workSheet.Cells[1, 14] = "PAÍS";
                workSheet.Cells[1, 15] = "FECHA INGRESO";
                workSheet.Cells[1, 16] = "GERENCIA";
                workSheet.Cells[1, 17] = "ÁREA *";
                workSheet.Cells[1, 18] = "CÓDIGO JEFE DIRECTO";
                workSheet.Cells[1, 19] = "JEFE DIRECTO";
                workSheet.Cells[1, 20] = "CATEGORÍA *";
                workSheet.Cells[1, 21] = "ROL";
                workSheet.Cells[1, 22] = "COMPAÑÍA *";
                workSheet.Cells[1, 23] = "PUESTO";
                workSheet.Cells[1, 24] = "USUARIO *";
                workSheet.Cells[1, 25] = "CONTRASEÑA";
                workSheet.Cells[1, 26] = "INACTIVO";
                workSheet.Cells[1, 27] = "CESADO";

                //COLOR DE LETRAS DE CABECERA
                range = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 27]];
                range.Font.Color = Color.White;
                range.Font.Name = "Arial";
                range.Font.Bold = true;
                range.RowHeight = 20;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;

                range = workSheet.Range[workSheet.Cells[fila, 1], workSheet.Cells[fila + lstTrabajador.Count, 27]];
                range.Font.Name = "Arial";
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;

                range = workSheet.Range[workSheet.Cells[fila, 26], workSheet.Cells[fila, 27]];
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                //BACKGROUND DE LA COLUMNA
                range = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 14]];
                range.Interior.Color = Color.FromArgb(234, 101, 68);
                range = workSheet.Range[workSheet.Cells[1, 15], workSheet.Cells[1, 23]];
                range.Interior.Color = Color.FromArgb(117, 113, 113);
                range = workSheet.Range[workSheet.Cells[1, 24], workSheet.Cells[1, 27]];
                range.Interior.Color = Color.FromArgb(119, 119, 181);

                //Data del procedure de TRABAJADORES

                Object[,] DataTrabajadores = null;
                Object[,] FormulasTrabajadores = null;

                if (lstTrabajador.Count > 0)
                {
                    DataTrabajadores = new Object[lstTrabajador.Count, lstTrabajador[0].Count];
                    FormulasTrabajadores = new Object[lstTrabajador.Count, 1];
                }

                for (int i = 0; i < lstTrabajador.Count; i++)
                {
                    Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] = (int)Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] + 1;
                    IDictionary<string, object> objReporteTrabajador = lstTrabajador[i];

                    Object[] lstValuesReporteTrabajador = objReporteTrabajador.Values.ToArray();
                    for (int j = 0; j < objReporteTrabajador.Count; j++)
                    {
                        if (j != 17)
                        {
                            DataTrabajadores[i, j] = lstValuesReporteTrabajador[j];
                        }
                        else
                        {
                            FormulasTrabajadores[i, 0] = lstValuesReporteTrabajador[j];
                        }
                    }

                }

                if (lstTrabajador != null)
                {
                    workSheet.Range[workSheet.Cells[fila + 1, 1], workSheet.Cells[fila + lstTrabajador.Count, lstTrabajador[0].Count]].Value = DataTrabajadores;
                    workSheet.Range[workSheet.Cells[fila + 1, 18], workSheet.Cells[fila + lstTrabajador.Count, 18]].Formula = FormulasTrabajadores;
                }

                //AutoAjustar
                workSheet.Columns.AutoFit();
                //workSheet.Rows.AutoFit();

                //Guardar el archivo
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reporte", "Base de usuarios " + FechaActual + ".xlsx")) == true)
                    File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reporte", "Base de usuarios " + FechaActual + ".xlsx"));
                workSheet.SaveAs(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reporte", "Reporte general detallado " + FechaActual + ".xlsx"));
                excelApp.Quit();

                Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] = (int)Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] + 1;

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ExcelDispose(excelApp, workSheet, worksheets, workbook, workbooks, range);
            }
        }

        internal void GenerarReporteGeneral(List<IDictionary<string, object>> ReporteGeneral, HttpSessionStateBase Session, string FechaActual)
        {
            try
            {
                Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] = 0;
                Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.TotalProgress] = ReporteGeneral.Count() + 1;
                Task.Run(() => TaskGenerarReporteGeneral(ReporteGeneral, Session, FechaActual));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void TaskGenerarReporteGeneral(List<IDictionary<string, object>> ReporteGeneral, HttpSessionStateBase Session, string FechaActual)
        {
            Application excelApp = null;
            Range range = null;
            Workbooks workbooks = null;
            Workbook workbook = null;
            Sheets worksheets = null;
            Worksheet workSheet = null;
            try
            {
                excelApp = new Application
                {
                    Visible = false
                };
                workbooks = excelApp.Workbooks;
                object misValue = Missing.Value;
                workbook = workbooks.Add(misValue);
                worksheets = workbook.Sheets;
                //workSheet = worksheets.Add();

                workSheet = excelApp.ActiveSheet;
                excelApp.Visible = false;
                excelApp.Interactive = false;

                int fila = 5;

                workSheet.Columns[1].EntireColumn.NumberFormat = "@";
                workSheet.Columns[2].EntireColumn.NumberFormat = "@";
                workSheet.Columns[18].EntireColumn.NumberFormat = "@";

                //Datos del trabajador

                workSheet.Cells[fila, 1] = "CÓDIGO";
                workSheet.Cells[fila, 2] = "DNI / C.E.";
                workSheet.Cells[fila, 3] = "ESTATUS";
                workSheet.Cells[fila, 4] = "COMPAÑÍA";
                workSheet.Cells[fila, 5] = "NOMBRES Y APELLIDOS";
                workSheet.Cells[fila, 6] = "ALIAS";
                workSheet.Cells[fila, 7] = "PUESTO";
                workSheet.Cells[fila, 8] = "CATEGORIA DE PUESTO";
                workSheet.Cells[fila, 9] = "ÁREA";
                workSheet.Cells[fila, 10] = "GERENCIA";
                workSheet.Cells[fila, 11] = "SEDE - OFICINA";
                workSheet.Cells[fila, 12] = "CORREO";
                workSheet.Cells[fila, 13] = "GRUPOS A LOS QUE PERTENECE";
                workSheet.Cells[fila, 14] = "PERFIL";
                workSheet.Cells[fila, 15] = "USUARIO";

                workSheet.Cells[fila, 16] = "CÓDIGO JEFE DIRECTO";
                workSheet.Cells[fila, 17] = "JEFE DIRECTO";
                workSheet.Cells[fila, 18] = "CORREO JEFE DIRECTO";

                workSheet.Cells[fila, 19] = "AÑO";
                workSheet.Cells[fila, 20] = "NOMBRE SLOT";
                workSheet.Cells[fila, 21] = "NOMBRE DEL CURSO";
                workSheet.Cells[fila, 22] = "N° DEL INTENTO";
                workSheet.Cells[fila, 23] = "ESTATUS DE APROBACIÓN DEL INTENTO";
                workSheet.Cells[fila, 24] = "PREGUNTAS CORRECTAS";
                workSheet.Cells[fila, 25] = "PREGUNTAS INCORRECTAS";
                workSheet.Cells[fila, 26] = "NOTA DEL EXAMEN";
                workSheet.Cells[fila, 27] = "FECHA DEL EXAMEN";
                workSheet.Cells[fila, 28] = "HORA DEL EXAMEN";

                workSheet.Cells[fila, 29] = "DESCARGÓ CERTIFICADO";
                workSheet.Cells[fila, 30] = "FECHA QUE ABRIÓ EL PRIMER CONTENIDO DEL CURSO";
                workSheet.Cells[fila, 31] = "HORA QUE ABRIÓ EL PRIMER CONTENIDO DEL CURSO";
                workSheet.Cells[fila, 32] = "FECHA QUE TERMINÓ DE REVISAR TODOS LOS CONTENIDOS DEL CURSO";
                workSheet.Cells[fila, 33] = "HORA QUE TERMINÓ DE REVISAR TODOS LOS CONTENIDOS DEL CURSO";

                workSheet.Cells[fila, 34] = "ESTATUS DEL CURSO";
                workSheet.Cells[fila, 35] = "TOTAL DE CONTENIDO DEL CURSO";
                workSheet.Cells[fila, 36] = "CONTENIDO REVISADO";
                workSheet.Cells[fila, 37] = "ESTATUS DE APROBACIÓN";
                workSheet.Cells[fila, 38] = "PRIMERA FECHA DE APROBACIÓN DEL CURSO";
                workSheet.Cells[fila, 39] = "PRIMERA HORA DE APROBACIÓN DEL CURSO";
                workSheet.Cells[fila, 40] = "NOTA MÁS ALTA";
                workSheet.Cells[fila, 41] = "NÚMERO DE INTENTOS";

                workSheet.Cells[fila, 42] = "ESTATUS DEL SLOT";
                workSheet.Cells[fila, 43] = "ESTATUS DE APROBACIÓN";
                workSheet.Cells[fila, 44] = "NÚMERO TOTAL DE CURSOS DEL SLOT";
                workSheet.Cells[fila, 45] = "NÚMERO DE CURSOS CON EXÁMEN EN EL SLOT";
                workSheet.Cells[fila, 46] = "NÚMERO DE CURSOS APROBADOS EN EL SLOT";

                //COLOR DE LETRAS DE CABECERA
                range = workSheet.Range[workSheet.Cells[fila, 1], workSheet.Cells[fila, 15]];
                range.Font.Color = Color.White;
                range = workSheet.Range[workSheet.Cells[fila, 16], workSheet.Cells[fila, 18]];
                range.Font.Color = Color.Black;
                range = workSheet.Range[workSheet.Cells[fila, 19], workSheet.Cells[fila, 46]];
                range.Font.Color = Color.White;

                //BACKGROUND DE LA COLUMNA
                range = workSheet.Range[workSheet.Cells[fila, 1], workSheet.Cells[fila, 15]];
                range.Interior.Color = Color.FromArgb(234, 101, 68);
                range = workSheet.Range[workSheet.Cells[fila, 16], workSheet.Cells[fila, 18]];
                range.Interior.Color = Color.FromArgb(252, 228, 214);
                range = workSheet.Range[workSheet.Cells[fila, 19], workSheet.Cells[fila, 28]];
                range.Interior.Color = Color.FromArgb(165, 165, 165);
                range = workSheet.Range[workSheet.Cells[fila, 29], workSheet.Cells[fila, 33]];
                range.Interior.Color = Color.FromArgb(80, 80, 80);
                range = workSheet.Range[workSheet.Cells[fila, 34], workSheet.Cells[fila, 41]];
                range.Interior.Color = Color.FromArgb(119, 119, 181);
                range = workSheet.Range[workSheet.Cells[fila, 42], workSheet.Cells[fila, 46]];
                range.Interior.Color = Color.FromArgb(124, 198, 196);

                //TEXTO CENTRADO
                range = workSheet.Range[workSheet.Cells[fila, 1], workSheet.Cells[fila, 46]];
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                range.Font.Name = "Arial";
                range.Font.Bold = true;
                range.RowHeight = 70;
                

                //COMBINA LAS CELDAS
                range = workSheet.Range[workSheet.Cells[fila - 1, 1], workSheet.Cells[fila - 1, 15]];
                range.Interior.Color = Color.FromArgb(234, 101, 68);
                range.Merge();
                range.Font.Bold = true;
                range.Font.Color = Color.White;
                range.Font.Name = "Arial";
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                workSheet.Cells[fila - 1, 1] = "DATOS DEL COLABORADOR";

                range = workSheet.Range[workSheet.Cells[fila - 1, 16], workSheet.Cells[fila - 1, 18]];
                range.Interior.Color = Color.FromArgb(252, 228, 214);
                range.Merge();
                range.Font.Bold = true;
                range.Font.Color = Color.Black;
                range.Font.Name = "Arial";
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                workSheet.Cells[fila - 1, 16] = "DATOS DEL JEFE DIRECTO";

                range = workSheet.Range[workSheet.Cells[fila - 1, 19], workSheet.Cells[fila - 1, 33]];
                range.Interior.Color = Color.FromArgb(165, 165, 165);
                range.Merge();
                range.Font.Bold = true;
                range.Font.Color = Color.White;
                range.Font.Name = "Arial";
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                workSheet.Cells[fila - 1, 19] = "DETALLES DEL CURSO";

                range = workSheet.Range[workSheet.Cells[fila - 1, 34], workSheet.Cells[fila - 1, 41]];
                range.Interior.Color = Color.FromArgb(119, 119, 181);
                range.Merge();
                range.Font.Bold = true;
                range.Font.Color = Color.White;
                range.Font.Name = "Arial";
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                workSheet.Cells[fila - 1, 34] = "DATOS GENERALES DEL CURSO";

                range = workSheet.Range[workSheet.Cells[fila - 1, 42], workSheet.Cells[fila - 1, 46]];
                range.Interior.Color = Color.FromArgb(124, 198, 196);
                range.Merge();
                range.Font.Bold = true;
                range.Font.Color = Color.White;
                range.Font.Name = "Arial";
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;
                workSheet.Cells[fila - 1, 42] = "DATOS GENERALES DEL SLOT";

                range = workSheet.Range[workSheet.Cells[fila - 2, 1], workSheet.Cells[fila - 2, 46]];
                range.Merge();

                range = workSheet.Range[workSheet.Cells[fila - 3, 1], workSheet.Cells[fila - 3, 46]];
                range.Merge();
                range.Font.Bold = true;
                range.Font.Color = Color.Black;
                range.Font.Size = 18;
                range.Font.Name = "Arial";
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                workSheet.Cells[fila - 3, 1] = "Reporte General";

                range = workSheet.Range[workSheet.Cells[fila - 4, 1], workSheet.Cells[fila - 4, 46]];
                range.Merge();
                range.Font.Bold = true;
                range.Font.Color = Color.Black;
                range.Font.Size = 18;
                range.Font.Name = "Arial";
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                workSheet.Cells[fila - 4, 1] = "CIUDAD POSITIVA";

                range = workSheet.Range[workSheet.Cells[fila, 1], workSheet.Cells[fila + ReporteGeneral.Count, 46]];
                range.Font.Name = "Arial";
                range.VerticalAlignment = XlHAlign.xlHAlignCenter;
                range.Borders.LineStyle = XlLineStyle.xlContinuous;

                //REPORTE GENERAL

                Object[,] DataTrabajadores = null;

                if (ReporteGeneral.Count > 0)
                {
                    DataTrabajadores = new Object[ReporteGeneral.Count, ReporteGeneral[0].Count];
                }

                for (int i = 0; i < ReporteGeneral.Count; i++)
                {
                    Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] = (int)Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] + 1;
                    IDictionary<string, object> objReporteTrabajador = ReporteGeneral[i];

                    Object[] lstValuesReporteTrabajador = objReporteTrabajador.Values.ToArray();
                    for (int j = 0; j < objReporteTrabajador.Count; j++)
                    {
                        DataTrabajadores[i, j] = lstValuesReporteTrabajador[j];
                    }

                }

                if (ReporteGeneral != null && ReporteGeneral.Count > 0)
                {
                    workSheet.Range[workSheet.Cells[fila + 1, 1], workSheet.Cells[fila + ReporteGeneral.Count, ReporteGeneral[0].Count]].Value = DataTrabajadores;
                }

                //AutoAjustar
                workSheet.Columns.AutoFit();
                //workSheet.Rows.AutoFit();

                range = workSheet.Range[workSheet.Cells[fila, 22], workSheet.Cells[fila, 46]];
                range.ColumnWidth = 25;
                range.WrapText = true;

                range = workSheet.Range[workSheet.Cells[fila, 22], workSheet.Cells[fila + ReporteGeneral.Count, 46]];
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                //Guardar el archivo
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reporte", "Reporte general detallado " + FechaActual + ".xlsx")) == true)
                    File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reporte", "Reporte general detallado " + FechaActual + ".xlsx"));
                workSheet.SaveAs(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reporte", "Reporte general detallado " + FechaActual + ".xlsx"));
                excelApp.Quit();

                Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] = (int)Session[ZP.SOOM.CursosVirtuales.Util.Constants.GetProgress.Porcentaje.CurrentProgress] + 1;

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ExcelDispose(excelApp, workSheet, worksheets, workbook, workbooks, range);
            }
        }

        private void ExcelDispose(Application excelApp, Worksheet workSheet, Sheets worksheets, Workbook workbook, Workbooks workbooks, Range range)
        {
            if (range != null) Marshal.ReleaseComObject(range);
            if (workSheet != null) Marshal.ReleaseComObject(workSheet);
            if (worksheets != null) Marshal.ReleaseComObject(worksheets);
            if (workbook != null) Marshal.ReleaseComObject(workbook);
            if (workbooks != null) Marshal.ReleaseComObject(workbooks);
            if (excelApp != null) Marshal.ReleaseComObject(excelApp);
            range = null;
            workSheet = null;
            worksheets = null;
            workbook = null;
            workbooks = null;
            excelApp = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

    }
}