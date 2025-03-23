using MongoDB.Driver;
using MyTeamsHub.ReviewAPI.Models;

namespace MyTeamsHub.ReviewAPI.Services;

public class ReviewService : IReviewService
{
    private readonly IMongoCollection<Review> _reviews;

    public ReviewService(IMongoDatabase database)
    {
        _reviews = database.GetCollection<Review>("reviews");
    }

    public async Task<List<Review>> GetAllAsync() =>
        await _reviews.Find(_ => true).ToListAsync();

    public async Task<Review?> GetByIdAsync(string id) =>
        await _reviews.Find(r => r.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Review review) =>
        await _reviews.InsertOneAsync(review);

    public async Task UpdateAsync(string id, Review review) =>
        await _reviews.ReplaceOneAsync(r => r.Id == id, review);

    public async Task DeleteAsync(string id) =>
        await _reviews.DeleteOneAsync(r => r.Id == id);
}
