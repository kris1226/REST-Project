﻿using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.ScriptHelpers.Interfaces
{
    public interface IUpwAsyncRepository
    {
        Task<IEnumerable<Upw>> FindAutoAgentId(string clientKey, string clientLocationKey);
        Task<IEnumerable<Upw>> FindWithEntKey(string entKey);
    }
}
