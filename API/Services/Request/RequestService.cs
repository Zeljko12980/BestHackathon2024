using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace API.Services
{
    public class RequestService
    {
        private readonly StoreContext _context;

        public RequestService(StoreContext context)
        {
            _context=context;
        }

        public async Task AddRequest(Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
        }
        public async Task<Request> GetRequest(int requestId)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.RequestId  == requestId);
            return request;
        }
        public async Task UpdateRequest(int requestId, Request updatedRequest)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.RequestId  == requestId);
            request.Approved = updatedRequest.Approved;

            _context.SaveChanges();
        }
        public async Task DeleteRequest(int requestId)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.RequestId  == requestId);
            if (request == null)
            {
                throw new Exception("Request with this id does not exist!!");
            }

            _context.Requests.Remove(request);
            _context.SaveChanges();
        }

    }
}