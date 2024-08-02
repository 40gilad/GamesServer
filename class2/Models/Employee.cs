using LobbyServer.Utils;

namespace LobbyServer.Models
{
    public class Employee
    {
        private int _id;
        private string _name;
        public int id { get { return _id; } }
        public string name { get { return _name; } }


        public Employee(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public static Employee ToEmployee(Dictionary<string, string> emp_dict) 
        {
            try
            {
                int id = -1;
                string name = string.Empty;

                if (emp_dict.ContainsKey("Id") && emp_dict.ContainsKey("Name"))
                {
                    id = int.Parse(emp_dict["Id"]);
                    name = emp_dict["Name"];
                }

                if (id != -1 && name != string.Empty)
                    return new Employee(id, name);

            }
            catch(Exception e){ PrintService.Print("Employee.cs Exceotion: " + e.Message); }
            return null;
        }

        public Dictionary<string, string> ToDict()
        {
            return new Dictionary<string, string>()
            { 
                {
                    "Id", _id.ToString() 
                },
                {
                    "Name", _name 
                } 
            };
        }


    }
}
