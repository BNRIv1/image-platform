using ClosedXML.Excel;
using ImagePlatform.Repository.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImagePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public AdminController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet("users")]
        public FileContentResult ExportAllUsers()
        {
            string fileName = "Users.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var result = userRepository.GetAllUsers();

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("All Users");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Username";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "IsPremium";

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.Username;
                    worksheet.Cell(i + 1, 3).Value = item.Email;
                    worksheet.Cell(i + 1, 4).Value = item.isUserPremium ? "true" : "false";
                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }

        }
    }
}
