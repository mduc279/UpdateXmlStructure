using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace UpdateXmlStruct
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dir = "D:\\AOD Projects\\DSS\\Aod.Dss.WebManager\\App_Data\\TemplateVersions\\";
            var xmlFiles = Directory.GetFiles(dir, "*.xml");

            var listXPathToUpdate = new List<string> {
                "//Source",
                "//AvailableIcon",
                "//OccupiedIcon",
                "//StartShortlyIcon",
                "//SelectedIndicator",
                "//UnselectedIndicator"
            };

            var listPlatforms = new List<string> { "iOS", "Android", "Windows" };

            foreach (var item in xmlFiles)
            {
                Console.WriteLine($"Updating {item.Split('/').Last()}");
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(File.ReadAllText(item));

                    //Customizable part

                    IEnumerable<XmlNode> nodeList = new List<XmlNode>();
                    foreach (var i in listXPathToUpdate)
                    {
                        var textNodes = xmlDoc.SelectNodes(i);
                        nodeList = nodeList.Concat(textNodes.Cast<XmlNode>());
                    }
                    foreach (var node in nodeList)
                    {
                        node.InnerText = string.Empty;
                        foreach (var attr in listPlatforms)
                        {
                            node.Attributes[attr].Value = string.Empty;
                        }
                    }
                    var outputText = XDocument.Parse(xmlDoc.OuterXml).ToString();
                    File.WriteAllText(item, outputText);
                }
                catch (Exception) { }
                Console.WriteLine($"Finished {item.Split('/').Last()}");
            }
        }
    }
}
