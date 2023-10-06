using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sib_love_site.Areas.Identity.Data;
using sib_love_site.Data;

namespace sib_love_site.Controllers
{
    [Authorize(Roles = "Admin")] /*Доступ к методам контроллера с ролью Админ*/
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager; /*управление пользователями*/
        private readonly AuthDbContext context; /*доступ к данным с бд*/

        public AdminController(UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        public async Task<IActionResult> ListUsers()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await userManager.DeleteAsync(user);
            return RedirectToAction("ListUsers");
        }

        public async Task<IActionResult> ListQUestions()
        {
            var questions = await context.Questions.ToListAsync();
            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(int id)
        {

            var answers = await context.Answers.Where(a => a.QuestionId == id).ToListAsync();
            context.Answers.RemoveRange(answers);

            var question = await context.Questions.FindAsync(id);
            if (question != null)
            {
                context.Questions.Remove(question);
                await context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("ListQuestions");
        }

    }
}
