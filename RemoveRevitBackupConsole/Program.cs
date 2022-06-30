/*
 References:
https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?view=net-6.0
https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getextension?view=net-6.0
https://www.geeksforgeeks.org/c-sharp-how-to-check-whether-a-list-contains-a-specified-element/
https://www.c-sharpcorner.com/article/c-sharp-regex-examples/
https://docs.microsoft.com/en-us/dotnet/api/system.io.file.delete?view=net-6.0

Workflow:
1. Get directory name
2. List files
2.1. Check if extension in a list of patterns
2.2. Check if file name is compliant with the regex patterns
3. Print matching items
4. Aks for confirmation
4.1. Delete files
4.2 Or continue
 */


using System;
using System.IO;

