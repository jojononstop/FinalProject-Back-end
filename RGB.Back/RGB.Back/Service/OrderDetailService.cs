//using RGB.Back.Interfaces;
////using RGB.Back.Repos;
//using RGB.Back.ViewModels;

//namespace RGB.Back.Service
//{
//    public class OrderDetailService
//    {
//        private readonly IOderDetailRepo _repo;

//        public OrderDetailService(IOderDetailRepo repo)
//        {
//            _repo = repo;
//        }

//        //查詢//
//        public List<OrderDetailVm> Search(int OrdersId)
//        {
//            return _repo.Search(OrdersId)
//                .Select(x => x.OrderDetailsVm())
//                .ToList();
//        }


//        //取得一筆資料
//        public OrderDetailVm Get(int id)
//        {
//            return _repo.Get(id).OrderDetailsVm();
//        }



//    }
//}
