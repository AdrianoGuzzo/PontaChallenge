using System.ComponentModel;

namespace ApiTask.DataInfrastructure.Entities.Enum
{
    public enum Status
    {
        [Description("Pendente")]
        Pending,
        [Description("Em progresso")]
        InProgress,
        [Description("Concluída")]
        Completed,
    }
}
