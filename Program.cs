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
            var destinationDir = "C:\\Users\\DucDoanMinh\\OneDrive - Add-On Products & Add-On Development\\Desktop\\tempTemplate";
            var xmlFiles = Directory.GetFiles(dir, "*.xml");

            var listXPathToEmpty = new List<string> {
                "//Source",
                "//AvailableIcon",
                "//OccupiedIcon",
                "//StartShortlyIcon",
                "//SelectedIndicator",
                "//UnselectedIndicator"
            };

            var listXPathToDisable = new List<string>
            {
                "//IsVisible",
                //"//IsEnabled",
                //"//IsEnabledShowFromCurrent"
            };

            var listPlatforms = new List<string> { "iOS", "Android", "Windows" };
            var updateValueEmpty = string.Empty;
            var updateValueDisable = "false";

            foreach (var item in xmlFiles)
            {
                var fileName = item.Split('/').Last();
                Console.WriteLine($"Updating {fileName}");
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(File.ReadAllText(item));

                    //Customizable part

                    IEnumerable<XmlNode> nodeList = new List<XmlNode>();
                    foreach (var i in listXPathToEmpty)
                    {
                        var textNodes = xmlDoc.SelectNodes(i);
                        nodeList = nodeList.Concat(textNodes.Cast<XmlNode>());
                    }
                    foreach (var node in nodeList)
                    {
                        node.InnerText = updateValueEmpty;
                        foreach (var attr in listPlatforms)
                        {
                            node.Attributes[attr].Value = string.Empty;
                        }
                    }

                    foreach (var i in listXPathToDisable)
                    {
                        var textNodes = xmlDoc.SelectNodes(i);
                        nodeList = nodeList.Concat(textNodes.Cast<XmlNode>());
                    }
                    foreach (var node in nodeList)
                    {
                        node.InnerText = updateValueDisable;
                        foreach (var attr in listPlatforms)
                        {
                            node.Attributes[attr].Value = string.Empty;
                        }
                    }
                    var outputText = XDocument.Parse(xmlDoc.OuterXml).ToString();
                    var destination = Path.Combine(destinationDir, fileName);
                    File.WriteAllText(destination, outputText);
                }
                catch (Exception) { }
                Console.WriteLine($"Finished {fileName}");
            }
        }
    }
}
