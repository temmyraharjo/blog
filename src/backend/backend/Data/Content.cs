using System.Collections.Generic;

namespace backend.Data
{
    public class Content : Entity
    {
        public string Title { get; set; }
        public string TitleEncode { get; set; }
        public string Body { get; set; }
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
    }
}