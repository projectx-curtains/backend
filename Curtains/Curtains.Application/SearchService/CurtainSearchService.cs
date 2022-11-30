﻿using AutoMapper;
using Curtains.Application.SearchService.Interfaces;
using Curtains.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Curtains.Domain.Projections;
using Curtains.Application.DTO;
using Nest;

namespace Curtains.Application.SearchService
{
    public class CurtainSearchService : ICurtainSearchService
    {
        #region FieldsRegion       
        private readonly ICurtainsRepository _curtainsRepository;
        private readonly IElasticCurtainsIndexRepository _elasticIndexRepository;
        private readonly ICurtainsSearchRepository _curtainsSearchRepository;
        private readonly IMapper _mapper;
        #endregion

        public CurtainSearchService(ILogger logger, ICurtainsRepository curtainsRepository,
            IMapper mapper, IElasticCurtainsIndexRepository elasticIndexRepository,
            ICurtainsSearchRepository curtainsSearchRepository)
        {
            _curtainsRepository = curtainsRepository;
            _elasticIndexRepository = elasticIndexRepository;
            _mapper = mapper;
            _curtainsSearchRepository = curtainsSearchRepository;
        }

        #region MethodsRegion
        public async Task AddAllCurtains(string indexName)
        {
            var ListModelsDTO = _mapper.Map<IEnumerable<CurtainsDTO>>(_curtainsRepository.GetAll());

            foreach (var modelDTO in ListModelsDTO)
            {
                var projectionModel = _mapper.Map<CurtainsProjection>(modelDTO);
                await _elasticIndexRepository.Index(projectionModel, indexName);
            }
        }

        public async Task<List<CurtainsProjection>> CurtainsSearch(CurtainsDTO modelDTO, string indexName)
        {
            var model = _mapper.Map<CurtainsProjection>(modelDTO);
            var response = await _curtainsSearchRepository.GetCurtains(model, indexName);
            return response;
        }
        #endregion
    }
}
