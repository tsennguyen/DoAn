using Shopping_Laptop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping_Laptop.Services.Recommendation
{
    public interface IRecommendationService
    {
        Task<List<int>> GetRecommendedProductsAsync(string userId, int count);
        Task RecordUserBehaviorAsync(string userId, int productId, BehaviorType behaviorType);
    }

    public enum BehaviorType
    {
        Clicked,
        AddedToCart,
        Purchased,
        Rated
    }
} 