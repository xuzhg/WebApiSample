namespace FilterOnTwoExpandDeepProperty.Models;

public class Thing1Thing2RelationTable
{
    public int Id { get; set; }

    public Thing1 Thing1 { get; set; }

    public int Thing2Id { get; set; }
}

public class Thing1
{
    public int Id { get; set; }
    public string DisplayName { get; set; }
    public AttributeInfo Attribute { get; set; }
}

public class AttributeInfo
{
   // public int Id { get;}
    public string Type { get; set; }
}
