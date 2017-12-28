using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace DellaSanta.Logging
{
    public class Log4NetLogger
    {
        public static void Configure(string connectionString)
        {
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            hierarchy.Root.AddAppender(CreateAdoNetAppender(connectionString));
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        private static IAppender CreateAdoNetAppender(string connectionString)
        {
            var appender = new AdoNetAppender
            {
                Name = "AdoNetAppender",
                ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version = 1.0.3300.0, Culture = neutral, PublicKeyToken = b77a5c561934e089",
                ConnectionString = connectionString,
                BufferSize = 1,
                CommandText = @"INSERT INTO[LogEntries](
                [LogDate],
                [Logger],
                [LogLevel],
                [Thread],
                [EntityFormalNamePlural],
                [EntityKeyValue],
                [UserName],
                [Message],
                [Exception]) VALUES(@log_date, @logger, @log_level, @thread, @entityFormalNamePlural, @entityKeyValue, @userName, @message, @exception)",

                CommandType = System.Data.CommandType.Text
            };
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@log_date",
                DbType = System.Data.DbType.DateTime,
                Layout = new RawTimeStampLayout()
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@logger",
                DbType = System.Data.DbType.String,
                Size = 256,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@log_level",
                DbType = System.Data.DbType.String,
                Size = 5,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@thread",
                DbType = System.Data.DbType.String,
                Size = 10,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@entityFormalNamePlural",
                DbType = System.Data.DbType.String,
                Size = 30,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%property{EntityFormalNamePlural}"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "entityKeyValue",
                DbType = System.Data.DbType.Int32,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%property{EntityKeyValue}"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@userName",
                DbType = System.Data.DbType.String,
                Size = 256,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%property{UserName}|%aspnet-request{AUTH_USER}"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@message",
                DbType = System.Data.DbType.String,
                Size = 256,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message"))
            });
            appender.AddParameter(new AdoNetAppenderParameter
            {
                ParameterName = "@exception",
                DbType = System.Data.DbType.String,
                Size = 10000,
                Layout = new Layout2RawLayoutAdapter(new ExceptionLayout())
            });

            appender.ActivateOptions();
            return appender;
        }
    }
}
