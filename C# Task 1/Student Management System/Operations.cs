using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentManagement.Models;

namespace StudentManagement
{
    class Operations : Helper
    {
        public void EnterSchoolName(School school)
        {
            cw("Enter The School Name: ");
            string userInput = ValidTextInput();
            school.SetSchoolName(userInput);
        }

        public void SetStudentDetails(School school, Student student)
        {
            cw("Enter Student Roll number : ");
            int rollNo = ValidNumberInput();
            if (!school.GetStudentList().Any(i => i.GetRollNumber() == rollNo)){
                cw("Enter Student Name : ");
                string name = ValidTextInput();
                student.SetStudentDetails(rollNo, name);
                cw("Student details are added successfully\n");
            }
            else
            {
                cw("Student with given roll number already exists. Please try again.");
                return;
            }
            
        }

        public void AddStudentMarks(School school)
        {
            cw("Enter Student Roll number : ");
            int rollNo = ValidNumberInput();
            if (!school.GetStudentList().Any(i => i.GetRollNumber() == rollNo))
            {
                cw("Student with given roll number does not exist. Please try again.\n");
                return;
            }
            else
            {
                Student student = school.GetStudentList().FirstOrDefault(i => i.GetRollNumber() == rollNo);
                foreach (string subjectname in school.GetSubjects())
                {
                    Subject subject = new Subject();
                    subject.SetName(subjectname);
                    subject.SetMarks(EnterSubjectMarks(subjectname));
                    student.SetSubjectMarks(subject);
                }
            }
            cw("Student marks are added successfully\n");
        }

        public int EnterSubjectMarks(string subject)
        {
            cw(String.Format("Enter Marks scored in {0} : ", subject));
            int marks;
            while (true)
            {
                marks = ValidNumberInput();
                if(marks < 100)
                {
                    break;
                }
                else
                {
                    cw("Please enter numerical input out of 100");
                }
            }
            return marks;
        }

        public void GenerateReportCard(School school)
        {
            cw("Enter Student Roll Number : ");
            int rollNo = ValidNumberInput();
            if (school.GetStudentList().Any(i => i.GetRollNumber() == rollNo))
            {
                Student student = school.GetStudentList().FirstOrDefault(i => i.GetRollNumber() == rollNo);
                if (student.GetSubjectMarks().Count() > 0)
                {
                    cw($"Student Roll Number : {rollNo}");
                    cw($"Student Name : {student.GetName()}");
                    cw("Student Marks");
                    cw("---------------");
                    List<int> setOfMarks = new List<int>();
                    foreach(Subject subject in student.GetSubjectMarks())
                    {
                        cw($"{subject.GetName()} : {subject.GetMarks()}");
                        setOfMarks.Add(subject.GetMarks());
                    }
                    cw("---------------\n");
                    cw($"Total Marks : {setOfMarks.Sum()}");
                    double percentage = setOfMarks.Average();
                    cw($"Percentage : {percentage}");
                    cw("---------------\n");
                }
                else
                {
                    cw("Student with given roll number does not have record of marks. Please try again\n");
                    cw("Press Any Key to Continue");
                    return;
                }
            }
            else
            {
                cw("Student with given roll number does not exist. Please try again.\n");
                cw("Press Any Key to Continue");
                return;
            }
            cw("Press any key to continue");
        }
    }
}
