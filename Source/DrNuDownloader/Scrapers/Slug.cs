using System;

namespace DrNuDownloader.Scrapers
{
    public class Slug
    {
        private readonly string _slug;

        public Slug(string slug)
        {
            if (slug == null) throw new ArgumentNullException("slug");

            _slug = slug;
        }

        public static implicit operator Slug(string slug)
        {
            return slug != null ? new Slug(slug) : null;
        }

        protected bool Equals(Slug other)
        {
            return string.Equals(_slug, other._slug);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Slug)obj);
        }

        public override int GetHashCode()
        {
            return _slug.GetHashCode();
        }

        public override string ToString()
        {
            return _slug;
        }
    }
}