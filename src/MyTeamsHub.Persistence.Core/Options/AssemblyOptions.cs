using System.Reflection;

namespace MyTeamsHub.Persistence.Core.Options;

public class AssemblyOptions
{
    public Assembly MigratorAssembly { get; set; } = default!;

    public AssemblyOptions Validate()
    {
        MigratorAssembly = MigratorAssembly ?? throw new ArgumentNullException(nameof(MigratorAssembly));

        return this;
    }
}