using Stubble.Core.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using XsDupFinder.Lib.Common;
using XsDupFinder.Lib.Finder;
using XsDupFinder.Lib.Output.ChangeTracker;
using XsDupFinder.Lib.Output.Json;

namespace XsDupFinder.Lib.Output.Renderer
{
    class RenderChangesHtml : IRender
    {
        public const string FileName = "changes.html";
        JsonChangesOutput Changes;
        Configuration Configuration;

        string Template =>
@"<!doctype html>
<html lang='en'>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
    <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css' integrity='sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh' crossorigin='anonymous'>
    <title>Changes diagram</title>
    <script src='https://cdn.jsdelivr.net/npm/chart.js@2.9.3/dist/Chart.min.js'></script>
    <style>
        html {
            scroll-padding-top: 70px
        }

        body {
            scroll-padding-top: 70px;
            padding-top: 70px
        }

        canvas {
            -moz-user-select: none;
            -webkit-user-select: none;
            -ms-user-select: none;
        }
    </style>
    <script>
        function buildLabels() {
            return [
                {{{labels}}}
            ]
        }

        function buildFragments() {
            return [
                {{fragments}}
            ]
        }

        function buildLocations() {
            return [
                {{locations}}
            ]
        }

        function buildConfig(data, label, color) {
            return {
                type: 'line',
                data: {
                    labels: buildLabels(),
                    datasets: [{
                        label: '# of ' + label,
                        fill: false,
                        borderColor: color,
                        data: data
                    }]
                },
                options: {
                    responsive: true,
                    tooltips: {
                        mode: 'index',
                        intersect: false,
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: true,
                                fontSize: 20,
                                labelString: 'Date'
                            }
                        }]
                    }
                }
            }
        }

        function initChart(canvasId, data, label, color) {
            var ctx = document.getElementById(canvasId).getContext('2d');
            new Chart(ctx, buildConfig(data, label, color));
        }

        window.onload = function () {
            initChart('canFragments', buildFragments(), 'fragments', 'rgb(255, 99, 132)');
            initChart('canLocations', buildLocations(), 'locations', 'rgb(54, 162, 235)');
        };
    </script>
</head>
<body>
    <script src='https://code.jquery.com/jquery-3.4.1.slim.min.js' integrity='sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n' crossorigin='anonymous'></script>
    <script src='https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js' integrity='sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo' crossorigin='anonymous'></script>
    <script src='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js' integrity='sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6' crossorigin='anonymous'></script>
    <nav class='navbar fixed-top navbar-expand-lg navbar-dark bg-primary'>
        <a class='navbar-brand' href='#'>Changes</a>
    </nav>

    <div class='container-xl'>
        <div class='card mb-4'>
            <div class='card-header'>
                <h2>Number of fragments</h2>
            </div>
            <div class='card-body pb-0'>
                <canvas id='canFragments'></canvas>
            </div>
        </div>

        <div class='card mb-4'>
            <div class='card-header'>
                <h2>Number of total locations</h2>
            </div>
            <div class='card-body pb-0'>
                <canvas id='canLocations'></canvas>
            </div>
        </div>

        <div class='card mb-4'>
            <div class='card-header'>
                <h4>Information</h4>
            </div>
            <div class='card-body pb-0'>
                <p>Dates marked with * show a change of the configuration</p>
            </div>
        </div>
    </div>
</body>
</html>
";
        bool LoadChanges()
        {
            const int maxItems = 50;

            Changes = JsonChangesOutput.Load(RenderFileHelper.BuildOutputFileName(Configuration, RenderChangeTracker.FileName));
            if (Changes != null && Changes.Items.Count > maxItems)
                Changes.Items = Changes.Items.Skip(Changes.Items.Count - maxItems).ToList();

            return Changes != null;
        }

        string FormatLabel(JsonChangesItem item)
            => $"['{item.Changed:dd.MM.}', '{item.Changed:yyyy}{(item.ConfigurationChanged ? "*" : "")}']";

        public void Execute(Configuration configuration, List<Duplicate> duplicates)
        {
            Configuration = configuration;

            if (!configuration.TrackChanges || !LoadChanges())
                return;

            var stubble = new StubbleBuilder().Build();

            var data = new Dictionary<string, object>
            {
                ["labels"] = string.Join(",", Changes.Items.Select(FormatLabel)),
                ["fragments"] = string.Join(",", Changes.Items.Select(q => q.NumberOfFragements)),
                ["locations"] = string.Join(",", Changes.Items.Select(q => q.NumberOfLocations)),
            };

            var output = stubble.Render(Template, data);
            RenderFileHelper.SaveRenderOutput(configuration, FileName, output);
        }
    }
}
