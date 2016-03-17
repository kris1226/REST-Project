using iAgentDataTool.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInquiry
{
    public static class TufsNetworkHealth
    {
        public static StringBuilder Login_Script(InternetExplorer browserVersion)
        {
            return new StringBuilder()
                .AppendFormat(@"SET !USERAGENT {0}\nSET !TIMEOUT_STEP 3\n", browserVersion.Ten)
                .Append(@"URL GOTO %%websiteDomain%%\nWAIT SECONDS=1\nTAG POS=1 TYPE=INPUT:TEXT FORM=* ATTR=NAME:username CONTENT=%%websiteUsername%%\n")
                .Append(@"TAG POS=1 TYPE=INPUT:PASSWORD FORM=ACTION:* ATTR=NAME:password CONTENT=%%websitePassword%%\nTAG POS=1 TYPE=BUTTON:SUBMIT FORM=ACTION:* ATTR=TXT:Sign<SP>In\n")
                .Append(@"TAG POS=1 TYPE=LABEL FORM=ACTION:* ATTR=TXT:User<SP>ID EXTRACT=TXT\n");
        }
        public static StringBuilder Goto_Referrals_Auhorizations_PatientSearch_Page()
        {
            return new StringBuilder()
                .Append(@"SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=A ATTR=TXT:Referrals<SP>&<SP>Authorizations\nTAG POS=2 TYPE=INPUT:RADIO FORM=NAME:* ATTR=NAME:searchby CONTENT=YES\nTAG POS=1 TYPE=INPUT:TEXT FORM=NAME:* ATTR=NAME:membertext CONTENT=%%MemberID%%\n")
                .Append(@"TAG POS=1 TYPE=SELECT FORM=NAME:* ATTR=NAME:d_sortby CONTENT=%member_last_name,<SP>member_first_name\nTAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:* ATTR=NAME:c_sortdirection CONTENT=NO\n")
                .Append(@"TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:* ATTR=NAME:c_sortdirection CONTENT=NO\nTAG POS=1 TYPE=SELECT FORM=NAME:* ATTR=NAME:d_sortby CONTENT=%effective_from_date\n")
                .Append(@"TAG POS=1 TYPE=SELECT FORM=NAME:* ATTR=NAME:d_sortby CONTENT=%effective_from_date\nTAG POS=1 TYPE=INPUT:SUBMIT FORM=NAME:* ATTR=NAME:submit_ref\nWAIT SECONDS=5\n");
        }
    }
}
