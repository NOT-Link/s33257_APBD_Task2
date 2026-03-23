using Tutorial3.Enums;
using Tutorial3.Models;

namespace Tutorial3.Services;

public class RentalService
{
    private readonly List<Rental> _rentals = new();
    private readonly IPenaltyCalculator _penaltyCalculator;

    public RentalService(IPenaltyCalculator penaltyCalculator)
    {
        _penaltyCalculator = penaltyCalculator;
    }


    public Rental RentEquipment(User user, Equipment equipment, int rentalDays)
    {
        if (rentalDays <= 0)
            throw new ArgumentException("Rental period must be at least 1 day.");

        if (!equipment.IsAvailable)
            throw new InvalidOperationException(
                $"Equipment '{equipment.Name}' is not available for rental (Status: {equipment.Status}).");

        int activeCount = GetActiveRentalsForUser(user.Id).Count;
        if (activeCount >= user.MaxActiveRentals)
            throw new InvalidOperationException(
                $"User '{user.FullName}' has reached the maximum of {user.MaxActiveRentals} active rentals.");

        equipment.Status = EquipmentStatus.Rented;
        var rental = new Rental(user, equipment, rentalDays);
        _rentals.Add(rental);
        return rental;
    }


    public decimal ReturnEquipment(int rentalId)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental == null)
            throw new ArgumentException($"Rental #{rentalId} not found.");

        if (!rental.IsActive)
            throw new InvalidOperationException($"Rental #{rentalId} has already been returned.");

        decimal penalty = rental.CompleteReturn(_penaltyCalculator);
        rental.Equipment.Status = EquipmentStatus.Available;
        return penalty;
    }

    public IReadOnlyList<Rental> GetActiveRentalsForUser(int userId)
    {
        return _rentals
            .Where(r => r.User.Id == userId && r.IsActive)
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<Rental> GetOverdueRentals()
    {
        return _rentals
            .Where(r => r.IsActive && r.IsOverdue)
            .ToList()
            .AsReadOnly();
    }

    public IReadOnlyList<Rental> GetAllRentals()
    {
        return _rentals.AsReadOnly();
    }
}
