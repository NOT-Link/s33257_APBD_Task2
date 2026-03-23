using Tutorial3.Models;

namespace Tutorial3.Services;

public interface IPenaltyCalculator
{
    decimal CalculatePenalty(Rental rental);
}
