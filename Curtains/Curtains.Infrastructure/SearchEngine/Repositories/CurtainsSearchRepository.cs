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
                            .Terms(t => t.Field(f => f.Color).Terms(model.Filters?.Colors)),
                            mu => mu
							.Terms(t => t.Field(f => f.CurtainsType).Terms(model.Filters?.CurtainsTypes)), 
                            mu => mu
							.Terms(t => t.Field(f => f.CurtainsKind).Terms(model.Filters?.CurtainsKind)), 
                            mu => mu
							.Terms(t => t.Field(f => f.Material).Terms(model.Filters?.Materials)),
							mu => mu
							.Terms(t => t.Field(f => f.Fabric).Terms(model.Filters?.Fabric)),
							mu => mu
							.Terms(t => t.Field(f => f.Purpose).Terms(model.Filters?.Purpose)))

						.Should(sh => sh
							.Range(r => r.Field(f => f.Price).GreaterThanOrEquals(model.MinPrice).LessThanOrEquals(model.MaxPrice)
							.Relation(RangeRelation.Within)))))

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