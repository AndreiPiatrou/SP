namespace SP.PSPP.Integration.Constants
{
    public class CommandConstants
    {
        public const string PearsonCorrelationCommandFormat = @"SET DECIMAL=dot.
                                                                GET DATA /TYPE=TXT 
		                                                             /FILE=""{0}""
		                                                             /ENCODING=""UTF-8""
		                                                             /DELIMITERS="","" 
		                                                             /FIRSTCASE=2 
		                                                             /VARIABLES={1} F4.

                                                                CORRELATION
	                                                                /VARIABLES= {2}.";

        public const string MiddleMeanFormat = @"SET DECIMAL=dot.
                                                 GET DATA /TYPE=TXT 
		                                                  /FILE=""{0}""
		                                                  /ENCODING=""UTF-8""
		                                                  /DELIMITERS="","" 
		                                                  /FIRSTCASE=2 
		                                                  /VARIABLES={1} F4.

                                                 DESCRIPTIVES 
                                                          /VARIABLES={2}
                                                          /STATISTICS=MEAN.";

        public const string MeanChanceFormat = @"SET DECIMAL=dot.
                                                 GET DATA /TYPE=TXT 
		                                                  /FILE=""{0}""
		                                                  /ENCODING=""UTF-8""
		                                                  /DELIMITERS="","" 
		                                                  /FIRSTCASE=2 
		                                                  /VARIABLES={1} F4.

                                                FREQUENCIES
                                                         /VARIABLES={2}
                                                         /FORMAT=AVALUE TABLE
                                                         /STATISTICS=MEAN STDDEV.";
    }
}
