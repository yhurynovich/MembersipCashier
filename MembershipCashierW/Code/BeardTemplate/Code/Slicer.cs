using SecurityUnified.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MembershipCashierW.Code.BeardTemplate.Code
{
    public class Slicer
    {
        string template;
        List<Slice> slices = new List<Slice>();
        static Regex fnRegEx = new Regex(@"{{#(\w+)\s+(\w+)");
        IDictionary<string, object> valuesToAssign;

        public IEnumerable<Slice> Cut(string sourceStr)
        {
            return Cut(sourceStr, new StringBuilder());
        }

        private IEnumerable<Slice> Cut(string sourceStr, StringBuilder sliceBlock = null)
        {
            var firstBlock = new StringBuilder();

            if (sliceBlock == null)
                sliceBlock = new StringBuilder();

            using (var reader = new StringReader(sourceStr))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.Contains("{{#"))
                        firstBlock.AppendLine(line);
                    else
                    {
                        var match = fnRegEx.Match(line);
                        string fn = match.Groups[1].Value;
                        string key = match.Groups[2].Value;

                        int fnInstancesFound = 1;
                        if (string.IsNullOrEmpty(fn))
                            throw new Xxception("Unable to extract operation name in [" + line + "]");
                        while (fnInstancesFound > 0 && (line = reader.ReadLine()) != null)
                        {
                            if (line.Contains("{{#" + fn))
                            {
                                fnInstancesFound++;
                                sliceBlock.Append(line);
                            }
                            else if (line.Contains("{{/" + fn))
                            {
                                fnInstancesFound--;
                                if (fnInstancesFound == 0)
                                {
                                    List<Slice> detectedSlices = new List<Slice>();
                                    if (firstBlock.Length > 0)
                                        detectedSlices.Add(new TextSlice(valuesToAssign, firstBlock.ToString()));
                                    switch (fn)
                                    {
                                        case "if":
                                            detectedSlices.Add(
                                                new Slice_IF(valuesToAssign, key, sliceBlock.ToString())
                                            );
                                            break;
                                        case "each":
                                            detectedSlices.Add(
                                                new Slice_EACH(valuesToAssign, key, sliceBlock.ToString())
                                            );
                                            break;
                                        default:
                                            throw new Xxception("Unknown template operation:" + fn);
                                    }
                                    var followingSlices = Cut(reader.ReadToEnd());
                                    if (followingSlices != null)
                                        detectedSlices.AddRange(followingSlices);
                                    return detectedSlices;
                                }
                                else
                                {
                                    sliceBlock.Append(line);
                                }
                            }
                            else
                                sliceBlock.AppendLine(line);
                        }
                    }
                }
                if (firstBlock.Length > 0)
                    return new Slice[] { new TextSlice(valuesToAssign, firstBlock.ToString()) };
            }
            return null;
        }

        public Slicer(string template)
        {
            this.template = template;
        }

        public Slicer(IDictionary<string, object> valuesToAssign, string template) : this(template)
        {
            this.valuesToAssign = valuesToAssign;
        }
    }
}