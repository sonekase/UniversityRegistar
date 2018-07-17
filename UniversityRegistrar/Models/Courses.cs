using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UniversityRegistrar;

namespace UniversityRegistrar.Models
{
  public class Course
  {
    private int id;
    private string courseName;
    private string courseNumber;

    public Student (string newCourseName, string newCourseNumber, int newId = 0)
    {
      id = newId;
      courseName = newCourseName;
      courseNumber = newCourseNumber;
    }
    public int GetId()
    {
      return id;
    }
    public int GetCourseName()
    {
      return courseName;
    }
    public int GetCourseNumber()
    {
      return courseNumber;
    }
    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = (this.GetId() == newCourse.GetId());
        bool courseNameEquality = (this.GetCourseName() == newCourse.GetCourseName());
        bool courseNumberEquality = (this.GetCourseNumber() == newCourse.GetCourseNumber());
        return (idEquality && courseNameEquality && courseNumberEquality);
      }
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses (course_name, course_number) VALUES (@inputName, @inputNumber);";
      MySqlParameter newName = new MySqlParameter();
      newName.ParameterName = "@inputName";
      newName.Value = this.name;
      cmd.Parameters.Add(newName);
      MySqlParameter newCourse = new MySqlParameter();
      newCourse.ParameterName = "@inputCourse";
      newCourse.Value = this.Course;
      cmd.Parameters.Add(newCourse);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }
    }
    public static List<Course> GetAll()
    {
      List <Course> newCourse = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);
        Course newCourse = new Course(courseName, courseNumber, id);
        allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allRestaurants;
    }
    public static Course FindById(int searchId)
    {
      int id = 0;
      string courseName = "";
      string courseId = "";
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = @idMatch;";
      MySqlParameter parameterId = new MySqlParameter();
      parameterId.ParameterName = "@idMatch";
      parameterId.Value = searchId;
      cmd.Parameters.Add(parameterId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        courseName = rdr.GetString(1);
        courseId = rdr.GetString(2);
      }
      Course foundCourse =  new Course(courseName, courseId, id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundCourse;
    }
    public List<Student> GetStudents()
    {
      List<Student> myStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT students.* FROM courses JOIN student_courses ON(courses.id = student_courses.course_id) JOIN students ON (student_course.student_id = students.id) WHERE courses.id = @idParameter;";
      MySqlParameter parameterId = new MySqlParameter();
      parameterId.ParameterName = "@idParameter";
      parameterId.Value =this.id;
      cmd.Parameters.Add(parameterId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.String(1);
        string enrollmentDate = rdr.String(2);
        Student foundStudent = new Student(name, enrollmentDate, id);
        myStudents.Add(foundStudent);
      }
      if (conn != null)
      {
        conn.Dispose();
      }
      return myStudents;
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM courses;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
