using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEB_KT_2.Models;
using System.Data.SqlClient;
using BCrypt.Net;

namespace WEB_KT_2.Controllers
{
    public class LoginController : Controller
    {
        private QuanLyBanSachEntities1 db = new QuanLyBanSachEntities1();

        // GET: Login
        public ActionResult Index()
        {
            return View(db.KhachHang.ToList());
        }

        // GET: Login/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: Login/Create

        public ActionResult DangNhap() {
           var TaiKhoan = Request.Form["TaiKhoan"];
            var MatKhau = Request.Form["MatKhau"];
            ViewBag.Mess = TaiKhoan+","+MatKhau;
            if (TaiKhoan == null)
                return View();
            var kh= db.KhachHang.Where(s=>s.TaiKhoan==TaiKhoan).FirstOrDefault();
            if (kh != null)
            {
                ViewBag.Mess = "OK1";
                if (BCrypt.Net.BCrypt.Verify(MatKhau, kh.MatKhau))
                {
                    ViewBag.Mess = "OK2";
                    Session["KhachHang"] = kh.TaiKhoan;
                    Session["Email"] = kh.Email;
                    Session["HoTen"] = kh.HoTen;
                    return RedirectToAction("DaDangNhap");
                }
            }
            return View(); 
        }
        public ActionResult DaDangNhap()
        {
            ViewBag.KhachHang = Session["KhachHang"];
            if (Session["KhachHang"] == null)
                return RedirectToAction("Index");
            return View();
        }
        public ActionResult Create()
        {

            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,HoTen,NgaySinh,GioiTinh,DienThoai,TaiKhoan,MatKhau,Email,DiaChi")] KhachHang khachHang)
        {
                if (ModelState.IsValid)
                {
                    //var pw2 = khachHang.MatKhau;
                    //pw2 = BCrypt.Net.BCrypt.HashPassword(pw2);
                    //khachHang.MatKhau = pw2;
                    khachHang.MatKhau = BCrypt.Net.BCrypt.HashPassword(khachHang.MatKhau);

                    db.KhachHang.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
            
            return View(khachHang);
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,HoTen,NgaySinh,GioiTinh,DienThoai,TaiKhoan,MatKhau,Email,DiaChi")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHang.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = db.KhachHang.Find(id);
            db.KhachHang.Remove(khachHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
