using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Bunker.Business.Entities;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Responses;
using Bunker.Business.Internal.Interfaces.Services;

namespace Bunker.Business.Internal.Services
{
    internal class MapInfo
    {
        public Type     In   { get; set; }
        public Type     Out  { get; set; }
        public Delegate Func { get; set; }
    }

    public class DelegateMefMapper : IMefMapper
    {
        private readonly List<MapInfo> _mapInfos;

        public DelegateMefMapper()
        {
            _mapInfos = new List<MapInfo>();

            AddMap<Challange, ChallangeResponse>(challange =>
                challange == null
                    ? null
                    : new ChallangeResponse
                    {
                        Id          = challange.Id,
                        Description = challange.Desciprion,
                        Name        = challange.Name,
                    });

            AddMap<Company, CompanyResponse>(company =>
                company == null
                    ? null
                    : new CompanyResponse
                    {
                        Id           = company.Id,
                        Descriptipon = company.Desciprion,
                        Name         = company.Name,
                    });

            AddMap<Task, TaskResponse>(task =>
                task == null
                    ? null
                    : new TaskResponse
                    {
                        Id          = task.Id,
                        Description = task.Description,
                        Name        = task.Name,
                        Score       = task.Score,
                    });

            AddMap<Task, TaskWithAnswerResponse>(task =>
                task == null
                    ? null
                    : new TaskWithAnswerResponse
                    {
                        Id          = task.Id,
                        Description = task.Description,
                        Name        = task.Name,
                        Score       = task.Score,
                        Answer      = task.Answer,
                    });

            AddMap<Team, TeamResponse>(team =>
                team == null
                    ? null
                    : new TeamResponse
                    {
                        Id   = team.Id,
                        Name = team.Name
                    });

            AddMap<Player, PlayerResponse>(player =>
                player == null
                    ? null
                    : new PlayerResponse
                    {
                        Id        = player.Id,
                        FirstName = player.FirstName,
                        LastName  = player.LastName,
                        NickName  = player.NickName
                    });
        }


        private static MapInfo GetMapInfo<TIn, TOut>(Func<TIn, TOut> method)
            => new MapInfo
            {
                In   = typeof(TIn),
                Out  = typeof(TOut),
                Func = method.GetInvocationList().FirstOrDefault()
            };

        public TOut Map<TIn, TOut>(TIn inValue)
        {
            var mapInfo = _mapInfos.FirstOrDefault(x => x.In == typeof(TIn) && x.Out == typeof(TOut));

            if (mapInfo == null)
                throw new ArgumentException("Mapper not found");

            return (TOut) mapInfo.Func.DynamicInvoke(inValue);
        }

        public void AddMap<TIn, TOut>(Func<TIn, TOut> func)
        {
            if (_mapInfos.Any(x => x.In == typeof(TIn) && x.Out == typeof(TOut)))
                throw new DuplicateNameException();

            _mapInfos.Add(GetMapInfo(func));
        }
    }
}