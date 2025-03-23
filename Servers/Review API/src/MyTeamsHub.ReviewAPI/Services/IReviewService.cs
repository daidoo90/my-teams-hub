using MyTeamsHub.ReviewAPI.Models;

namespace MyTeamsHub.ReviewAPI.Services;

public interface IReviewService
{
    Task<List<Review>> GetAllAsync();
}