using System;

namespace SecurityUnified.Serialization.Expressions.Parser.Exceptions
{
    [Serializable]
    abstract public class ParseException : Exception
    {
        public ParseException(string message, int errorIndex) : this(message, errorIndex, null) { }
        public ParseException(string message, int errorIndex, Exception inner)
            : base(string.Format("Parse Exception：{1}", errorIndex, message), inner) { }

        static public void Assert(string strInput, string strNeed, int index)
        {
            if (strInput != strNeed)
            {
                throw new ParseWrongSymbolException(strNeed, strInput, index);
            }
        }
    }

    [Serializable]
    abstract public class CompileException : Exception
    {
        public CompileException(string message, int errorIndex) : this(message, errorIndex, null) { }
        public CompileException(string message, int errorIndex, Exception inner)
            : base(string.Format("Compile Exception：{1}", errorIndex, message), inner) { }
    }

    [Serializable]
    public class ParseNoEndException : ParseException
    {
        public ParseNoEndException(string symbol, int errorIndex)
            : base(string.Format("No ending detected：“{0}”", symbol), errorIndex)
        {
        }
    }

    [Serializable]
    public class ParseUnknownException : ParseException
    {
        public ParseUnknownException(string symbol, int errorIndex)
            : base(string.Format("Unknown symbol：“{0}”", symbol), errorIndex)
        {
        }
    }

    [Serializable]
    public class ParseUnmatchException : ParseException
    {
        public ParseUnmatchException(string startSymbol, string endSymbol, int errorIndex)
            : base(string.Format("No matching symbols. Starting from:“{0}” to:“{1}”", startSymbol, endSymbol), errorIndex)
        {
        }
    }

    [Serializable]
    public class ParseWrongSymbolException : ParseException
    {
        public ParseWrongSymbolException(string rightSymbol, string wrongSymbol, int errorIndex)
            : base(string.Format("Incorrect symbol. expected:“{0}”；found:“{1}”", rightSymbol, wrongSymbol), errorIndex)
        {
        }
    }

    [Serializable]
    public class ParseUnfindTypeException : ParseException
    {
        public ParseUnfindTypeException(string typeName, int errorIndex)
            : base(string.Format("Type not found：“{0}”", typeName), errorIndex)
        {
        }
    }
}
