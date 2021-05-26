using System;
using System.ComponentModel.DataAnnotations;

namespace GossipCheck.DAO.Entities
{
    public interface IEntity<T> where T : struct, IComparable
    {
        [Key]
        T Id { get; set; }
    }
}