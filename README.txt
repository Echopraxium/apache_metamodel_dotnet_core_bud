*****************************************************************************************
                                        README
                                         
Project:                  Porting Apache MetaModel from Java to C# .Net Core 1.1
Author:                   Michel Kern (echopraxium)
License of this document: Apache 2.0
Version:                  0.1.0
Date:                     9 august 2017
IDE:                      Visual Studio 2017 Community 15.2
*****************************************************************************************

Foreword
------------------------------
Here is the latest version of a conversion from Apache MetaModel's Java code to C# 
(.Net Core 1.1). Please notice that it is a limited subset which has neen ported ATM. 

It compiles without errors but has not yet been tested at all (not even the Unit Tests 
ATM). Previously I'd applied a "brute force" and "bottom up" approach. Now I've started 
to "fill the blanks" in order to build a complete conversion of the 'Json connector'.
This is then now more a "top down" walkthrough of the java codebase. 
ATM I miss the System Architect skills for Apache MetaModel (even those of a seasoned 
developer using Apache MetaModel as a solution). 
If the codebase is big enough (29% of the 'core' package), it would be helpful if 
the core team considers doing a code review to validate / invalidate my conversion 
choices.

0.1.0 Release Notes 
------------------------------
* Metrics : 
- 120 java source files ported in C#
  - 29% of the 'core' package of Apache MetaModel (416 java source files)
  - 16% of the full code base (748 java source files, including implementation of 
    connectors like 'Jdbc', 'Json', etc...)
- 43 'helper' classes (to emulate java classes without a .Net equivalence, these classes 
  are prefixed by 'N', e.g. (e.g. Java's 'Number' class converted to 'NNumber')).
 

Conversion Issues
------------------------------
Even with a limited subset of the java source code files (120), I've encountered a lot of  
conversions issues, from the very straightforward (e.g. 'str.toLowerCase()' converted to
'str.ToLower()'), to the ""equivalence craft required"" (e.g. anonymous implementation of
 interface, like in 'getComparable()' method of 'BooleanComparator' class) via 'compiles
 but untested conversion'.

I've compiled the 'conversion recipes' that I've found or crafted on the way, they are 
documented in 'doc/java2dotnet_recipes.txt').

Best Regards
Michel Kern (echopraxium on GitHub)
