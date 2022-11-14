using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private bool _isActive;

    [SerializeField]
    private Node[] _nodes;

    public ChainEntity ChainEntity { get; private set; }
    private void Awake()
    {
        ChainEntity = new ChainEntity(
            _nodes.Select(node => node.ChainEntity).ToArray(), _isActive);

        ChainEntity.StateChanged += () => GetComponent<SpriteRenderer>().color =
        ChainEntity.isActive ? Color.green : Color.red;
    }

    private void Start()
    {
        ChainEntity.OnStart();
    }

    private void OnMouseDown()
    {
        ChainEntity.ChangeState();
    }
}
