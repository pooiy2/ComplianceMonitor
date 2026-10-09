using NpgsqlTypes;

namespace Backend.Enums
{
    public enum Result
    {
        [PgName("COMPLIES")]
        COMPLIES,

        [PgName("DEVIATES")]
        DEVIATES,

        [PgName("UNCLEAR")]
        UNCLEAR
    }
}
