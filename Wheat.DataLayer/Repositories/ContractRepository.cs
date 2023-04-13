using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Wheat.DataLayer.DataBase;

using Wheat.DataLayer.Repositories.Interfaces;
using Wheat.Models.Entities;
using Wheat.Models.Responses;

namespace Wheat.DataLayer.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly WheatDbContext _context;
        private readonly IMapper _mapper;
        public ContractRepository(WheatDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ContractsResult> GetMyContractsAsync(string userId)
        {
            var sell = await GetMySellContracts(userId);
            var buy = await GetMyBuyContracts(userId);
            return new ContractsResult { MySellContracts = sell, MyBuyContracts = buy };
        }

        public async Task<Contracts> GetMyBuyContracts(string userId)
        {
            await _context.Users.LoadAsync();
            await _context.SellContracts.LoadAsync();
            var user = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts)).Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new Exception("404");
            var contractPersons = user.SelfBuyContracts.GroupBy(x => x.SellerCode ?? "").SelectMany(x =>
            {
                return x.GroupBy(f => f.OldPrice).SelectMany(g => g.GroupBy(jj=>jj.OnTrade).Select(ht=>
                    new PriceRow
                    {
                        Code = x.Key,
                        Count = user.SelfBuyContracts.Count(y => y.OldPrice == g.Key && (y.SellerCode ?? "") == x.Key),
                        Contract = g.First()
                    }
                    ));

            }).ToList();
            var balance = user.SelfBuyContracts.Sum(x => x.OldPrice);
            return new Contracts { PriceRow = contractPersons, Count = user.SelfBuyContracts.Count, Balance = balance, SelfCode = user.Code };
        }

        public async Task<Contracts> GetMySellContracts(string userId)
        {
            await _context.Users.LoadAsync();
            await _context.SellContracts.LoadAsync();
            var user = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts)).Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new Exception("404");
            var contractPersons = user.SelfSellContracts.GroupBy(x => x.PurchaserCode ?? "").SelectMany(x =>
            {
                return x.GroupBy(f => f.OldPrice).SelectMany(g => g.GroupBy(jj=>jj.OnTrade).Select(ht=>
                    new PriceRow
                    {
                        Code = x.Key,
                        Count = user.SelfSellContracts.Count(y => y.OldPrice == g.Key && x.Key == (y.PurchaserCode ?? "")),
                        Contract = g.First()
                    }
                    )).ToList();

            }).ToList();
            var balance = user.SelfSellContracts.Sum(x => x.OldPrice);
            return new Contracts { PriceRow = contractPersons, Count = user.SelfSellContracts.Count, Balance = balance, SelfCode = user.Code };
        }

        public async Task CreateContractsAsync(string userId, int count, double price, bool? sell)
        {
            sell ??= true;
            var time = DateTime.UtcNow;
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("404 User not found");
            if (sell.Value)
            {
                for (int i = 0; i < count; i++)
                {
                    var x = new SellContract
                    { SellerCode = user.Code, Price = price, OldPrice = price, Id = Guid.NewGuid().ToString(), Updated = time, OnTrade = true };
                    await _context.SellContracts.AddAsync(x);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var x = new SellContract
                    { PurchaserCode = user.Code, Price = price, OldPrice = price, Id = Guid.NewGuid().ToString(), Updated = time, OnTrade = true };
                    await _context.SellContracts.AddAsync(x);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DealAsync(string userId, string code, int count, double price, bool? buy)
        {
            buy ??= true;
            var time = DateTime.UtcNow;
            await _context.Users.LoadAsync();
            await _context.SellContracts.LoadAsync();
            var user = await _context.Users.FindAsync(userId);
            if (user.Code == code)
            {
                if (buy.Value)
                {
                    var seller = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts))
                        .Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Code == code);
                    if (seller == null) throw new Exception("404 Seller not found");
                    if (seller.SelfSellContracts.Count(x => x.Price == price && x.PurchaserCode == null && x.OnTrade) >=
                        count)
                    {
                        var contractsToBuy = seller.SelfSellContracts
                            .Where(x => x.Price == price && x.PurchaserCode == null).Take(count).ToList();
                        _context.RemoveRange(contractsToBuy);
                        await _context.SaveChangesAsync();
                        return;
                    }
                    throw new Exception("There are not that many contracts to deal with.");
                }
                else
                {
                    var seller = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts))
                        .Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Code == code);
                    if (seller == null) throw new Exception("404 Seller not found");
                    if (seller.SelfBuyContracts.Count(x => x.Price == price && x.SellerCode == null && x.OnTrade) >=
                        count)
                    {
                        var contractsToBuy = seller.SelfBuyContracts
                            .Where(x => x.Price == price && x.SellerCode == null).Take(count).ToList();
                        _context.RemoveRange(contractsToBuy);
                        await _context.SaveChangesAsync();
                        return;
                    }
                    throw new Exception("There are not that many contracts to deal with.");
                }
            }
            if (buy.Value)
            {
                var buyer = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts)).Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Id == userId);
                if (buyer == null) throw new Exception("404 User not found");
                var seller = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts)).Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Code == code);
                if (seller == null) throw new Exception("404 Seller not found");
                if (seller.SelfSellContracts.Count(x => x.Price == price && x.PurchaserCode == null && x.OnTrade) >= count)
                {
                    var contractsToBuy = seller.SelfSellContracts.Where(x => x.Price == price && x.PurchaserCode == null).Take(count).ToList();
                    contractsToBuy.ForEach(x =>
                    {
                        x.PurchaserCode = buyer.Code;
                        x.Updated = time;
                        x.OnTrade = false;
                    });
                    _context.UpdateRange(contractsToBuy);
                    await _context.SaveChangesAsync();
                    return;
                }
                throw new Exception("There are not that many contracts to deal with.");
            }
            else
            {
                var seller = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts)).Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Id == userId);
                if (seller == null) throw new Exception("404 User not found");
                var buyer = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts)).Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Code == code);
                if (buyer == null) throw new Exception("404 Buyer not found");
                if (buyer.SelfBuyContracts.Count(x => x.Price == price && x.SellerCode == null && x.OnTrade) >= count)
                {
                    var contractsToSell = buyer.SelfBuyContracts.Where(x => x.Price == price && x.SellerCode == null).Take(count).ToList();
                    contractsToSell.ForEach(x =>
                    {
                        x.SellerCode = seller.Code;
                        x.Updated = time;
                        x.OnTrade = false;
                    });
                    _context.UpdateRange(contractsToSell);
                    await _context.SaveChangesAsync();
                    return;
                }
                throw new Exception("There are not that many contracts to deal with.");
            }
        }

        public async Task DealAsync(string userId, string bCode, string sCode, int count, double price)
        {

            var time = DateTime.UtcNow;
            await _context.Users.LoadAsync();
            await _context.SellContracts.LoadAsync();
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("404 User not found");
            var seller = await _context.Users.FirstOrDefaultAsync(x => x.Code == sCode);
            if (seller == null) throw new Exception("404 Seller not found");
            var buyer = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts)).Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Code == bCode);
            if (buyer == null) throw new Exception("404 Buyer not found");
            var scount = seller.SelfSellContracts.Count(x => x.Price == price && x.PurchaserCode == buyer.Code && x.OnTrade);
            if (scount >= count)
            {
                var bcount = buyer.SelfBuyContracts.Count(x => x.Price == price && x.SellerCode == seller.Code && x.OnTrade);
                if (bcount >= count)
                {
                    var contracts = seller.SelfSellContracts.Where(x => x.Price == price && x.PurchaserCode == buyer.Code && x.OnTrade)
                        .Take(count).ToList();
                    var tempResult = await _context.OperationResults.FirstOrDefaultAsync(x =>
                        x.Complete == false && x.UserId == buyer.Id && x.SellerId == seller.Id && x.NewPrice == price);
                    contracts.ForEach(x =>
                    {
                        x.PurchaserCode = user.Code;
                        x.OnTrade = false;
                    });

                    if (tempResult == null) throw new Exception("There is some critical error happened with operations.");
                    if (tempResult.Count < count) throw new Exception("There is some critical error happened with contract counts.");
                    var operationResult = new OperationResult
                    {
                        OldPrice = tempResult.OldPrice,
                        NewPrice = tempResult.NewPrice,
                        SellerId = tempResult.SellerId,
                        UserId = tempResult.UserId,
                        BuyerId = user.Id,
                        Count = count,
                        Time = time,
                        Complete = true,
                        Balance = (tempResult.NewPrice - tempResult.OldPrice) * count
                    };
                    if (tempResult.Count == count)
                        _context.RemoveRange(tempResult);
                    else
                    {
                        tempResult.Count -= count;
                        if (tempResult.Count < 0) throw new Exception("Ebaniy pizdec eto suka kak nahui.");
                        _context.UpdateRange(tempResult);
                    }
                    _context.UpdateRange(contracts);
                    _context.AddRange(operationResult);
                    await _context.SaveChangesAsync();
                    return;
                }
                throw new Exception("Ebaniy pizdec eto suka kak nahui.");
            }
            throw new Exception("There are not that many contracts to deal with.");
        }


        public async Task ReSellContractsAsync(string userId, string code, int count, double price, double newPrice)
        {
            var time = DateTime.UtcNow;
            await _context.Users.LoadAsync();
            await _context.SellContracts.LoadAsync();
            var user = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts)).Include(nameof(WIdentityUser.SelfBuyContracts)).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new Exception("404 User not found");
            var seller = await _context.Users.FirstOrDefaultAsync(x => x.Code == code);
            if (seller == null) throw new Exception("404 Seller not found");
            var ucount = user.SelfBuyContracts.Count(x =>
                x.Price == price && x.SellerCode == seller.Code && x.OnTrade == false);
            if ( ucount >= count)
            {
                var contractsToBuy = user.SelfBuyContracts.Where(x => x.Price == price && x.SellerCode == seller.Code).Take(count).ToList();
                contractsToBuy.ForEach(x =>
                {
                    x.Price = newPrice;
                    x.OnTrade = true;
                });
                var opResult = new OperationResult
                {
                    UserId = user.Id,
                    SellerId = seller.Id,
                    Time = time,
                    OldPrice = price,
                    NewPrice = newPrice,
                    Complete = false,
                    Count = count
                };
                _context.UpdateRange(contractsToBuy);
                _context.AddRange(opResult);
                await _context.SaveChangesAsync();
                return;
            }
            throw new Exception("There are not that many contracts to deal with.");
        }

        public async Task<BalanceResults> GetTotalBalanceAsync(string userId)
        {
            await _context.Users.LoadAsync();
            await _context.SellContracts.LoadAsync();
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("404 User not found");
            var fullUser = await _context.Users.Include(nameof(WIdentityUser.SelfSellContracts))
                .Include(nameof(WIdentityUser.SelfBuyContracts)).FirstAsync(x => x.Id == userId);
            var operations = _context.OperationResults.Where(x => x.UserId == userId && x.Complete);
            var result = new BalanceResults
            {
                Count = fullUser.SelfBuyContracts.Count - fullUser.SelfSellContracts.Count,
                Total = fullUser.SelfSellContracts.Sum(x => x.OldPrice) - fullUser.SelfBuyContracts.Sum(x => x.OldPrice) + operations.Sum(x => x.Balance),
                OperationResultsDto = operations.Select(x => _mapper.Map<OperationResultDto>(x)).ToList()
            };
            return result;
        }

        public async Task<List<ContractPrice>> GetPublicSellContracts(string userId)
        {
            await _context.SellContracts.LoadAsync();
            var code = (await _context.Users.FindAsync(userId)).Code;
            
            var contractPersons = _context.SellContracts.Local.Where(x => x.OnTrade && x.SellerCode != null && x.SellerCode!=code).GroupBy(x => x.SellerCode).SelectMany(x =>
            
                x.GroupBy(f => f.OldPrice).Select(g => new ContractPrice
                {
                    Count = _context.SellContracts.Count(y => y.OldPrice == g.Key && y.SellerCode == x.Key && y.OnTrade),
                    Contract = g.First()
                })
            ).ToList();

            return contractPersons;
        }

        public async Task<List<ContractPrice>> GetPublicBuyContracts(string userId)
        {
            await _context.SellContracts.LoadAsync();
            var code = (await _context.Users.FindAsync(userId)).Code;
            var contractPersons = _context.SellContracts.Local.Where(x => x.OnTrade && x.SellerCode == null && x.PurchaserCode != code).GroupBy(x => x.PurchaserCode).SelectMany(x =>

                x.GroupBy(f => f.OldPrice).Select(g => new ContractPrice
                {
                    Count = _context.SellContracts.Count(y => y.OldPrice == g.Key && y.PurchaserCode == x.Key && y.OnTrade),
                    Contract = g.First()
                })
            ).ToList();

            return contractPersons;
        }
    }
}
