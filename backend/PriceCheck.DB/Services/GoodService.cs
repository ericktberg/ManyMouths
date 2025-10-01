using PriceCheck.DB.DTOs;
using PriceCheck.DB.Persistence.Entities;
using PriceCheck.DB.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceCheck.DB.Services
{
    public interface IGoodService
    {
        Task<IList<GoodDTOLight>> SearchGoodsAsync(string query);
        Task<GoodDTOLight> CreateGoodAsync(CreateGoodDTO goodDto);
        Task<GoodTransactionDTO> AddPriceAsync(int goodId, AddPriceDTO priceDto);
        Task<GoodTransactionDTO?> GetLatestPriceAsync(int goodId);
    }

    public class GoodService : IGoodService
    {
        private readonly IGoodRepository _goodRepository;
        public GoodService(IGoodRepository goodRepository)
        {
            _goodRepository = goodRepository;
        }

        public async Task<IList<GoodDTOLight>> SearchGoodsAsync(string query)
        {
            var goods = await _goodRepository.SearchGoodsAsync(query);
            return goods.Select(g => new GoodDTOLight(g)).ToList();
        }

        public async Task<GoodDTOLight> CreateGoodAsync(CreateGoodDTO goodDto)
        {
            var good = new Good
            {
                FriendlyName = goodDto.FriendlyName,
                CodeType = CodeTypes.None // Or from DTO
            };
            var created = await _goodRepository.AddGoodAsync(good);
            return new GoodDTOLight(created);
        }

        public async Task<GoodTransactionDTO> AddPriceAsync(int goodId, AddPriceDTO priceDto)
        {
            var good = await _goodRepository.GetGoodByIdAsync(goodId);
            if (good == null) return null;
            var transaction = new GoodTransaction
            {
                GoodId = goodId,
                Price = (int)priceDto.Price,
                Unit = priceDto.Unit,
                StoreLocationId = priceDto.StoreLocationId ?? 1
            };
            var created = await _goodRepository.AddPriceAsync(transaction);
            return new GoodTransactionDTO(created);
        }

        public async Task<GoodTransactionDTO?> GetLatestPriceAsync(int goodId)
        {
            var transaction = await _goodRepository.GetLatestPriceAsync(goodId);
            return transaction == null ? null : new GoodTransactionDTO(transaction);
        }
    }
}
