using Tutorial3.Enums;

public abstract class Equipment
{
    private static int _nextId = 1;

    public int Id { get; }
    public string Name { get; set; }
    public EquipmentStatus Status { get; set; }
    public DateTime DateAdded { get; }

    protected Equipment(string name)
    {
        Id = _nextId++;
        Name = name;
        Status = EquipmentStatus.Available;
        DateAdded = DateTime.Now;
    }

    public bool IsAvailable => Status == EquipmentStatus.Available;

    public abstract string GetDetails();

    public override string ToString()
    {
        return $"[{Id}] {Name} ({GetType().Name}) - Status: {Status}";
    }
}
