namespace Bunker.Business.Interfaces.Models
{
    public class TaskResponse
    {
        public int    Id          { get; set; }
        public string Name        { get; set; }
        public string Description { get; set; }
        public int    Score       { get; set; }
        public string Answer      { get; set; }
    }
}