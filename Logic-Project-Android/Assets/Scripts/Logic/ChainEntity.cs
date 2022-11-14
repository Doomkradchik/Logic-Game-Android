using System;

public class ChainEntity
{
    public ChainEntity(ChainEntity[] neighbours, bool state)
    {
        Neighbours = neighbours;
        _isActive = state;
    }
    public event Action StateChanged;
    public ChainEntity[] Neighbours { get; }
    private bool _isActive;
    public bool isActive
    {
        get => _isActive;
        private set
        {
            _isActive = value;
            foreach (var neightbour in Neighbours)
                neightbour.ChangeState();
        }
    }
    public void OnStart()
    {
        Validate(this);
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
            if (entity.Equals(parent))
                throw new InvalidOperationException("Circular dependency detected");

            entity.Validate(parent);
        }
    }
}
