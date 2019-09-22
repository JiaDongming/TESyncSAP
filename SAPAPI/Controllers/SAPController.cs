using SAPSyncAPI;
using SAPSyncdll;
using SAPSyncdll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SAPAPI.Controllers
{
    public class SAPController : ApiController
    {

        static List<ReturnMessage> SyncProject2SAP(ProjectBinder projectBinder, DelegateSyncSAP delegateSyncSAP)
        {
            return delegateSyncSAP(projectBinder);
        }


        [HttpGet]

        public IHttpActionResult SyncProject(int projectId, int spaceId)
        {
            DelegateHandler delegateHandler = new DelegateHandler();
            ProjectBinder projectBinder = new ProjectBinder()
            {
                ProjectID = projectId,
                SpaceID = spaceId
            };
            return Ok<List<ReturnMessage>>(SyncProject2SAP(projectBinder, delegateHandler.SyncProjectInfo));
        }

    }
}
