using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Lab2.Models;
using MySql.Data.MySqlClient;
namespace asp1pask.Controllers
{
    public class StudentController : Controller
    {
        MySqlConnection db_contex = new MySqlConnection("server=localhost;uid=root;database=moodle");
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addStudent(FormCollection form)
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }
            string query = String.Format("INSERT INTO studentai (`s_vardas`,`s_pavarde`,`s_kursas`) VALUES ('{0}','{1}','{2}')", form["studentName"], form["studentSurName"], form["studyCourse"]);
            MySqlCommand select_all_student = new MySqlCommand(query, db_contex);
            select_all_student.ExecuteReader();
            @ViewBag.Success = true;
            return View("Index");
        }

        public ActionResult StudentList()
        {

            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }

            MySqlCommand select_all_student = new MySqlCommand("Select * From studentai Order By id", db_contex);
            MySqlDataReader cmd = select_all_student.ExecuteReader();

            List<StudentList> all_students = new List<StudentList>();


            if (cmd.FieldCount > 0)
            {
                while (cmd.Read())
                {
                    all_students.Add(new StudentList()
                    {
                        id = int.Parse(cmd["id"].ToString()),
                        s_vardas = (cmd["s_vardas"].ToString()),
                        s_pavarde = (cmd["s_pavarde"].ToString()),
                        s_modulis = (cmd["s_modulis"].ToString()),
                    });
                }
            }

            ViewData["Students"] = all_students;
            return View();
        }
    }
}