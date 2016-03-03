using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers
{
    public abstract class SubmitScript
    {
        public StringBuilder Pause()
        {
            return new StringBuilder()
                .Append("@PAUSESUBMIT|SET !TIMEOUT_STEP 2\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n");
        }
        public StringBuilder PauseOnError()
        {
            return new StringBuilder()
                .Append(@"PAUSEERR|SET !TIMEOUT_STEP 3\nTAG POS=1 TYPE=HTML ATTR=* EXTRACT=HTM\nSAVEAS TYPE=PNG FOLDER=* FILE=*\n");
        }
    }
}
