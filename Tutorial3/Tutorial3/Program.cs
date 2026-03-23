using Tutorial3.Models;
using Tutorial3.Services;

namespace Tutorial3;

class Program
{
    static void Main(string[] args)
    {
        var penaltyCalculator = new DefaultPenaltyCalculator();
        var equipmentService = new EquipmentService();
        var userService = new UserService();
        var rentalService = new RentalService(penaltyCalculator);
        var reportService = new ReportService(equipmentService, userService, rentalService);

        var laptop = new Laptop("Dell XPS 15", 16, "Intel i7-13700H");
        var projector = new Projector("Epson EB-X51", 3800, "1024x768");
        var camera = new Camera("Canon EOS R6", 20.1, true);

        equipmentService.Add(laptop);
        equipmentService.Add(projector);
        equipmentService.Add(camera);

        var student = new Student("Jan", "Kowalski", "s12345");
        var employee = new Employee("Anna", "Nowak", "IT Department");

        userService.Add(student);
        userService.Add(employee);

        var rental = rentalService.RentEquipment(student, laptop, 7);
        Console.WriteLine($"Rented '{laptop.Name}' to {student.FullName} for 7 days");

        try
        {
            rentalService.RentEquipment(employee, laptop, 3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot rent: {ex.Message}");
        }

        var penalty = rentalService.ReturnEquipment(rental.Id);
        Console.WriteLine($"Returned '{laptop.Name}' — penalty: {penalty} PLN");

        Console.WriteLine();
        Console.WriteLine(reportService.GenerateSummary());
    }
}