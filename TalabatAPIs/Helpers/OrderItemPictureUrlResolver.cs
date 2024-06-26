﻿using AutoMapper;
using Talabat.Core.Entities.Order_Aggregate;
using TalabatAPIs.DTOs;

namespace TalabatAPIs.Helpers
{
	public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration _configuration;

		public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.PictureUrl))
			{
				return $"{_configuration["ApiBaseUrl"]}/{source.Product.PictureUrl}";
			}
			return string.Empty;
		}
	}
}
