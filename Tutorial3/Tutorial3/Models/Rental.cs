using Tutorial3.Services;

namespace Tutorial3.Models;

public class Rental
{
    private static int _nextId = 1;

    public int Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime RentalDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; private set; }

    public bool IsActive => ReturnDate == null;
    public bool IsOverdue => IsActive && DateTime.Now > DueDate;

    public Rental(User user, Equipment equipment, int rentalDays)
    {
        Id = _nextId++;
        User = user;
        Equipment = equipment;
        RentalDate = DateTime.Now;
        DueDate = RentalDate.AddDays(rentalDays);
    }

    public decimal CompleteReturn(IPenaltyCalculator penaltyCalculator)
    {
        ReturnDate = DateTime.Now;
        return penaltyCalculator.CalculatePenalty(this);
    }

    public int GetDaysOverdue()
    {
        var referenceDate = ReturnDate ?? DateTime.Now;
        var overdue = (referenceDate - DueDate).Days;
        return overdue > 0 ? overdue : 0;
    }

    public override string ToString()
    {
        var status = IsActive ? (IsOverdue ? "OVERDUE" : "Active") : "Returned";
        return $"Rental #{Id}: {User.FullName} -> {Equipment.Name} | " +
               $"Rented: {RentalDate:yyyy-MM-dd} | Due: {DueDate:yyyy-MM-dd} | Status: {status}";
    }
}
