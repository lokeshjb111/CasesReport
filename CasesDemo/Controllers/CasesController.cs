using CasesDemo.data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasesDemo.Controllers
{
    public class CasesController : Controller
    {
        private ICasesDemoRepository _repository;
        public CasesController(ICasesDemoRepository repository)
        {
            _repository = repository;
        }

        [HttpPost, Route("api/getCases")]
        public IActionResult getCases([FromBody] CasesViewModel casesViewModel)
        {
            try
            {
                List<CasesSummary> cases = _repository.getCases(casesViewModel);
                return Ok(new
                {
                    Message = "",
                    HttpStatus = 200,
                    Result = cases
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    Message = "Error While Fetching Data",
                    HttpStatus = 500,
                    Result = ""
                });
            }
        }

        [HttpPost, Route("api/updateCases")]
        public IActionResult updateCases([FromBody] Cases cases)
        {
            try
            {
                string status = _repository.addCases(cases);
                return Ok(new
                {
                    Message = status,
                    HttpStatus = 200,
                    Result = status
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    Message = "Error While Fetching Data",
                    HttpStatus = 500,
                    Result = ""
                });
            }
        }
    }
}
