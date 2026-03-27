using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class LaptopRepository : ILaptopRepository
    {
        private readonly AppDbContext _context;

        public LaptopRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(Laptop laptop, List<HinhAnhLaptop> images)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Laptops.Add(laptop);
                    if (images != null && images.Count > 0)
                    {
                        _context.HinhAnhLaptops.AddRange(images);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool Delete(string id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = _context.Laptops.FirstOrDefault(x => x.maLaptop == id);
                    if (entity != null)
                    {
                        var images = _context.HinhAnhLaptops.Where(x => x.maLaptop == id).ToList();
                        _context.HinhAnhLaptops.RemoveRange(images);
                        
                        var reviews = _context.DanhGias.Where(x => x.maLaptop == id).ToList();
                        _context.DanhGias.RemoveRange(reviews);

                        _context.Laptops.Remove(entity);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public IEnumerable<Laptop> GetAll()
        {
            return _context.Laptops.ToList();
        }

        public Laptop GetById(string id)
        {
            return _context.Laptops.FirstOrDefault(x => x.maLaptop == id);
        }

        public bool Update(Laptop laptop, List<HinhAnhLaptop> images)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Laptops.Update(laptop);
                    
                    if (images != null)
                    {
                        var oldImages = _context.HinhAnhLaptops.Where(x => x.maLaptop == laptop.maLaptop).ToList();
                        _context.HinhAnhLaptops.RemoveRange(oldImages);
                        _context.HinhAnhLaptops.AddRange(images);
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public IEnumerable<HangLaptop> GetAllBrands()
        {
            return _context.HangLaptops.ToList();
        }

        public IEnumerable<DanhGia> GetReviews(string maLaptop)
        {
            return _context.DanhGias.Where(x => x.maLaptop == maLaptop).ToList();
        }

        public bool AddReview(DanhGia review)
        {
            _context.DanhGias.Add(review);
            return _context.SaveChanges() > 0;
        }
    }
}
