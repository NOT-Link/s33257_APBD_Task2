using Tutorial3.Models;
using Tutorial3.Services;

namespace Tutorial3;

class Program
{
    private static EquipmentService _equipmentService = null!;
    private static UserService _userService = null!;
    private static RentalService _rentalService = null!;
    private static ReportService _reportService = null!;

    static void Main(string[] args)
    {
        var penaltyCalculator = new DefaultPenaltyCalculator();
        _equipmentService = new EquipmentService();
        _userService = new UserService();
        _rentalService = new RentalService(penaltyCalculator);
        _reportService = new ReportService(_equipmentService, _userService, _rentalService);

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Equipment Rental System ---");
            Console.WriteLine("1.  Add user");
            Console.WriteLine("2.  Add equipment");
            Console.WriteLine("3.  Show all equipment");
            Console.WriteLine("4.  Show available equipment");
            Console.WriteLine("5.  Rent equipment");
            Console.WriteLine("6.  Return equipment");
            Console.WriteLine("7.  Mark equipment as unavailable");
            Console.WriteLine("8.  Show rentals for user");
            Console.WriteLine("9.  Show overdue rentals");
            Console.WriteLine("10. Show report");
            Console.WriteLine("0.  Exit");
            Console.Write("\nChoice: ");

            var input = Console.ReadLine()?.Trim();
            Console.WriteLine();

            try
            {
                switch (input)
                {
                    case "1": AddUser(); break;
                    case "2": AddEquipment(); break;
                    case "3": ShowAllEquipment(); break;
                    case "4": ShowAvailableEquipment(); break;
                    case "5": RentEquipment(); break;
                    case "6": ReturnEquipment(); break;
                    case "7": MarkUnavailable(); break;
                    case "8": ShowUserRentals(); break;
                    case "9": ShowOverdueRentals(); break;
                    case "10": ShowReport(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void AddUser()
    {
        Console.Write("Type (1 = Student, 2 = Employee): ");
        var type = Console.ReadLine()?.Trim();

        Console.Write("First name: ");
        var first = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Last name: ");
        var last = Console.ReadLine()?.Trim() ?? "";

        if (type == "1")
        {
            Console.Write("Student number: ");
            var number = Console.ReadLine()?.Trim() ?? "";
            var student = new Student(first, last, number);
            _userService.Add(student);
            Console.WriteLine($"Added student: {student.FullName} (ID: {student.Id})");
        }
        else if (type == "2")
        {
            Console.Write("Department: ");
            var dept = Console.ReadLine()?.Trim() ?? "";
            var employee = new Employee(first, last, dept);
            _userService.Add(employee);
            Console.WriteLine($"Added employee: {employee.FullName} (ID: {employee.Id})");
        }
        else
        {
            Console.WriteLine("Invalid user type.");
        }
    }

    static void AddEquipment()
    {
        Console.Write("Type (1 = Laptop, 2 = Projector, 3 = Camera): ");
        var type = Console.ReadLine()?.Trim();

        Console.Write("Name: ");
        var name = Console.ReadLine()?.Trim() ?? "";

        switch (type)
        {
            case "1":
                Console.Write("RAM (GB): ");
                var ram = int.Parse(Console.ReadLine()?.Trim() ?? "0");
                Console.Write("Processor: ");
                var proc = Console.ReadLine()?.Trim() ?? "";
                var laptop = new Laptop(name, ram, proc);
                _equipmentService.Add(laptop);
                Console.WriteLine($"Added laptop: {laptop.Name} (ID: {laptop.Id})");
                break;
            case "2":
                Console.Write("Lumens: ");
                var lumens = int.Parse(Console.ReadLine()?.Trim() ?? "0");
                Console.Write("Resolution: ");
                var res = Console.ReadLine()?.Trim() ?? "";
                var projector = new Projector(name, lumens, res);
                _equipmentService.Add(projector);
                Console.WriteLine($"Added projector: {projector.Name} (ID: {projector.Id})");
                break;
            case "3":
                Console.Write("Megapixels: ");
                var mp = double.Parse(Console.ReadLine()?.Trim() ?? "0");
                Console.Write("Has video recording (y/n): ");
                var video = Console.ReadLine()?.Trim()?.ToLower() == "y";
                var camera = new Camera(name, mp, video);
                _equipmentService.Add(camera);
                Console.WriteLine($"Added camera: {camera.Name} (ID: {camera.Id})");
                break;
            default:
                Console.WriteLine("Invalid equipment type.");
                break;
        }
    }

    static void ShowAllEquipment()
    {
        var items = _equipmentService.GetAll();
        if (items.Count == 0)
        {
            Console.WriteLine("No equipment registered.");
            return;
        }
        foreach (var e in items)
            Console.WriteLine($"  {e} — {e.GetDetails()}");
    }

    static void ShowAvailableEquipment()
    {
        var items = _equipmentService.GetAvailable();
        if (items.Count == 0)
        {
            Console.WriteLine("No equipment available.");
            return;
        }
        foreach (var e in items)
            Console.WriteLine($"  {e} — {e.GetDetails()}");
    }

    static void RentEquipment()
    {
        Console.Write("User ID: ");
        var userId = int.Parse(Console.ReadLine()?.Trim() ?? "0");
        var user = _userService.GetById(userId);
        if (user == null) { Console.WriteLine("User not found."); return; }

        Console.Write("Equipment ID: ");
        var eqId = int.Parse(Console.ReadLine()?.Trim() ?? "0");
        var equipment = _equipmentService.GetById(eqId);
        if (equipment == null) { Console.WriteLine("Equipment not found."); return; }

        Console.Write("Rental days: ");
        var days = int.Parse(Console.ReadLine()?.Trim() ?? "0");

        var rental = _rentalService.RentEquipment(user, equipment, days);
        Console.WriteLine($"Rental created (#{rental.Id}): {user.FullName} rented '{equipment.Name}' until {rental.DueDate:yyyy-MM-dd}");
    }

    static void ReturnEquipment()
    {
        Console.Write("Rental ID: ");
        var rentalId = int.Parse(Console.ReadLine()?.Trim() ?? "0");

        var penalty = _rentalService.ReturnEquipment(rentalId);
        if (penalty > 0)
            Console.WriteLine($"Returned with penalty: {penalty} PLN");
        else
            Console.WriteLine("Returned on time, no penalty.");
    }

    static void MarkUnavailable()
    {
        Console.Write("Equipment ID: ");
        var id = int.Parse(Console.ReadLine()?.Trim() ?? "0");

        var result = _equipmentService.MarkAsUnavailable(id);
        if (result)
            Console.WriteLine("Equipment marked as unavailable.");
        else
            Console.WriteLine("Equipment not found.");
    }

    static void ShowUserRentals()
    {
        Console.Write("User ID: ");
        var userId = int.Parse(Console.ReadLine()?.Trim() ?? "0");

        var rentals = _rentalService.GetActiveRentalsForUser(userId);
        if (rentals.Count == 0)
        {
            Console.WriteLine("No active rentals for this user.");
            return;
        }
        foreach (var r in rentals)
            Console.WriteLine($"  {r}");
    }

    static void ShowOverdueRentals()
    {
        var rentals = _rentalService.GetOverdueRentals();
        if (rentals.Count == 0)
        {
            Console.WriteLine("No overdue rentals.");
            return;
        }
        foreach (var r in rentals)
            Console.WriteLine($"  {r}");
    }

    static void ShowReport()
    {
        Console.WriteLine(_reportService.GenerateSummary());
    }
}