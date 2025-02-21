using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IRequestService 
    {
        public Task AddRequest(Request request);
        public Task<Request> GetRequest(int requestId);
        public Task UpdateRequest(int requestId, Request updatedRequest);
        public Task DeleteRequest(int requestId);
    }
}