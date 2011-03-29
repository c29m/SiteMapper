using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Linq;

namespace SiteMapper
{
    class DirectoryParser
    {
        private String siteRoot;

        String[] includeFiles;
        String[] excludeDirectories;
        String[] excludeFiles;

        string folderSeparator = "\\";

        string nodeName = "siteMapNode";
        XNamespace ns = "http://schemas.microsoft.com/AspNet/SiteMap-File-1.0";
        XDocument xdoc = new XDocument(
            new XDeclaration("1.0","utf-8","yes"),
            new XComment("Created: " + DateTime.Now.ToString()));
        XElement siteMap;

		
        public XDocument getXmlSiteMap(String siteRoot, String[] includeFiles, String[] excludeDirectories, String[] excludeFiles)
        {
            this.includeFiles = includeFiles;
            this.excludeDirectories = excludeDirectories;
            this.excludeFiles = excludeFiles;
            this.siteRoot = siteRoot;
            
            siteMap = new XElement(ns + "siteMap");
            siteMap.Add(createSiteMap(siteRoot));          
            xdoc.Add(siteMap);

            return xdoc;
        }


        private XElement createSiteMap(string sDir)
        {
            Boolean addDir = true;
            XElement pNode = createSiteMapNode(ns, replaceWithVirtualPath(sDir), getDirTitle(sDir), "");

           foreach (XElement child in getChildNodes(sDir))
           {
               pNode.Add(child);
           }

           foreach (string d in Directory.GetDirectories(sDir))
           {
               foreach (string dfilter in excludeDirectories)
               {
                   addDir = true;

                   if (d.EndsWith(folderSeparator + dfilter))
                   {
                       addDir = false;
                       break;
                   }
               }
               
               if (addDir)
               {
                   pNode.Add(createSiteMap(d));
               }
           }
           return pNode;
       }
        
       
        private String replaceWithVirtualPath(String path)
        {
            return path.Replace(this.siteRoot, "~").Replace("\\", "/");

        }

		
        private XElement[] getChildNodes(string sDir)
        {
           Boolean addFile = true;
           FileInfo finfo;
           ArrayList childNodes = new ArrayList();
           WebHelper wh = new WebHelper();

           foreach (string f in Directory.GetFiles(sDir, "*.*"))
           {
               addFile = true;
               finfo = new FileInfo(f);
               foreach (string includeExt in includeFiles)
               {
                   if (finfo.Extension == includeExt)
                   {
                       addFile = true;
                       break;
                   }
                   else
                   {
                       addFile = false;
                   }

               }
               if (addFile)
               {
                   foreach (string filter in excludeFiles)
                   {
                       if (f.Contains(filter))
                       {
                           addFile = false;
                           break;
                       }
                   }

                   if (addFile)
                   {
                       string fileTitle = wh.GetTitleFromHtmlFile(f);
                       childNodes.Add(createSiteMapNode(ns, replaceWithVirtualPath(f), fileTitle, ""));
                   }
               }
           }

           return (XElement[])childNodes.ToArray(typeof(XElement));
        }

		
        private string getDirTitle(string dir)
        {
            string defaultDoc = "Default.aspx";
            string defaultHtml = "Index.html";
            WebHelper wh = new WebHelper();
            string title = "";

            if (File.Exists(dir + folderSeparator + defaultDoc))
            {
                title = wh.GetTitleFromHtmlFile(dir + folderSeparator + defaultDoc);
            }
            else if (File.Exists(dir + folderSeparator + defaultHtml))
            {
                title = wh.GetTitleFromHtmlFile(dir + folderSeparator + defaultHtml);
            }

            return title;

        }


        private XElement createSiteMapNode(XNamespace ns,  string url, string title, string description)
        {
           XElement node =  new XElement(ns + nodeName,
                            new XAttribute("url", url),
                            new XAttribute("title", title),
                            new XAttribute("description", description));

           return node;
        }
    }
}
