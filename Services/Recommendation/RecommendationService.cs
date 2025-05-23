using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Shopping_Laptop.Models;
using Shopping_Laptop.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Shopping_Laptop.Services.Recommendation
{
    public class RecommendationService : IRecommendationService
    {
        private readonly DataContext _context;
        private readonly ILogger<RecommendationService> _logger;
        private readonly MLContext _mlContext;
        private ITransformer? _model;

        public RecommendationService(DataContext context, ILogger<RecommendationService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mlContext = new MLContext();
            LoadOrTrainModel();
        }

        private void LoadOrTrainModel()
        {
            try
            {
                // Load model from file if exists
                if (File.Exists("recommendation_model.zip"))
                {
                    _model = _mlContext.Model.Load("recommendation_model.zip", out var _);
                    _logger.LogInformation("Loaded existing recommendation model");
                    return;
                }

                // Train new model
                var trainingData = GetTrainingData();
                if (!trainingData.Any())
                {
                    _logger.LogWarning("No training data available for recommendation model");
                    return;
                }

                var trainingDataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                var options = new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserIdEncoded",
                    MatrixRowIndexColumnName = "ProductIdEncoded",
                    LabelColumnName = "Rating",
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                };

                var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("UserIdEncoded", "UserId")
                    .Append(_mlContext.Transforms.Conversion.MapValueToKey("ProductIdEncoded", "ProductId"))
                    .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(options));

                _model = pipeline.Fit(trainingDataView);
                _mlContext.Model.Save(_model, trainingDataView.Schema, "recommendation_model.zip");
                _logger.LogInformation("Trained and saved new recommendation model");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoadOrTrainModel");
            }
        }

        private List<ProductRating> GetTrainingData()
        {
            var ratings = _context.Ratings
                .Select(r => new ProductRating
                {
                    UserId = r.UserId,
                    ProductId = r.ProductId,
                    Rating = r.Star
                })
                .ToList();

            // Add implicit feedback from user behaviors
            var behaviors = _context.UserBehaviors
                .GroupBy(b => new { b.UserId, b.ProductId })
                .Select(g => new ProductRating
                {
                    UserId = g.Key.UserId,
                    ProductId = g.Key.ProductId,
                    Rating = CalculateBehaviorRating(g)
                })
                .ToList();

            return ratings.Concat(behaviors).ToList();
        }

        private float CalculateBehaviorRating(IGrouping<dynamic, UserBehaviorModel> behaviors)
        {
            float rating = 0;
            foreach (var behavior in behaviors)
            {
                switch (behavior.BehaviorType)
                {
                    case "Clicked":
                        rating += 1;
                        break;
                    case "AddedToCart":
                        rating += 2;
                        break;
                    case "Purchased":
                        rating += 5;
                        break;
                    case "Rated":
                        // Rating is already included in explicit ratings
                        break;
                }
            }
            return Math.Min(rating, 5); // Cap at 5
        }

        public async Task<List<int>> GetRecommendedProductsAsync(string userId, int count)
        {
            try
            {
                if (_model == null)
                {
                    return await GetFallbackRecommendations(count);
                }

                var userBehaviors = await _context.UserBehaviors
                    .Where(b => b.UserId == userId)
                    .Select(b => b.ProductId)
                    .Distinct()
                    .ToListAsync();

                var allProducts = await _context.Products
                    .Select(p => p.Id)
                    .ToListAsync();

                var predictions = new List<(int ProductId, float Score)>();
                foreach (var productId in allProducts)
                {
                    if (!userBehaviors.Contains(productId))
                    {
                        var prediction = PredictRating(userId, productId);
                        predictions.Add((productId, prediction));
                    }
                }

                return predictions
                    .OrderByDescending(p => p.Score)
                    .Take(count)
                    .Select(p => p.ProductId)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRecommendedProductsAsync");
                return await GetFallbackRecommendations(count);
            }
        }

        private float PredictRating(string userId, int productId)
        {
            try
            {
                var predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductRating, ProductRatingPrediction>(_model);
                var prediction = predictionEngine.Predict(new ProductRating
                {
                    UserId = userId,
                    ProductId = productId
                });
                return prediction.Score;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PredictRating");
                return 0;
            }
        }

        private async Task<List<int>> GetFallbackRecommendations(int count)
        {
            // Fallback to popularity-based recommendations
            return await _context.Products
                .OrderByDescending(p => p.Rating)
                .ThenByDescending(p => p.Sold)
                .Take(count)
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task RecordUserBehaviorAsync(string userId, int productId, BehaviorType behaviorType)
        {
            try
            {
                var behavior = new UserBehaviorModel
                {
                    UserId = userId,
                    ProductId = productId,
                    BehaviorType = behaviorType.ToString(),
                    Timestamp = DateTime.UtcNow
                };

                _context.UserBehaviors.Add(behavior);
                await _context.SaveChangesAsync();

                // Retrain model periodically (e.g., every 1000 new behaviors)
                var behaviorCount = await _context.UserBehaviors.CountAsync();
                if (behaviorCount % 1000 == 0)
                {
                    LoadOrTrainModel();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RecordUserBehaviorAsync");
            }
        }
    }

    public class ProductRating
    {
        public required string UserId { get; set; }
        public int ProductId { get; set; }
        public float Rating { get; set; }
    }

    public class ProductRatingPrediction
    {
        public float Score { get; set; }
    }
} 