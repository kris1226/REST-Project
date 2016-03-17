using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers
{
    public static class NetworkSubmit
    {
        public static StringBuilder LoginScript(string browserVersion)
        {
            return new StringBuilder()
                .AppendFormat(@"SET !USERAGENT {0}\nSET !TIMEOUT_STEP 5\n", browserVersion)
                .Append(@"URL GOTO %%websiteDomain%%\nWAIT SECONDS=1\nTAG POS=1 TYPE=INPUT:TEXT ATTR=NAME:*Username CONTENT=%%websiteUsername%%\n")
                .Append(@"TAG POS=1 TYPE=INPUT:PASSWORD ATTR=NAME:*Password CONTENT=%%websitePassword%%\nTAG POS=1 TYPE=INPUT:SUBMIT ATTR=ID:btnSignInSubmit\n")
                .Append(@"SET !TIMEOUT_STEP 2\nTAG POS=1 TYPE=DIV ATTR=ID:SignInErrorInfo EXTRACT=TXT\n"); ;
        }
    }
}
