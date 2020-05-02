
using System;

public class ResultObject
{
    public int page { get; set; }
    public Row[] rows { get; set; }
}

public class Row
{
    public string[] cell { get; set; }
}