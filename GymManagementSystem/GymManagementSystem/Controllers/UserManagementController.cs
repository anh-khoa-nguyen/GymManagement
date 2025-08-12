using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels.UserManagement;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class UserManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        #region CRUD
        // GET: UserManagement
        public async Task<ActionResult> Index(string searchString)
        {
            var usersQuery = db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                usersQuery = usersQuery.Where(u => u.HoTen.Contains(searchString) || u.Email.Contains(searchString));
            }

            var users = await usersQuery.Select(u => new UserListViewModel
            {
                Id = u.Id,
                HoTen = u.HoTen,
                Email = u.Email,
                VaiTro = u.VaiTro,
                IsLockedOut = u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > System.DateTime.UtcNow,
                AvatarUrl = u.AvatarUrl
            }).OrderBy(u => u.HoTen).ToListAsync();

            ViewBag.CurrentFilter = searchString;
            return View(users);
        }

        // GET: UserManagement/Create
        public ActionResult Create()
        {
            var viewModel = new RegisterViewModel();
            if (Request.IsAjaxRequest())
            {
                return PartialView("Create", viewModel);
            }
            return View(viewModel);
        }

        // POST: UserManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model, HttpPostedFileBase avatarFile)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, HoTen = model.HoTen, VaiTro = model.VaiTro };

                if (avatarFile != null && avatarFile.ContentLength > 0)
                {
                    var cloudinaryService = new CloudinaryService();
                    user.AvatarUrl = await cloudinaryService.UploadImageAsync(avatarFile);
                }

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, user.VaiTro);
                    if (Request.IsAjaxRequest())
                    {
                        return Json(new { success = true });
                    }
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Create", model);
            }
            return View(model);
        }

        // GET: UserManagement/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return HttpNotFound();

            var viewModel = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                HoTen = user.HoTen,
                VaiTro = user.VaiTro,
                AvatarUrl = user.AvatarUrl // Thêm dòng này
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Edit", viewModel);
            }
            return View(viewModel);
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditViewModel viewModel, HttpPostedFileBase avatarFile)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(viewModel.Id);
                if (user == null) return HttpNotFound();

                if (avatarFile != null && avatarFile.ContentLength > 0)
                {
                    var cloudinaryService = new CloudinaryService();
                    user.AvatarUrl = await cloudinaryService.UploadImageAsync(avatarFile);
                }

                user.HoTen = viewModel.HoTen;
                user.VaiTro = viewModel.VaiTro;
                var updateResult = await UserManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    AddErrors(updateResult);
                    return Request.IsAjaxRequest() ? PartialView("Edit", viewModel) : (ActionResult)View(viewModel);
                }

                if (!string.IsNullOrWhiteSpace(viewModel.NewPassword))
                {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var passwordResult = await UserManager.ResetPasswordAsync(user.Id, token, viewModel.NewPassword);
                    if (!passwordResult.Succeeded)
                    {
                        AddErrors(passwordResult);
                        return Request.IsAjaxRequest() ? PartialView("Edit", viewModel) : (ActionResult)View(viewModel);
                    }
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            return Request.IsAjaxRequest() ? PartialView("Edit", viewModel) : (ActionResult)View(viewModel);
        }

        // POST: UserManagement/ToggleLockout/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleLockout(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (id == User.Identity.GetUserId())
            {
                TempData["ErrorMessage"] = "Bạn không thể tự vô hiệu hóa tài khoản của chính mình.";
                return RedirectToAction("Index");
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return HttpNotFound();

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                await UserManager.SetLockoutEndDateAsync(user.Id, System.DateTimeOffset.UtcNow.AddMinutes(-1));
            }
            else
            {
                await UserManager.SetLockoutEndDateAsync(user.Id, System.DateTimeOffset.MaxValue);
            }
            return RedirectToAction("Index");
        }

        // GET: UserManagement/Details/<id>
        public async Task<ActionResult> Details(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userAccount = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userAccount == null) return HttpNotFound();

            HoiVien hoiVienProfile = null;
            if (userAccount.VaiTro == "HoiVien")
            {
                hoiVienProfile = await db.HoiViens
                                         .Include(h => h.HangHoiVien)
                                         .FirstOrDefaultAsync(h => h.ApplicationUserId == id);
            }

            var viewModel = new UserDetailsViewModel
            {
                UserAccount = userAccount,
                HoiVienProfile = hoiVienProfile
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", viewModel);
            }
            return View(viewModel);
        }
        #endregion
   
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}