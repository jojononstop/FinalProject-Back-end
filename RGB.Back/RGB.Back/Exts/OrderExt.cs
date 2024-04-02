//using RGB.Back.DTOs;
//using RGB.Back.ViewModels;

//namespace RGB.Back.Exts
//{
//    public static class OrderExt
//    {
//        public static OrderIndexVm OrderIndexVm(this OdersDTO dto)
//        {

//            return new OrderIndexVm()
//            {
//                Id = dto.Id,
//                OrderDate = dto.OrderDate,
//                PaymentMethod = dto.PaymentMethod,
//                TotalAmount = dto.TotalAmount,
//                Status = dto.Status,
//                Message = dto.Message
//            };
//        }
//        public static OdersDTO ToDto(this OrderIndexVm vm)
//        {

//            return new OdersDTO()
//            {
//                Id = vm.Id,
//                OrderDate = vm.OrderDate,
//                PaymentMethod = vm.PaymentMethod,
//                TotalAmount = vm.TotalAmount,
//                Status = vm.Status,
//                Message = vm.Message
//            };
//        }
//    }
//}
