using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2.Models;
using MySql.Data.MySqlClient;

namespace Lab2.Controllers
{
    public class ModuleController : Controller
    {

        MySqlConnection db_contex = new MySqlConnection("server=localhost;uid=root;database=moodle");//db connection

        // GET: Module
        public ActionResult Index()
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();   
            }

            MySqlCommand select_all_modules = new MySqlCommand("Select * From module Order By id", db_contex);
            MySqlDataReader cmd = select_all_modules.ExecuteReader();

            List<ModuleList> all_Modules = new List<ModuleList>();


            if (cmd.FieldCount > 0)
            {
                while (cmd.Read())
                {
                    all_Modules.Add(new ModuleList()
                    {
                        id = int.Parse(cmd["id"].ToString()),
                        m_name = cmd["m_name"].ToString(),
                        m_desc = cmd["m_desc"].ToString()
                    });
                }
            }
            


            ViewData["Modules"] = all_Modules;
            cmd.Close();
            return View();
        }

        [HttpPost]
        public ActionResult AddModuleForm(FormCollection form)
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }

            string query = String.Format("INSERT INTO module (`m_name`,`m_desc`) VALUES ('{0}','{1}')", form["m_name"], form["m_desc"]);
            MySqlCommand AddModule = new MySqlCommand(query, db_contex);
            AddModule.ExecuteReader();
            @ViewBag.Success = true;
            return View("Add_Remove_Module");
        }

        public ActionResult Add_Remove_Module()
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }

            return View();
        }
       
        public ActionResult UniqueModule(int? id, FormCollection form)
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }

            int? Mod_id = id;


            string query = String.Format("INSERT INTO topics (`Mod_id`,`Topic_name`,`Topic_desc`) VALUES ('{0}','{1}','{2}')", Mod_id ,form["Topic_name"], form["Topic_desc"]);
            MySqlCommand AddTopic = new MySqlCommand(query, db_contex);
            AddTopic.ExecuteReader();
            @ViewBag.Success = true;

            MySqlCommand select_module_topics = new MySqlCommand("Select * From topics Order By Topic_Id", db_contex);
            MySqlDataReader cmd = select_module_topics.ExecuteReader();

            List<TopicList> Topics = new List<TopicList>();


            if (cmd.FieldCount > 0)
            {
                while (cmd.Read())
                {
                    Topics.Add(new TopicList()
                    {
                        Topic_Id = int.Parse(cmd["Topic_Id"].ToString()),
                        Mod_Id = int.Parse(cmd["Mod_Id"].ToString()),
                        Topic_name = cmd["m_name"].ToString(),
                        Topic_desc = cmd["m_desc"].ToString()
                    });
                }
            }


            cmd.Close();
            ViewData["mod_topics"] = Topics;



            return View("UnModule");
        }


    }
}