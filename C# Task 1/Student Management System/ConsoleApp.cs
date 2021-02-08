using System;
using System.Collections.Generic;
using System.Linq;

namespace Student_Management_System
{
    class ConsoleApp
    {
        private string school_name;
        private Dictionary<int, string> student_names = new Dictionary<int, string>();
        private Dictionary<int, Dictionary<string, int>> student_marks = new Dictionary<int, Dictionary<string, int>>();

        // To perform operation of printing progress card of existing student with existing marks record
        private void ShowProgress()
        {
            Console.WriteLine("Enter Student Roll Number : ");
            while (true)
            {
                String strin = Console.ReadLine();
                if (strin.All(char.IsDigit))
                {
                    int roll_no = Int32.Parse(strin);
                    if (this.student_names.ContainsKey(roll_no))
                    {
                        if (this.student_marks.ContainsKey(roll_no))
                        {
                            Console.WriteLine("Student Roll Number : " + roll_no);
                            Console.WriteLine("Student Name : " + this.student_names[roll_no]);
                            Console.WriteLine("---------------");
                            Dictionary<string, int> marks = this.student_marks[roll_no];
                            Console.WriteLine("Telugu : " + marks["Telugu"]);
                            Console.WriteLine("Hindi : " + marks["Hindi"]);
                            Console.WriteLine("English : " + marks["English"]);
                            Console.WriteLine("Maths : " + marks["Maths"]);
                            Console.WriteLine("Science : " + marks["Science"]);
                            Console.WriteLine("Social : " + marks["Social"]);
                            Console.WriteLine("---------------\n");
                            int sum = marks.Values.ToList().Sum();
                            Console.WriteLine("Total Marks : " + sum);
                            int percentage = sum / 6 * 100;
                            Console.WriteLine("Percentage : " + percentage);
                            Console.WriteLine("---------------\n");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Student with given roll number does not have record of marks. Please try again\n");
                            this.PrintMenu();
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
                        this.PrintMenu();
                        return;

                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter numerical input.");
                }
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(false);
            Console.Clear();
            this.PrintMenu();
        }

        // To perform operation for adding marks for existing student
        private void AddMarks()
        {
            int roll_no = 0;
            Dictionary<string, int> marks = new Dictionary<string, int>();
            int all_marks_flag = 0;
            Console.WriteLine("Enter Student Roll number : ");
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsDigit))
                {
                    roll_no = Int32.Parse(strin);
                    if (!this.student_names.ContainsKey(roll_no))
                    {
                        Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
                        this.PrintMenu();
                        return;
                    }
                    else
                    {
                        string marksinput;
                        Console.WriteLine("Enter Marks scored in Telugu");
                        while (true)
                        {
                            marksinput = Console.ReadLine();
                            if (marksinput.All(char.IsDigit) && Int32.Parse(marksinput) <= 100)
                            {
                                marks["Telugu"] = Int32.Parse(marksinput);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter numerical marks out of 100");
                            }
                        }
                        Console.WriteLine("Enter Marks scored in Hindi");
                        while (true)
                        {
                            marksinput = Console.ReadLine();
                            if (marksinput.All(char.IsDigit) && Int32.Parse(marksinput) <= 100)
                            {
                                marks["Hindi"] = Int32.Parse(marksinput);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter numerical marks out of 100");
                            }
                        }
                        Console.WriteLine("Enter Marks scored in English");
                        while (true)
                        {
                            marksinput = Console.ReadLine();
                            if (marksinput.All(char.IsDigit) && Int32.Parse(marksinput) <= 100)
                            {
                                marks["English"] = Int32.Parse(marksinput);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter numerical marks out of 100");
                            }
                        }
                        Console.WriteLine("Enter Marks scored in Maths");
                        while (true)
                        {
                            marksinput = Console.ReadLine();
                            if (marksinput.All(char.IsDigit) && Int32.Parse(marksinput) <= 100)
                            {
                                marks["Maths"] = Int32.Parse(marksinput);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter numerical marks out of 100");
                            }
                        }
                        Console.WriteLine("Enter Marks scored in Science");
                        while (true)
                        {
                            marksinput = Console.ReadLine();
                            if (marksinput.All(char.IsDigit) && Int32.Parse(marksinput) <= 100)
                            {
                                marks["Science"] = Int32.Parse(marksinput);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter numerical marks out of 100");
                            }
                        }
                        Console.WriteLine("Enter Marks scored in Social");
                        while (true)
                        {
                            marksinput = Console.ReadLine();
                            if (marksinput.All(char.IsDigit) && Int32.Parse(marksinput) <= 100)
                            {
                                marks["Social"] = Int32.Parse(marksinput);
                                all_marks_flag = 1;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter numerical marks out of 100");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter numerical input.");
                }
                if (all_marks_flag == 1)
                {
                    break;
                }
            }
            this.student_marks.Add(roll_no, value: marks);
            Console.WriteLine("Student marks are added successfully\n");
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey(false);
            Console.Clear();
            this.PrintMenu();
        }

        // To perform operation of adding new student details
        private void AddStudent()
        {
            int roll_no;
            string name;
            Console.WriteLine("Enter Student Roll number : ");
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsDigit))
                {
                    int num = Int32.Parse(strin);
                    if (student_names.ContainsKey(num))
                    {
                        Console.WriteLine("Student with given roll number already exists. Please try again\n");
                        this.PrintMenu();
                        return;
                    }
                    else
                    {
                        roll_no = num;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter numerical input.");
                }
            }
            Console.WriteLine("Enter Student Name : ");
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsLetter))
                {
                    name = strin;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Student Name can only be alphabetical. Please try again.");
                }
            }
            this.student_names.Add(roll_no, name);
            Console.WriteLine("Student details are added successfully\n");
            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey(false);
            Console.Clear();
            this.PrintMenu();
        }

        // For printing main menu
        private void PrintMenu()
        {
            Console.WriteLine("Welcome to " + this.school_name + " Student Information Management");
            Console.WriteLine(new String('-', 50));
            Console.WriteLine("1. Add student\n2. Add marks for student\n3. Show student progress card");
            Console.WriteLine("Please provide valid input from menu options :");
        }

        // For main student management options
        private void MainMenu()
        {
            Console.Clear();
            PrintMenu();
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsDigit))
                {
                    int opt = Convert.ToInt32(strin);
                    switch (opt)
                    {
                        case 1:
                            this.AddStudent();
                            break;
                        case 2:
                            this.AddMarks();
                            break;
                        case 3:
                            this.ShowProgress();
                            break;
                        default:
                            Console.WriteLine("Out of bounds. Please enter a number among the given options.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input type. Please enter numerical input.");
                }
            }
        }

        // For startup message and prompt
        public void StartUp()
        {
            Console.WriteLine("Enter The School Name: ");
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsLetter))
                {
                    this.school_name = strin;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input. School Name can only be alphabetical. Please try again.");
                }
            }
            this.MainMenu();
        }
        static void Main(string[] args)
        {
            ConsoleApp conapp = new ConsoleApp();
            conapp.StartUp();
        }
    }
}
