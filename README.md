# XsDupFinder

## About

**XsDupFinder** is a duplicate code finder for the [XSharp](https://www.xsharp.info/) programming language. 
The goal is to analyze large XSharp code bases and detect duplicate code fragments, that were created by copy and paste source code.

## How does it work

XsDupFinder uses the original parser, developed by the XSharp Team to analyze the source code and extract methods and statements. 
Because of that, it is smarter then a generic duplicate text finder, because:
* only the code inside methods will be analyzed
* comments and white spaces will be ignored

## How do I use it

XsDupFinder comes in two flavors:

* XsDupFinderWin is a windows application. The configuration can be made using the gui.
* XsDupFinderCmd is a console application. A configuration file must be passed as command line option. 

Both applications produce a Json and a HTML file containing the duplicate code fragments.

## What are the minimum requirements

XsDupFinder is a .Net Framework 4.7.2 application. This version of the .Net Framework should be installed on all recent Windows 10 versions.

## How do I get support

Disclaimer: I develop this project in my spare time. So no promises... But i'll try my best. 

If you find any bugs, you can file an issue or propose a bugfix.