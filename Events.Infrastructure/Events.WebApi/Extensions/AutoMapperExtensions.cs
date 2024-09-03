﻿using AutoMapper;
using AutoMapper.QueryableExtensions;


namespace Events.WebApi.Extensions;


public static class AutoMapperExtensions
{
    public static IQueryable<TDestination> ProjectTo<TDestination>(this IQueryable source, IMapper mapper)
    {
        return source.ProjectTo<TDestination>(mapper.ConfigurationProvider);
    }
}
