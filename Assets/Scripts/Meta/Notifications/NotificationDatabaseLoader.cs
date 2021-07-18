using System;
using System.Collections.Generic;
using QuizFramework.Utils;

namespace QuizFramework.Notifications
{
    public static class NotificationDatabaseLoader
    {
        private const char RowsSeparator = '\n';
        private const char AdditionalRowsSeparator = '\r';
        private const char ColumnsSeparator = ';';
        
        private const int StartRow = 1;
        private const int IdColumn = 0;
        private const int TitleColumn = 1;
        private const int TextColumn = 2;
        private const int TimeSpanColumn = 3;
        private const int RepeatIntervalColumn = 4;
        
        public static NotificationDatabase LoadDatabase(string database)
        {
            var databaseRows = database.Split(RowsSeparator);
            TryRemoveAdditionalLineSeparatorSymbol(ref databaseRows);
            
            var notifications = new List<NotificationData>();
            for (var i = StartRow; i < databaseRows.Length; i++)
            {
                if (string.IsNullOrEmpty(databaseRows[i]))
                {
                    continue;
                }
                
                var databaseColumns = databaseRows[i].Split(ColumnsSeparator);
                var id = ParserUtils.ParseEnum<NotificationId>(databaseColumns[IdColumn]);
                var title = databaseColumns[TitleColumn];
                var text = databaseColumns[TextColumn];
                var timeSpan = string.IsNullOrEmpty(databaseColumns[TimeSpanColumn])
                    ? TimeSpan.Zero
                    : ParserUtils.ParseTimeSpan(databaseColumns[TimeSpanColumn]);
                var repeatInterval = string.IsNullOrEmpty(databaseColumns[RepeatIntervalColumn])
                    ? (TimeSpan?) null
                    : ParserUtils.ParseTimeSpan(databaseColumns[RepeatIntervalColumn]);
                
                var notification = new NotificationData(id, title, text, timeSpan, repeatInterval);
                notifications.Add(notification);
            }

            return new NotificationDatabase(notifications);
        }

        private static void TryRemoveAdditionalLineSeparatorSymbol(ref string[] databaseRows)
        {
            for (var i = StartRow; i < databaseRows.Length; i++)
            {
                databaseRows[i] = databaseRows[i].TrimEnd(AdditionalRowsSeparator);
            }
        }
    }
}