using GossipCheck.DAO.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace GossipCheck.DAO.Entities
{
    public class SourceReputation : IEntity<int>
    {
        public int Id { get; set; }

        [Required]
        public string BaseUrl { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Reputation { get; set; }
    }
}