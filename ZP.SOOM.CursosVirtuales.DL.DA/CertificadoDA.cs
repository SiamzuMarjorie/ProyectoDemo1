using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZP.SOOM.CursosVirtuales.DL.DM;

using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Configuration;


namespace ZP.SOOM.CursosVirtuales.DL.DA
    {
    public class CertificadoDA
    {
        private List<CertificadoEntity> tranform (IQueryable<CertificadoEntity> dato)
        {

            List<CertificadoEntity> lista = new List<CertificadoEntity>();

            foreach (var item in dato)
            {
                CertificadoEntity entity = new CertificadoEntity();
                entity.Certificado = item.Certificado;
                entity.Codigo = item.Codigo;
                entity.Compañia = item.Compañia;

                lista.Add(entity);
            }

            return lista;
        } 
        
        public List<CertificadoEntity> ListarCertificadosbyNombreCampo(string NombreCampo, int? Ano, string[] OpcionSeleccionada)
        {
            List<CertificadoEntity> lista = new List<CertificadoEntity>();

            SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();

            switch (NombreCampo)
            {
                case "C":

                    var query1 = from I in objModel.Inscripcion
                                                     join CS in objModel.CursoSlot on I.IdCursoSlot equals CS.IdCursoSlot
                                                     join C in objModel.Curso on CS.IdCurso equals C.IdCurso
                                                     join T in objModel.Trabajador on I.IdTrabajador equals T.IdTrabajador
                                                     where I.Certificado != null && I.FechaHoraRegistro.Year == Ano && OpcionSeleccionada.Contains(T.Compañia)
                                                     select new CertificadoEntity()
                                                     {
                                                         Certificado = I.Certificado,
                                                         Compañia =  T.Compañia,
                                                         Codigo = C.Codigo

                                                     };

                    lista = tranform(query1);

                   // query1.ToList<CertificadoEntity>();

                    break;
                case "S":

                   var query2 = from I in objModel.Inscripcion
                                                     join CS in objModel.CursoSlot on I.IdCursoSlot equals CS.IdCursoSlot
                                                     join C in objModel.Curso on CS.IdCurso equals C.IdCurso
                                                     join T in objModel.Trabajador on I.IdTrabajador equals T.IdTrabajador
                                                     where I.Certificado != null && I.FechaHoraRegistro.Year == Ano && OpcionSeleccionada.Contains(T.Sede)
                               select new CertificadoEntity()
                               {
                                   Certificado = I.Certificado,
                                   Compañia = T.Compañia,
                                   Codigo = C.Codigo

                               };

                    lista = tranform(query2);


                    break;
                case "G":

                    var query3 = from I in objModel.Inscripcion
                                                     join CS in objModel.CursoSlot on I.IdCursoSlot equals CS.IdCursoSlot
                                                     join C in objModel.Curso on CS.IdCurso equals C.IdCurso
                                                     join T in objModel.Trabajador on I.IdTrabajador equals T.IdTrabajador
                                                     where I.Certificado != null && I.FechaHoraRegistro.Year == Ano && OpcionSeleccionada.Contains(T.Gerencia)
                            select new CertificadoEntity()
                            {
                                Certificado = I.Certificado,
                                Compañia = T.Compañia,
                                Codigo = C.Codigo

                            };

                    lista = tranform(query3);

                    break;
                case "CAT":

                    var query4 = from I in objModel.Inscripcion
                                                     join CS in objModel.CursoSlot on I.IdCursoSlot equals CS.IdCursoSlot
                                                     join C in objModel.Curso on CS.IdCurso equals C.IdCurso
                                                     join T in objModel.Trabajador on I.IdTrabajador equals T.IdTrabajador
                                                     where I.Certificado != null && I.FechaHoraRegistro.Year == Ano && OpcionSeleccionada.Contains(T.GrupoOcupacional.Nombre)
                             select new CertificadoEntity()
                             {
                                 Certificado = I.Certificado,
                                 Compañia = T.Compañia,
                                 Codigo = C.Codigo

                             };

                    lista = tranform(query4);

                    break;
                
                case "SG":

                    var query5 = from I in objModel.Inscripcion
                                                     join CS in objModel.CursoSlot on I.IdCursoSlot equals CS.IdCursoSlot
                                                     join C in objModel.Curso on CS.IdCurso equals C.IdCurso
                                                     join T in objModel.Trabajador on I.IdTrabajador equals T.IdTrabajador
                                                     join GCu in objModel.GrupoCurso on C.IdCurso equals GCu.IdCurso
                                                     join GR in objModel.Grupo on GCu.IdGrupo equals GR.IdGrupo
                                                     where I.Certificado != null && I.FechaHoraRegistro.Year == Ano && OpcionSeleccionada.Contains(GR.Nombre)
                             select new CertificadoEntity()
                             {
                                 Certificado = I.Certificado,
                                 Compañia = T.Compañia,
                                 Codigo = C.Codigo

                             };

                    lista = tranform(query5);

                    break;
                default:
                    lista = new List<CertificadoEntity>();
                    break;
            }




           return lista;

        }

        public List<CertificadoEntity> ListarCertificadosbyCursoSlot(int? Ano, int[] idCursoSlot)
        {
            List<CertificadoEntity> lista = new List<CertificadoEntity>();

            SOOMCursosVirtualesEntities objModel = new SOOMCursosVirtualesEntities();

            var query1 = from I in objModel.Inscripcion
                         join CS in objModel.CursoSlot on I.IdCursoSlot equals CS.IdCursoSlot
                         join C in objModel.Curso on CS.IdCurso equals C.IdCurso
                         join T in objModel.Trabajador on I.IdTrabajador equals T.IdTrabajador
                        where I.Certificado != null  && idCursoSlot.Contains(CS.IdCursoSlot) && I.FechaHoraRegistro.Year == Ano
                         select new CertificadoEntity()
                         {
                             Certificado = I.Certificado,
                             Compañia = T.Compañia,
                             Codigo = C.Codigo

                         };

            lista = tranform(query1);

            return lista;
        }
    }
}
