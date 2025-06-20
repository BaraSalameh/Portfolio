﻿using MediatR;

namespace Application.Common.Entities
{
    public class ListQuery<TResponse> : IRequest<ListQueryResponse<TResponse>> where TResponse : class
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}