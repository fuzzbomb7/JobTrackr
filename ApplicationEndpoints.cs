using JobTrackrApi.Model;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
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
			app.MapGet("/Applications", async (JobTrackrContext db, ClaimsPrincipal user) =>
			{
				var userId = user.Identity?.Name;

				var applications = await db.JobApplications.Where(a => a.UserId == userId)
					.Include(a => a.StatusHistories).ToListAsync();

				// Sort status history by date, then sort applications by last status date
				applications.ForEach(applications => applications.StatusHistories = applications.StatusHistories.OrderBy(o => o.Date).ToList());
				applications = applications.OrderBy(a => a.StatusHistories.Last().Date).ToList();

				TypeAdapterConfig<StatusHistory, StatusHistoryModel>.NewConfig()
					.Map(dest => dest.Status, src => src.StatusId)
					.Map(dest => dest.Date, src => src.Date.ToString("yyyy-MM-dd"));

				TypeAdapterConfig<JobApplication, JobApplicationModel>.NewConfig()
					.Map(dest => dest.StatusHistory, src => src.StatusHistories.Adapt<List<StatusHistoryModel>>());

				var applicationsModel = applications.Adapt<List<JobApplicationModel>>();

				return TypedResults.Ok(applicationsModel);
			}).RequireAuthorization();

			// Add application
			app.MapPost("/Application", async (AddJobApplicationModel application, [FromServices] JobTrackrContext db, ClaimsPrincipal user) =>
			{
				var userId = user.Identity?.Name;
				application.UserId = userId!;

				try
				{
					var jobApplication = application.Adapt<JobApplication>();
					jobApplication.StatusHistories.Add(new StatusHistory { StatusId = ApplicationStatus.Applied, Date = DateTime.Parse(application.ApplicationDate) });
					db.JobApplications.Add(jobApplication);
					await db.SaveChangesAsync();
				}
				catch (Exception)
				{
					return TypedResults.Ok(false);
				}
				return TypedResults.Ok(true);
				
			}).RequireAuthorization();

			// Add application status
			app.MapPatch("/Application/{id}/{status}", async (int id, string status, string? date, JobTrackrContext db, ClaimsPrincipal user) =>
			{
				var userId = user.Identity?.Name;

				try
				{
					var application = await db.JobApplications.FindAsync(id);
					if (application is null || application.UserId != userId) return Results.NotFound();

					DateTime statusDate = date is not null ? DateTime.Parse(date).AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute) : DateTime.Now;
					application!.StatusHistories.Add(new StatusHistory { StatusId = status!, Date = statusDate });
					await db.SaveChangesAsync();
				}
				catch (Exception)
				{
					return TypedResults.Ok(false);
				}
				return TypedResults.Ok(true);
			}).RequireAuthorization();

			// Delete application
			app.MapDelete("/Application/{id}", async (int id, JobTrackrContext db, ClaimsPrincipal user) =>
			{
				var userId = user.Identity?.Name;

				try
				{
					var application = await db.JobApplications.FindAsync(id);
					if (application is null || application.UserId != userId) return Results.NotFound();

					db.JobApplications.Remove(application!);
					await db.SaveChangesAsync();
				}
				catch (Exception)
				{
					return TypedResults.Ok(false);
				}
				return TypedResults.Ok(true);
			}).RequireAuthorization();

			// Get report
			app.MapGet("/Report", async (JobTrackrContext db, ClaimsPrincipal user) =>
			{
				var report = new ReportModel();
				var userId = user.Identity?.Name;

				try
				{
					var applications = await db.JobApplications.Where(a => a.UserId == userId)
					.Include(a => a.StatusHistories).ToListAsync();

					report.Total = applications.Count;
					report.Applied = applications.Count(a => a.StatusHistories.OrderBy(o => o.Date).Last().StatusId == ApplicationStatus.Applied);
					report.Rejected = applications.Count(a => a.StatusHistories.OrderBy(o => o.Date).Last().StatusId == ApplicationStatus.Rejected);
					report.Screened = applications.Count(a => a.StatusHistories.OrderBy(o => o.Date).Last().StatusId == ApplicationStatus.PhoneScreen);
					report.Interviewing = applications.Count(a => a.StatusHistories.OrderBy(o => o.Date).Last().StatusId == ApplicationStatus.Interviewing);
					report.NoOffer = applications.Count(a => a.StatusHistories.OrderBy(o => o.Date).Last().StatusId == ApplicationStatus.NoOffer);
					report.Offer = applications.Count(a => a.StatusHistories.OrderBy(o => o.Date).Last().StatusId == ApplicationStatus.Offer);
					report.Accepted = applications.Count(a => a.StatusHistories.OrderBy(o => o.Date).Last().StatusId == ApplicationStatus.Accepted);
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
