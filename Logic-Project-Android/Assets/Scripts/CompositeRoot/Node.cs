﻿using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Node : CompositeRoot
{
    [HideInInspector]
    [SerializeField]
    private bool _isActive;

    [HideInInspector]
    [SerializeField]
    private Node[] _nodes;

    public ChainEntity ChainEntity { get; private set; }

    private Animator _animator;
    private AudioSource _pushedSoundSource;

    private void OnMouseDown()
    {
        ChainEntity.ChangeState();
        _pushedSoundSource.Play();
    }

    public override void Compose()
    {
        _animator = GetComponent<Animator>();
        _pushedSoundSource = GetComponent<AudioSource>();
        ChainEntity = new ChainEntity(_isActive);

        ChainEntity.StateChanged += OnStateChanged;
    }

    public override void Initialize()
    {
        ChainEntity.BindNeighbours(_nodes.Select(node => node.ChainEntity).ToArray());
    }

    public override void Launch()
    {
        ChainEntity.OnStart();
    }

    private void OnStateChanged(bool state)
    {
        if (state)
            _animator.SetTrigger("enable");
        else
            _animator.SetTrigger("disable");  
    }
}
