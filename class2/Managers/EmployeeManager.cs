using LobbyServer.Models;
using LobbyServer.Services;
namespace LobbyServer.Managers
{
    public class EmployeeManager
    {

        #region Singleton
        public static EmployeeManager _instance;
        public static EmployeeManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmployeeManager();
                return _instance;
            }
        }
        #endregion
        public EmployeeManager(){}

        public Employee GetEmployee(int id)
        {
            Dictionary<string, string> res_emp = null;
            if (id  >= 0)
                res_emp = RedisService.GetEmployeeDetails(id.ToString());
            else
                return null;

            if (res_emp!=null && res_emp.Count<=0)
                return null;

            if (res_emp!=null)
                return Employee.ToEmployee(res_emp);
            return null;
        }

        public bool AddEmployee(Employee emp)
        {
            string emp_id = emp.id.ToString();
            if (emp == null || emp.id<0)
                return false;

            Dictionary<string, string> temp_emp = RedisService.GetEmployeeDetails(emp_id);
            if (temp_emp.Count == 0) // Uid not exist
            {
                RedisService.SetEmployeeDetails(emp_id, emp.ToDict());
                return true;
            }
            else return false;
        }

        public bool UpdateEmployee(Employee emp)
        {
            Dictionary<string, string> temp_emp = RedisService.GetEmployeeDetails(emp.id.ToString());
            if (emp == null || temp_emp.Count <= 0)
                return false;
            RedisService.SetEmployeeDetails(emp.id.ToString(), emp.ToDict());
            return true;
        }

        public bool DeleteEmployee(int id)
        {
            bool is_success=RedisService.DeleteEmployeeDetails(id.ToString());
            if (id<0 || ! is_success)
                return false;
            return true;
        }
    }
}
