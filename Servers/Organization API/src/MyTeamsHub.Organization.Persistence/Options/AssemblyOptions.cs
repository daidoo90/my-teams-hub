using System.Reflection;

namespace MyTeamsHub.Organization.Persistence.Options;

public class AssemblyOptions
{
    public Assembly MigratorAssembly { get; set; } = default!;

    public AssemblyOptions Validate()
    {
        MigratorAssembly = MigratorAssembly ?? throw new ArgumentNullException(nameof(MigratorAssembly));

        return this;
    }
}
