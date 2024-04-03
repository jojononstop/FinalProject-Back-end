//using Microsoft.EntityFrameworkCore;
//using RGB.Back.DTOs;
//using RGB.Back.Interfaces;
//using RGB.Back.Models;
//using System;

//namespace RGB.Back.Repos
//{
//    public class OrderRepo : IOrderRepo
//    {
//        private readonly RizzContext _context;
//        public void Edit(OdersDTO dto)
//        {
//            var model = _context.Orders.Find(dto.Id);
//            model.Message = dto.Message;
//            _context.SaveChanges();
//        }
//        public int Create(OdersDTO dto)
//        {
//            var model = new Order
//            {
//                OrderDate = dto.OrderDate,
//                PaymentMethod = dto.PaymentMethod,
//                TotalAmount = dto.TotalAmount,
//                Status = dto.Status,
//                MemberId = dto.MemberId
//            };

//            _context.Orders.Add(model);
//            _context.SaveChanges();
//            return model.Id;
//        }

//        public void Delete(int id)
//        {
//            var orders = _context.Orders.Find(id);
//            _context.Orders.Remove(orders);
//            _context.SaveChanges();
//        }

//        public OdersDTO Get(int id)
//        {
//            var orders = _context.Orders.Include(x => x.MemberId)
//                                .FirstOrDefault(x => x.Id == id);

//            if (orders == null)
//            {
//                return null;
//            }
//            var OrderDto = new OdersDTO
//            {
//                Id = orders.Id,
//                MemberId = orders.MemberId,
//                PaymentMethod = orders.PaymentMethod,
//                OrderDate = orders.OrderDate,
//                TotalAmount = orders.TotalAmount,
//                Status = orders.Status,
//                Message = orders.Message
//            };
//            return OrderDto;
//        }



//        public List<OdersDTO> SearchByDate(DateTime startDate, DateTime endDate)
//        {
//            var orders = _context.Orders
//                .Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate)
//                .Select(x => new OdersDTO
//                {
//                    Id = x.Id,
//                    OrderDate = x.OrderDate,
//                    PaymentMethod = x.PaymentMethod,
//                    TotalAmount = x.TotalAmount,
//                    Status = x.Status,
//                    MemberId = x.MemberId,
//                    DiscountAmount = x.DiscountAmount ?? 0,
//                }).ToList();
//            return orders;
//        }


//        public void Update(OdersDTO dto)
//        {
//            var orders = _context.Orders.Find(dto.Id);


//            orders.OrderDate = dto.OrderDate;
//            orders.Status = dto.Status;
//            orders.TotalAmount = dto.TotalAmount;
//            orders.Message = dto.Message;

//            _context.SaveChanges();
//        }

//        public List<OdersDTO> Search(int memberId, string status)
//        {
//            var query = _context.Orders.AsQueryable(); // Create a queryable object

//            // Include the Member navigation property
//            query = query.Include(x => x.MemberId);

//            // Apply filters based on status and memberId
//            if (!string.IsNullOrEmpty(status))
//            {
//                query = query.Where(x => x.Status.Contains(status));
//            }

//            if (memberId != 0) // Assuming memberId of 0 means no filter on memberId
//            {
//                query = query.Where(x => x.MemberId == memberId);
//            }

//            // Execute the query and materialize the results
//            var orders = query.ToList();

//            // Map Order entities to OrderDTOs
//            var orderDTOs = orders.Select(x => new OdersDTO
//            {
//                Id = x.Id,
//                OrderDate = x.OrderDate,
//                PaymentMethod = x.PaymentMethod,
//                TotalAmount = x.TotalAmount,
//                Status = x.Status,
//                MemberId = x.MemberId, // Assuming MemberId is an integer property in OdersDTO
//                DiscountAmount = x.DiscountAmount ?? 0,
//            }).ToList();

//            return orderDTOs;
//        }

//    }
//}
