﻿using Curtains.Infrastructure.Interfaces;
using Nest;
using Curtains.Domain.Projections;
using Curtains.Infrastructure.SearchQueries;
using Elasticsearch.Net;

namespace Curtains.Infrastructure.SearchEngine
{
    public class CurtainsSearchRepository : ICurtainsSearchRepository
    {
		#region FieldsRegion
		private readonly IElasticClient _elasticClient;
		// private readonly ElasticSearchOptions _options;
		#endregion

		public CurtainsSearchRepository(IElasticClient elasticsearchClient)
        {
            _elasticClient = elasticsearchClient;
        }

		#region MethodsRegion
		/// <summary>
		/// This method get all <c> CurtainsProjection </c> satisfying 
		/// the <paramref name="model"/> request 
		/// </summary>
		/// <param name="model"> Search request </param>
		/// <returns> Collection of <c> SearchResults</c> </returns>
		/// <exception cref="Exception"></exception>
		public async Task<List<SearchResults<CurtainsProjection>>> GetCurtains(ElasticSearchQuery<CurtainsProjection> model)
        {

            var searchFields = SearchRules.CurtainsSearchFields;

            var response = await _elasticClient.SearchAsync<CurtainsProjection>(s => s
                .Take(model.Take)
                .Skip(model.Skip)
                .Query(q => q
                    .Bool(b => b
                        .Should(mu => mu
							.QueryString(qs => qs
							.Query(model.Search)
							.Fields(searchFields.Select(x => new Field(x)).ToArray())))

						.Must(mu => mu
							.Range(r => r.Field(f => f.Price).GreaterThanOrEquals(model.MinPrice).LessThanOrEquals(model.MaxPrice)
							.Relation(RangeRelation.Within)),

							mu => mu.Terms(t => t.Field(f => f.Color).Terms(model.Filters?.Colors)),
							mu => mu.Terms(t => t.Field(f => f.CurtainsType).Terms(model.Filters?.CurtainsTypes)),
							mu => mu.Terms(t => t.Field(f => f.CurtainsKind).Terms(model.Filters?.CurtainsKind)),
							mu => mu.Terms(t => t.Field(f => f.Material).Terms(model.Filters?.Materials)),
							mu => mu.Terms(t => t.Field(f => f.Fabric).Terms(model.Filters?.Fabric)),
							mu => mu.Terms(t => t.Field(f => f.Purpose).Terms(model.Filters?.Purpose)))))

					.Sort(ss => (model.Order == OrderBy.Desc) ? ss.Descending(f => f.Price) : ss.Ascending(f => f.Price))

				.Highlight(h => h
					.Fields(searchFields.Select<string, Func<HighlightFieldDescriptor<CurtainsProjection>, IHighlightField>>(s =>
						 	hf => hf
								.Field(s)).ToArray())));

			if (!response.IsValid)
			{
				throw new Exception(response.DebugInformation);
			}

			var list = response.Hits.Select(x => new SearchResults<CurtainsProjection>()
			{
				Result = x.Source,
				Highlight = x.Highlight

			}).ToList();

			return list;
		}

		/// <summary>
		/// This method indexes <c> CurtainsProjection </c> model into ElasticSearch
		/// </summary>
		/// <param name="model"> ElasticSearch Object </param>
		/// <returns> True if <paramref name="model"/> successfully added </returns>
		/// <exception cref="Exception"></exception>
		public async Task<bool> Index(CurtainsProjection model)
		{
			var response = await _elasticClient.IndexAsync(model, i => i
				.Index("_Option.DefaultIndex")
				.Id(model.Id)
				.Refresh(Refresh.True)
			);

			if (!response.IsValid)
			{
				throw new Exception(response.DebugInformation);
			}
			else
				return true;
		}

		/// <summary>
		/// This method deleted object by <paramref name="id"/> from ElasticSearch
		/// </summary>
		/// <param name="id"></param>
		/// <returns> True if object successfully deleted</returns>
		/// <exception cref="Exception"></exception>
		public async Task<bool> Deleted(string id)
		{
			var response = await _elasticClient.DeleteAsync(new DeleteRequest("_Option.DefaultIndex",id.Trim()));

			if (!response.IsValid)
			{
				throw new Exception(response.DebugInformation);
			}
			else
				return true;
		}
		#endregion
	}
}