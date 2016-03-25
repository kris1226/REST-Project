using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Helpers;
using iAgentDataTool.Models.Common;

namespace Submits
{
    public static class MMG
    {
        private static readonly Guid _websiteKey = new Guid("86ee2f58-33f9-4dbb-8db2-a43c89da5dfc");
        private static string _websiteDescription = "Mavericks Submit 00";
        private static readonly string _deviceId = "MMG";

        public static Script Login_Script()
        {
            return Script.CreateScript(
                websiteDescription: string.Concat(_websiteDescription, 1, ": Loginscript, login error check"),
                code: new StringBuilder()
                    .Append(@"SET !USERAGENT ""MSIE 09.0""\nSET !TIMEOUT_STEP 3\nURL GOTO %%websiteDomain%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:* ATTR=NAME:txtUserName CONTENT=%%websiteUsername%%\n")
                    .Append(@"TAG POS=1 TYPE=INPUT:PASSWORD FORM=ACTION:LoginDefault.aspx ATTR=NAME:txtPassword CONTENT=%%websitePassword%%\nTAG POS=1 TYPE=INPUT:BUTTON FORM=ACTION:* ATTR=ID:imgbtnPgSubmit\n")
                    .Append(@"TAG POS=1 TYPE=DIV FORM=ACTION:* ATTR=TXT:Please<SP>log<SP>in. EXTRACT=TXT\n")
                    .ToString(),
                deviceId: _deviceId,
                category: "Login",
                websiteKey: _websiteKey
            );
        }
        public static Script GotoRefferalsPage(int scriptOrder)
        {
            return Script.CreateScript(
                websiteDescription: string.Concat(_websiteDescription, scriptOrder, ": Submit Refferal"),
                code: new StringBuilder()
                    .Append(@"SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=A ATTR=TXT:Submit<SP>Online<SP>Referrals\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:txtFilter1 CONTENT=%%MemberID%%\n")
                    .Append(@"TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:txtFilter2 CONTENT=%%PatLname%%\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:txtFilter3 CONTENT=%%PatFname%%\n")
                    .Append(@"TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:txtFilter5 CONTENT=%%PatDOB%%\n")
                    .ToString(),
                deviceId: string.Concat(_deviceId, scriptOrder),
                category: "PatientSearch",
                websiteKey: _websiteKey
            );
        }
        public static Script LogOutScript(int scriptOrder)
        {
            return Script.CreateScript(
                websiteDescription: string.Concat(_websiteDescription, 3, ": Logout Script"),
                code: new StringBuilder()
                    .Append(@"SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=A ATTR=TXT:Log<SP>out\n")
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
