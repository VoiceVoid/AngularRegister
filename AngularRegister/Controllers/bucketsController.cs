using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AngularRegister.Models;

namespace AngularRegister.Controllers
{
	[Authorize]
    public class bucketsController : ApiController
    {
        private ToshlEntities db = new ToshlEntities();

        // GET: api/buckets
        public IQueryable<bucket> Getbuckets()
        {
            return db.buckets;
        }

        // GET: api/buckets/5
		[HttpGet]
        [ResponseType(typeof(bucket))]
		[Route("api/getid/{id}")]
        public IHttpActionResult Getbucket(int id)
        {
            bucket bucket = db.buckets.Find(id);
            if (bucket == null)
            {
                return NotFound();
            }

            return Ok(bucket);
        }

        // PUT: api/buckets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putbucket(int id, bucket bucket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bucket.id)
            {
                return BadRequest();
            }

            db.Entry(bucket).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bucketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/buckets
        [ResponseType(typeof(bucket))]
        public IHttpActionResult Postbucket(bucket bucket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.buckets.Add(bucket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bucket.id }, bucket);
        }

		// DELETE: api/buckets/5
		
		[ResponseType(typeof(bucket))]
		[HttpDelete]
		[Route("api/buckets/{id}")]
		public IHttpActionResult Deletebucket(int id)
		{
			bucket bucket = db.buckets.Find(id);
			var products = db.products.Where(x => x.bucketId == id);
			if (bucket == null)
			{
				return NotFound();
			}
			db.products.RemoveRange(products);
			db.buckets.Remove(bucket);
			db.SaveChanges();

			return Ok(bucket);
		}

		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool bucketExists(int id)
        {
            return db.buckets.Count(e => e.id == id) > 0;
        }
    }
}