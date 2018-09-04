using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    //создайте класс AccountingModel здесь
    public class AccountingModel : ModelBase
    {
        // Price: цена за ночь.
        // Ограничения: 0 <= Price
        public double Price
        {
            get => price;
            set
            {
                if (value == price) return;
                if (value < 0)
                    throw new ArgumentException("Price must be non-negative");
                price = value;
                Notify("Price");
                RecalculateTotal();
            }
        }

        // NightsCount: количество ночей.
        // Ограничения: 0 < NightsCount
        public int NightsCount
        {
            get => nightsCount;
            set
            {
                if (value == nightsCount) return;
                if (value <= 0)
                    throw new ArgumentException("NightsCount must be greater than zero");
                nightsCount = value;
                Notify("NightsCount");
                RecalculateTotal();
            }
        }

        // Discount: скидка в процентах
        // Ограничения: нет (в спецификации задачи)
        // Замечание: Скрытое ограничение в <= 100.0 
        //            по причине неотрицательности Total 
        public double Discount
        {
            get => discount;
            set
            {
                if (value == discount) return;
                if (100.0 < value)
                    throw new ArgumentException("Discount must not be greater than 100");
                discount = value;
                Notify("Discount");
                if (!nested) RecalculateTotal();
            }
        }

        // Total: сумма счета
        // Ограничения: 0 <= Total, 
        //              Total == Price * NightsCount * (1 - Discount / 100)
        public double Total
        {
            get => total;
            set
            {
                if (value == total) return;
                if (value < 0)
                    throw new ArgumentException("Total must be non-negative");
                total = value;
                Notify("Total");
                if (!nested) RecalculateDiscount();
            }
        }

        private void RecalculateTotal()
        {
            nested = true;
            Total = Price * NightsCount * (1.0 - Discount / 100.0);
            nested = false;
        }

        private void RecalculateDiscount()
        {
            nested = true;
            Discount = Price == 0 ? 100.0
                                  : 100.0 * (1 - Total / (Price * NightsCount));
            nested = false;
        }

        // backstore
        private double price;
        private int nightsCount;
        private double discount;
        private double total;
        private bool nested;
    }
}
