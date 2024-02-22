using System.Runtime.Serialization;

namespace MilesCarRental.Domain.Exceptions;


/// <summary>
/// Representa una excepción específica del dominio de negocio dentro de la aplicación MilesCarRental.
/// Esta excepción se utiliza para manejar errores que son específicos de las reglas y lógica del negocio,
/// proporcionando una forma de identificar y tratar estos errores de manera diferenciada respecto a
/// excepciones más generales del sistema o de la aplicación.
/// </summary>
[Serializable]
public class CoreBusinessException : Exception
{
    public CoreBusinessException()
    {

    }
    public CoreBusinessException(string message) : base(message)
    {
    }

    public CoreBusinessException(string message, Exception inner) : base(message, inner)
    {
    }

    protected CoreBusinessException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
    }
}
