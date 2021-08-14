using System;
using System.Linq;
using NUnit.Framework;
using QuizFramework.Utils;

namespace QuizFramework.UnitTests
{
    public class CsvLineParserTests
    {
        [Test]
        public void ParseEmptyString()
        {
	        var line = CsvLineParser.Split("", ';').ToArray();
	        Assert.AreEqual(0, line.Length);
        }
        
        [Test]
        public void ParseSingleString()
        {
	        var line = CsvLineParser.Split("Hello", ';').ToArray();
	        Assert.AreEqual(1, line.Length);
	        Assert.AreEqual("Hello", line[0]);
        }
        
        [Test]
        public void Parse2Strings()
        {
	        var line = CsvLineParser.Split("Hello, world!", ',').ToArray();
	        Assert.AreEqual(2, line.Length);
	        Assert.AreEqual("Hello", line[0]);
	        Assert.AreEqual(" world!", line[1]);
        }
        
        [Test]
        public void ParseStringWithEmpty()
        {
	        var line = CsvLineParser.Split("Hello,", ',').ToArray();
	        Assert.AreEqual(2, line.Length);
	        Assert.AreEqual("Hello", line[0]);
	        Assert.AreEqual("", line[1]);
        }
        
        [Test]
        public void ParseEmptyWithString()
        {
	        var line = CsvLineParser.Split(",world", ',').ToArray();
	        Assert.AreEqual(2, line.Length);
	        Assert.AreEqual("", line[0]);
	        Assert.AreEqual("world", line[1]);
        }
                
        [Test]
        public void Parse2Empty()
        {
	        var line = CsvLineParser.Split(";", ';').ToArray();
	        Assert.AreEqual(2, line.Length);
	        Assert.AreEqual("", line[0]);
	        Assert.AreEqual("", line[1]);
        }
        
        [Test]
        public void ParseStringWithQuotes()
        {
	        var line = CsvLineParser.Split("\"hello\"", ';').ToArray();
	        Assert.AreEqual(1, line.Length);
	        Assert.AreEqual("\"hello\"", line[0]);
        }
        
        [Test]
        public void Parse2StringsWithQuotes()
        {
	        var line = CsvLineParser.Split("hello;\"1;2;3\"", ';').ToArray();
	        Assert.AreEqual(2, line.Length);
	        Assert.AreEqual("hello", line[0]);
	        Assert.AreEqual("\"1;2;3\"", line[1]);
        }

        [Test]
        public void ParseQuotes()
        {
	        var line = CsvLineParser.Split("\"1;2;3;4;5\"", ';').ToArray();
	        Assert.AreEqual(1, line.Length);
	        Assert.AreEqual("\"1;2;3;4;5\"", line[0]);
        }
        
        [Test]
        public void ParseQuotes2()
        {
	        var line = CsvLineParser.Split("\"1;2;3;4;5\";", ';').ToArray();
	        Assert.AreEqual(2, line.Length);
	        Assert.AreEqual("\"1;2;3;4;5\"", line[0]);
	        Assert.AreEqual("", line[1]);
        }
        
        [Test]
        public void ParseQuotes3()
        {
	        var line = CsvLineParser.Split(";\"1;2;3;4;5\"", ';').ToArray();
	        Assert.AreEqual(2, line.Length);
	        Assert.AreEqual("", line[0]);
	        Assert.AreEqual("\"1;2;3;4;5\"", line[1]);
        }
        
        [Test]
        public void TestParseError()
        {
	        try
	        {
		        CsvLineParser.Split("Hello;\"1;2;3;4", ';').ToArray();
	        }
	        catch (Exception)
	        {
		        return;
	        }
	        
	        Assert.Fail("ParseException expected");
        }

        [Test]
        public void ParseRealString()
        {
	        var result = CsvLineParser.Split("5,1200,14400,\"1, 2 ,3 ,4 ,5\",1800", ',').ToArray();
		    Assert.AreEqual(5, result.Length);
        }
    }
}