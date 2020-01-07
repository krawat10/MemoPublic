using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Memo.Models
{
    public class Marker : Identity
    {
        protected Marker()
        {
        }

        public Marker(Coordinates coordinates)
        {
            Coordinates = coordinates;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Obsolete = false;
        }

        public Coordinates Coordinates { get; protected set; }
        public DateTime Created { get; protected set; }
        public DateTime Updated { get; protected set; }
        public DateTime? Removed { get; protected set; }

        public bool Obsolete { get; private set; }

        public void UpdateCoordinates(Coordinates coordinates)
        {
            Coordinates = coordinates;
            Updated = DateTime.Now;
        }

        public virtual void DeleteMarker()
        {
            Obsolete = true;
            Updated = DateTime.Now;
            Removed = DateTime.Now;
        }
    }

    public class MemoMarker : Marker
    {
        public MemoMarker(string name, string description, Coordinates coordinates, IdentityUser user)
            : base(coordinates)
        {
            Name = name;
            Description = description;
            User = user;
            Shared = false;
        }

        private MemoMarker()
        {
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public IdentityUser User { get; set; }
        [ForeignKey(nameof(User))] public string UserId { get; set; }


        public void UpdateName(string name)
        {
            Name = name;
            Updated = DateTime.Now;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
            Updated = DateTime.Now;
        }

        public void SetAsCenterPoint()
        {
            IsCenterPoint = true;
        }

        public bool IsCenterPoint { get; private set; }
        public bool Shared { get; private set; }

        public void SetAsNotCenterPoint()
        {
            IsCenterPoint = false;
        }

        public void SetAsShared()
        {
            Shared = true;
        }

        public void SetAsNotShared()
        {
            Shared = false;
        }
    }

    [Owned]
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}