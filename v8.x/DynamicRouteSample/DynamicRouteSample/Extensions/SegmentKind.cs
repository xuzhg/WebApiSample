namespace DynamicRouteSample.Extensions;

public enum SegmentKind
{
    EntitySet,
    Singleton,
    Key,
    Property,
    Navigation,
    Cast,
    Function,
    Action,
    Raw // Raw identifier without "{" and "}"
}
