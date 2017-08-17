using ArchPM.Core.Enums;
namespace ArchPM.Core.Attributes
{
    public enum RepositoryTypes
    {
        [EnumDescription("General")]
        General,
        [EnumDescription("Custom")]
        Custom
    }

    public enum ApplicationEnvironments
    {
        [EnumDescription("Development")]
        Dev = 1,
        [EnumDescription("Test")]
        Test = 2,
        [EnumDescription("Production")]
        Prod = 3
    }

    public enum ApplicationDataTypes
    {
        [EnumDescription("ADO.NET")]
        Ado,
        [EnumDescription("Memory")]
        Memory,
        [EnumDescription("Entity Framework")]
        EF,
        [EnumDescription("Dapper")]
        Dapper
    }

}
