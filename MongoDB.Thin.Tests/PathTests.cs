using FluentAssertions;
using Xunit;

namespace MongoDB.Thin.Tests
{
    public class PathTests
    {
        private class Document
        {
            public string Field { get; set; }
            public Document[] SubDocuments { get; set; }
        }

        [Fact]
        public void When_getting_path_for_document_field()
        {
            var path = Path.From<Document, string>(d => d.Field);
            path.Should().BeEquivalentTo("Field");
        }

        [Fact]
        public void When_getting_path_for_field_in_array()
        {
            var path = Path.From<Document, Document>(d => d.SubDocuments, d => d.Field);
            path.Should().BeEquivalentTo("SubDocuments.Field");
        }
    }
}
