// Trong file Helpers/VideoHelper.cs
using System.Text.RegularExpressions;
using System.Web;

public static class VideoHelper
{
    public static IHtmlString EmbedYouTubeVideo(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        var regex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})");
        var match = regex.Match(url);

        if (match.Success)
        {
            var videoId = match.Groups[1].Value;
            var iframe = $"<div class='embed-responsive embed-responsive-16by9'><iframe class='embed-responsive-item' src='https://www.youtube.com/embed/{videoId}' allowfullscreen></iframe></div>";
            return new HtmlString(iframe);
        }

        return null;
    }
}