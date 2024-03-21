using System;
using System.Text.Json;
using System.Text.Json.Serialization;

var mySnake = new MySnake()
{
    ComposedName = "snake",
};

var myCamel = new MyCamel()
{
    ComposedName = "camel",
};

var snakeJson = JsonSerializer.Serialize(mySnake);
var camelJson = JsonSerializer.Serialize(myCamel);

Console.WriteLine(snakeJson);
Console.WriteLine(camelJson);

interface IMyBase
{
    public string ComposedName { get; set; }
}

class MySnake : IMyBase
{
    [JsonPropertyName("composed_name")]
    public string ComposedName { get; set; } = "";
}

class MyCamel : IMyBase
{
    [JsonPropertyName("composedName")]
    public string ComposedName { get; set; } = "";
}
