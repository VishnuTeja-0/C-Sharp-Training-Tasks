using System;
using System.Collections.Generic;
using StudentManagement.Models;

namespace StudentManagement
{
    public class SchoolService
    {
        private School _school;

        public void AddSchool(string schoolName)
        {
            School school = new School();
            school.Name = schoolName;
            school.Students = new List<Student>();
            _school = school;
        }

        public void AddStudentDetails(int rollNo, string name)
        {
            Student student = new Student();
            student.RollNo = rollNo;
            student.Name = name;
            student.Subjects = new List<Subject>();
            _school.Students.Add(student);
        }

        public void AddStudentMarks(int rollNo, List<Subject> subjects)
        {
            Student student = _school.Students.GetStudentByRollNumber(rollNo);
            student.Subjects = subjects;
        }

        public List<Subject> GenerateReportCard(int rollNo)
        {
            Student student = _school.Students.GetStudentByRollNumber(rollNo);
            return student.Subjects;
        }

        public Student GetStudent(int rollNo)
        {
            return _school.Students.GetStudentByRollNumber(rollNo);
        }

        public bool IsStudentAvailable(int rollNo)
        {
            return _school.Students.IsStudentExisting(rollNo);
        }

    }
}
