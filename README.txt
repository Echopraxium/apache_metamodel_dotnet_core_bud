*****************************************************************************************
*****************************************************************************************
                                        README

Project:                  Porting Apache MetaModel from Java to C# .Net Core
Author:                   Michel Kern (echopraxium)
License of this document: Apache 2.0
*****************************************************************************************
*****************************************************************************************

Foreword
------------------------------
Here is a 'bud' version for a port of Apache MetaModel from Java to C# (.Net Core). Please 
notice that it is a very limited subset which has neen ported ATM. 

Conversion Issues
------------------------------
Even with a limited subset of the java source code files (40), I've encountered a lot of  
conversions issues, from the very straightforward (e.g. 'String s = input_string.trim()'  
becomes 'string s = input_string.Trim()'), to the ""equivalence required"" (e.g. anonymous 
implementation of interface, like in getComparable() method of BooleanComparator class).

Anyway, I've tried to do my best (even if my choices may reveal erroneous or even absurd), 
at least to compile the 'conversion recipes' that I've crafted on the way (they are 
documented in doc/java2csharp_recipes.txt).

I've also provided a set of 'Helper classes' (under apache_metamodel_dotnet_core/org/
apache/metamodel/j2cs), like class extensions (e.g. add a GetHashcode() method to the 
C# 'object' class), type replacement (e.g. Java's Number class converted to 'CsNumber')
, etc..

Here is the result of my 'J2Cs' (Java To C#) adventures until now. In the hope that it may 
bring interest for validation and maybe a "child project" Apache MetaModel which is a great 
and innovative solution for interoperability in the Biotop of Data Stores, well beyond the
 ODBC and ORM solutions IMHO.

Best Regards
Michel Kern (echopraxium on GitHub)
