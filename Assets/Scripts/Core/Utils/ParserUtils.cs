using System;
using System.Globalization;

namespace QuizFramework.Utils
{
    public static class ParserUtils
    {
        public static T ParseEnum<T>(string str)
        {
            try
            {
                return (T) Enum.Parse(typeof(T), str);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Can't parse enum {typeof(T)} from string {str}, {ex}");
            }
        }
        
        public static TimeSpan ParseTimeSpan(string str)
        {
            try
            {
                return TimeSpan.ParseExact(str, @"d\:hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
            }
            catch(Exception ex)
            {
                throw new ArgumentException($"Can't parse time span from string {str}", str, ex);
            }
        }
    }
}