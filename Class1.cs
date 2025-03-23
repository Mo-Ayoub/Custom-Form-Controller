using System;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Forms.Core.Services;

namespace MyCustomLibrary.Controllers
{
    public class FormsController : SurfaceController
    {
        private readonly IRecordReaderService _recordReaderService;

        public FormsController(IRecordReaderService recordReaderService)
        {
            _recordReaderService = recordReaderService;
        }


        public ActionResult GetFormRecords(Guid formId)
        {
            if (formId == Guid.Empty)
            {
                return Content("Invalid form ID.");
            }

            var records = _recordReaderService.GetRecordsFromForm(formId, 1, 10);

            if (!records.Items.Any())
            {
                return Content("No records found.");
            }

            var formattedRecords = records.Items.Select(record => new
            {
                record.Id,
                record.Created,
                Fields = record.RecordFields.ToDictionary(k => k.Key.ToString(), v => v.Value)
            });

            return Json(formattedRecords, JsonRequestBehavior.AllowGet);
        }
    }
}
