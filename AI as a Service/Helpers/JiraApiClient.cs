using static AI_as_a_Service.Helpers.JiraApiClient.JiraSearchResult;

namespace AI_as_a_Service.Helpers
{
    public class JiraApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://your-domain.atlassian.net/rest/api/3/";

        public JiraApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JiraSearchResult> SearchIssuesAsync(string jql, int startAt = 0, int maxResults = 50, string fields = "*all")
        {
            var response = await _httpClient.GetAsync($"search?jql={Uri.EscapeDataString(jql)}&startAt={startAt}&maxResults={maxResults}&fields={fields}");
            response.EnsureSuccessStatusCode();

            var searchResult = await response.Content.ReadFromJsonAsync<JiraSearchResult>();
            return searchResult;
        }

        public async Task<JiraIssue> CreateIssueAsync(string projectKey, string summary, string issueType, string description)
        {
            var issueData = new
            {
                fields = new
                {
                    project = new { key = projectKey },
                    summary = summary,
                    issuetype = new { name = issueType },
                    description = new { type = "doc", version = 1, content = new[] { new { type = "paragraph", content = new[] { new { type = "text", text = description } } } } }
                }
            };

            var response = await _httpClient.PostAsJsonAsync("issue", issueData);
            response.EnsureSuccessStatusCode();

            var createdIssue = await response.Content.ReadFromJsonAsync<JiraIssue>();
            return createdIssue;
        }


        public class JiraSearchResult
        {
            public int StartAt { get; set; }
            public int MaxResults { get; set; }
            public int Total { get; set; }
            public List<JiraIssue> Issues { get; set; }

            public class JiraIssue
            {
                public string Id { get; set; }
                public string Key { get; set; }
                public JiraIssueFields Fields { get; set; }
            }

            public class JiraIssueFields
            {
                public string Summary { get; set; }
                public JiraIssueType IssueType { get; set; }
                public JiraIssueStatus Status { get; set; }
            }

            public class JiraIssueType
            {
                public string Id { get; set; }
                public string Name { get; set; }
            }

            public class JiraIssueStatus
            {
                public string Id { get; set; }
                public string Name { get; set; }
            }
        }

    }
}
