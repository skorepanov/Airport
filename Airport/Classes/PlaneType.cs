using System.ComponentModel;

namespace Airport.Classes
{
    /// <summary>
    /// Тип самолёта
    /// </summary>
    public enum PlaneType
    {
        [Description("Ту-134")]
        TU134 = 96,

        [Description("Ту-204")]
        TU204 = 214,

        [Description("ИЛ-62")]
        IL62 = 198,

        [Description("A310")]
        A310 = 183
    }
}
