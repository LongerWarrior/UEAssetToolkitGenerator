namespace UAssetAPI;
public class Singleton {

    public static Singleton Current => instance;
    private static readonly Singleton instance = new Singleton();
    private Dictionary<string, bool> _optionOverrides = new();

    protected Singleton() {
    }
    
    public static void Init(Dictionary<string, bool> options) {
        Current._optionOverrides = options;
    }

    public static bool GetOption(string key, out bool value) {
        return Current._optionOverrides.TryGetValue(key,out value);
    }
}