using System;
using System.Collections.Generic;

var firstRecord = new MyRecord(0, "Tim");
var secondRecord = new MyRecord(0, "Tim");

var myList = new List<MyRecord>()
{
    firstRecord,
};

var index = myList.IndexOf(secondRecord);

Console.WriteLine(index);

record MyRecord(int Id, string Name);
