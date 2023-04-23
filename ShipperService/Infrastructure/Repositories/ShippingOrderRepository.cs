using AutoMapper;
using Base.Infrastructure;
using Base.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShipperService.Application.Repositories;
using ShipperService.Domain.Entities;
using ShipperService.Domain.Enums;
using ShipperService.Infrastructure.Daos.Interfaces;
using ShipperService.Infrastructure.Database;
using ShipperService.Infrastructure.Models.ShippingOrders;
using System;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.Repositories
{
    public class ShippingOrderRepository : RepositoryBase<ShipperDbContext, ShippingOrder, long>, IShippingOrderRepository
    {
        private readonly IShippingOrderLogDao _shippingOrderLogDao;
        private readonly IMapper _mapper;

        public ShippingOrderRepository(
            ShipperDbContext dbContext,
            IShippingOrderLogDao orderLogDao,
            IMapper mapper) : base(dbContext)
        {
            _shippingOrderLogDao = orderLogDao;
            _mapper = mapper;
        }

        public override ShippingOrder Add(ShippingOrder entity)
        {
            base.Add(entity);
            AddOrderLog(entity);
            return entity;
        }

        public override async Task<ShippingOrder> AddAsync(ShippingOrder entity)
        {
            await base.AddAsync(entity);
            await AddOrderLogAsync(entity);
            return entity;
        }

        public override void Update(ShippingOrder entity)
        {
            base.Update(entity);
            AddOrderLog(entity);
        }

        public async Task UpdateAsync(ShippingOrder entity)
        {
            base.Update(entity);
            await AddOrderLogAsync(entity);
        }

        private void AddOrderLog(ShippingOrder entity)
        {
            var orderLog = GenerateOrderLog(entity);
            _shippingOrderLogDao.Add(orderLog);
        }

        private async Task AddOrderLogAsync(ShippingOrder entity)
        {
            var orderLog = GenerateOrderLog(entity);
            await _shippingOrderLogDao.AddAsync(orderLog);
        }

        private ShippingOrderLog GenerateOrderLog(ShippingOrder order)
        {
            var orderWithExtraInfo = _mapper.Map<ShippingOrderWithExtraInfo>(order);
            var data = JsonConvert.SerializeObject(orderWithExtraInfo, NewtonJsonSerializerSettings.CAMEL);
            return new ShippingOrderLog
            {
                ShippingOrderId = order.Id,
                OrderId = order.OrderId,
                Data = data,
                CreatedAt = DateTime.UtcNow,
            };
        }
        public async Task<ShippingOrder> GetCurrentAssignedShippingOrder(long shipperId)
        {
            return await _context.ShippingOrders
                .FirstOrDefaultAsync(x => x.AssignedShipperId == shipperId &&
                    (
                        x.Status == ShippingOrderStatus.PLACED ||
                        x.Status == ShippingOrderStatus.ACCEPTED ||
                        x.Status == ShippingOrderStatus.PACKING ||
                        x.Status == ShippingOrderStatus.DELIVERING
                    )
                );
        }

    }
}
