using Microsoft.Azure.Cosmos;

namespace JobTrackrApi.Data
{
	public class ApplyTrackContext
	{
		private readonly CosmosClient cosmos;
		private readonly Container container;

		public ApplyTrackContext(CosmosClient cosmos)
		{
			this.cosmos = cosmos;
			this.container = cosmos.GetDatabase("ApplyTrack").GetContainer("Applications");
		}

		public async Task<List<JobApplication>> GetApplicationsAsync(string userId)
		{
			var query = new QueryDefinition("SELECT * FROM c WHERE c.userId = @userId")
				.WithParameter("@userId", userId);
			var iterator = this.container.GetItemQueryIterator<JobApplication>(query);
			var applications = new List<JobApplication>();
			while (iterator.HasMoreResults)
			{
				var response = await iterator.ReadNextAsync();
				applications.AddRange(response);
			}
			return applications;
		}

		public async Task AddApplicationAsync(JobApplication application)
		{
			var response = await this.container.CreateItemAsync(application);
			Console.WriteLine(response.Resource);
		}

		public async Task UpdateApplicationStatusAsync(Guid id, string userId, string status, DateTime date)
		{
			var application = await this.container.ReadItemAsync<JobApplication>(id.ToString(), new PartitionKey(userId));
			application.Resource.statusHistory.Add(new StatusHistory { status = status, date = date });
			await this.container.ReplaceItemAsync(application.Resource, id.ToString());
		}

		public async Task DeleteApplicationAsync(Guid id, string userId)
		{
			await this.container.DeleteItemAsync<JobApplication>(id.ToString(), new PartitionKey(userId));
		}
	}
}
