using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using Gdpr.Domain;
using Gdpr.UI.WebApp.Pages.Shared;
using Gdpr.UI.WebApp.Services;
using MxReturnCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gdpr.UI.WebApp.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        private readonly string _conn;
        public string URDCount { get; set; }


        public IndexModel(IConfiguration config, ILogger<IndexModel> logger) 
        {
           _config = config;
           _conn = _config.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string msgJson)
        {
            MxReturnCode<IActionResult> rc = new MxReturnCode<IActionResult>("Index.OnGetAsync()", Page());

            try
            {
                using (IAdminRepository repository = new AdminRepository(_conn))
                {
                    var resCnt = await repository.GetRoleCountAsync();
                    rc += resCnt;
                    if (rc.IsSuccess())
                    {
                        URDCount = String.Format("URD Count = {0}", resCnt.GetResult());
                        SetPageStatusMsg("Database access ok", ExistingMsg.Keep);
                        rc.SetResult(Page());
                    }
                }
            }
            catch(Exception e)
            {
                rc.SetError(3130101, MxError.Source.Exception, e.Message, MxMsgs.MxErrUnknownException, true);
            }
            if (rc.IsError())
            {
                _logger.LogError(rc.GetErrorTechMsg());
                SetPageStatusMsg(rc.GetErrorUserMsgHtml(Startup.WebAppName, WebErrorHandling.GetMxRcReportToEmailBody()), ExistingMsg.Overwrite);
            }
            return rc.GetResult();
        }
    }
}
