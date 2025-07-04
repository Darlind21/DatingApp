using API.Data_Layer.DTOs;
using API.Data_Layer.Models;
using API.Helpers;
using API.Repository_Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data_Layer.Repositories
{
    public class LikesRepository(DataDbContext context, IMapper mapper) : ILikesRepository
    {
        public void AddLike(UserLike like)
        {
            context.Likes.Add(like);
        }

        public void DeleteLike(UserLike like)
        {
            context.Likes.Remove(like);
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
        {
            return await context.Likes
                .Where(x => x.SourceUserId == currentUserId)
                .Select(x => x.TargetUserId)
                .ToListAsync();
        }

        public async Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await context.Likes
                .FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<MemberDTO>> GetUserLikes(LikesParams likesParams)
        {
            var likes = context.Likes
                .AsQueryable();

            IQueryable<MemberDTO> query;
            
            switch (likesParams.Predicate)
            {
                case "liked":
                    query = likes
                        .Where(x => x.SourceUserId == likesParams.UserId)
                        .Select(x => x.TargetUser)
                        .ProjectTo<MemberDTO>(mapper.ConfigurationProvider);
                    break;

                case "likedBy":
                    query = likes
                        .Where(x => x.TargetUserId == likesParams.UserId)
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDTO>(mapper.ConfigurationProvider);
                    break;

                default:
                    var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);

                    query = likes
                        .Where(x => x.TargetUserId == likesParams.UserId && likeIds.Contains(x.SourceUserId))
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDTO>(mapper.ConfigurationProvider);
                    break;
            }

            return await PagedList<MemberDTO>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
        }
    }
}
