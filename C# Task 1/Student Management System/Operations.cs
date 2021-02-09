using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentManagement
{
    class Operations
    {
        public void AddMarks(School school)
        {
            int rollNo = 0;
            Dictionary<string, int> marks = new Dictionary<string, int>();
            int allMarksFlag = 0;
            Console.WriteLine("Enter Student Roll number : ");
            while (true)
            {
                string strin = Console.ReadLine();
                if (strin.All(char.IsDigit))
                {
                    rollNo = Int32.Parse(strin);
                    if (!school.GetStudentList().Any(i => i.GetRollNumber() == rollNo))
                    {
                        Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
                        return;
                    }
                    else
                    {
                        foreach (string subject in school.GetSubjects())
                        {
                            marks[subject] = EnterSubjectMarks(subject);
                        }
                        allMarksFlag = 1;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter numerical input.");
                }
                if (allMarksFlag == 1)
                {
                    break;
                }
            }
            Student student = school.GetStudentList().FirstOrDefault(i => i.GetRollNumber() == rollNo);
            foreach(KeyValuePair<string, int> k in marks)
            {
                student.SetSubjectMarks(k);
            }
            Console.WriteLine("Student marks are added successfully\n");
        }

        public int EnterSubjectMarks(string subject)
        {
            Console.WriteLine("Enter Marks scored in " + subject + " : ");
            while (true)
            {
                string marksinput = Console.ReadLine();
                if (marksinput.All(char.IsDigit) && Int32.Parse(marksinput) <= 100)
                {
                    return Int32.Parse(marksinput);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter numerical marks out of 100");
                }
            }
        }

        public void GenerateReportCard(School school)
        {
            Console.WriteLine("Enter Student Roll Number : ");
            while (true)
            {
                String strin = Console.ReadLine();
                if (strin.All(char.IsDigit))
                {
                    int rollNo = Int32.Parse(strin);
                    if (school.GetStudentList().Any(i => i.GetRollNumber() == rollNo))
                    {
                        Student student = school.GetStudentList().FirstOrDefault(i => i.GetRollNumber() == rollNo);
                        if (student.GetSubjectMarks().Count() > 0)
                        {
                            Console.WriteLine("Student Roll Number : " + rollNo);
                            Console.WriteLine("Student Name : " + student.GetName());
                            Console.WriteLine("---------------");
                            Dictionary<string, int> marks = student.GetSubjectMarks();
                            foreach(string subject in school.GetSubjects())
                            {
                                Console.WriteLine(subject + " : " + marks[subject]);
                            }
                            Console.WriteLine("---------------\n");
                            int sum = marks.Values.ToList().Sum();
                            Console.WriteLine("Total Marks : " + sum);
                            float percentage = sum / 600 * 100;
                            Console.WriteLine("Percentage : " + percentage);
                            Console.WriteLine("---------------\n");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Student with given roll number does not have record of marks. Please try again\n");
                            Console.WriteLine("Press Any Key to Continue");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Student with given roll number does not exist. Please try again.\n");
                        Console.WriteLine("Press Any Key to Continue");
                        return;

                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter numerical input.");
                }
            }
            Console.WriteLine("Press any key to continue");
        }
    }
}
