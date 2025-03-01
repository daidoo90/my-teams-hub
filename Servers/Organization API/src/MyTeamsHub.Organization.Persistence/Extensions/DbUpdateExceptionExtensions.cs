using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Organization.Persistence.Exceptions;

namespace MyTeamsHub.Organization.Persistence.Extensions;

public static class DbUpdateExceptionExtensions
{
    public static CustomDetailedDbUpdateException GetDetailedDbUpdateException(this DbUpdateException ex)
    {
        // This is thrown if any errors are pushed up from the database, e.g. foreign key conflicts.
        var inner = ex.InnerException;

        while (inner?.InnerException != null)
        {
            inner = inner.InnerException;
        }

        var errors = new List<string>();

        if (inner is SqlException sqlException)
        {
            foreach (var error in sqlException.Errors)
            {
                var sqlError = error as SqlError;
                errors.Add($"[{sqlError?.Number}] {sqlError?.Message}");
            }

            return new CustomDetailedDbUpdateException(
                $"Could not save the changes due to a SQL error ({sqlException.ErrorCode}) - see InnerException for more details.",
                new CustomDetailedDbUpdateException($"SQL errors: {string.Join("; ", errors.ToArray())}"));
        }
        else
        {
            return new CustomDetailedDbUpdateException(
                "Could not save the changes as there was an error in the database - see InnerException for more details.",
                ex);
        }
    }
}
