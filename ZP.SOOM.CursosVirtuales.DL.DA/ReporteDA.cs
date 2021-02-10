using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Configuration;
using Dapper;

using ZP.SOOM.CursosVirtuales.DL.DM;
using ZP.SOOM.CursosVirtuales.Util;

namespace ZP.SOOM.CursosVirtuales.DL.DA
{
    public class ReporteDA
    {
        public IEnumerable<IDictionary<string, object>> ObtenerReporteGeneral(int Ano, string[] Intentos, string FechaInicio, string FechaFin, int[] listaTrabajadores, int?[] IdCursoSlot, string[] Estatus)
        {
            try
            {
                IEnumerable<IDictionary<string, object>> resultado;


                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["SOOMCursosVirtuales"].ConnectionString))
                {
                    cnn.Open();
                    DataTable dtIdTrabajador = new DataTable();
                    dtIdTrabajador.Columns.Add("IdTrabajador", typeof(int));
                    if (listaTrabajadores != null)
                    {
                        foreach (int idTrabajador in listaTrabajadores)
                        {
                            dtIdTrabajador.Rows.Add(idTrabajador);
                        }
                    }

                    DataTable dtIdCursoSlot = new DataTable();
                    dtIdCursoSlot.Columns.Add("IdCursoSlot", typeof(int));
                    if (IdCursoSlot != null)
                    {
                        foreach (int idCursoSlot in IdCursoSlot)
                        {
                            dtIdCursoSlot.Rows.Add(idCursoSlot);
                        }
                    }


                    DataTable dtEstatus = new DataTable();
                    dtEstatus.Columns.Add("IdEstatus", typeof(string));


                    if (Estatus != null)
                    {
                        foreach (string idEstatus in Estatus)
                        {
                            dtEstatus.Rows.Add(idEstatus);
                        }
                    }

                    DataTable dtCampos = new DataTable();
                    dtCampos.Columns.Add("IdCampo", typeof(string));
                    if (Intentos != null)
                    {
                        foreach (string idCampo in Intentos)
                        {
                            dtCampos.Rows.Add(idCampo);
                        }
                    }
                    else {
                        dtCampos.Rows.Add('T');
                    }


                    var p = new DynamicParameters();
                    p.Add("@Ano", (object)Ano ?? DBNull.Value, DbType.Int32);
                    //p.Add("@Intentos", (object)Intentos ?? DBNull.Value, DbType.String);
                    p.Add("@ListaFiltroCampo", dtCampos.AsTableValuedParameter("[dbo].[ListaFiltroCampo]"));
                    p.Add("@FechaInicio", FechaInicio == null ? (object) DBNull.Value : (object)FechaInicio, DbType.String);
                    p.Add("@FechaFin", FechaFin == null ? (object) DBNull.Value : (object)FechaFin, DbType.String);
                    p.Add("@ListaTrabajadores", dtIdTrabajador.AsTableValuedParameter("[dbo].[ListaTrabajadores]"));
                    p.Add("@ListaCursoSlot", dtIdCursoSlot.AsTableValuedParameter("[dbo].[ListaCursoSlot]"));
                    p.Add("@ListaEstatus", dtEstatus.AsTableValuedParameter("[dbo].[ListaEstatus]"));
                   
                    resultado = (IEnumerable<IDictionary<string, object>>)
                                cnn.Query(sql: "GenerarReporteGeneral",
                                param: p,
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10000000);
                }

                return resultado;


            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
