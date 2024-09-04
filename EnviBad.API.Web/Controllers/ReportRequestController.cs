using EnviBad.API.Common;
using EnviBad.API.Common.DTO;
using EnviBad.API.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnviBad.API.Web.Controllers
{
    [ApiController]
    [Route("report")]
    public class ReportRequestController : ControllerBase
    {
        public ReportRequestController()
        {
        }

        private List<UserReportRequest> _mockReports = new List<UserReportRequest> {
            new UserReportRequest{
                Id=1, ReportName="�������� ����� 1", UserInfoId=1, AreaRadius=125,
                CenterLat=66.4316, CenterLong=59.12894, LastStatus=ReportStatus.InProgress.ToString()
            },
            new UserReportRequest{
                Id=2, ReportName="������ �������� �����", UserInfoId=2, AreaRadius=430.6,
                CenterLat=66.1230, CenterLong=60.0765, LastStatus=ReportStatus.Failed.ToString()
            }
        };

        /// <summary> ������ ���� �������� �� �������� ������� �� ������� </summary>
        /// <returns>������ �������� �� ������</returns>
        [HttpGet, Route("requested")]
        public IEnumerable<UserReportRequest> GetReportRequestsList()
        {
            return _mockReports;
        }

        /// <summary> ������� ������� �� ����� �� ��� ID � �� </summary>
        /// <returns>���� ������ �� �������� ������</returns>
        [HttpGet, Route("requested/{id:int}")]
        public UserReportRequest? GetReportRequestById(int id)
        {
            return _mockReports.FirstOrDefault(x => x.Id == id);
        }

        /// <summary> �������� ������� �� ���������� ������ �� ������� </summary>
        /// <param name="model">��������� ������ �� �������</param>
        /// <returns>ID ������� �� ���������� ������ � ��</returns>
        [HttpPost, Route("requested")]
        public ActionResult<int> RequestReport([FromBody] ReportRequestCreationDto model)
        {
            if (model.AreaRadius != null && 
                (model.AreaRadius < Const.MinReportAreRadius || model.AreaRadius > Const.MaxReportAreRadius))
            {
                return BadRequest(
                    $"������ ������� ������ {Const.MinReportAreRadius}, ���� ������ {Const.MaxReportAreRadius}");
            }
            return 0;
        }
    }
}
