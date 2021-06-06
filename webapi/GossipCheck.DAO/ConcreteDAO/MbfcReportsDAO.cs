﻿using GossipCheck.DAO.Entities;
using GossipCheck.DAO.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GossipCheck.DAO.ConcreteDAO
{
    internal sealed class MbfcReportsDAO : DataAccessObject<MbfcReport, int>, IMbfcReportsDAO
    {
        public MbfcReportsDAO(DbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<MbfcReport> GetLatestByUrls(IEnumerable<string> urls)
        {
            return table
                .Where(x => urls.Contains(x.Source))
                .AsEnumerable()
                .GroupBy(x => x.Source)
                .Select(x => x.OrderByDescending(x => x.Date).First())
                .ToList();
        }
    }
}
