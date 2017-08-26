*****************************************************************************************
                                        README
                                         
Project:                  Porting Apache MetaModel from Java to C# .Net Core 1.1
                          https://github.com/Echopraxium/apache_metamodel_dotnet_core_bud
Author:                   Michel Kern (echopraxium)
License of this document: Apache 2.0
Version:                  0.2.0
Date:                     26 august 2017
IDE:                      Visual Studio 2017 Community 15.3
*****************************************************************************************

Foreword
------------------------------
Here is the latest version of a conversion from Apache MetaModel's Java code to C# 
(.Net Core 1.1). Please notice that it is a subset which has neen ported ATM. 

Now the codebase is big enough (i.e. the 'engine' is ready) which allows to run the unit 
tests (via 'MetaModel-cli-test' console app) but still a lot of validation / debug 
pending (e.g. in JsonDataContextTest.testDocumentsOnEveryLineFile() only the first 
Assert succeeds). 

Previously I sais that I applied a "brute force" and "bottom up" approach. Now I 
would say that it's more the approach seems more like porting a legacy codebase.

I agree with Kasper that a rewrite with CSharp strengths and weaknesses makes more
sense than a "1 to 1 Force Fit" from Java to CSharp. I just hope that this prototype
may raise enough motivation to "dotnet" child project within Apache MetaModel.

0.2.0 Release Notes 
------------------------------
- 'QueryPostprocessDataContext' and 'MetaModelHelper' class fully ported, no more
  'commented out' lines of code
- 52% of the 'core' package ported
- Json connector has been ported
- Unit tests are now possible to run via 'MetaModel-cli-test' project (console app)

Note
------------------------------
I've compiled the 'conversion recipes' that I've found or crafted on the way, they are 
documented in 'doc/java2dotnet_recipes.txt').

Best Regards
Michel Kern (echopraxium on GitHub)
