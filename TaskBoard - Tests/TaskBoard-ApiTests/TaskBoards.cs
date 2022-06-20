using System.Text.Json.Serialization;

namespace TaskBoard.APITests
{
    public class TaskBoards
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }
    }
}