using MediatR;
using Otomasyon.Core.Entities;
using Otomasyon.Core.Repositories;
using Otomasyon.Core.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Otomasyon.Core.Requests
{
    // Query classes
    public class GetAllBasvuruFormuQuery : IRequest<List<BasvuruFormu>> { }
    public class GetAllMevcutSinavlarQuery : IRequest<List<MevcutSinavlar>> { }
    public class GetAllKisilerQuery : IRequest<List<Kisiler>> { }
    // Handler classes


}

    public class GetAllBasvuruFormuHandler : IRequestHandler<GetAllBasvuruFormuQuery, List<BasvuruFormu>>
    {
        private readonly IRepository<BasvuruFormu> _repository;

        public GetAllBasvuruFormuHandler(IRepository<BasvuruFormu> repository)
        {
            _repository = repository;
        }

        public async Task<List<BasvuruFormu>> Handle(GetAllBasvuruFormuQuery request, CancellationToken cancellationToken)
        {
            return (List<BasvuruFormu>)await _repository.ListAllAsync();
        }
    }
    public class GetAllMevcutSinavlarHandler : IRequestHandler<GetAllMevcutSinavlarQuery, List<MevcutSinavlar>>
    {
    private readonly IRepository<MevcutSinavlar> _repository;

    public GetAllMevcutSinavlarHandler(IRepository<MevcutSinavlar> repository)
    {
        _repository = repository;
    }

    public async Task<List<MevcutSinavlar>> Handle(GetAllMevcutSinavlarQuery request, CancellationToken cancellationToken)
    {
        return (List<MevcutSinavlar>)await _repository.ListAllAsync();
    }


}
public class GetAllKisilerHandler : IRequestHandler<GetAllKisilerQuery, List<Kisiler>>
{
    private readonly IRepository<Kisiler> _repository;

    public GetAllKisilerHandler(IRepository<Kisiler> repository)
    {
        _repository = repository;
    }

    public async Task<List<Kisiler>> Handle(GetAllKisilerQuery request, CancellationToken cancellationToken)
    {
        return (List<Kisiler>)await _repository.ListAllAsync();
    }


}

