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
using System.Web;
using System.IO;

namespace AngularRegister.Controllers
{
	[Authorize]
    public class productsController : ApiController
    {
        private ToshlEntities db = new ToshlEntities();

		[Route("api/products/{bucketId}")]
		public IQueryable<product> Getproducts(int bucketId)
		{
			return db.products.Where(x => x.bucketId == bucketId);
		}
		

        // GET: api/products/5
        [ResponseType(typeof(product))]
        public IHttpActionResult Getproduct(int id)
        {
            product product = db.products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putproduct(int id, product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productExists(id))
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

        // POST: api/products
        [ResponseType(typeof(product))]
        public IHttpActionResult Postproduct(product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.id }, product);
        }

		// DELETE: api/products/5
		[ResponseType(typeof(product))]
		[HttpDelete]
		[Route("api/products/{productId}")]
		public IHttpActionResult Deleteproduct(int productId)
		{
			var product = db.products.Find(productId);
			if (product == null)
			{
				return NotFound();
			}
			product prod = (product)product;
			//File.Delete("~/Image/" + prod.name);
			db.products.Remove(prod);
			db.SaveChanges();

			return Ok(product);
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("api/UploadFile")]
		public HttpResponseMessage UploadImage()
		{


			// Create new FileInfo object and get the Length.
			int bucket;
			string imageName = null;
			var httpRequest = HttpContext.Current.Request;
			//upload Image

			var postedFile = httpRequest.Files["Image"];
			var bucketId = httpRequest["BucketId"];

			bucket = Int32.Parse(bucketId);
			//Create custom filename

			imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");


			imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
			//this is where we create the filePath to be saved
			var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
			//and this is where we save the file to backend
			postedFile.SaveAs(filePath);

			//Save to DB
			using (ToshlEntities db = new ToshlEntities())
			{
				product product = new product()
				{
					//image caption is the imageCaption from the front end it is the same as we wrote the name for caption
					name = imageName,
					size = postedFile.ContentLength,
					modified = DateTime.Now.ToString(),
					bucketId = bucket
					//image name is the filtered name with max 10 letters with added time at the end

				};
				db.products.Add(product);
				db.SaveChanges();
				//}
				return Request.CreateResponse(HttpStatusCode.Created, "it works");
			}
		}


		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool productExists(int id)
        {
            return db.products.Count(e => e.id == id) > 0;
        }
    }
}