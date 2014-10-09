using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

using System.Diagnostics;

using System.Windows;


using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPLASBrowse
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //           HelloWorldLabel.Text = "Hello, world!";
        }
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (LASSelector.HasFile)
            {
                try
                {
                    if (Path.GetExtension(LASSelector.FileName) != "LAS")
                    {
                        if (LASSelector.PostedFile.ContentLength < 100000000)
                        {
                            string filename = Path.GetFileName(LASSelector.FileName);
                            LASSelector.SaveAs(Server.MapPath("~/") + filename);
                            StatusLabel.Text = " is being loaded on Azure.";
                            Log inputLog = LoadLAS(Server.MapPath("~/") + filename);
                            if (inputLog == null)
                            {
                                StatusLabel.Text = LASSelector.FileName + " was not loaded, LoadLASS returned a null result.";
                            }
                            StatusLabel.Text = " LAS file loaded on Azure.";
                            D3LogDisplay display = new D3LogDisplay(inputLog);
                            //                            C3LogDisplay display = new C3LogDisplay(inputLog);
                            StatusLabel.Text = " setting up display Javascript and JSON data file.";
                            StatusLabel.Text = " sending display Javascript to client browser.";
                            LiteralControl JSLiteral = new LiteralControl(display.script);
                            DisplayCodeLocation.Controls.Add(JSLiteral);
                            StatusLabel.Text = " Display Javascript sent.";
                        }
                        else
                            StatusLabel.Text = LASSelector.FileName + " Upload error: More than 100000000 Bytes of data";
                    }
                    else
                        StatusLabel.Text = LASSelector.FileName + " Upload error: Not a LAS file.";
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = LASSelector.FileName + " Upload or processing error: " + ex.Message;
                }
            }
        }

        // Load and parse a LAS file, which only contains numerical well logs.
        // Go the quick and dirty suck it all into memory and string split approach.
        // No error handling.
        // Untested for LAS 3.x files or for contractor specific non-standard variations.

        private static Log LoadLAS(string filename)
        {
            var headerSegments = new List<LogHeaderSegment>();
            LogHeader resultHeader = new LogHeader();
            List<LogStringDatum> stringData = new List<LogStringDatum>();
            LogData resultData = new LogData();
            int logCount = 0;

            FileStream fs = File.OpenRead(filename);
            StreamReader sr = new StreamReader(fs);

            // The segments of a LAS file are separated by a tilde.
            // Each type of segment has a label.
            string data = sr.ReadToEnd();
            string[] segments = data.Split('~');

            for (int i = 1; i < segments.Length; i++)
            {
                // Preserve the header meta data as strings as the most likely way it will be useful.
                LogHeaderSegment segment = null;
                switch (segments[i][0])
                {
                    case 'A':
                        // The ASCII log data.
                        if (logCount > 0)
                        // Silently bypass if the compulsory log data segment is out of order.
                        {
                            resultData = new LogData((int)logCount, segments[i]);
                        }
                        break;

                    case 'O':
                        // The Other segment - non-delimited text format - stored as a string.
                        segment = new LogHeaderSegment(segments[i], true);
                        break;
                    case 'C':
                        // The Curve names, units, API code, description.
                        // Delimited by '.' and ':' and parsed as one LogDataQuadruple per line
                        segment = new LogHeaderSegment(segments[i], false);
                        headerSegments.Add(segment);
                        logCount = segment.data.Count;
                        break;
                    default:
                        // The Version, Parameter and Well information blocks.
                        // Delimited by '.' and ':' and parsed as one LogDataQuadruple per line
                        segment = new LogHeaderSegment(segments[i], false);
                        headerSegments.Add(segment);
                        break;
                }
            }
            resultHeader.segments = headerSegments;
            sr.Close();
            fs.Close();
            
            return new Log(resultHeader, resultData);
        }

        // Storage class for unparsed generalised LAS file segments
        public class LASSegment
        {
            public string segmentName;
            public string segmentData;
            public int quantity;

            public LASSegment()
            {
                segmentName = "";
                segmentData = null;
                quantity = 0;
            }
            public LASSegment(string segment)
            {
                segmentName = "";
                segmentData = segment;
                quantity = 0;
            }
        }

        // A well log internal representation holding metadata in the header member
        // and the log data in the data member.
        public class Log
        {
            public LogHeader header;
            public LogData data;

            public Log()
            {
                header = new LogHeader();
                data = new LogData();
            }
            public Log(LogHeader header, LogData data)
            {
                this.header = header;
                this.data = data;
            }
            public string LogToJSON(int maxlogs, int thin)
            {
                string JSONString = "{" + Environment.NewLine;
                int curveInfoIndex = 0;
                while (header.segments[curveInfoIndex].name[0] != 'C')
                {
                    curveInfoIndex++;
                }

                // Thin the data out, ensuring the thinning inputs are sensible
                if (data.sampleCount < thin) thin = data.sampleCount;
                if (maxlogs > data.logCount) maxlogs = data.logCount;
                int maxsamples = data.sampleCount - data.sampleCount % thin;
                for (int i = 0; i < maxlogs; i++)
                {
                    JSONString += "'" + header.segments[curveInfoIndex].data[i].mnemonic + "': [";
                    for (int j = 0; j < maxsamples; j += thin)
                    {
                        if (j == maxsamples - thin)
                        {
                            JSONString += data.doubleData[i][j];
                        }
                        else
                        {
                            JSONString += data.doubleData[i][j] + ", ";
                        }
                    }
                    if (i == maxlogs - 1)
                        JSONString += "]" + Environment.NewLine + Environment.NewLine;
                    else
                        JSONString += "]," + Environment.NewLine + Environment.NewLine;
                }
                JSONString += "}" + Environment.NewLine;
                return JSONString;
            }
            public string GetDepths(int thin)
            {
                int curveInfoIndex = 0;
                while (curveInfoIndex < data.logCount && (header.segments[curveInfoIndex].name[0] != 'C'))
                {
                    curveInfoIndex++;
                }

                const int depthIndex = 0;
                // The depth log should always be the first column in a LAS file ASCII data section.

                //                const string depthholder = "DEPT";
                //                while (depthIndex < data.sampleCount && !(String.Equals(header.segments[curveInfoIndex].data[depthIndex].mnemonic.ToUpperInvariant(), depthholder)))
                //                {
                //                    depthIndex++;
                //                }
                // Thin the data out, ensuring the thinning inputs are sensible
                if (data.sampleCount < thin) thin = data.sampleCount;
                int maxsamples = data.sampleCount - data.sampleCount % thin;

                string depthString = "'" + header.segments[curveInfoIndex].data[depthIndex].mnemonic + "': [";
                for (int j = 0; j < maxsamples; j += thin)
                {
                    if (j == maxsamples - thin)
                    {
                        depthString += data.doubleData[depthIndex][j];
                    }
                    else
                    {
                        depthString += data.doubleData[depthIndex][j] + ", ";
                    }
                }
                depthString += "]" + Environment.NewLine;
                return depthString;
            }
        }

        // A LAS Log has a header describing metadata and data layout and meaning.
        public class LogHeader
        {
            public List<LogHeaderSegment> segments;
            public LogHeader()
            {
                segments = new List<LogHeaderSegment>();
            }
            public LogHeader(List<LogHeaderSegment> insegments)
            {
                segments = insegments;
            }

        }

        // A LAS Log has a data section containing well log data in arbitrary quantity,
        // set out as described in the log header.  Each log can be string or doubles represented as strings.
        // This implementation assumes no string logs for the time being.
        public class LogData
        {
            public int logCount;
            public int sampleCount;
            public double[][] doubleData;
            public LogStringDatum[][] stringData;

            public LogData()
            {
                logCount = 0;
                sampleCount = 0;
                doubleData = null;
                stringData = null;
            }
            public LogData(int lC, int sC, double[][] dd, LogStringDatum[][] sd)
            {
                logCount = lC;
                sampleCount = sC;
                doubleData = dd;
                stringData = sd;
            }
            public LogData(int lC, string inString)
            {
                int wordCount = 0;

                if (inString == null || inString.Length == 0 || inString == String.Empty)
                {
                    logCount = 0;
                    sampleCount = 0;
                    doubleData = null;
                    stringData = null;
                    return;
                }

                // Remove the first line containing the ~ASCII identifier
                int index = inString.IndexOf(System.Environment.NewLine);
                string inString1 = inString.Substring(index + System.Environment.NewLine.Length).Trim();
                // Split into words and convert to raw log data
                string[] words = Regex.Split(inString1, @"\s+");
                logCount = lC;
                wordCount = (int)words.Length;
                sampleCount = wordCount / logCount;
                stringData = null;
                doubleData = new double[logCount][];
                for (int logIndex = 0; logIndex < logCount; logIndex++)
                {
                    doubleData[logIndex] = new double[sampleCount];
                    for (int wordIndex = 0; wordIndex < wordCount; wordIndex += logCount)
                    {
                        int sampleIndex = wordIndex / logCount;
                        string word = words[wordIndex + logIndex];
                        if (!string.IsNullOrEmpty(word))
                        {
                            doubleData[logIndex][sampleIndex] = Convert.ToDouble(word);
                        }
                    }
                }
            }
        }

        // A LAS Log header is composed of lines each with at most four items of non-formatting information. 
        public class LogHeaderQuadruple
        {
            public string mnemonic;
            public string unit;
            public string value;
            public string name;

            public LogHeaderQuadruple(string incoming)
            {
                string[] dotSplit = incoming.Split(new char[] { '.' }, 2);
                string[] colonSplit = dotSplit[1].Split(new char[] { ':' }, 2);
                string[] spaceSplit = colonSplit[0].Split(new char[] { ' ' }, 2);
                string firstField = dotSplit[0].Trim();
                string secondField = spaceSplit[0].Trim();
                string thirdField = String.Empty;
                string fourthField = String.Empty;
                if (spaceSplit.Length > 1) thirdField = spaceSplit[1].Trim();
                if (colonSplit.Length > 1) fourthField = colonSplit[1].Trim();

                mnemonic = firstField;
                unit = secondField;
                value = thirdField;
                name = fourthField;
            }
        }

        // Log Headers are named and are composed of segments composed of four items
        public class LogHeaderSegment
        {
            public string name;
            public List<LogHeaderQuadruple> data;
            public string otherInformation;

            public LogHeaderSegment()
            {
                name = String.Empty;
                data = new List<LogHeaderQuadruple>();
                otherInformation = String.Empty;
            }

            public LogHeaderSegment(string inString, Boolean other)
            {
                name = String.Empty;
                data = new List<LogHeaderQuadruple>();
                otherInformation = String.Empty;

                if (inString == null || inString.Length == 0)
                {
                    return;
                }

                if (other == true)
                {
                    otherInformation = inString;
                    return;
                }

                string[] lines = Regex.Split(inString, "\r\n|\r|\n");
                name = lines[0];
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (!string.IsNullOrEmpty(line) && (line[0] != '#'))
                    {
                        data.Add(new LogHeaderQuadruple(line));
                    }
                }
            }
        }

        // A LogDoubleDatum attaches a depth to a double numerical value from a well log.
        // For example, it could be a resistivity value.
        public class LogDoubleDatum
        {
            public double depth;
            public double datum;

            public LogDoubleDatum()
            {
                depth = 0;
                datum = 0;
            }
            public LogDoubleDatum(string inDepth, string inDatum)
            {
                depth = Convert.ToDouble(inDepth);
                datum = Convert.ToDouble(inDatum);
            }
        }

        // A LogStringDatum is a string attached to a depth.
        // Could be a rock chip logging descriptor for example.
        public class LogStringDatum
        {
            public double depth;
            public string datum;

            public LogStringDatum()
            {
                depth = 0;
                datum = null;
            }
            public LogStringDatum(string inDepth, string inDatum)
            {
                depth = Convert.ToDouble(inDepth);
                datum = inDatum;
            }
        }

        // Generate a Well log display Javascript using the C3 library, from a Log class.
        public class C3LogDisplay
        {
            public Log log;
            public string script;

            public C3LogDisplay(Log inputLog)
            {
                log = inputLog;
                script = "";

                // Convert the log to JSON format for entry into C3 Javascript for display
                string JSONString = log.LogToJSON(40, 12);
                // Get the depth log mnemonic for use in identifyiong the X axis prior to rotation to the vertical.
                int endMnemonic = JSONString.IndexOf(':');
                string depthMnemonic = JSONString.Substring(Environment.NewLine.Length + 1, endMnemonic - 3).Trim();
                //                string depthLog = log.GetDepths(12);

                // Generate the script.
                script += "<!-- Load c3.css -->" + Environment.NewLine;
                script += "<link href=\"c3.css\" rel=\"stylesheet\" type=\"text/css\">" + Environment.NewLine;
                script += "<!-- Load d3.js and c3.js -->" + Environment.NewLine;
                script += "<script src=\"d3.min.js\" charset=\"utf-8\"></script>" + Environment.NewLine;
                script += "<script src=\"c3.min.js\"></script>" + Environment.NewLine;
                script += "<div id=\"chart\"></div>" + Environment.NewLine;
                script += "<!-- Call generate() with arguments: -->" + Environment.NewLine;
                script += "<script>" + Environment.NewLine;
                script += "var chart = c3.generate({" + Environment.NewLine;
                script += "    bindto: '#chart'," + Environment.NewLine;
                script += "    size: {" + Environment.NewLine;
                script += "        height: 640," + Environment.NewLine;
                script += "        width: 300" + Environment.NewLine;
                script += "    }," + Environment.NewLine;

                script += "    data: {" + Environment.NewLine;
                script += "           x: " + depthMnemonic + "," + Environment.NewLine;
                //                script += "           x: {" + depthLog + "}," + Environment.NewLine;
                script += "           json: " + JSONString + Environment.NewLine;
                script += "    }," + Environment.NewLine;
                script += "    axis: { 'rotated': true }," + Environment.NewLine;
                //                script += "    axis: { 'x': { 'min': 0, 'max': 5000 }, 'rotated': true }," + Environment.NewLine;
                //                script += "    zoom: { 'enabled': true }," + Environment.NewLine;
                //                script += "    subchart: { 'show': true }," + Environment.NewLine;
                script += "    point: { 'show': false }" + Environment.NewLine;

                script += "  });" + Environment.NewLine;
                script += "</script>" + Environment.NewLine;

            }
        }
        // Generate a Well log display Javascript using the C3 library, from a Log class.
        public class D3LogDisplay
        {
            public Log log;
            public string script;

            public D3LogDisplay(Log inputLog)
            {
                log = inputLog;
                script = "";

                // Convert the log to JSON format for entry into C3 Javascript for display
                string JSONString = log.LogToJSON(40, 12);
                // Get the depth log mnemonic for use in identifyiong the X axis prior to rotation to the vertical.
                int endMnemonic = JSONString.IndexOf(':');
                string depthMnemonic = JSONString.Substring(Environment.NewLine.Length + 1, endMnemonic - 3).Trim();
                //                string depthLog = log.GetDepths(12);
#if false
                script += "       <!-- Load well.css -->" + Environment.NewLine;
                script += "       <link href=\"well.css\" rel=\"stylesheet\" type=\"text/css\">" + Environment.NewLine;
                script += "       <!-- Load d3.js -->" + Environment.NewLine;
                script += "       <script src=\"d3.min.js\" charset=\"utf-8\"></script>" + Environment.NewLine;
                script += "       <div id=\"chart\"></div>" + Environment.NewLine;
                script += "       <!-- Call generate() with arguments: -->" + Environment.NewLine;
                script += "       <script type=\"text/javascript\">" + Environment.NewLine;
                script += "         $(function(){ alert('hi there');";
                script += "		    var data = [3, 6, 2, 7, 5, 2, 1, 3, 8, 9, 2, 5, 7]" + Environment.NewLine;
                script += "			var w = 400" + Environment.NewLine;
                script += "			var h = 200" + Environment.NewLine;
                script += "			var margin = 20" + Environment.NewLine;
                script += "			var y = d3.scale.linear().domain([0, d3.max(data)]).range([0 + margin, h - margin])" + Environment.NewLine;
                script += "			var x = d3.scale.linear().domain([0, data.length]).range([0 + margin, w - margin])" + Environment.NewLine;

                script += "			var vis = d3.select(\"chart\").append(\"svg:svg\").attr(\"width\", w).attr(\"height\", h)" + Environment.NewLine;

                script += "			var g = vis.append(\"svg:g\").attr(\"transform\", \"translate(0, 200)\")" + Environment.NewLine;

                script += "			var line = d3.svg.line().x(function(d,i) { return x(i); }).y(function(d) { return -1 * y(d); })" + Environment.NewLine;

                script += "			g.append(\"svg:path\").attr(\"d\", line(data))" + Environment.NewLine;

                script += "			g.append(\"svg:line\").attr(\"x1\", x(0)).attr(\"y1\", -1 * y(0)).attr(\"x2\", x(w)).attr(\"y2\", -1 * y(0))" + Environment.NewLine;

                script += "			g.append(\"svg:line\").attr(\"x1\", x(0)).attr(\"y1\", -1 * y(0)).attr(\"x2\", x(0)).attr(\"y2\", -1 * y(d3.max(data)))" + Environment.NewLine;

                script += "			g.selectAll(\".xLabel\").data(x.ticks(5)).enter().append(\"svg:text\").attr(\"class\", \"xLabel\").text(String).attr(\"x\", function(d) { return x(d) }).attr(\"y\", 0).attr(\"text-anchor\", \"middle\")" + Environment.NewLine;

                script += "			g.selectAll(\".yLabel\").data(y.ticks(4)).enter().append(\"svg:text\").attr(\"class\", \"yLabel\").text(String).attr(\"x\", 0).attr(\"y\", function(d) { return -1 * y(d) }).attr(\"text-anchor\", \"right\").attr(\"dy\", 4)" + Environment.NewLine;

                script += "			g.selectAll(\".xTicks\").data(x.ticks(5)).enter().append(\"svg:line\").attr(\"class\", \"xTicks\").attr(\"x1\", function(d) { return x(d); }).attr(\"y1\", -1 * y(0)).attr(\"x2\", function(d) { return x(d); }).attr(\"y2\", -1 * y(-0.3))" + Environment.NewLine;

                script += "			g.selectAll(\".yTicks\").data(y.ticks(4)).enter().append(\"svg:line\").attr(\"class\", \"yTicks\").attr(\"y1\", function(d) { return -1 * y(d); }).attr(\"x1\", x(-0.3)).attr(\"y2\", function(d) { return -1 * y(d); }).attr(\"x2\", x(0))" + Environment.NewLine;
                script += "         })";
                script += "       </script>" + Environment.NewLine;

#else
                // Generate the script.
                script += "<!-- Load c3.css -->" + Environment.NewLine;
                script += "<link href=\"c3.css\" rel=\"stylesheet\" type=\"text/css\">" + Environment.NewLine;
                script += "<!-- Load d3.js and c3.js -->" + Environment.NewLine;
                script += "<script src=\"d3.min.js\" charset=\"utf-8\"></script>" + Environment.NewLine;
                script += "<script src=\"c3.min.js\"></script>" + Environment.NewLine;
                script += "<div id=\"chart\"></div>" + Environment.NewLine;
                script += "<!-- Call generate() with arguments: -->" + Environment.NewLine;
                script += "<script>" + Environment.NewLine;
                script += "var chart = c3.generate({" + Environment.NewLine;
                script += "    bindto: '#chart'," + Environment.NewLine;
                script += "    size: {" + Environment.NewLine;
                script += "        height: 640," + Environment.NewLine;
                script += "        width: 300" + Environment.NewLine;
                script += "    }," + Environment.NewLine;

                script += "    data: {" + Environment.NewLine;
                script += "           x: " + depthMnemonic + "," + Environment.NewLine;
                //                script += "           x: {" + depthLog + "}," + Environment.NewLine;
                script += "           json: " + JSONString + Environment.NewLine;
                script += "    }," + Environment.NewLine;
                script += "    axis: { 'rotated': true }," + Environment.NewLine;
                //                script += "    axis: { 'x': { 'min': 0, 'max': 5000 }, 'rotated': true }," + Environment.NewLine;
                //                script += "    zoom: { 'enabled': true }," + Environment.NewLine;
                //                script += "    subchart: { 'show': true }," + Environment.NewLine;
                script += "    point: { 'show': false }" + Environment.NewLine;

                script += "  });" + Environment.NewLine;
                script += "</script>" + Environment.NewLine;
#endif
            }
        }
    }
}
