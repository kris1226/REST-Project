using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.ScriptHelpers
{
    public static class NaviNet
    {
        public static StringBuilder LoginScript(string browserVersion)
        {
            return new StringBuilder()
                .AppendFormat(@"SET !USERAGENT {0}\nSET !TIMEOUT_STEP 5\n", browserVersion)
                .Append(@"URL GOTO %%websiteDomain%%\nWAIT SECONDS=1\nTAG POS=1 TYPE=INPUT:TEXT ATTR=NAME:*Username CONTENT=%%websiteUsername%%\n")
                .Append(@"TAG POS=1 TYPE=INPUT:PASSWORD ATTR=NAME:*Password CONTENT=%%websitePassword%%\nTAG POS=1 TYPE=INPUT:SUBMIT ATTR=ID:btnSignInSubmit\n")
                .Append(@"SET !TIMEOUT_STEP 2\nTAG POS=1 TYPE=DIV ATTR=ID:SignInErrorInfo EXTRACT=TXT\n");            ;
        }
        public static StringBuilder Pause()
        {
            return new StringBuilder()
                .Append("@PAUSESUBMIT|SET !TIMEOUT_STEP 2\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n");
        }
        public static StringBuilder PauseOnError()
        {
            return new StringBuilder()
                .Append(@"PAUSEERR|SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n");
        }
        public static class HorizonNJ
        {
            public static StringBuilder GotoHorizonSubmitPage()
            {
                return new StringBuilder()
                    .Append(@"SET !TIMEOUT_STEP 12\nTAG POS=1 TYPE=A ATTR=TXT:Horizon<SP>NJ<SP>Health\n")
                    .Append(@"TAG POS=1 TYPE=A ATTR=TXT:Utilization<SP>Management<SP>Requests\nFRAME NAME=appContentFrame\nTAG POS=1 TYPE=DIV ATTR=CLASS:linkIconAuth\n")
                    .Append(@"TAG POS=1 TYPE=DIV ATTR=CLASS:lookupIcon\nWAIT SECONDS=2\nTAG POS=1 TYPE=SELECT ATTR=DATA-ID:otherIdType CONTENT=%SUBSID\n")
                    .Append(@"TAG POS=1 TYPE=INPUT:TEXT ATTR=DATA-ID:otherIdentifier CONTENT=%%MemberID%%\nTAG POS=1 TYPE=INPUT:TEXT ATTR=DATA-ID:lastName CONTENT=%%PatLname%%\n")
                    .Append(@"TAG POS=1 TYPE=INPUT:TEXT ATTR=DATA-ID:firstName CONTENT=%%PatFname%%\nTAG POS=1 TYPE=INPUT:TEXT ATTR=data-id:dateOfBirth CONTENT=\n%%PatDOB%%\nDS CMD=CLICK X=561 Y=518 CONTENT=\n")
                    .Append(@"TAG POS=1 TYPE=A ATTR=TXT:Auth<SP>Submission\nTAG POS=1 TYPE=A ATTR=TXT:GeneralServices\nWAIT SECONDS=1\n")
                    .Append(@"WAIT SECONDS=1\nTAG POS=1 TYPE=BUTTON:SUBMIT ATTR=TXT:Search\n");
            }
        }
    }
}
