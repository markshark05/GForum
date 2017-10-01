using System.Web;
using System.Web.Mvc;
using MarkdownSharp;

namespace GForum.Web.Helpers
{
    /// <summary>
    /// Helper class for transforming Markdown.
    /// </summary>
    public static class MarkdownToHtmlHelper
    {
        /// <summary>
        /// Transforms a string of Markdown into HTML.
        /// </summary>
        /// <param name="markdown">The Markdown that should be transformed.</param>
        /// <returns>The HTML representation of the supplied Markdown.</returns>
        public static IHtmlString MarkdownToHtml(this HtmlHelper helper, string markdown)
        {
            var markdownTransformer = new Markdown();

            // Strip any existing html tags as we'll be returning raw html
            var htmlEncodedMarkdown = helper.Encode(markdown);

            // Generate html tags from markdown
            var html = markdownTransformer.Transform(htmlEncodedMarkdown);

            return new MvcHtmlString(html);
        }
    }
}