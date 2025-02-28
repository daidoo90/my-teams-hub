namespace MyTeamsHub.Persistence.Constants;

public static class PersistenceConstants
{
    public static class DbSchemas
    {
        public const string Default = "dbo";
        public const string Organizations = "organizations";
    }

    public static class Tables
    {
        public const string Organization = "Organization";
        public const string Team = "Team";
        public const string TeamMember = "TeamMember";
        public const string User = "User";
    }
}
