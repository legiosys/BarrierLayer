using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace BarrierLayer.LibCandidates
{
    public class JsonConfigurationParser
    {
        private readonly IDictionary<string, string> _data =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly Stack<string> _context = new Stack<string>();
        private string _rootPath;

        public static IDictionary<string, string> Parse(JObject obj, string rootPath)
            => new JsonConfigurationParser().ParseInternal(obj, rootPath);

        private IDictionary<string, string> ParseInternal(JObject obj, string rootPath)
        {
            _data.Clear();
            _rootPath = rootPath;
            VisitElement(obj);

            return _data;
        }

        private void VisitElement(JToken element)
        {
            switch (element.Type)
            {
                case JTokenType.Property:
                case JTokenType.Array:
                case JTokenType.Object:
                    foreach (var child in element.Children())
                        VisitElement(child);
                    break;
                case JTokenType.Boolean:
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Null:
                case JTokenType.Date:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                    _data[ConfigurationPath.Combine(_rootPath, element.Path)] = element.ToString();
                    break;
            }
        }
    }
}