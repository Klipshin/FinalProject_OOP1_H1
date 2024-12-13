using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Media;
using System.Security.Cryptography;
using System.Globalization;

namespace FinalProject_PLS
{
    public abstract class Person
    {
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name;
        }

        public abstract void Greet();
    }
    public interface ICalories
    {
        void CalculateRecommendedCalories();


    }
    public class Customer : Person, ICalories
    {
        public int Age { get; set; }
        public string Sex { get; set; }
        public int RecommendedCalories { get; private set; }


        public Customer(string name, int age, string sex) : base(name)
        {
            Age = age;
            Sex = sex;
            CalculateRecommendedCalories();
        }

        public void CalculateRecommendedCalories()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nChoose the method to calculate recommended calories:");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[1]");
            Console.ResetColor();
            Console.Write(" WebMD Calorie Intake (Based on Age, Sex, and Activity Level)\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[2]");
            Console.ResetColor();
            Console.Write(" Harris-Benedict principle (Based on BMR, Weight, and Height)\n");

            int choice;
            while (true)
            {
                try
                {
                    Console.Write("Enter your choice (1 or 2): ");
                    choice = int.Parse(Console.ReadLine());
                    if (choice != 1 && choice != 2)

                        throw new Exception("Please choose either 1 or 2.");

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine();
            if (choice == 1)
            {
                BasicCalculateRecommendedCal(Age, Sex);
            }
            else
            {
                AdvancedCalculateRecommendedCal(Age, Sex);
            }
        }
        public void BasicCalculateRecommendedCal(int age, string sex)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[1]");
            Console.ResetColor();
            Console.Write("Sedentary, ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[2]");
            Console.ResetColor();
            Console.Write("Moderately Active, ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[3]");
            Console.ResetColor();
            Console.Write("Active: ");

            Console.Write("\nEnter your activity level: ");
            string activityLevel = Console.ReadLine();

            string calorieRange = GetCalorieRecommendation(sex, age, activityLevel);

            if (!string.IsNullOrEmpty(calorieRange))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nRecommended calorie intake: {calorieRange} calories/day.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Invalid input. Please check your details and try again.");
            }
        }

        static string GetCalorieRecommendation(string sex, int age, string activityLevel)
        {
            sex = sex.ToLower();
            if (age >= 2 && age <= 3)
            {
                return GetActivityCalories(activityLevel, "1000", "1000-1400", "1000-1400");
            }
            if (sex == "male")
            {
                if (age >= 4 && age <= 8)
                    return GetActivityCalories(activityLevel, "1200-1400", "1400-1600", "1600-2000");
                if (age >= 9 && age <= 13)
                    return GetActivityCalories(activityLevel, "1600-2000", "1800-2200", "2000-2600");
                if (age >= 14 && age <= 18)
                    return GetActivityCalories(activityLevel, "2000-2400", "2400-2800", "2800-3200");
                if (age >= 19 && age <= 30)
                    return GetActivityCalories(activityLevel, "2400-2600", "2600-2800", "3000");
                if (age >= 31 && age <= 50)
                    return GetActivityCalories(activityLevel, "2200-2400", "2400-2600", "2800-3000");
                if (age >= 51)
                    return GetActivityCalories(activityLevel, "2000-2200", "2200-2400", "2400-2800");
            }
            else if (sex == "female")
            {
                if (age >= 4 && age <= 8)
                    return GetActivityCalories(activityLevel, "1200-1400", "1400-1600", "1400-1800");
                if (age >= 9 && age <= 13)
                    return GetActivityCalories(activityLevel, "1400-1600", "1600-2000", "1800-2200");
                if (age >= 14 && age <= 18)
                    return GetActivityCalories(activityLevel, "1800", "2000", "2400");
                if (age >= 19 && age <= 30)
                    return GetActivityCalories(activityLevel, "1800-2000", "2000", "2400");
                if (age >= 31 && age <= 50)
                    return GetActivityCalories(activityLevel, "1800", "2000", "2200");
                if (age >= 51)
                    return GetActivityCalories(activityLevel, "1600", "1800", "2000-2200");
            }
            return null;
        }

        static string GetActivityCalories(string activityLevel, string sedentary, string moderatelyActive, string active)
        {
            switch (activityLevel)
            {
                case "1":
                    return sedentary;
                case "2":
                    return moderatelyActive;
                case "3":
                    return active;
                default:
                    return null;
            }
        }


        public void AdvancedCalculateRecommendedCal(int age, string sex)
        {

            double weight = GetPositiveDouble("Enter your weight (in kilograms): ");
            double height = GetPositiveDouble("Enter your height (in centimeters): ");

            double activityFactor = GetActivityFactor();

            double bmr = CalculateBMR(sex, age, weight, height);
            double recommendedCalories = bmr * activityFactor;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nYour recommended daily calorie intake is: {Math.Round(recommendedCalories)} calories.");
            Console.ResetColor();
        }

        static double CalculateBMR(string sex, int age, double weight, double height)
        {
            double bmr;
            if (sex == "male")
            {
                bmr = 66.5 + (13.75 * weight) + (5 * height) - (6.75 * age);
            }
            else
            {
                bmr = 655.1 + (9.563 * weight) + (1.85 * height) - (4.676 * age);
            }
            return bmr;
        }

        static double GetActivityFactor()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nSelect your activity level:");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[1] ");
                Console.ResetColor();
                Console.Write("Sedentary (little or no exercise)\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[2] ");
                Console.ResetColor();
                Console.Write("Lightly active (light exercise 1-3 days per week)\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[3] ");
                Console.ResetColor();
                Console.Write("Moderately active (moderate exercise 3-5 days per week)\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[4] ");
                Console.ResetColor();
                Console.Write("Very active (hard exercise 6-7 days per week)\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[5] ");
                Console.ResetColor();
                Console.Write("Extra active (very hard exercise/physical job)\n");
                Console.Write("Enter your choice (1-5): ");

                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    double multiplier;

                    switch (choice)
                    {
                        case 1:
                            multiplier = 1.2;
                            break;
                        case 2:
                            multiplier = 1.375;
                            break;
                        case 3:
                            multiplier = 1.55;
                            break;
                        case 4:
                            multiplier = 1.725;
                            break;
                        case 5:
                            multiplier = 1.9;
                            break;
                        default:
                            throw new Exception("Invalid choice. Please enter a number between 1 and 5.");
                    }

                    return multiplier;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
            }
        }

        static double GetPositiveDouble(string prompt)
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    double value = double.Parse(Console.ReadLine());
                    if (value > 0)
                        return value;
                    throw new Exception("Value must be a positive number.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        public override void Greet()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"\nWelcome {Name}! Delicious meals made with love, just for you!");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("\"(｡♥‿♥｡)\"");
            Console.ResetColor();
        }
    }

    public interface IManageDishes
    {
        void AddDish(Dish dish);
        void RemoveDish(string dishName);
        void DisplayDishes();
        void DisplayWeeklySalesReport();
    }

    public class Owner : Person, IManageDishes
    {
        private List<Dish> dishes;
        private string password;
        private WeeklySalesReport weeklySalesReport;
        private SoundPlayer _soundPlayer;
        private bool _isMusicPlaying;
        private const string DishesFilePath = "dishes.txt"; 

        public Owner(string name, List<Dish> initialDishes, string ownerPassword) : base(name)
        {
            password = ownerPassword;
            weeklySalesReport = new WeeklySalesReport();
            _soundPlayer = new SoundPlayer();
            _isMusicPlaying = false;
            
            dishes = File.Exists(DishesFilePath) ? LoadDishesFromFile() : initialDishes;
        }

        public override void Greet()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔═╗╦ ╦╔╗╔╔═╗╦═╗  ╔═╗╔═╗╔═╗╔═╗╔═╗");
            Console.WriteLine("║ ║║║║║║║║╣ ╠╦╝  ╠═╣║  ║  ║╣ ╚═╗╚═╗");
            Console.WriteLine("╚═╝╚╩╝╝╚╝╚═╝╩╚═  ╩ ╩╚═╝╚═╝╚═╝╚═╝╚═╝");
            Console.ResetColor();
        }

        public bool VerifyPassword(string inputPassword)
        {
            return inputPassword == password;
        }

        public void AddDish(Dish dish)
        {
            dishes.Add(dish);
            SaveDishesToFile(); 
            Console.WriteLine($"\n{dish.Name} has been added.");
        }

        public void RemoveDish(string dishName)
        {
            var dish = dishes.Find(d => d.Name.Equals(dishName, StringComparison.OrdinalIgnoreCase));
            if (dish != null)
            {
                dishes.Remove(dish);
                SaveDishesToFile(); 
                Console.WriteLine($"\n{dishName} has been removed.");
            }
            else
            {
                Console.WriteLine($"\n{dishName} not found.");
            }
        }

        public void SearchDish(string dishName)
        {
            var dish = dishes.Find(d => d.Name.Equals(dishName, StringComparison.OrdinalIgnoreCase));
            if (dish != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{dish.Name} - {dish.CaloriesPerServing} calories, PHP {dish.PricePerServing:F2} per serving (Available servings: {dish.ServingsAvailable})");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"\n{dishName} not found.");
            }
        }

        public void DisplayDishes()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine(@"
 ╔═╗╦  ╦╔═╗╦╦  ╔═╗╔╗ ╦  ╔═╗  ╔╦╗╦╔═╗╦ ╦╔═╗╔═╗
 ╠═╣╚╗╔╝╠═╣║║  ╠═╣╠╩╗║  ║╣    ║║║╚═╗╠═╣║╣ ╚═╗
 ╩ ╩ ╚╝ ╩ ╩╩╩═╝╩ ╩╚═╝╩═╝╚═╝  ═╩╝╩╚═╝╩ ╩╚═╝╚═╝
");
            Console.WriteLine($"{"Dish Name",-20} {"Calories",-10} {"Price (PHP)",-15} {"Available Servings",-15}");
            Console.WriteLine(new string('-', 70));

            foreach (var dish in dishes)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                string formattedPrice = dish.PricePerServing.ToString("F2");
                Console.WriteLine($"{dish.Name,-20} {dish.CaloriesPerServing,-10} {formattedPrice,-15} {dish.ServingsAvailable,-15}");
                Console.ResetColor();
            }
        }
        
        public List<Dish> GetDishes()
        {
            return dishes;
        }

        public bool StopOperation(string inputPassword)
        {
            return VerifyPassword(inputPassword);
        }

        public void DisplayWeeklySalesReport()
        {
            weeklySalesReport.DisplayReport();
        }

        public void RecordSale(double totalSales)
        {
            weeklySalesReport.AddSale(DateTime.Now, totalSales);
        }

        public void PlayMusic(string filePath)
        {
            try
            {
                if (!_isMusicPlaying)
                {
                    _soundPlayer.SoundLocation = filePath;
                    _soundPlayer.PlayLooping();
                    _isMusicPlaying = true;
                    Console.WriteLine("   Music is now playing.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("   Music is already playing.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to play music: {ex.Message}");
                Console.ReadKey();
            }
        }

        public void StopMusic()
        {
            if (_isMusicPlaying)
            {
                _soundPlayer.Stop();
                _isMusicPlaying = false;
                Console.WriteLine("   Music stopped.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("   No music is currently playing.");
                Console.ReadKey();
            }
        }

        
        private void SaveDishesToFile()
        {
            using (StreamWriter writer = new StreamWriter(DishesFilePath, false)) 
            {
                foreach (var dish in dishes)
                {
                    writer.WriteLine($"{dish.Name},{dish.CaloriesPerServing},{dish.PricePerServing},{dish.ServingsAvailable}");
                }
            }
        }
        
        private List<Dish> LoadDishesFromFile()
        {
            if (!File.Exists(DishesFilePath))
                return null;

            var loadedDishes = new List<Dish>();
            string[] lines = File.ReadAllLines(DishesFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 4 &&
                    int.TryParse(parts[1], out int calories) &&
                    double.TryParse(parts[2], out double price) &&
                    int.TryParse(parts[3], out int servings))
                {
                    loadedDishes.Add(new Dish(parts[0], calories, price, servings));
                }
            }
            return loadedDishes;
        }
    }
    public class Dish
    {
        public string Name { get; set; }
        public int CaloriesPerServing { get; set; }
        public double PricePerServing { get; set; }
        public int ServingsAvailable { get; set; }

        public Dish(string name, int calories, double price, int servings)
        {
            Name = name;
            CaloriesPerServing = calories;
            PricePerServing = price;
            ServingsAvailable = servings;
        }
    }

    public class SalesReport
    {
        public DateTime Date { get; set; }
        public double TotalSales { get; set; }

        public SalesReport(DateTime date, double totalSales)
        {
            Date = date;
            TotalSales = totalSales;

        }

        public override string ToString()
        {
            return $"{Date.ToShortDateString()}, PHP {TotalSales:F2}";
        }
    }

    public class WeeklySalesReport
    {
        private List<SalesReport> salesReports;
        private const string FilePath = "weekly_sales_report.txt";

        public WeeklySalesReport()
        {
            salesReports = new List<SalesReport>();
            LoadReports(); // Load reports from file on initialization
        }

        public void AddSale(DateTime date, double totalSales)
        {
            salesReports.Add(new SalesReport(date, totalSales));
            SaveReports(); // Save the updated report to file
        }

        public void DisplayReport()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nWeekly Sales Report:");
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine($"{"Date",-15} {"Total Sales (PHP)",-15}");
            Console.WriteLine(new string('-', 35));

            var weeklySales = salesReports.GroupBy(s => s.Date.Date)
                                           .Select(g => new SalesReport(g.Key, g.Sum(s => s.TotalSales)))
                                           .ToList();

            foreach (var report in weeklySales)
            {
                Console.WriteLine($"{report.Date.ToShortDateString(),-15} {report.TotalSales,-15:F2}");
                Console.WriteLine();
            }
        }

        private void SaveReports()
        {
            using (StreamWriter writer = new StreamWriter(FilePath, false)) // Overwrite the existing file
            {
                foreach (var report in salesReports)
                {
                    writer.WriteLine(report.ToString());
                }
            }
        }

        private void LoadReports()
        {
            if (File.Exists(FilePath))
            {
                string[] lines = File.ReadAllLines(FilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 &&
                        DateTime.TryParse(parts[0], out DateTime date) &&
                        double.TryParse(parts[1].Replace("PHP ", "").Trim(), out double totalSales))
                    {
                        salesReports.Add(new SalesReport(date, totalSales));
                    }
                }
            }
        }
    }
    public class Order
    {
        private List<(Dish Dish, int Servings)> selectedDishes = new List<(Dish, int)>();

        public IReadOnlyList<(Dish Dish, int Servings)> SelectedDishes => selectedDishes;

        public void AddToOrder(Dish dish, int servings)
        {
            selectedDishes.Add((dish, servings));
            Console.WriteLine($"{servings} servings of {dish.Name} added to order.");
        }

        public void DisplayOrderedDishes()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" ╦ ╦╔═╗╦ ╦╦═╗  ╔═╗╦═╗╔╦╗╔═╗╦═╗");
            Console.WriteLine(" ╚╦╝║ ║║ ║╠╦╝  ║ ║╠╦╝ ║║║╣ ╠╦╝");
            Console.WriteLine("  ╩ ╚═╝╚═╝╩╚═  ╚═╝╩╚══╩╝╚═╝╩╚═");
            Console.WriteLine();
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,-25} {1,-10} {2,-15} {3,-10}", "Dish Name", "Servings", "Calories", "Price");
            Console.WriteLine(new string('-', 60));
            Console.ResetColor();


            foreach (var (dish, servings) in selectedDishes)
            {
                var totalCalories = dish.CaloriesPerServing * servings;
                var totalPrice = dish.PricePerServing * servings;

                Console.WriteLine("{0,-25} {1,-10} {2,-15} {3,-10:F2}",
                                  dish.Name,
                                  servings,
                                  totalCalories,
                                  totalPrice);
            }
        }

        public int DisplayTotalCalories()
        {
            int totalCalories = selectedDishes.Sum(item => item.Dish.CaloriesPerServing * item.Servings);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nTotal Calories: {totalCalories}");
            Console.ResetColor();
            return totalCalories;
        }

        public double DisplayTotalPrice()
        {
            double totalPrice = selectedDishes.Sum(item => item.Dish.PricePerServing * item.Servings);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Total Price: {totalPrice:F2}");
            Console.ResetColor();
            return totalPrice;
        }

        public void OrderLogReport(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    foreach (var (dish, servings) in selectedDishes)
                    {
                        writer.WriteLine($"{DateTime.Now}, {dish.Name}, {servings} serving/s, PHP {dish.PricePerServing * servings}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Console.Write("\nEnjoy your meal!");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("(づ｡◕‿‿◕｡)づ");
                Console.ResetColor();
            }
        }
        

    }
    public class Program
    {
        static void Main(string[] args)
        {            
            Owner owner = new Owner("Owner", new List<Dish>(), "owner123");
            bool running = true;

            while (running)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine("        ╔╦╗╔═╗╔╗╔╔═╗╔╗╔╔═╗  ╦  ╔═╗╦═╗╔╗╔╔═╗╔═╗     ");
                Console.WriteLine("        ║║║╠═╣║║║╠═╣║║║║ ╦  ║  ║ ║╠╦╝║║║╠═╣╚═╗     ");
                Console.WriteLine("        ╩ ╩╩ ╩╝╚╝╩ ╩╝╚╝╚═╝  ╩═╝╚═╝╩╚═╝╚╝╩ ╩╚═╝     ");
                Thread.Sleep(450);
                Console.WriteLine("  ╦╔═╔═╗╦  ╔═╗╦═╗╦   ╦╔═╔═╗╦═╗╔═╗╔╗╔╔╦╗╔═╗╦═╗╦ ╦╔═╗");
                Console.WriteLine("  ╠╩╗╠═╣║  ║ ║╠╦╝║───╠╩╗╠═╣╠╦╝║╣ ║║║ ║║║╣ ╠╦╝╚╦╝╠═╣");
                Console.WriteLine("  ╩ ╩╩ ╩╩═╝╚═╝╩╚═╩   ╩ ╩╩ ╩╩╚═╚═╝╝╚╝═╩╝╚═╝╩╚═ ╩ ╩ ╩");
                Console.WriteLine();
                Thread.Sleep(450);
                Console.ResetColor();
                Console.WriteLine("Enter mode: (Customer / Owner)");
                string mode = Console.ReadLine();

                if (mode.Equals("Owner", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter Owner password: ");
                    string password = Console.ReadLine();

                    if (owner.VerifyPassword(password))
                    {
                        Console.Clear();

                        bool exitOwnerMode = false;

                        while (!exitOwnerMode)
                        {
                            owner.Greet();
                            Console.WriteLine("\nWould you like to: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("\n[1] ");
                            Console.ResetColor();
                            Console.Write("Add Dish\n");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("\n[2] ");
                            Console.ResetColor();
                            Console.Write("Remove Dish\n");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("\n[3] ");
                            Console.ResetColor();
                            Console.Write("Search Dish\n");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("\n[4] ");
                            Console.ResetColor();
                            Console.Write("View Sales Report\n");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("\n[5] ");
                            Console.ResetColor();
                            Console.Write("Play Music\n");

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\n[6] ");
                            Console.ResetColor();
                            Console.Write("Exit Owner Mode\n");

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\n[7] ");
                            Console.ResetColor();
                            Console.Write("Stop Operation\n");

                            Console.Write("\nPlease Select an option ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("[1-7]");
                            Console.ResetColor();
                            Console.Write(": ");
                            string ownerChoice = Console.ReadLine();
                            Console.Clear();

                            switch (ownerChoice)
                            {
                                case "1":
                                    owner.DisplayDishes();
                                    Console.WriteLine();
                                    Console.Write("Enter dish name: ");
                                    string dishName = Console.ReadLine();
                                    Console.Write("Enter calories per serving: ");
                                    int calories = int.Parse(Console.ReadLine());
                                    Console.Write("Enter price per serving: ");
                                    double price = double.Parse(Console.ReadLine());
                                    Console.Write("Enter number of servings available: ");
                                    int servings = int.Parse(Console.ReadLine());
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write($"\nAdding {dishName}");
                                    Thread.Sleep(600);
                                    Console.Write(".");
                                    Thread.Sleep(400);
                                    Console.Write(".");
                                    Thread.Sleep(400);
                                    Console.Write(".");
                                    Thread.Sleep(300);
                                    Console.Write(".");
                                    Thread.Sleep(300);
                                    Console.Clear();

                                    Dish newDish = new Dish(dishName, calories, price, servings);
                                    owner.AddDish(newDish);
                                    Console.ResetColor();
                                    owner.DisplayDishes();
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;

                                case "2":
                                    owner.DisplayDishes();
                                    Console.Write("\nEnter dish name to remove: ");
                                    dishName = Console.ReadLine();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write($"\nRemoving {dishName}");
                                    Thread.Sleep(500);
                                    Console.Write(".");
                                    Thread.Sleep(500);
                                    Console.Write(".");
                                    Thread.Sleep(400);
                                    Console.Write(".");
                                    Thread.Sleep(400);
                                    Console.Write(".");
                                    Thread.Sleep(300);
                                    Console.Clear();

                                    owner.RemoveDish(dishName);
                                    Console.ResetColor();
                                    owner.DisplayDishes();
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;

                                case "3":
                                    Console.Write("Enter dish name to search: ");
                                    string searchName = Console.ReadLine();
                                    owner.SearchDish(searchName);
                                    Console.ReadKey();
                                    break;

                                case "4":
                                    owner.DisplayWeeklySalesReport();
                                    Console.ReadKey();
                                    break;

                                case "7":
                                    Console.Write("Enter password to confirm stopping the operation: ");
                                    string stopPassword = Console.ReadLine();
                                    if (owner.StopOperation(stopPassword))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Operation stopped by Owner.");
                                        Console.ResetColor();
                                        exitOwnerMode = true;
                                        running = false;
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Incorrect password. Operation continues.");
                                        Console.ReadKey();
                                    }
                                    break;

                                case "5":
                                    try
                                    {
                                        Console.Clear();
                                        Console.OutputEncoding = System.Text.Encoding.UTF8;
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine();
                                        Console.WriteLine();
                                        Console.WriteLine("        █▀▀▀▀▀▀▀▀▀▀▀▀▀▀█");
                                        Console.WriteLine("      ██▀▀▀██▀▀▀▀▀▀██▀▀▀██");
                                        Console.WriteLine("      █▒▒▒▒▒█▒▀▀▀▀▒█▒▒▒▒▒█");
                                        Console.WriteLine("      █▒▒▒▒▒█▒████▒█▒▒▒▒▒█");
                                        Console.WriteLine("      ██▄▄▄██▄▄▄▄▄▄██▄▄▄██");
                                        Console.WriteLine();
                                        Console.WriteLine("       » [Music Player] «");
                                        Console.WriteLine("       0:00 ─〇───── 0:00");
                                        Console.WriteLine("      ⇄   ◃◃   ▶︎  ▹▹   ↻");
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine();
                                        Console.WriteLine();
                                        Console.Write("   Play Music? (y/n): ");
                                        string play = Console.ReadLine();
                                        Console.ResetColor();

                                        if (play == "y" || play == "Y")
                                        {
                                            owner.PlayMusic("minsan.wav");
                                        }
                                        else if (play == "n" || play == "N")
                                        {
                                            owner.StopMusic();
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException("Invalid input. Please enter 'y' or 'n'.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"An error occurred: {ex.Message}");
                                    }
                                    break;

                                case "6":
                                    exitOwnerMode = true;
                                    Console.WriteLine("Exiting Owner Mode...");
                                    Console.ReadKey();
                                    break;

                                default:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid choice. Please select a valid option.");
                                    Console.ResetColor();
                                    Console.ReadKey();
                                    break;
                            }
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorrect password. Access denied.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else if (mode.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter your name: ");
                    string name = Console.ReadLine();
                    int age = 0;
                    string sex = "";

                    while (true)
                    {
                        try
                        {
                            Console.Write("Enter your age: ");
                            age = int.Parse(Console.ReadLine());
                            if (age < 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                throw new ArgumentOutOfRangeException("Age must be a positive number");
                                Console.ResetColor();
                            }
                            break;
                        }
                        catch (FormatException ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input format, please enter a valid number.");
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"An error occurred: {ex.Message}");
                            Console.ResetColor();
                        }
                    }
                    while (true)
                    {
                        try
                        {
                            Console.Write("Enter your sex (Male/Female): ");
                            sex = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(sex) || !(sex.Equals("Male", StringComparison.OrdinalIgnoreCase) || sex.Equals("Female", StringComparison.OrdinalIgnoreCase)))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                throw new ArgumentException("Pick 'Male' or 'Female'");
                                Console.ResetColor();
                            }
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{ex.Message}");
                            Console.ResetColor();
                        }
                    }

                    Customer customer = new Customer(name, age, sex);
                    customer.Greet();

                    Console.ReadKey();
                    Order order = new Order();

                    bool ordering = true;
                    int currentIndex = 0;
                    const int itemsPerPage = 6;
                    bool orderCompleted = false;

                    while (ordering)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(@"
 ╔═╗╦  ╦╔═╗╦╦  ╔═╗╔╗ ╦  ╔═╗  ╔╦╗╦╔═╗╦ ╦╔═╗╔═╗ 
 ╠═╣╚╗╔╝╠═╣║║  ╠═╣╠╩╗║  ║╣    ║║║╚═╗╠═╣║╣ ╚═╗ 
 ╩ ╩ ╚╝ ╩ ╩╩╩═╝╩ ╩╚═╝╩═╝╚═╝  ═╩╝╩╚═╝╩ ╩╚═╝╚═╝ 
");

                                               
                        Console.WriteLine($"{"Dish Name",-20} {"Calories",-10} {"Price (PHP)",-15} {"Available Servings",-15}");
                        Console.WriteLine(new string('-', 70));
                        
                        var dishes = owner.GetDishes(); 
                       
                        int startIndex = (currentIndex / itemsPerPage) * itemsPerPage;
                        int endIndex = Math.Min(startIndex + itemsPerPage, dishes.Count);
                        
                        for (int i = startIndex; i < endIndex; i++)
                        {
                            var dish = dishes[i];
                            if (i == currentIndex)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }

                            string formattedPrice = dish.PricePerServing.ToString("F2");
                            Console.WriteLine($" {dish.Name,-20} {dish.CaloriesPerServing,-10} {formattedPrice,-15} {dish.ServingsAvailable,-15}");
                            Console.ResetColor();
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[Up/Down] arrows to scroll, [Enter] to select a dish, [D] to finish and pay, [C] to cancel:");
                        Console.ResetColor();

                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            var selectedDish = dishes[currentIndex];
                            if (selectedDish.ServingsAvailable <= 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Sorry, {selectedDish.Name} is out of stock.");
                                Console.ReadKey();
                                Console.ResetColor();
                                continue;
                            }

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"You selected: {selectedDish.Name}");
                            Console.WriteLine($"Enter the number of servings for {selectedDish.Name}:");
                            Console.ResetColor();
                            int servings = 0;

                            while (true)
                            {
                                try
                                {
                                    servings = int.Parse(Console.ReadLine());
                                    if (servings <= 0 || servings > selectedDish.ServingsAvailable)
                                    {
                                        throw new ArgumentOutOfRangeException("Enter a valid number of servings.");
                                    }
                                    break;
                                }
                                catch (FormatException)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid input format. Please enter a valid number.");
                                    Console.ResetColor();
                                }
                                catch (ArgumentOutOfRangeException ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ex.Message);
                                    Console.ResetColor();
                                }
                            }

                            order.AddToOrder(selectedDish, servings);
                        }
                        else if (keyInfo.Key == ConsoleKey.UpArrow)
                        {                            
                            currentIndex = (currentIndex - 1 + dishes.Count) % dishes.Count;
                            
                            if (currentIndex < (currentIndex / itemsPerPage) * itemsPerPage)
                            {
                                currentIndex = Math.Max(0, currentIndex - itemsPerPage);
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.DownArrow)
                        {                            
                            currentIndex = (currentIndex + 1) % dishes.Count;                           
                            if (currentIndex >= (currentIndex / itemsPerPage + 1) * itemsPerPage)
                            {
                                currentIndex = Math.Min(dishes.Count - 1, currentIndex + itemsPerPage);
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.D)
                        {                            
                            ordering = false;
                            orderCompleted = true;

                            foreach (var (dish, servings) in order.SelectedDishes)
                            {
                                dish.ServingsAvailable -= servings;
                            }

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Thank you for your order! Your order has been completed.");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                        else if (keyInfo.Key == ConsoleKey.C)
                        {                            
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Order has been cancelled. Press any key to return to the main menu.");
                            Console.ResetColor();
                            ordering = false;
                            continue;
                        }
                    }
                    if (orderCompleted)
                    {
                        Console.Clear();
                        order.DisplayOrderedDishes();
                        int totalCalories = order.DisplayTotalCalories();
                        double totalPrice = order.DisplayTotalPrice();
                        owner.RecordSale(totalPrice);
                        string logFilePath = "order_log.txt";
                        order.OrderLogReport(logFilePath);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No items were ordered.");
                        Console.ResetColor();
                    }

                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid mode. Please enter 'Customer' or 'Owner'.");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}


     