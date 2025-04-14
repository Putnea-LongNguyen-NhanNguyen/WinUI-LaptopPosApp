using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LaptopPosApp.Model;

namespace LaptopPosApp.ViewModels
{
    public enum AddTemporaryPriceResult
    {
        Success,
        StartTimeInThePast,
        EndTimeBeforeStartTime,
        PriceNotChanged,
        AnotherTemporaryPriceInProgress,
    }
    public partial class ChangeTemporaryPricesViewModel: ObservableObject
    {
        readonly Product Product;

        [ObservableProperty]
        public partial IList TemporaryPrices { get; private set; } = null!;

        public ChangeTemporaryPricesViewModel(Product product)
        {
            this.Product = product;
            NewTemporaryPrice = new()
            {
                ProductID = product.ID,
                Price = product.Price,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            Refresh();
        }

        private void Refresh()
        {
            TemporaryPrices = Product.TemporaryPrices
                .OrderBy(p => p.StartDate)
                .ThenBy(p => p.EndDate)
                .ToList();
        }

        [ObservableProperty]
        public partial ProductTemporaryPrice NewTemporaryPrice { get; set; }

        [ObservableProperty]
        public partial ProductTemporaryPrice? SelectedTemporaryPrice { get; set; }
        partial void OnSelectedTemporaryPriceChanged(ProductTemporaryPrice value)
        {
            NewTemporaryPrice = new()
            {
                ProductID = Product.ID,
                Price = value.Price,
                StartDate = value.StartDate,
                EndDate = value.EndDate
            };
        }
        private (AddTemporaryPriceResult, ProductTemporaryPrice?) Add()
        {
            var NewPrice = NewTemporaryPrice.Price;
            var StartTime = NewTemporaryPrice.StartDate;
            var EndTime = NewTemporaryPrice.EndDate;

            if (NewPrice == Product.Price)
            {
                return (AddTemporaryPriceResult.PriceNotChanged, null);
            }
            if (StartTime < DateTime.Now)
            {
                return (AddTemporaryPriceResult.StartTimeInThePast, null);
            }
            if (EndTime < StartTime)
            {
                return (AddTemporaryPriceResult.EndTimeBeforeStartTime, null);
            }
            var overlapping = Product.TemporaryPrices.First(p =>
                (p.StartDate <= StartTime && StartTime <= p.EndDate) ||
                (p.StartDate <= EndTime && EndTime <= p.EndDate)
            );
            if (overlapping is not null)
            {
                return (AddTemporaryPriceResult.AnotherTemporaryPriceInProgress, overlapping);
            }
            ProductTemporaryPrice temporaryPrice = new()
            {
                ProductID = Product.ID,
                Price = NewPrice,
                StartDate = StartTime,
                EndDate = EndTime
            };
            Product.TemporaryPrices.Add(temporaryPrice);
            return (AddTemporaryPriceResult.Success, temporaryPrice);
        }
        public (AddTemporaryPriceResult, ProductTemporaryPrice?) AddOrChange(bool change)
        {
            var oldPrice = SelectedTemporaryPrice;
            if (change)
            {
                Remove();
            }
            var (result, newPrice) = Add();
            if (result != AddTemporaryPriceResult.Success && change && oldPrice is not null)
            {
                Product.TemporaryPrices.Add(oldPrice);
            }
            return (result, newPrice);
        }
        public void Remove()
        {
            if (SelectedTemporaryPrice is null)
            {
                return;
            }
            Product.TemporaryPrices.Remove(SelectedTemporaryPrice);
        }
    }
}
