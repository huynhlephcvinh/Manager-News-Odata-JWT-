﻿using BusinessObject;
using DTO.Category;
using DTO.Tag;
using FE_NewsManagementSystem_HuynhLePhucVinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UniversityMVC.Controllers;
using static FE_NewsManagementSystem_HuynhLePhucVinh.Controllers.CategoryController;

namespace FE_NewsManagementSystem_HuynhLePhucVinh.Controllers
{

    public class TagController : BaseController
    {
        public class TagResponse
        {
            [JsonProperty("value")]
            public List<Tag> Value { get; set; }
            [JsonProperty("@odata.count")]
            public int TotalCount { get; set; }
        }
        public TagController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
: base(httpClientFactory, configuration, httpContextAccessor) { }

        public async Task<IActionResult> Index(int? skip = 0, int? top = 2, string? searchString = null)
        {
            if (HttpContext.Session.GetString("UserRole") != "1")
            {
                return RedirectToAction("Login", "Auth");
            }
            string str = "";
            str = TagAPIURL;
            if (skip != null && top != null)
            {
                str += "?$skip=" + skip + "&$top=" + top + "&$count=true";
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                str += $"&$filter=contains(tolower(TagName), '{searchString.ToLower()}')";
            }
            HttpResponseMessage res = await _httpClient.GetAsync(str);
            if (!res.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ??
                HttpContext.TraceIdentifier
                });
            }
            string rData = await res.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TagResponse>(rData);

            int totalRecords = response.TotalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / top.Value);
            ViewBag.CurrentPage = skip.Value / top.Value + 1;
            ViewBag.PageSize = top;
            ViewBag.SearchString = searchString ?? string.Empty;
            return View(response.Value);
        }

        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "1")
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTagDTO tagDTO)
        {
            if (HttpContext.Session.GetString("UserRole") != "1")
            {
                return RedirectToAction("Login", "Auth");
            }
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(tagDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await _httpClient.PostAsync(TagAPIURL, content);
                //string responseContent = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Server Error");
            }    
            return View(tagDTO);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "1")
            {
                return RedirectToAction("Login", "Auth");
            }
            string str = "";
            str = TagAPIURL;
            HttpResponseMessage res = await _httpClient.GetAsync($"{str}/{id}");
            if (!res.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ??
                HttpContext.TraceIdentifier
                });
            }
            string rData = await res.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Tag>(rData);
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTagDTO updateTagDTO)
        {
            if (HttpContext.Session.GetString("UserRole") != "1")
            {
                return RedirectToAction("Login", "Auth");
            }
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(updateTagDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await _httpClient.PutAsync($"{TagAPIURL}/{id}", content);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return View(updateTagDTO);

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "1")
            {
                return RedirectToAction("Login", "Auth");
            }
            HttpResponseMessage res = await _httpClient.GetAsync($"{TagAPIURL}/{id}");
            if (!res.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });

            }
            string rData = await res.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Tag>(rData);
            return View(response);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "1")
            {
                return RedirectToAction("Login", "Auth");
            }
            HttpResponseMessage res = await _httpClient.DeleteAsync($"{TagAPIURL}/{id}");
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Server Error");
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "1")
            {
                return RedirectToAction("Login", "Auth");
            }
            HttpResponseMessage res = await _httpClient.GetAsync($"{TagAPIURL}/{id}");
            if (!res.IsSuccessStatusCode)
            {
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });

            }
            string rData = await res.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Tag>(rData);

            return View(response);

        }
    }
}
