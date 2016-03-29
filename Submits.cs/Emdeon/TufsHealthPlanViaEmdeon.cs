using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submits
{
    public static class TufsHealthPlanViaEmdeon
    {
        private static readonly Guid _websiteKey = new Guid("6e072d7e-523a-e211-b35b-000c29729dff");
        private static string _websiteDescription = "Tufs via Emdeon Submit 00";
        private static readonly string _deviceId = "Tufs";

        public static Script Login_Script()
        {
            return Script.CreateScript(
                websiteDescription: string.Concat(_websiteDescription, 1, "-- Loginscript, login error check"),
                code: new StringBuilder()
                    .Append(@"SET !USERAGENT ""MSIE 10.0""\nSET !TIMEOUT_STEP 3\nURL GOTO %%websiteDomain%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:username CONTENT=%%websiteUsername%%\n")
                    .Append(@"TAG POS=1 TYPE=INPUT:PASSWORD FORM=NAME:* ATTR=NAME:password CONTENT=%%websitePassword%%\nTAG POS=1 TYPE=INPUT:BUTTON FORM=NAME:* ATTR=NAME:btnLogIn\n")
                    .Append(@"TAG POS=1 TYPE=FONT FORM=NAME:Login ATTR=TXT:User<SP>ID EXTRACT=TXT\n")
                    .ToString(),
                deviceId: _deviceId,
                category: "Login",
                websiteKey: _websiteKey
            );
        }
        public static Script GotoRefferalsPage(int scriptOrder)
        {
            return Script.CreateScript(
                websiteDescription: string.Concat(_websiteDescription, scriptOrder, "-- New Review, patient demographics"),
                code: new StringBuilder()
                    .Append(@"TAG POS=1 TYPE=A ATTR=TXT:New<SP>Review\nFRAME NAME=patmain\nTAG POS=1 TYPE=SELECT FORM=NAME:payer ATTR=NAME:payerlist CONTENT=$Tufts<SP>Health<SP>Plan\n")
                    .Append(@"TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:F2 CONTENT=%%MemberID%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:F3 CONTENT=%%PatLname%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:F4 CONTENT=%%PatFname%%\n")
                    .Append(@"TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:F13 CONTENT=%%PatDOB%%\n")
                    .ToString(),
                deviceId: string.Concat(_deviceId, scriptOrder),
                category: "PatientSearch",
                websiteKey: _websiteKey
            );
        }
        public static Script LogOutScript(int scriptOrder)
        {
            return Script.CreateScript(
                websiteDescription: string.Concat(_websiteDescription, 3, "-- Logout Script"),
                code: new StringBuilder()
                    .Append(@"SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=IMG ATTR=SRC:https://office.emdeon.com/images/Exit.gif\n")
                    .ToString(),
                deviceId: string.Concat(_deviceId, scriptOrder),
                category: "Logout",
                websiteKey: _websiteKey
            );
        }
        public static Script PauseScript(int scriptOrder)
        {
            return Script.CreateScript(
                websiteDescription: string.Concat(_websiteDescription, scriptOrder, ": Pause"),
                code: new StringBuilder()
                    .Append(@"PAUSESUBMIT|SET !TIMEOUT_STEP 2\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n")
                    .ToString(),
                deviceId: string.Concat(_deviceId, scriptOrder),
                category: "Extraction",
                websiteKey: _websiteKey
            );
        }
        public static Script PauseErrorScript(int scriptOrder)
        {
            return Script.CreateScript(
                websiteDescription: string.Concat(_websiteDescription, scriptOrder, ": Pause error"),
                code: new StringBuilder()
                    .Append(@"PAUSEERR|SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n")
                    .ToString(),
                deviceId: string.Concat(_deviceId, scriptOrder),
                category: "Extraction",
                websiteKey: _websiteKey
            );
        }     
    }
}
