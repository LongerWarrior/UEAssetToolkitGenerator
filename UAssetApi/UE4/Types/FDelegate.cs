namespace UAssetAPI.PropertyTypes;

/// <summary>
/// Describes a function bound to an Object.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class FDelegate
{
    /** Uncertain what this is for; if you find out, please let me know */
    [JsonProperty]
    public FPackageIndex Object;
    /** Uncertain what this is for; if you find out, please let me know */
    [JsonProperty]
    public FName Delegate;


    public FDelegate(FPackageIndex @object, FName @delegate)
    {
        Object = @object;
        Delegate = @delegate;
    }

    public FDelegate()
    {

    }
}
