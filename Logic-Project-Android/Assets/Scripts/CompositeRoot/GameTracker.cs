using UnityEngine;
using System;

public class GameTracker : MonoBehaviour
{
    private Node[] _nodes;

    public event Action GameEnding;
    public void LateInit()
    {
        _nodes = FindObjectsOfType<Node>();

        foreach (var node in _nodes)
            node.ChainEntity.StateChanged += OnButtonsStateChanged;
    }

    private void OnButtonsStateChanged(bool state)
    {
        if (IsGameEnded())
            GameEnding?.Invoke();
    }

    private bool IsGameEnded()
    {
        foreach (var node in _nodes)
            if (node.ChainEntity.isActive == false)
                return false;

        return true;
    }
}
