using Stubble.Core.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;

namespace XsDupFinder.Lib.Output
{
    class RenderMainHtml : IRender
    {
        class ItemFirstLast<T>
        {
            public T Item;
            public bool IsFirst;
            public bool IsLast;
        }

        class DuplicateExt
        {
            public readonly Duplicate Data;
            public readonly List<ItemFirstLast<int>> OverlappingIDs;

            public DuplicateExt(Duplicate data)
            {
                Data = data;
                OverlappingIDs = data.OverlappingIDs.Select(q => new ItemFirstLast<int> { Item = q }).ToList();
                if (OverlappingIDs.Count > 0)
                {
                    OverlappingIDs.First().IsFirst = true;
                    OverlappingIDs.Last().IsLast = true;
                }
            }
        }


        class Data
        {

            public List<DuplicateExt> Duplicates { get; set; }
        }

        string Template =>
@"<!doctype html>
<html lang='en'>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
  <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css' integrity='sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh' crossorigin='anonymous'>
  <title>Duplicates</title>
</head>
<body>
  <script src='https://code.jquery.com/jquery-3.4.1.slim.min.js' integrity='sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n' crossorigin='anonymous'></script>
  <script src='https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js' integrity='sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo' crossorigin='anonymous'></script>
  <script src='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js' integrity='sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6' crossorigin='anonymous'></script>
 
  <div class='container-xl'>
    <h1>Duplicate code fragments</h1> 

{{#Duplicates}}
    <div class='card'>
      <div class='card-header'>
        <h4><a id='F{{Data.ID}}'/>Fragment {{Data.ID}} ({{Data.LineCount}} lines, {{Data.Locations.Count}} locations)</h4>
      </div>
      <div class='card-body'>

{{#OverlappingIDs}}{{#IsFirst}}<p>Overlapping fragments: {{/IsFirst}}<a href='#F{{Item}}'>{{Item}}</a>{{^IsLast}}, {{/IsLast}}{{#IsLast}}</p>{{/IsLast}}{{/OverlappingIDs}}

        <table class='table table-sm mb-1'>
          <thead class='thead-light'>
            <tr>
              <th scope='col'>Filename </th>
              <th scope='col'>Method</th>
              <th scope='col'> Startline </th>
              <th scope='col'>Endtline</th>
              <th scope='col'>% of method</th>
            </tr>
          </thead>
          <tbody>
{{#Data.Locations}}
            <tr>
	          <td>{{Filename}}</td>
              <td>{{MethodName}}</td>
              <td>{{StartLine}}</td>
              <td>{{EndLine}}</td>
              <td>{{PercentOfMethod}}</td>
            </tr>
{{/Data.Locations}}
          </tbody>
        </table>

        <button class='btn btn-link p-0' type='button' data-toggle='collapse' data-target='#code{{Data.ID}}'>Source code</button>

	    <div class='card bg-light rounded collapse' id='code{{Data.ID}}'>
          <div class='card-body p-2'>
            <pre class='m-0'><code>{{Data.Code}}</code></pre>
          </div>
        </div>
      </div>
    </div>
{{/Duplicates}}
  </div>
</body>
</html>
";

        public void Execute(Configuration configuration, List<Duplicate> duplicates)
        {
            var stubble = new StubbleBuilder().Build();
            var data = new Data
            {
                Duplicates = duplicates.Select(q => new DuplicateExt(q)).ToList(),
            };
            var output = stubble.Render(Template, data);

            var fileName = Path.Combine(configuration.OutputDirectory, "duplicates.html");
            File.WriteAllText(fileName, output);
        }
    }
}
