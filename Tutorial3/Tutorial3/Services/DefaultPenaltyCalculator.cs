using Tutorial3.Models;

namespace Tutorial3.Services;

public class DefaultPenaltyCalculator : IPenaltyCalculator
{
    private const decimal PenaltyPerDay = 10.00m;

    public decimal CalculatePenalty(Rental rental)
    {
        int daysOverdue = rental.GetDaysOverdue();
        return daysOverdue * PenaltyPerDay;
    }
}
