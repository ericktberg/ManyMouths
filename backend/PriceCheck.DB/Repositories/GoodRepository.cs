using PriceCheck.DB.Persistence;
using PriceCheck.DB.Persistence.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceCheck.DB.Repositories
{
    public interface IGoodRepository
    {
        Task<IList<Good>> SearchGoodsAsync(string query);
        Task<Good> AddGoodAsync(Good good);
        Task<Good> GetGoodByIdAsync(int goodId);
        Task<GoodTransaction> AddPriceAsync(GoodTransaction transaction);
        Task<GoodTransaction> GetLatestPriceAsync(int goodId);
    }

    public class GoodRepository : IGoodRepository
    {
        private readonly ManyMouthsDbContext _context;
        public GoodRepository(ManyMouthsDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Good>> SearchGoodsAsync(string query)
        {
            return await Task.Run(() =>
                string.IsNullOrEmpty(query)
                    ? _context.Goods.OrderBy(g => g.FriendlyName).ToList()
                    : _context.Goods.Where(g => g.FriendlyName.Contains(query)).OrderBy(g => g.FriendlyName).ToList()
            );
        }

        public async Task<Good> AddGoodAsync(Good good)
        {
            _context.Goods.Add(good);
            await _context.SaveChangesAsync();
            return good;
        }

        public async Task<Good> GetGoodByIdAsync(int goodId)
        {
            return await _context.Goods.FindAsync(goodId);
        }

        public async Task<GoodTransaction> AddPriceAsync(GoodTransaction transaction)
        {
            _context.GoodTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<GoodTransaction> GetLatestPriceAsync(int goodId)
        {
            // TODO: Replace this kludge with a proper CreatedDate column for reliable ordering
            return await Task.Run(() =>
                _context.GoodTransactions
                    .Where(gt => gt.GoodId == goodId)
                    .OrderByDescending(gt => gt.Id) // Kludge: assumes Id is always ascending with time
                    .FirstOrDefault()
            );
        }
    }
}
