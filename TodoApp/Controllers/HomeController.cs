using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using TodoApp.Models;
using TodoApp.Models.ViewModels;


namespace TodoApp.Controllers;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
             _logger = logger;
        }

        public IActionResult Index()
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
            
        }

        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            var todoApp = GetById(id);
            return Json(todoApp);
        }

        internal TodoAppViewModel GetAllTodos()
        {
            List<TodoItem> todoList = new();

                using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
                {
                    using (var tableCmd = con.CreateCommand())
                    {
                        con.Open();
                        tableCmd.CommandText ="SELECT * FROM todo";

                        using (var reader = tableCmd.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    todoList.Add(
                                        new TodoItem
                                        {
                                            Id= reader.GetInt32(0),
                                            Name = reader.GetString(1)
                                        });
                                }
                            }
                            else
                            {
                                return new TodoAppViewModel
                                {
                                    TodoList = todoList
                                };
                            }
                        };
                    }
                }
            return new TodoAppViewModel
            {
                TodoList = todoList
            };
        }

        internal TodoItem GetById(int id)
        {
            var todoApp = new TodoItem{ Id=id, Name="Todo" };

            using (var connection = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo WHERE Id = '{id}'";

                    using (var reader= tableCmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            reader.Read();
                            todoApp.Id = reader.GetInt32(0);
                            todoApp.Name = reader.GetString(1);
                        } 
                        else
                        {
                            return todoApp;
                        }    
                    };
                }
            }
                return todoApp;
        }

        public RedirectResult Insert(TodoItem todoApp)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd= con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"INSERT INTO todo (name) VALUES('{todoApp.Name}')";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }     
            }
                return Redirect("http://localhost:5298/");
        }
    

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = $"DELETE from todo WHERE Id= '{id}'";
                tableCmd.ExecuteNonQuery();
            }
        }
        return Json(new {});
    }

    public RedirectResult Update(TodoItem todoApp)
    {
        using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = $"UPDATE todo SET name = '{todoApp.Name}' WHERE Id = $'{todoApp.Id}'";
                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
            return Redirect("http://localhost:5298/");
        }
    }


        
        

    

    

