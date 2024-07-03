using Hl7.Fhir.Model;
using Ihis.FhirEngine;
using Ihis.FhirEngine.Core;
using Ihis.FhirEngine.Core.Data;
using Ihis.FhirEngine.Core.Exceptions;
using Ihis.FhirEngine.Core.Models;
using Ihis.FhirEngine.Core.Search;
using System.Net;
using Task = System.Threading.Tasks.Task;

namespace DemoFhirNexusAsyncHandler.Handlers
{
    [FhirHandlerClass(AcceptedType = nameof(Appointment))]
    public class PutAppointmentHandler
    {
        private readonly IDataService<Appointment> appointmentService;
        private static readonly OperationOutcome successOperationOutcome = new()
        {
            Issue = new List<OperationOutcome.IssueComponent>
            {
                new()
                {
                    Severity = OperationOutcome.IssueSeverity.Information,
                    Code = OperationOutcome.IssueType.Informational,
                    Details = new CodeableConcept
                    {
                        Text = "All OK"
                    }
                }
            }
        };

        public PutAppointmentHandler(IDataService<Appointment> appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [FhirHandler("Sync_Put_Appointmentx", HandlerCategory.CRUD, FhirInteractionType.OperationType, CustomOperation = "put-appointment")]
        public async Task SyncPutAppointment(IFhirContext context, Appointment appointment, CancellationToken cancellationToken)
        {
            var outcome = await appointmentService.UpsertAsync(appointment, null, true, true, false, cancellationToken);
            context.SetCacheResourceId(outcome.Resource.Id, outcome.Resource.VersionId);
            context.Response.AddResource(successOperationOutcome);
            context.Response.StatusCode = HttpStatusCode.OK;
        }

        [AsyncFhirHandler("Async_Put_Appointment", HandlerCategory.PostInteraction, FhirInteractionType.OperationType, CustomOperation = "put-appointment")]
        public async Task AsyncPutAppointment(IFhirContext context, CancellationToken cancellationToken)
        {
            var appointment = context.Request.Resource as Appointment;
            // Process the appointment based on your requirement
        }
    }
}
