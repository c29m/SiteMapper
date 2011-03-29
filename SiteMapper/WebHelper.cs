using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SiteMapper
{
    class WebHelper
    {
        // based on http://dotnetperls.com/title-html

        public string GetTitleFromHtmlFile(string fileName)
        {
            string html="";

            html = File.ReadAllText(fileName);

            return GetTitle(html);
        }

        private string GetTitle(string html)
        {

            Match m = Regex.Match(html, @"<title>\s*(.+?)\s*</title>");
            if (m.Success)
            {
                return m.Groups[1].Value.Trim();
            }
            else
            {
                return "";
            }
        }


        //http://codeasp.net/blogs/teisenhauer/microsoft-net/170/parse-meta-tags-in-c-sharp
        private string GetDescription(string html)
        {

            Match DescriptionMatch = Regex.Match(html, "<meta name=\"description\" content=\"([^<]*)\">", RegexOptions.IgnoreCase | RegexOptions.Multiline);


            return "";
        }

        String[] GetTitleDescFromHtmlFile(string fileName)
        {
            string html = "";
            string title = "";
            string description = "";

            html = File.ReadAllText(fileName);

            title = GetTitle(html);
            description = GetDescription(html);

            String[] rs = { title, description };

            return rs;
        }
        
    }
}
