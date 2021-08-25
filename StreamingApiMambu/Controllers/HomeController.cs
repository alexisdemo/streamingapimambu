using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StreamingApiMambu.Models;
using StreamingApiMambu.entities;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using MambuStreamingReader.entities;
using MambuStreamingReader;
using MambuDbServer.entities;
using MambuDbServer;
using System.Text;
using System.IO;

namespace StreamingApiMambu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            RequestStreamingApi req = new RequestStreamingApi();

            try
            {
                if (TempData["ApiKey"] != null ||
                    TempData["MambuServer"] != null ||
                    TempData["Topic"] != null ||
                    TempData["TimeExecution"] != null)
                {
                    req.ApiKey = TempData["ApiKey"].ToString();
                    req.MambuServer = TempData["MambuServer"].ToString();
                    req.Topic = TempData["Topic"].ToString();
                    req.TimeExecution = int.Parse(TempData["TimeExecution"].ToString());
                }

                TempData.Keep("ApiKey");
                TempData.Keep("MambuServer");
                TempData.Keep("Topic");
                TempData.Keep("TimeExecution");
            }
            catch (Exception ex)
            {
                ViewBag.SystemWarningMessage = ex.Message;
            }

            return View(req);
        }

        [HttpPost]
        public IActionResult Index(RequestStreamingApi request)
        {

            try
            {
                ValidationContext context = new ValidationContext(request, null, null);
                List<ValidationResult> validationResults = new List<ValidationResult>();

                bool valid = Validator.TryValidateObject(request, context, validationResults, true);


                ValidationResult valWrite = validateWriteInDB(request);

                if (valid && valWrite == null)
                {

                    MambuClientService mcs = new MambuClientService(request.ApiKey, request.MambuServer, request.Topic, request.TimeExecution);
                    TempData["ApiKey"] = request.ApiKey;
                    TempData["MambuServer"] = request.MambuServer;
                    TempData["Topic"] = request.Topic;
                    TempData["TimeExecution"] = request.TimeExecution;
                    TempData["TimeStart"] = DateTime.Now;
                    mcs.Ejecutar();

                    TempData["EnableExecute"] = "false";
                    return View();
                }
                else
                {
                    if (valWrite != null)
                        validationResults.Add(valWrite);

                    ViewBag.SystemWarningMessage = validationResults;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SystemWarningMessage = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                var customerData = (from tempcustomer in GetListMessage()
                                    select tempcustomer);


                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Mensaje == searchValue);
                }

                //total number of rows count     
                recordsTotal = customerData.Count();
                //Paging     
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data
                var dataRender = JsonConvert.SerializeObject(data);
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                //return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return Json(jsonData);

            }
            catch (Exception ex)
            {
                ViewBag.SystemWarningMessage = ex.Message;
                throw;
            }

        }


        public IActionResult Privacy()
        {
            try
            {
                var lista = GetListMessage();
            }
            catch (Exception ex)
            {
                ViewBag.SystemWarningMessage = ex.Message;
                throw;
            }

            return View();
        }

        public IActionResult Clear()
        {
            UpdateGlobalData();
            MySqlConnForMambu mysql = new MySqlConnForMambu();
            mysql.ClearDataBaseMambu();

            return RedirectToAction("Index");
        }

        public IActionResult ExportCsv()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                UpdateGlobalData();
                var list = GetListMessage();
                foreach (var item in list)
                {
                    sb.Append(item.UUID + ";" + item.Mensaje);
                    //Append new line character.
                    sb.Append("\r\n");
                }
            }
            catch (Exception ex)
            {
                ViewBag.SystemWarningMessage = ex.Message;
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Grid.csv");
        }

        public IActionResult Refresh()
        {
            UpdateGlobalData();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void UpdateGlobalData()
        {

            int timeSeconds = 0;
            var timeStart = DateTime.MinValue;


            if (TempData["TimeExecution"] != null)
            {
                timeSeconds = (int)TempData["TimeExecution"];
            }

            if (TempData["TimeStart"] != null)
            {
                timeStart = (DateTime)TempData["TimeStart"];
            }


            if (timeStart.AddSeconds(timeSeconds) <= DateTime.Now)
            {
                TempData["EnableExecute"] = "true";
            }
            else
            {
                TempData["EnableExecute"] = "false";
            }
        }

        private ValidationResult validateWriteInDB(RequestStreamingApi request)
        {
            ValidationResult var = null;
            if (request.WriteInDB == true && string.IsNullOrEmpty(request.StringConnection))
            {
                var = new ValidationResult("No Existe datos de base de datos");
            }

            return var;

        }

        public IEnumerable<Eventos> GetListMessage()
        {
            List<Eventos> list = new List<Eventos>();

            try
            {
                MySqlConnForMambu mysql = new MySqlConnForMambu();
                list = mysql.ReadDataBaseMambu();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        

    }


}
