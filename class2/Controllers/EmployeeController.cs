using LobbyServer.Managers;
using LobbyServer.Models;
using LobbyServer.Services;
using LobbyServer.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EmployeeController : Controller
    {
        #region Get Employee
        [HttpGet("GetEmployee/{id}")]
        public Dictionary<string, object> GetEmployee(int id)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Response: ", "GetEmployee " + id);
            PrintService.Print(txt: "GetEmployee with id " + id, get: true);
            Employee emp = EmployeeManager.Instance.GetEmployee(id);

            if (emp != null)
            {
                ret.Add("Message", "Success");
                ret.Add("Id", emp.id);
                ret.Add("Name", emp.name);
                PrintService.Print(txt: "Returns employee " + emp.name);
            }
            else
            {
                ret.Add("Message", "Employee " + id + " is <0 or not exist");
                PrintService.Print(txt: "Returned error: Emplyoee id is <0 or not exist ");
            }
            return ret;
        }

        #endregion

        #region Post Employee
        [HttpPost("PostEmployee")]
        public Dictionary<string, object> PostEmployee([FromBody] Dictionary<string, object> data)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Response: ", "PostEmployee");
            try
            {
                PrintService.Print(txt: "PostEmployee", post: true);
                if (data.ContainsKey("Id") && data.ContainsKey("Name"))
                {
                    int new_emp_id = int.Parse(data["Id"].ToString());
                    string new_emp_name = data["Name"].ToString();
                    Employee curr = new Employee(new_emp_id, new_emp_name);

                    if (EmployeeManager.Instance.AddEmployee(curr))
                    {
                        PrintService.Print(txt: "Employee " + data["Name"] + ", " + data["Id"] + " Added to EmployeeManeger");
                        ret.Add("Message", "Employee " + data["Name"] + ", " + data["Id"] + " Added to EmployeeManeger");
                    }
                    else
                    {
                        ret.Add("Message", " id is <0, Employee is null or already in the system");
                        PrintService.Print(txt: "Employee " + data["Name"] + ", " + data["Id"] + " is null or already in the system");
                    }

                }
            }
            catch (Exception e)
            {
                ret.Add("Exception Message", e);
            }
            return ret;
        }

        #endregion

        #region Put Employee
        [HttpPut("PutEmployee")]
        public Dictionary<string, object> PutEmployee([FromBody] Dictionary<string, object> data)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Response: ", "PutEmployee");
            try
            {
                PrintService.Print(txt: "PutEmployee", put: true);
                if (data.ContainsKey("Id") && data.ContainsKey("Name"))
                {
                    int emp_id = int.Parse(data["Id"].ToString());
                    string new_emp_name = data["Name"].ToString();
                    Employee curr = new Employee(emp_id, new_emp_name);
                    if (EmployeeManager.Instance.UpdateEmployee(curr))
                    {
                        PrintService.Print(txt: "Employee " + emp_id + " Updated name: " + new_emp_name);
                        ret.Add("Message", "Employee " + emp_id + " Updated name: " + new_emp_name);
                    }
                    else
                    {
                        ret.Add("Message", "Employee " + emp_id + " is null or not exist");
                        PrintService.Print(txt: "Returned error: Emplyoee" + emp_id + " is null or not exist ");
                    }

                }
            }
            catch (Exception e)
            {
                ret.Add("Exception Message", e);
            }
            return ret;
        }

        #endregion

        #region Delete Employee
        [HttpDelete("DeleteEmployee/{id}")]
        public Dictionary<string, object> DeleteEmployee(int id)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Response: ", "DeleteEmployee");

            PrintService.Print(txt: "DeleteEmployee", delete: true);
            if (EmployeeManager.Instance.DeleteEmployee(id))
            {
                PrintService.Print(txt: "Employee " + id + " Removed");
                ret.Add("Message", "Employee " + id + " Removed");
            }
            else
            {
                ret.Add("Message", "Employee " + id + "<0, null or not exist");
                PrintService.Print(txt: "Returned error: Emplyoee" + id + "<0, null or not exist ");
            }
            return ret;
        }
        #endregion

    }
}
