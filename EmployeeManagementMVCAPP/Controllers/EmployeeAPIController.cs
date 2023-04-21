using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using EmployeeManagementMVCAPP.Data;
using EmployeeManagementMVCAPP.Models.Domain;
using EmployeeManagementMVCAPP.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagementMVCAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeAPIController : ControllerBase
    {
        // GET: api/<ValuesController>/GetEmployeeData
        private readonly EmployeeDbContext _employeeDbContext;
        public EmployeeAPIController(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
        [HttpGet("GetEmployeeData")]
        public List<EmployeeMaster> Get()
        {
            List<EmployeeMaster> lstEmployeeMasters = new List<EmployeeMaster>();
            lstEmployeeMasters = _employeeDbContext.EmployeeMasters.ToList();
            return lstEmployeeMasters;
        }
        [HttpPost("GetEmployeeDataSerach")]
        public List<EmployeeMaster> GetEmployeeDataSerach([FromBody] SearchModel searchModel)
        {
            string searchvalue = searchModel.employeesearch;
            List<EmployeeMaster> lstEmployeeMasters = new List<EmployeeMaster>();
            if (!string.IsNullOrEmpty(searchvalue))
                lstEmployeeMasters = _employeeDbContext.EmployeeMasters.Where(x => x.Name.ToLower().Contains(searchvalue.ToLower()) || x.Email.ToLower().Contains(searchvalue.ToLower()) || x.Department.ToLower().Contains(searchvalue.ToLower())).ToList();
            else
                lstEmployeeMasters = _employeeDbContext.EmployeeMasters.ToList();

            return lstEmployeeMasters;
        }

        // GET api/<ValuesController>/GetByID/5
        [HttpGet("GetByID/{id}")]
        public EmployeeMaster Get(long id)
        {
            EmployeeMaster employeeMaster = new EmployeeMaster();
            employeeMaster = _employeeDbContext.EmployeeMasters.Where(x => x.Emp_ID == id).FirstOrDefault();
            return employeeMaster;
        }

        // POST api/<ValuesController>/InsertEmployee
        [HttpPost("InsertEmployee")]
        public void Post([FromBody] EmployeeMaster employeeMaster)
        {
            employeeMaster.Emp_ID = 0;
            _employeeDbContext.EmployeeMasters.Add(employeeMaster);
            _employeeDbContext.SaveChanges();

        }

        // PUT api/<ValuesController>/UpdateEmployee/5
        [HttpPut("UpdateEmployee/{id}")]
        public void Put(long id, [FromBody] EmployeeMaster employeeMaster)
        {
            EmployeeMaster editEmployeeMaster = new EmployeeMaster();
            editEmployeeMaster = _employeeDbContext.EmployeeMasters.Where(x => x.Emp_ID == id).FirstOrDefault();
            if (editEmployeeMaster != null)
            {
                editEmployeeMaster.Email = employeeMaster.Email;
                editEmployeeMaster.Name = employeeMaster.Name;
                editEmployeeMaster.DateOfBirth = Convert.ToDateTime(employeeMaster.DateOfBirth.ToString("yyyy-MM-dd"));
                editEmployeeMaster.Department = employeeMaster.Department;
                _employeeDbContext.SaveChanges();

            }
        }

        // DELETE api/<ValuesController>/DeleteEmployeeById/5
        [HttpDelete("DeleteEmployeeById/{id}")]
        public void Delete(long id)
        {
            EmployeeMaster editEmployeeMaster = new EmployeeMaster();
            editEmployeeMaster = _employeeDbContext.EmployeeMasters.Where(x => x.Emp_ID == id).FirstOrDefault();
            if (editEmployeeMaster != null)
            {
                _employeeDbContext.EmployeeMasters.Remove(editEmployeeMaster);
                _employeeDbContext.SaveChanges();

            }
        }
    }
}
