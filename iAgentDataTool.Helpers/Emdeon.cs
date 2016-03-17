using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.ScriptHelpers
{
    public class Emdeon
    {
        public static StringBuilder LoginScript(string browserVersion)
        {
            return new StringBuilder()
                .AppendFormat(@"SET !USERAGENT {0}\nSET !TIMEOUT_STEP 5\n", browserVersion)
                .Append(@"URL GOTO %%websiteDomain%%\nWAIT SECONDS=1\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:username CONTENT=%%websiteUsername%%\n")
                .Append(@"TAG POS=1 TYPE=INPUT:PASSWORD FORM=NAME:* ATTR=NAME:password CONTENT=%%websitePassword%%\nTAG POS=1 TYPE=INPUT:BUTTON FORM=NAME:* ATTR=NAME:btnLogIn\n")
                .Append(@"SET !TIMEOUT_STEP 2\nTAG POS=1 TYPE=DIV ATTR=ID:SignInErrorInfo EXTRACT=TXT\n"); ;
        }
        public static StringBuilder GoToServiceReviewPage()
        {
            return new StringBuilder()
                .Append(@"SET !TIMEOUT_STEP 5\nTAG POS=1 TYPE=A ATTR=TXT:New<SP>Review\nFRAME NAME=patmain\n")
                .Append(@"TAG POS=1 TYPE=SELECT FORM=NAME:payer ATTR=NAME:payerlist CONTENT=$]]EmdeonInsuranceName]]\nWAIT SECONDS=1\n");
        }
        public static StringBuilder EnterPatientDemographics()
        {
            return new StringBuilder()
                .Append(@"SET !TIMEOUT_STEP 5\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:F10 CONTENT=%%MemberID%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:F4 CONTENT=%%PatFname%%\n")
                .Append(@"TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:F13 CONTENT=%%PatDOB%%\n");
        }
    }
}
