using EmployeeManagementMVCAPP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EmployeeManagementMVCAPP.Data;
using EmployeeManagementMVCAPP.Models.Domain;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Web.Helpers;

namespace EmployeeManagementMVCAPP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        // //public IActionResult Index()
        //{
        //   return View();
        // }
        string Baseurl = "https://localhost:7067/";
        //string Baseurl = "http://localhost:5067/";

        public async Task<ActionResult> Index()
        {
            List<EmployeeMaster> EmpInfo = new List<EmployeeMaster>();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage Res = await client.GetAsync("api/EmployeeAPI/GetEmployeeData");


                if (Res.IsSuccessStatusCode)
                {

                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;


                    EmpInfo = JsonConvert.DeserializeObject<List<EmployeeMaster>>(EmpResponse);

                }
                //returning the employee list to view
                return View(EmpInfo);
            }
        }



        [HttpPost]
        public async Task<JsonResult> SearchEmployees([FromBody] SearchModel searchModel)
        {
            List<EmployeeMaster> EmpInfo = new List<EmployeeMaster>();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                var jsonData = searchModel;
                var employeedata = JsonConvert.SerializeObject((jsonData));
                var data = new StringContent(employeedata, Encoding.UTF8, "application/json");

                HttpResponseMessage Res = await client.PostAsync("api/EmployeeAPI/GetEmployeeDataSerach", data);


                if (Res.IsSuccessStatusCode)
                {

                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;


                    EmpInfo = JsonConvert.DeserializeObject<List<EmployeeMaster>>(EmpResponse);

                }
            }

            return Json(EmpInfo.ToList());
        }

        public IActionResult Create()
        {
            ModelState.Clear();

            ViewData["Title"] = "Add Employee";
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(AddUpdateEmployeeModel employeeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime dt = new DateTime(employeeModel.DateOfBirth.Year, employeeModel.DateOfBirth.Month, employeeModel.DateOfBirth.Day);

                    var jsonData = new EmployeeMaster
                    {
                        Name = employeeModel.Name,
                        Email = employeeModel.Email,
                        Department = employeeModel.Department,
                        DateOfBirth = dt
                    };
                    var employeedata = JsonConvert.SerializeObject((jsonData));

                    var data = new StringContent(employeedata, Encoding.UTF8, "application/json");

                    var url = Baseurl + "api/EmployeeAPI/InsertEmployee";
                    using var client = new HttpClient();

                    var response = await client.PostAsync(url, data);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        ModelState.Clear();
                        ViewBag.Message = "Save Successfully.";
                    }
                    else
                    {
                        ViewBag.Message = response.StatusCode + " Error While Saving data.";

                    }


                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Edit(long id)
        {
            ModelState.Clear();

            ViewData["Title"] = "Edit Employee";
            AddUpdateEmployeeModel EmpEditInfo = new AddUpdateEmployeeModel();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage Res = await client.GetAsync("api/EmployeeAPI/GetByID/" + id);

                if (Res.IsSuccessStatusCode)
                {

                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    EmployeeMaster EmpInfo = new EmployeeMaster();
                    EmpInfo = JsonConvert.DeserializeObject<EmployeeMaster>(EmpResponse);
                    EmpEditInfo.Emp_ID=EmpInfo.Emp_ID;
                    EmpEditInfo.Name= EmpInfo.Name;
                    EmpEditInfo.Email= EmpInfo.Email;
                    EmpEditInfo.DateOfBirth=EmpInfo.DateOfBirth;
                    EmpEditInfo.Department= EmpInfo.Department;
                }
            }

            return View(EmpEditInfo);
        }

        [HttpPost]
        public async Task<ActionResult> Edit( AddUpdateEmployeeModel employeeModel)
        {
            try
            {
                long id = employeeModel.Emp_ID;
                DateTime dt = new DateTime(employeeModel.DateOfBirth.Year, employeeModel.DateOfBirth.Month, employeeModel.DateOfBirth.Day);

                var jsonData = new EmployeeMaster
                {
                    Emp_ID=employeeModel.Emp_ID,
                    Name = employeeModel.Name,
                    Email = employeeModel.Email,
                    Department = employeeModel.Department,
                    DateOfBirth = dt
                };
                var employeedata = JsonConvert.SerializeObject((jsonData));
                var data = new StringContent(employeedata, Encoding.UTF8, "application/json");

                var url = Baseurl + "api/EmployeeAPI/UpdateEmployee/"+ employeeModel.Emp_ID;
                using var client = new HttpClient();
                var response = await client.PutAsync(url, data);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    ModelState.Clear();
                    ViewBag.Message = "Save Successfully.";
                }
                else
                {
                    ViewBag.Message = response.StatusCode + " Error While Saving data.";

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        
        [HttpPost]
        public async Task<JsonResult> Delete([FromBody] DeleteModel deleteModel)
        {
            string msg = "";

            try
            {
                var url = Baseurl + "api/EmployeeAPI/DeleteEmployeeById/" + deleteModel.id;
                using var client = new HttpClient();
                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    msg= "Deleted Successfully.";
                }
                else
                {
                    msg= response.StatusCode + " Error While Deleting data.";

                }
                
            }
            catch(Exception ex) 
            {
                msg= "Error While Deleting data.";
            }
            return Json(msg);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}