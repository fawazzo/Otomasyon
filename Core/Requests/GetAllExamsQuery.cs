using MediatR;
using Otomasyon.Core.Entities;
using System.Collections.Generic;

namespace Otomasyon.Core.Requests
{
    public class GetAllBasvuruFormuQuery : IRequest<List<BasvuruFormu>>
    {
    }
    public class GetAllMevcutSinavlarQuery : IRequest<List<MevcutSinavlar>>
    {
    }
    public class GetAllKisilerQuery : IRequest<List<Kisiler>>
    {
    }
}
