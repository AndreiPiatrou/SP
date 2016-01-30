namespace SP.PSPP.Integration.Constants
{
    public class CommandConstants
    {
        public const string CorrelationCommandFormat = @"SET DECIMAL=dot.
                                                         GET DATA /TYPE=TXT 
		                                                     /FILE=""{0}""
		                                                     /ENCODING=""UTF-8""
		                                                     /DELIMITERS="","" 
		                                                     /FIRSTCASE=2 
		                                                     /VARIABLES={1} F4.

                                                        CORRELATION
	                                                        /VARIABLES= {2}.";
    }
}
