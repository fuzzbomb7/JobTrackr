using JobTrackrApi.Data;
using JobTrackrApi.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobTrackrApi
{
	public class ApplicationEndpoints
	{
		public static void Map(WebApplication app)
		{
			// Get all applications
			app.MapGet("/Applications", async (ClaimsPrincipal user, [FromServices] ApplyTrackContext cosmos) =>
			{
				var userId = user.Identity?.Name;

				var applications = await cosmos.GetApplicationsAsync(userId!);

				// Sort status history by date, then sort applications by last status date
				applications.ForEach(applications => applications.statusHistory = applications.statusHistory.OrderBy(o => o.date).ToList());
				applications = applications.OrderBy(a => a.statusHistory.Last().date).ToList();

				TypeAdapterConfig<StatusHistory, StatusHistoryModel>.NewConfig()
					.Map(dest => dest.date, src => src.date.ToString("yyyy-MM-dd"));

				TypeAdapterConfig<JobApplication, JobApplicationModel>.NewConfig()
					.Map(dest => dest.statusHistory, src => src.statusHistory.Adapt<List<StatusHistoryModel>>());

				var applicationsModel = applications.Adapt<List<JobApplicationModel>>();

				return TypedResults.Ok(applicationsModel);
			}).RequireAuthorization();


			// Add application
			app.MapPost("/Application", async (AddJobApplicationModel application, ClaimsPrincipal user,
				[FromServices] ApplyTrackContext cosmos) =>
			{
				var userId = user.Identity?.Name;

				try
				{
					var jobApplication = application.Adapt<JobApplication>();

					jobApplication.id = Guid.NewGuid();
					jobApplication.userId = userId!;
					jobApplication.statusHistory.Add(new StatusHistory { status = ApplicationStatus.Applied, date = DateTime.Parse(application.applicationDate) });

					await cosmos.AddApplicationAsync(jobApplication);
				}
				catch (Exception)
				{
					return TypedResults.Ok(false);
				}
				return TypedResults.Ok(true);
				
			}).RequireAuthorization();


			// Add application status
			app.MapPatch("/Application/{id}/{status}", async (Guid id, string status, string? date,
				ClaimsPrincipal user, [FromServices] ApplyTrackContext cosmos) =>
			{
				var userId = user.Identity?.Name;

				try
				{
					DateTime statusDate = date is not null ? DateTime.Parse(date).AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute) : DateTime.Now;
					await cosmos.UpdateApplicationStatusAsync(id, userId!, status, statusDate);
				}
				catch (Exception)
				{
					return TypedResults.Ok(false);
				}
				return TypedResults.Ok(true);
			}).RequireAuthorization();


			// Delete application
			app.MapDelete("/Application/{id}", async (Guid id, ClaimsPrincipal user, [FromServices] ApplyTrackContext cosmos) =>
			{
				var userId = user.Identity?.Name;

				try
				{
					await cosmos.DeleteApplicationAsync(id, userId!);
				}
				catch (Exception)
				{
					return TypedResults.Ok(false);
				}
				return TypedResults.Ok(true);
			}).RequireAuthorization();


			// Get report
			app.MapGet("/Report", async (ClaimsPrincipal user, [FromServices] ApplyTrackContext cosmos) =>
			{
				var report = new ReportModel();
				var userId = user.Identity?.Name;

				try
				{
					var applications = await cosmos.GetApplicationsAsync(userId!);

					report.Total = applications.Count;
					report.Applied = applications.Count(a => a.statusHistory.OrderBy(o => o.date).Last().status == ApplicationStatus.Applied);
					report.Rejected = applications.Count(a => a.statusHistory.OrderBy(o => o.date).Last().status == ApplicationStatus.Rejected);
					report.Screened = applications.Count(a => a.statusHistory.OrderBy(o => o.date).Last().status == ApplicationStatus.PhoneScreen);
					report.Interviewing = applications.Count(a => a.statusHistory.OrderBy(o => o.date).Last().status == ApplicationStatus.Interviewing);
					report.NoOffer = applications.Count(a => a.statusHistory.OrderBy(o => o.date).Last().status == ApplicationStatus.NoOffer);
					report.Offer = applications.Count(a => a.statusHistory.OrderBy(o => o.date).Last().status == ApplicationStatus.Offer);
					report.Accepted = applications.Count(a => a.statusHistory.OrderBy(o => o.date).Last().status == ApplicationStatus.Accepted);
				}
				catch (Exception)
				{
					return TypedResults.Ok(report);
				}

				report.Success = true;
				return TypedResults.Ok(report);
			}).RequireAuthorization();
		}
	}
}
