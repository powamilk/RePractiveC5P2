using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Entities;
using System.Text;
using System.Text.Json;

namespace AppView.Controllers
{
    public class XeThuesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7006/api/xethue";

        public XeThuesController()
        {
            _httpClient = new();
        }

        // GET: XeThues
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var xeThues = await response.Content.ReadAsStringAsync();
                var xeThueList = JsonSerializer.Deserialize<List<XeThue>>(xeThues);
                return View(xeThueList);
            }
            ModelState.AddModelError(string.Empty, "Không thể tải danh sách xe thuê.");
            return View(new List<XeThue>());
        }

        // GET: XeThues/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var xeThue = await response.Content.ReadAsStringAsync();
                var xeThueDetail = JsonSerializer.Deserialize<XeThue>(xeThue);
                return View(xeThueDetail);
            }

            return NotFound();
        }

        // GET: XeThues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: XeThues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TenXe,HangXe,NgayThue,NgayTra,TrangThai,GiaThueMoiNgay")] XeThue xeThue)
        {
            if (ModelState.IsValid)
            {
                xeThue.ID = Guid.NewGuid();
                var jsonContent = JsonSerializer.Serialize(xeThue);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiBaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Không thể tạo mới xe thuê.");
            }

            return View(xeThue);
        }

        // GET: XeThues/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var xeThue = await response.Content.ReadAsStringAsync();
                var xeThueDetail = JsonSerializer.Deserialize<XeThue>(xeThue);
                return View(xeThueDetail);
            }

            return NotFound();
        }

        // POST: XeThues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,TenXe,HangXe,NgayThue,NgayTra,TrangThai,GiaThueMoiNgay")] XeThue xeThue)
        {
            if (id != xeThue.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var jsonContent = JsonSerializer.Serialize(xeThue);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Không thể cập nhật xe thuê.");
            }

            return View(xeThue);
        }

        // GET: XeThues/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var xeThue = await response.Content.ReadAsStringAsync();
                var xeThueDetail = JsonSerializer.Deserialize<XeThue>(xeThue);
                return View(xeThueDetail);
            }

            return NotFound();
        }

        // POST: XeThues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Không thể xóa xe thuê.");
            return View();
        }

        private bool XeThueExists(Guid id)
        {
            var response = _httpClient.GetAsync($"{_apiBaseUrl}/{id}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
