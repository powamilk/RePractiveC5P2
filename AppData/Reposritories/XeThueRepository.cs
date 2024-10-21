using AppData.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Reposritories
{
    public class XeThueRepository : IXeThueRepository
    {
        private readonly AppDbContext _context;

        public XeThueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<XeThue>> GetAllXeThueAsync()
        {
            return await _context.XeThues.ToListAsync();
        }

        public async Task<XeThue> GetXeThueByIdAsync(Guid id)
        {
            return await _context.XeThues.FindAsync(id);
        }

        public async Task AddXeThueAsync(XeThue xeThue)
        {
            await _context.XeThues.AddAsync(xeThue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateXeThueAsync(XeThue xeThue)
        {
            _context.XeThues.Update(xeThue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteXeThueAsync(XeThue xeThue)
        {
            _context.XeThues.Remove(xeThue);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> TinhChiPhiThueXeAsync(Guid xeThueId, DateTime ngayThue, DateTime ngayTra)
        {
            var xeThue = await _context.XeThues.FindAsync(xeThueId);
            if (xeThue == null)
            {
                throw new KeyNotFoundException("Không tìm thấy xe với ID đã cho.");
            }

            if (ngayThue >= ngayTra)
            {
                throw new InvalidOperationException("Ngày thuê phải trước ngày trả.");
            }

            var soNgayThue = (ngayTra - ngayThue).Days;
            var tongChiPhi = soNgayThue * xeThue.GiaThueMoiNgay;

            return tongChiPhi;
        }
    }
}
