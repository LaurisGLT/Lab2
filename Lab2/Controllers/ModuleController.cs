using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Lab2.Models;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

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
                db_contex.Close();

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
            db_contex.Close();
            return View("Add_Remove_Module");
        }

        [HttpPost]
        public ActionResult RemoveModuleForm(FormCollection form)
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }
        
            string delquery = String.Format("DELETE FROM module WHERE id ="+form["mod_id"]+"");
            MySqlCommand DeleteModule = new MySqlCommand(delquery, db_contex);
            DeleteModule.ExecuteReader();
            @ViewBag.Deletion = true;
            db_contex.Close();
            return View("Add_Remove_Module");
        }

        [HttpPost]
        public ActionResult EditModuleForm(FormCollection form)
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }
            
            string upquery = String.Format("UPDATE module SET m_name = "+"'"+form["mod_name"]+"'"+ ", m_desc = "+"'"+form["mod_desc"]+"'"+" WHERE id = " + form["module_id"]+"");
            MySqlCommand UpdateModule = new MySqlCommand(upquery, db_contex);
            UpdateModule.ExecuteReader();
            @ViewBag.Update = true;
            db_contex.Close();
            return View("Add_Remove_Module");
        }
      
        public ActionResult Add_Remove_Module()
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }
            db_contex.Close();
            return View();
        }

        
        public ActionResult UniqueModule(int? id, FormCollection form)
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }

            int? Mod_id = id;
            if (String.IsNullOrWhiteSpace(form["Topic_name"]) != true)
            {
                string query =
                    String.Format("INSERT INTO topics (`Mod_id`,`Topic_name`,`Topic_desc`) VALUES ('{0}','{1}','{2}')",
                        Mod_id, form["Topic_name"], form["Topic_desc"]);
                MySqlCommand AddTopic = new MySqlCommand(query, db_contex);
                AddTopic.ExecuteReader();
                @ViewBag.Success = true;
            }



            db_contex.Close();
                db_contex.Open();
          
            MySqlCommand select_module_topics = new MySqlCommand("Select * From topics Where Mod_Id = "+Mod_id+"", db_contex);
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
                                Topic_name = cmd["Topic_name"].ToString(),
                                Topic_desc = cmd["Topic_desc"].ToString()
                            });
                        

                    }
                }




                ViewData["mod_topics"] = Topics;
            

        return View("UnModule");
        }

        public ActionResult UnEditForm(FormCollection form)
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }

            
            if (String.IsNullOrWhiteSpace(form["Topic_name"]) != true)
            {
                string upquery = String.Format("UPDATE topics SET Topic_name = " + "'" + form["Topic_name"] + "'" + ", Topic_desc = " + "'" + form["Topic_desc"] + "'" + " WHERE Topic_Id = " + form["Topic_Id"] + "");
                MySqlCommand UpdateTopic = new MySqlCommand(upquery, db_contex);
                UpdateTopic.ExecuteReader();
                @ViewBag.Update = true;
                db_contex.Close();
            }

            return View("UnModule");
        }

        public ActionResult UnDeleteForm( FormCollection form)
        {
            if (db_contex.State != ConnectionState.Open)
            {
                db_contex.Open();
            }

            
            if (String.IsNullOrWhiteSpace(form["Topic_name"]) != true)
            {
                string delquery = String.Format("DELETE FROM topics WHERE Topic_Id =" + form["Topic_Id"] + "");
                MySqlCommand DeleteModule = new MySqlCommand(delquery, db_contex);
                DeleteModule.ExecuteReader();
                @ViewBag.Deletion = true;
                db_contex.Close();
            }

            return View("UnModule");
        }
    }
}