using System;
using System.Collections.Generic;

public class ChainEntity
{
    public ChainEntity(bool state)
    {
        _isActive = state;
    }

    public ChainEntity BindNeighbours(ChainEntity[] entities)
    {
        _neighbours = entities;
        return this;
    }

    public event Action StateChanged;
    public IEnumerable<ChainEntity> Neighbours => _neighbours;
    public bool isActive
    {
        get => _isActive;
        private set
        {
            _isActive = value;
            Validate(this);
            foreach (var neightbour in Neighbours)
                neightbour.ChangeState();
        }
    }
    private ChainEntity[] _neighbours;
    private bool _isActive;

    public void OnStart()
    {
        //Validate(this);
        StateChanged?.Invoke();
    }
    public void ChangeState()
    {
        isActive = !isActive;
        StateChanged?.Invoke();
    }
    public void Validate(ChainEntity parent)
    {
        foreach (var entity in Neighbours)
        {
            if (entity == parent)
                throw new InvalidOperationException("Circular dependency detected");

            entity.Validate(parent);
        }
    }
}
