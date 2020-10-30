using Extensions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Parser;
using XsDupFinder.Lib.Common;

namespace XsDupFinder.Lib.Finder
{
    public class DuplicateFinder
    {
        readonly Configuration Configuration;
        public DuplicateFinder(Configuration configuration)
        {
            Configuration = configuration;
        }

        class StatementLocation
        {
            public CodeInfo CodeInfo { get; set; }
            public MethodInfo Method { get; set; }
            public int Offset { get; set; }
        }

        readonly Dictionary<CodeInfo, SourceCodeFile> SourceCodeFiles = new Dictionary<CodeInfo, SourceCodeFile>();
        readonly Dictionary<uint, List<StatementLocation>> Locations = new Dictionary<uint, List<StatementLocation>>();
        readonly Dictionary<uint, List<StatementLocation>> FullMethods = new Dictionary<uint, List<StatementLocation>>();
        readonly Dictionary<MethodInfo, HashSet<string>> UsedLocations = new Dictionary<MethodInfo, HashSet<string>>();
        readonly List<Duplicate> DuplicateList = new List<Duplicate>();

        uint GetStatementListHashCode(StringBuilder hashCodeBlock)
            => XXHash.XXH32(Encoding.UTF8.GetBytes(hashCodeBlock.ToString()));

        void RemoveElementsWithSingleLocations()
        {
            void RemoveFromDict(Dictionary<uint, List<StatementLocation>> dict)
                => dict.Where(q => q.Value.Count < 2).Select(q => q.Key).ToList().ForEach(q => dict.Remove(q));

            RemoveFromDict(Locations);
            RemoveFromDict(FullMethods);
        }

        bool ValidIndex(StatementLocation location, int offset)
        {
            var index = location.Offset + offset;
            return index >= 0 && index < location.Method.StatementList.Count;
        }

        int GetOffset(List<StatementLocation> locationList, int direction = -1)
        {
            var result = 0;
            var firstLocation = locationList[0];
            var otherLocations = locationList.Skip(1).ToList();

            while (true)
            {
                var newResult = result + direction;
                if (!ValidIndex(firstLocation, newResult))
                    return result;

                var firstStatementHashCode = firstLocation.Method.StatementList[firstLocation.Offset + newResult].Hashcode;
                foreach (var location in otherLocations)
                {
                    if (!ValidIndex(location, newResult) || firstStatementHashCode != location.Method.StatementList[location.Offset + newResult].Hashcode)
                        return result;
                }

                result = newResult;
            }
        }

        string BuildLocationKey(StatementLocation location, int startOffset, int endOffset)
            => $"{location.Offset + startOffset}-{location.Offset + endOffset}";

        bool CheckLocationUsed(StatementLocation location, int startOffset, int endOffset)
        {
            if (UsedLocations.TryGetValue(location.Method, out var locations))
                return locations.Contains(BuildLocationKey(location, startOffset, endOffset));
            return false;
        }

        void AddLocationUsed(StatementLocation location, int startOffset, int endOffset)
        {
            if (!UsedLocations.TryGetValue(location.Method, out var locations))
            {
                locations = new HashSet<string>();
                UsedLocations[location.Method] = locations;
            }
            locations.Add(BuildLocationKey(location, startOffset, endOffset));
        }

        string GetSourceCodeBlock(StatementLocation location, int startOffset, int endOffset)
        {
            var sourceCode = SourceCodeFiles[location.CodeInfo].SourceCode;

            var start = location.Method.StatementList[location.Offset + startOffset].Start;
            while (start > 0 && sourceCode[start - 1] != '\n' && sourceCode[start - 1] != '\r')
                start--;

            var length = location.Method.StatementList[location.Offset + endOffset].End - start;
            return sourceCode.Substring(start, length);
        }

        void AddDupelicateLocation(Duplicate duplicate, StatementLocation location, int startOffset, int endOffset)
        {
            AddLocationUsed(location, startOffset, endOffset);

            startOffset += location.Offset;
            endOffset += location.Offset;

            var percentOfMethod = (endOffset - startOffset + 1) * 100 / location.Method.StatementList.Count;

            duplicate.Locations.Add(new Duplicate.Location
            {
                Filename = location.CodeInfo.FileName,
                ClassName = location.Method.ClassName,
                MethodName = location.Method.Name,
                StartLine = location.Method.StatementList[startOffset].StartLine,
                EndLine = location.Method.StatementList[endOffset].StartLine,
                PercentOfMethod = percentOfMethod,
            });
        }

        void AddDupelicate(List<StatementLocation> locationList, int startOffset, int endOffset)
        {
            var group = new Duplicate { Code = GetSourceCodeBlock(locationList[0], startOffset, endOffset) };
            foreach (var location in locationList)
                AddDupelicateLocation(group, location, startOffset, endOffset);

            DuplicateList.Add(group);
        }

        void CheckDuplicates(List<StatementLocation> locationList)
        {
            var firstLocation = locationList[0];
            var startOffset = GetOffset(locationList, -1);
            var endOffset = GetOffset(locationList, 1);

            if (CheckLocationUsed(firstLocation, startOffset, endOffset))
                return;

            AddDupelicate(locationList, startOffset, endOffset);
        }

        List<Duplicate> PrepareResult()
        {
            var result = DuplicateList.OrderByDescending(q => q.LineCount).ThenByDescending(q => q.Locations.Count).ToList();
            var id = 0;
            foreach (var item in result)
                item.ID = ++id;

            var duplocateOverlapIDs = DuplicateList.ToDictionary(q => q, _ => new HashSet<int>());
            var duplocateLocations = DuplicateList.SelectMany(q => q.Locations, (m, d) => new { Duplicate = m, Location = d });
            var overlapping = from q in duplocateLocations
                              group q by $"{ q.Location.Filename}#{q.Location.ClassName}#{q.Location.MethodName}" into g
                              select new { Items = g.OrderBy(o => o.Location.StartLine).ToList() };
            foreach (var group in overlapping)
            {
                for (int i = 0; i < group.Items.Count; i++)
                {
                    var first = group.Items[i];
                    for (int j = i + 1; j < group.Items.Count; j++)
                    {
                        var current = group.Items[j];
                        if (first.Duplicate.ID != current.Duplicate.ID && 
                            first.Location.StartLine <= current.Location.EndLine && 
                            first.Location.EndLine >= current.Location.StartLine)
                        {
                            duplocateOverlapIDs[current.Duplicate].Add(first.Duplicate.ID);
                            duplocateOverlapIDs[first.Duplicate].Add(current.Duplicate.ID);
                        }
                    }
                }
            }

            foreach (var item in duplocateOverlapIDs)
                item.Key.OverlappingIDs = item.Value.OrderBy(q => q).ToList();

            return result;
        }

        public void AddSourceCodeFile(SourceCodeFile sourceCodeFile, CodeInfo codeInfo)
        {
            if (sourceCodeFile == null || codeInfo == null || sourceCodeFile.FileName != codeInfo.FileName)
                throw new ArgumentException();


            SourceCodeFiles.Add(codeInfo, sourceCodeFile);

            var buffer = new StringBuilder();
            foreach (var methodInfo in codeInfo.MethodList)
            {
                void AddMethodLocations(Dictionary<uint, List<StatementLocation>> dict, int startOffset, int count)
                {
                    buffer.Length = 0;
                    for (int i = 0; i < count; i++)
                        buffer.Append(methodInfo.StatementList[startOffset + i].Hashcode.ToString()).Append('#');

                    var statementBlockHashCode = GetStatementListHashCode(buffer);
                    if (!dict.TryGetValue(statementBlockHashCode, out var locationList))
                    {
                        locationList = new List<StatementLocation>();
                        dict[statementBlockHashCode] = locationList;
                    }

                    locationList.Add(new StatementLocation { CodeInfo = codeInfo, Method = methodInfo, Offset = startOffset });
                }

                if (methodInfo.StatementList.Count >= Configuration.MinLineForFullMethodDuplicateCheck && methodInfo.StatementList.Count < Configuration.MinLineForDuplicate)
                    AddMethodLocations(FullMethods, 0, methodInfo.StatementList.Count);

                if (methodInfo.StatementList.Count >= Configuration.MinLineForDuplicate)
                {
                    for (int i = 0; i < methodInfo.StatementList.Count - Configuration.MinLineForDuplicate; i++)
                        AddMethodLocations(Locations, i, Configuration.MinLineForDuplicate);
                }
            }
        }

        public List<Duplicate> Execute()
        {
            RemoveElementsWithSingleLocations();
            DuplicateList.Clear();

            foreach (var locationList in FullMethods.Values)
                CheckDuplicates(locationList);

            foreach (var locationList in Locations.Values)
                CheckDuplicates(locationList);

            return PrepareResult();
        }
    }
}
