﻿namespace SP.PSPP.Integration.Constants
{
    public class CommandConstants
    {
        public const string ConfigurationFormat = @"SET DECIMAL=dot.
                                                    GET DATA /TYPE=TXT 
		                                                /FILE=""{0}""
		                                                /ENCODING=""UTF-8""
		                                                /DELIMITERS="","" 
		                                                /FIRSTCASE=2 
		                                                /VARIABLES={1}.";
        public const string PearsonCorrelationCommonFormat = @"CORRELATION
                                                                   /VARIABLES={0}.
                                                               FILTER OFF.";

        public const string PearsonCorrelationFilterFormat = @"COMPUTE groupVar = {0}.
                                                               EXECUTE.
                                                               FILTER BY groupVar.
                                                               VALUE LABELS 
		                                                            /{1}
CORRELATION
	                                                                /VARIABLES={2}.
                                                               FILTER OFF.";

        public const string MiddleMeanCommonFormat = @"DESCRIPTIVES
                                                               /VARIABLES={0}
                                                               /FORMAT=LABELS
                                                               /STATISTICS=MEAN.
                                                       FILTER OFF.";

        public const string MiddleMeanFilterFormat = @"COMPUTE groupVar = {0}.
                                                       EXECUTE.
                                                       FILTER BY groupVar.
                                                       VALUE LABELS 
		                                                       /{1}
DESCRIPTIVES
                                                               /VARIABLES={2}
                                                               /FORMAT=LABELS
                                                               /STATISTICS=MEAN.
                                                       FILTER OFF.";

        public const string MeanChanceCommonFormat = @"FREQUENCIES
                                                               /VARIABLES={0}
                                                               /FORMAT=AVALUE TABLE
                                                               /STATISTICS=MEAN.
                                                       FILTER OFF.";

        public const string MeanChanceFilterFormat = @"COMPUTE groupVar = {0}.
                                                       EXECUTE.
                                                       FILTER BY groupVar.
                                                       VALUE LABELS 
		                                                       /{1}
FREQUENCIES
                                                               /VARIABLES={2}
                                                               /FORMAT=AVALUE TABLE
                                                               /STATISTICS=MEAN.
                                                       FILTER OFF.";
    }
}
