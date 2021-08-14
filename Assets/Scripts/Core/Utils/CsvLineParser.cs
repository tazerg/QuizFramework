using System;
using System.Collections.Generic;

namespace QuizFramework.Utils
{
    public static class CsvLineParser
    {
        private const char QuoteChar = '"';
        
        public static IEnumerable<string> Split(string str, char cellSeparator)
        {
            if (str == string.Empty)
                yield break;
            
            var tokenStart = 0;
            for(var i = 0; i < str.Length; ++i)
            {
                if (str[i] == cellSeparator)
                {
                    yield return str.Substring(tokenStart, i - tokenStart);
                    tokenStart = i + 1;
                    continue;
                }

                if (str[i] != QuoteChar) 
                    continue;
                
                do
                {
                    ++i;
                    if (i >= str.Length)
                        throw new Exception("Can't find closing \" in line " + str);
                } while (str[i] != QuoteChar);
            }
			
            if (tokenStart <= str.Length)
                yield return str.Substring(tokenStart, str.Length - tokenStart);
        }
    }
}