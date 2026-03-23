using Tutorial3.Enums;
using Tutorial3.Models;

namespace Tutorial3.Services;

public class EquipmentService
{
    private readonly List<Equipment> _equipment = new();

    public void Add(Equipment item)
    {
        _equipment.Add(item);
    }

    public Equipment? GetById(int id)
    {
        return _equipment.FirstOrDefault(e => e.Id == id);
    }

    public IReadOnlyList<Equipment> GetAll()
    {
        return _equipment.AsReadOnly();
    }

    public IReadOnlyList<Equipment> GetAvailable()
    {
        return _equipment.Where(e => e.IsAvailable).ToList().AsReadOnly();
    }

    public bool MarkAsUnavailable(int equipmentId)
    {
        var item = GetById(equipmentId);
        if (item == null) return false;

        if (item.Status == EquipmentStatus.Rented)
            throw new InvalidOperationException(
                $"Equipment '{item.Name}' is currently rented and cannot be marked unavailable.");

        item.Status = EquipmentStatus.Unavailable;
        return true;
    }

    public bool MarkAsAvailable(int equipmentId)
    {
        var item = GetById(equipmentId);
        if (item == null) return false;

        item.Status = EquipmentStatus.Available;
        return true;
    }
}
