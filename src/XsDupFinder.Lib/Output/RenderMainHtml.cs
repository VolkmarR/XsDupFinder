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

        class DuplicateDisplay : Duplicate
        {
            public class LocationDisplay : Duplicate.Location
            {
                public string RelativeFileName;
                public string BadgeType;

                public LocationDisplay(string sourceDirectory, Duplicate.Location data)
                {
                    Filename = data.Filename;
                    MethodName = data.MethodName;
                    StartLine = data.StartLine;
                    EndLine = data.EndLine;
                    PercentOfMethod = data.PercentOfMethod;

                    if (!string.IsNullOrWhiteSpace(sourceDirectory))
                        RelativeFileName = Filename.Substring(sourceDirectory.Length);
                    else
                        RelativeFileName = Filename;
                    BadgeType = IsFullMethod ? "success" : "warning";
                }
            }

            public readonly List<ItemFirstLast<int>> OverlappingIDItems;
            public readonly List<LocationDisplay> LocationsDisplay;

            public DuplicateDisplay(string sourceDirectory, Duplicate data)
            {
                ID = data.ID;
                Code = data.Code;
                Locations = data.Locations;
                OverlappingIDs = data.OverlappingIDs;
                LocationsDisplay = data.Locations.Select(q => new LocationDisplay(sourceDirectory, q)).ToList();
                OverlappingIDItems = data.OverlappingIDs.Select(q => new ItemFirstLast<int> { Item = q }).ToList();
                if (OverlappingIDItems.Count > 0)
                {
                    OverlappingIDItems.First().IsFirst = true;
                    OverlappingIDItems.Last().IsLast = true;
                }

            }
        }

        string Template =>
@"<!doctype html>
<html lang='en'>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
  <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css' integrity='sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh' crossorigin='anonymous'>
  <title>Duplicate code fragments</title>
  <style>
    body { padding-top: 70px;}
    .anchor { padding-top: 70px; margin-top: -70px; }
  </style>
</head>
<body style='padding-top: 70px;'>
  <script src='https://code.jquery.com/jquery-3.4.1.slim.min.js' integrity='sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n' crossorigin='anonymous'></script>
  <script src='https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js' integrity='sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo' crossorigin='anonymous'></script>
  <script src='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js' integrity='sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6' crossorigin='anonymous'></script>
 
  <nav class='navbar fixed-top navbar-expand-lg navbar-dark bg-primary'>
    <a class='navbar-brand' href='#'>Duplicate code fragments</a>

    <div class='collapse navbar-collapse' id='navbarSupportedContent'>
      <ul class='navbar-nav mr-auto'>
        <li class='nav-item'>
          <a class='nav-link' href='#cfg'>Configuration</a>
        </li>
        <li class='nav-item'>
          <a class='nav-link' href='#sum'>Summary</a>
        </li>
        <li class='nav-item'>
          <a class='nav-link' href='#f1'>Fragments</a>
        </li>
	  </ul >
	</div>

  </nav>

  <div class='container-xl'>

    <div class='card mb-4'>
      <div class='card-header'>
        <h4><a class='anchor' name='cfg'/>Configuration</h4>
      </div>
      <div class='card-body pb-0'>
	    <dl class='row'>
          <dt class='col-sm-5'>Source directory</dt>
          <dd class='col-sm-7'>{{Configuration.SourceDirectory}}</dd>
	    </dl>
	    <dl class='row'>
          <dt class='col-sm-5'>Output directory</dt>
          <dd class='col-sm-7'>{{Configuration.OutputDirectory}}</dd>
	    </dl>
	    <dl class='row'>
          <dt class='col-sm-5'>Cache filename</dt>
          <dd class='col-sm-7'>{{Configuration.CacheFileName}}</dd>
	    </dl>
	    <dl class='row'>
          <dt class='col-sm-5'>Minimum count of lines necessary for a duplicate fragment</dt>
          <dd class='col-sm-7'>{{Configuration.MinLineForDuplicate}}</dd>
	    </dl>
	    <dl class='row'>
          <dt class='col-sm-5'>Minimum count of lines necessary for a duplicate method</dt>
          <dd class='col-sm-7'>{{Configuration.MinLineForFullMethodDuplicateCheck}}</dd>
	    </dl>
      </div>
    </div>

    <div class='card mb-4'>
      <div class='card-header'>
        <h4><a class='anchor' name='sum'/>Summary</h4>
      </div>
      <div class='card-body  pb-0'>
        <dl class='row'>
          <dt class='col-sm-5'>Fragment count</dt>
          <dd class='col-sm-7'>{{Duplicates.Count}}</dd>
        </dl>
      </div>
    </div>


{{#Duplicates}}
    <div class='card mb-3'>
      <div class='card-header'>
        <h4><a class='anchor' name='f{{ID}}'/>Fragment {{ID}} ({{LineCount}} lines, {{Locations.Count}} locations)</h4>
      </div>
      <div class='card-body table-responsive'>

{{#OverlappingIDItems}}{{#IsFirst}}<p>Overlapping fragments: {{/IsFirst}}<a href='#f{{Item}}'>{{Item}}</a>{{^IsLast}}, {{/IsLast}}{{#IsLast}}</p>{{/IsLast}}{{/OverlappingIDItems}}

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
{{#LocationsDisplay}}
            <tr>
	          <td><a href='{{Filename}}' target='_blank'>{{RelativeFileName}}</a></td>
              <td>{{MethodName}}</td>
              <td>{{StartLine}}</td>
              <td>{{EndLine}}</td>
              <td><span class='badge badge-{{BadgeType}}'>{{PercentOfMethod}}</span></td>
            </tr>
{{/LocationsDisplay}}
          </tbody>
        </table>

        <button class='btn btn-link p-0' type='button' data-toggle='collapse' data-target='#code{{ID}}'>Show source code fragment</button>

	    <div class='card bg-light rounded collapse' id='code{{ID}}'>
          <div class='card-body p-2'>
            <pre class='m-0'><code>{{Code}}</code></pre>
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

            var data = new Dictionary<string, object>
            {
                ["Duplicates"] = duplicates.Select(q => new DuplicateDisplay(configuration.SourceDirectory, q)).ToList(),
                ["Configuration"] = configuration,
            };

            var output = stubble.Render(Template, data);

            var fileName = Path.Combine(configuration.OutputDirectory, "duplicates.html");
            File.WriteAllText(fileName, output);
        }
    }
}
