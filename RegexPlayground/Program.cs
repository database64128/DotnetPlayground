using System;
using System.Text.RegularExpressions;

var msg = @"Here are the [rules](https://telegra.ph/rules-05-06) Please read carefully and give the details which mentioned below.
*Name:*
*Place:*
*Education:*
*Experience:*
You can also call me on (01234567890)
__For premium service please contact with admin__";

var pattern = @"[_*[\]()~`>#+\-=|{}.!]";

var output = Regex.Replace(msg, pattern, @"\$&");

Console.WriteLine(output);
