using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZP.SOOM.CursosVirtuales.Util
{
    public class Constants
    {
        public class Usuario
        {
            public class Perfil
            {
                public const String ADMINISTRADOR = "A";
                public const String USUARIO = "U";
            }
            public const String MASTER_USER = "masteradministrator";

            public class FOTO
            {
                public const string SINFOTO = "ic_user.svg";
            }
        }

        public class Trabajador
        {
            public class Estado
            {
                public const String ACTIVO = "A";
                public const String INACTIVO = "I";
            }
            public class Genero
            {
                public const String MASCULINO = "M";
                public const String FEMENINO = "F";
                public const String OTROS = "N";
            }
        }

        public class Avatar
        {
            public class Genero
            {
                public const String HOMBRE = "H";
                public const String MUJER = "M";
            }
        }

        public class Dashboard
        {
            public class Color
            {
                public const String MORA = "#ff6d46";
                public const String CELESTE = "#76cebf";
            }
            public class Nivel
            {
                public const String GERENCIA = "G";
                public const String AREA = "A";
                public const String AÑO = "Y";
                public const String MES = "M";
                public const String DIA = "D";
                public const String HORA = "H";
            }
        }

        public class ReporteGeneral
        {
            public class Intento
            {
                public const String TODO = "T";
                public const String PRIMERINTENTOAPROBADO = "N";
                public const String PRIMERINTENTOMAYORNOTA = "M";
                public const String SININTENTO = "S";

                public const String DESAPROBADOS = "D";
                public const String ULTIMOINTENTOMAYORNOTA = "U";
            }
            public class Campos
            {
                public const String COMPANIA = "C";
                public const String SEDE = "S";
                public const String GERENCIA = "G";
                public const String CATEGORIA = "CAT";
                public const String CURSOSLOT = "CS";
                public const String SEGMENTO = "SG";
            }

            public class Estatus
            {
                public const String TODOS = "T";
                public const String ACTIVOS = "A";
                public const String INACTIVOS = "I";
                public const String CESADOS = "C";
            }

        }

        public class Cursos
        {
            public class Secciones
            {
                public const String CURSOS = "C";
                public const String ARCHIVADOS = "A";
                public const String HISTORIAL = "H";
            }

            public class ContenidoCurso
            {
                public const String PDF = "P";
                public const String VIDEO = "V";
                public const String SCORM = "S";
            }

            public class Material
            {
                public class TipoMaterial
                {
                    public const String VIDEO = "V";
                    public const String DOCUMENTO = "D";
                    public const String LINK = "L";
                }
                //public class Archivo
                //{
                //    public class Video
                //    {
                //        public const int PESOVIDEO = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TamanioMaximoArchivos"].ToString());
                //    }
                //}
            }
            public class Estado
            {
                public const String INACTIVO = "I";
                public const String HISTORIAL = "H";
                public const String ACTIVO = "A";
                public const String OCULTO = "O";
                public const String NOINICIADO = "N";
                public const String ENCURSO = "C";
                public const String EXPIRADO = "E";
                public const String TERMINADO = "T";
                public const String ARCHIVADO = "F";
            }
            public class Filtro
            {
                public const String NOMBRE = "Nombre";
                public const String CODIGO = "Codigo";
            }
        }

        public class Certificado
        {
            public class TipoFormato
            {
                public const String PDF = ".pdf";
                public const String POWER_POINT = ".pptx";
            }
        }

        public class Configuracion
        {
            public const String FORMATO_CERTIFICADO = "FC";
        }

        public class Reporte
        {
            public class TipoReporte
            {
                public const String USUARIO = "U";
                public const String CURSO = "C";
            }
        }

        public class TipoActividad
        {
            public const String CURSO = "C";
            public const String LOGIN = "L";
        }

        public class GetProgress
        {
            public class Porcentaje
            {
                public const String CurrentProgress = "CurrentProgress";
                public const String TotalProgress = "TotalProgress";
            }

            public const String MESSAGE = "Message";
        }

        public class Extenciones
        {
            public const String ZIP = ".zip";
            public const String RAR = ".rar";
            public const String PDF = ".pdf";
            public const String VIDEO = ".mp4";

        }
    }
}
