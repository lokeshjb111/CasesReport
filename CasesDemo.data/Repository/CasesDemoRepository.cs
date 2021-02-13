using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CasesDemo.data.Repository
{
    public interface ICasesDemoRepository
    {
        public string addCases(Cases cases);
        public List<CasesSummary> getCases(CasesViewModel casesViewModel);
    }
    public class CasesDemoRepository : ICasesDemoRepository
    {
        private readonly CasesDbContext _context;
        public CasesDemoRepository(CasesDbContext context)
        {
            _context = context;
        }
        public string addCases(Cases cases)
        {
            try
            {
                var oldCase = _context.Cases.Where(c => cases.Date == c.Date && cases.State == c.State).FirstOrDefault();
                if (oldCase != null)
                {
                    oldCase.NewCases += cases.NewCases;
                    oldCase.Recovery += cases.Recovery;
                    oldCase.Death += cases.Death;
                    _context.Cases.Update(oldCase);
                }
                else
                {
                    _context.Cases.Add(cases);
                }

                _context.SaveChanges();

                return "Cases Updated";
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<CasesSummary> getCases(CasesViewModel casesViewModel)
        {
            try
            {
                DateTime fromDate = DateTime.Parse(casesViewModel.FromDate);
                DateTime toDate = DateTime.Parse(casesViewModel.ToDate);
                List<Cases> caseList =  _context.Cases.ToList();
                List<Cases> filteredList = new List<Cases>();
                for (var count=0; count<caseList.Count; count++)
                {
                    if(DateTime.Parse(caseList[count].Date) >= fromDate && DateTime.Parse(caseList[count].Date) <= toDate){
                        filteredList.Add(caseList[count]);
                    }
                }

                


                return filteredList.GroupBy(g => g.State).Select(s => new CasesSummary
                {
                    State = s.Key,
                    NewCases = s.Sum(c => c.NewCases),
                    Recovery = s.Sum(c => c.Recovery),
                    Death = s.Sum(c => c.Death)
                }).OrderByDescending(f => f.NewCases).Where(l => l.NewCases > 0 || l.Recovery > 0 || l.Death > 0).ToList();

            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
