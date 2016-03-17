using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptDataHelpers
{
    public static class RemixCollectionItems
    {
        public static Dictionary<string, Guid> GetCollectionItemKeys()
        {
            return new Dictionary<string, Guid>()
            {
                {"%%websiteDomain%%", new Guid("96F51FD2-6539-49BD-A1C8-1F8DDE73CE1E")},
                {"%%websiteUsername%%", new Guid("D8BB9D68-DC56-E011-B21D-001E4F27A50B")},
                {"%%websitePassword%%", new Guid("D9BB9D68-DC56-E011-B21D-001E4F27A50B")},
                {"%%MemberID%%", new Guid("AAC881E8-ECB6-4B9B-835E-A56C89E5973F")},
                {"%%FromDate%%", new Guid("3A9C11D3-782B-4894-AD51-06B6BD3B282B")},
                {"%%AdmitDate%%", new Guid("9050D851-8C24-42AE-9FD4-D8029A0927DD")},
                {"%%PatLname%%", new Guid("CD1DAB7C-4F7A-493B-B68A-71AAE648FD61")},
                {"%%PatFname%%", new Guid("1169AEEF-AFD2-4860-81B3-665B302736F1")},
                {"%%PatDOB%%", new Guid("E020CDF7-8E16-4AB1-A990-CCDE749E3071")}
            };
        }
    }
}
