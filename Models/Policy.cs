using System;
using System.Collections.Generic;

#nullable disable

namespace TraxHrPolicy.Models
{
    public partial class Policy
    {
        public long Id { get; set; }
        public string Id1 { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Icon { get; set; }
        public bool? Published { get; set; }
        public bool Quiz { get; set; }
        public long? CategoryId { get; set; }
        public string Type { get; set; }
        public string ChangeType { get; set; }
        public string ContentType { get; set; }
        public string Version { get; set; }
        public int TotalVersions { get; set; }
        public string ParentType { get; set; }
        public long? Parent { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
    }
}
