﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utillity;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/couponAPI"
			});
        }
        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/couponAPI/GetByCode/" + couponCode,
            });
        }
        public async Task<ResponseDto?> GetCouponByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/couponAPI/" + couponId
            });
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto coupon)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.CouponAPIBase + "/api/couponAPI/",
                Data = coupon
            });
        }
        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto coupon)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.CouponAPIBase + "/api/couponAPI/",
                Data = coupon
            });
        }
        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + "/api/couponAPI/" + id
            });
        }
    }
}
