using System.Linq;
using UnityEngine;

public class Node : CompositeRoot
{
    [HideInInspector]
    [SerializeField]
    private bool _isActive;

    [HideInInspector]
    [SerializeField]
    private Node[] _nodes;

    public ChainEntity ChainEntity { get; private set; }

    private void OnMouseDown()
    {
        ChainEntity.ChangeState();
    }

    public override void Compose()
    {
        ChainEntity = new ChainEntity(_isActive);

        ChainEntity.StateChanged += () => GetComponent<SpriteRenderer>().color =
            ChainEntity.isActive ? Color.green : Color.red;
    }

    public override void Initialize()
    {
        ChainEntity.BindNeighbours(_nodes.Select(node => node.ChainEntity).ToArray());
    }

    public override void Launch()
    {
        ChainEntity.OnStart();
    }
}
