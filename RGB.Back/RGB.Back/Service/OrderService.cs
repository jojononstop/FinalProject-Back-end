using RGB.Back.DTOs;
using RGB.Back.Exts;
using RGB.Back.Interfaces;
using RGB.Back.ViewModels;

namespace RGB.Back.Service
{
    public class OrderService
    {
        private IOrderRepo _repo;
        public OrderService(IOrderRepo repos)
        {
            _repo = repos;
        }
        public void Edit(OdersDTO dto)
        {
            _repo.Edit(dto);
        }

        //新增
        public int Create(OdersDTO dto)
        {
            return _repo.Create(dto);
        }
        //更新
        public void Update(OdersDTO dto)
        {
            _repo.Update(dto);
        }

        //查詢//
        public List<OrderIndexVm> Search(int memberId, string status)
        {
            return _repo.Search(memberId, status)
                .Select(x => x.OrderIndexVm())
                .ToList();
        }

        //查詢日期
        public List<OrderIndexVm> SearchByDate(DateTime startDate, DateTime endDate)
        {
            return _repo.SearchByDate(startDate, endDate)
                .Select(x => x.OrderIndexVm())
                .ToList();
        }


        //刪除
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
        //取得一筆資料
        public OdersDTO Get(int id)
        {
            return _repo.Get(id);
        }

    }
}
