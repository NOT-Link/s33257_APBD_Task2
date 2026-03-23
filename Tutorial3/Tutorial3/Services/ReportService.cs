using Tutorial3.Enums;
using Tutorial3.Models;

namespace Tutorial3.Services;

public class ReportService
{
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly RentalService _rentalService;

    public ReportService(EquipmentService equipmentService, UserService userService, RentalService rentalService)
    {
        _equipmentService = equipmentService;
        _userService = userService;
        _rentalService = rentalService;
    }

    public string GenerateSummary()
    {
        var allEquipment = _equipmentService.GetAll();
        var allRentals = _rentalService.GetAllRentals();
        var overdueRentals = _rentalService.GetOverdueRentals();
        var allUsers = _userService.GetAll();

        var available = allEquipment.Count(e => e.Status == EquipmentStatus.Available);
        var rented = allEquipment.Count(e => e.Status == EquipmentStatus.Rented);
        var unavailable = allEquipment.Count(e => e.Status == EquipmentStatus.Unavailable);
        var activeRentals = allRentals.Count(r => r.IsActive);
        var completedRentals = allRentals.Count(r => !r.IsActive);

        return $"""
            RENTAL SERVICE REPORT
            
            Equipment Summary:
              Total items:      {allEquipment.Count}
              Available:        {available}
              Currently rented: {rented}
              Unavailable:      {unavailable}
            
            User Summary:
              Total users:      {allUsers.Count}
              Students:         {allUsers.Count(u => u.Type == UserType.Student)}
              Employees:        {allUsers.Count(u => u.Type == UserType.Employee)}
            
            Rental Summary:
              Total rentals:    {allRentals.Count}
              Active:           {activeRentals}
              Completed:        {completedRentals}
              Overdue:          {overdueRentals.Count}
            
            """;
    }
}
