using System.Linq;

public class CompositionOrder : CompositeRoot
{
    private CompositeRoot[] _roots;
    private void Awake()
    {
        _roots = FindObjectsOfType<CompositeRoot>()
            .Where(root => root != this)
            .ToArray();
    }
    private void Start()
    {
        Compose();
        Initialize();
        Launch();

        FindObjectOfType<GameTracker>().LateInit();
    }

    public override void Compose()
    {
        foreach (var root in _roots)
            root.Compose();
    }

    public override void Initialize()
    {
        foreach (var root in _roots)
            root.Initialize();
    }

    public override void Launch()
    {
        foreach (var root in _roots)
            root.Launch();
    }    
}
