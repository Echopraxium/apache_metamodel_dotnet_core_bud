*****************************************************************************************
*****************************************************************************************
                                        README

Project:                  Porting Apache MetaModel from Java to C# .Net Core
Author:                   Michel Kern (echopraxium)
License of this document: Apache 2.0
Version:                  0.0.25
Date:                     20 july 2017
*****************************************************************************************
*****************************************************************************************

Foreword
------------------------------
Here is an update of a 'bud' for a port of Apache MetaModel from Java to C# (.Net Core). 
Please notice that it is a very limited subset which has neen ported ATM. 
Previously I'd applied a "brute force" and "bottom up" approach, now, I've started porting
the 'Json connector', so it's more "top down". ATM I dont have the System Architect
skills for Apache MetaModel so a code review by the core developers would be very helpful
to go forward and make it a worth as a child project of Apache MetaModel.


0.0.25 Release Notes 
------------------------------
* 93 java sources converted (vs 40 previously) even though I must inform that a lot 
of code is commented out (especially in the new sources released). I had to "cut the 
branches" in order that the project still compiles without errors.
* The prefix to differentiate the 'Hemper classes' classes from their Java 
counterpart is now 'N' instead of 'Cs' (e.g. the class 'CsNumber', an equivalent for 
Java's 'Number' becomes 'NNumber').

* The namespace of the 'Helper classes' is now 'org.apache.metamodel.j2n' 
(vs 'org.apache.metamodel.j2cs' previously).

Conversion Issues
------------------------------
Even with a limited subset of the java source code files (93), I've encountered a lot of  
conversions issues, from the very straightforward (e.g. 'String s = input_string.trim()'  
becomes 'string s = input_string.Trim()'), to the ""equivalence required"" (e.g. anonymous 
implementation of interface, like in 'getComparable()' method of 'BooleanComparator' class).

Anyway, I've compiled the 'conversion recipes' that I've found or crafted on the way and
(they are documented in 'java2dotnet_recipes.txt', inside 'doc' folder).

I've also provided a set of 'Helper classes' (under core/org/apache/metamodel/j2n), 
like class extensions (e.g. add a GetHashcode() method to the C# 'object' class), type 
replacement (e.g. Java's Number class converted to 'NNumber')
, etc.. 

Best Regards
Michel Kern (echopraxium on GitHub)
