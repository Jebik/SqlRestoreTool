namespace SqlManager.Tools
{
    internal partial class SqlQuery
    {
        // SQL to Clean unused table after restore.
        public const string CleanTable = @"
            drop table Test

            CREATE TABLE[dbo].[Test]
            (
                [Id] [int] IDENTITY(1,1) NOT NULL,
                [Date] [datetime] NOT NULL,
            ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";
    }
}