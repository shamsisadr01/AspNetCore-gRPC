using AspNetCore_gRPC.Protos.v1;
using Azure.Core;
using Context;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Models;
using static AspNetCore_gRPC.Protos.v1.ProductService;

namespace Services.v1
{
    public class ProductServiceGRPC : ProductServiceBase
    {
        private readonly GRPCContext gRPCContext;

        public ProductServiceGRPC(GRPCContext context)
        {
            gRPCContext = context;
        }

        public override async Task CreateProduct(IAsyncStreamReader<CreateProductRequest> requestStream, IServerStreamWriter<CreateProductReply> responseStream, ServerCallContext context)
        {
            int createProductCount = 0;
            while (await requestStream.MoveNext())
            {
                gRPCContext.Products.Add(new Product()
                {
                    CreateDateTime = DateTime.Now,
                    Descriptions = requestStream.Current.Descriptions,
                    Price = requestStream.Current.Price,
                    Title = requestStream.Current.Title
                });

                createProductCount++;
            }

            await gRPCContext.SaveChangesAsync();

            await responseStream.WriteAsync(new CreateProductReply()
            {
                CreatedItemCount = createProductCount,
                Message = "created succefuly",
                Status = 200
            });
            //return base.CreateProduct(requestStream, responseStream, context);
        }

        public override async Task<UpdateProductReply> UpdateProduct(UpdateProductRequset request, ServerCallContext context)
        {
            Product? product = gRPCContext.Products.FirstOrDefault(p => p.Id == request.Id);

            if (product == null)
                return null;

            product.Descriptions = request.Descriptions;
            product.Price = request.Price;
            product.Title = request.Title;

            gRPCContext.Products.Update(product);
            await gRPCContext.SaveChangesAsync();

            return new UpdateProductReply()
            {
                Message = "Update",
                Status = 200
            };
        }

        public override async Task<GetProductByIDReply> GetProductByID(GetProductByIDRequset request, ServerCallContext context)
        {
            Product? product = gRPCContext.Products.FirstOrDefault(p => p.Id == request.Id);
            if (product == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            Metadata headers = new Metadata()
            {
                { "Name", "Ali" },
                { "age", "20" }
            };
            await context.WriteResponseHeadersAsync(headers);

            context.ResponseTrailers.Add("key", "123");


            return new GetProductByIDReply()
            {
                Id = product.Id,
                CreateDate = Timestamp.FromDateTime(DateTime.SpecifyKind(product.CreateDateTime,DateTimeKind.Utc)),
                Descriptions = product.Descriptions,
                Price = product.Price,
                Title = product.Title
            };
        }

        public override async Task<RemoveProductByIDReply> RemoveProduct(IAsyncStreamReader<RemoveProductByIDRequset> requestStream, ServerCallContext context)
        {
            int removeItemCount = 0;
            while (await requestStream.MoveNext())
            {
                Product? product = gRPCContext.Products.FirstOrDefault(p => p.Id == requestStream.Current.Id);
                if (product == null)
                    continue;

                gRPCContext.Products.Remove(product);
                removeItemCount++;
            }

            await gRPCContext.SaveChangesAsync();

            return new RemoveProductByIDReply()
            {
                Message = "Remove",
                RemoveItemCount = removeItemCount,
                Status = 200
            };
        }
        public override async Task GetAllProducts(GetAllProductsRequset request, IServerStreamWriter<GetAllProductsReply> responseStream, ServerCallContext context)
        {
            int skip = (request.Page - 1) * request.Take;
            List<Product> products = await gRPCContext.Products
                .Skip(skip)
                .Take(request.Take).ToListAsync();

            foreach(var item in products)
            {
                 await responseStream.WriteAsync(new GetAllProductsReply()
                {
                     Id = item.Id,
                     CreateDate = Timestamp.FromDateTime(DateTime.SpecifyKind(item.CreateDateTime, DateTimeKind.Utc)),
                     Descriptions = item.Descriptions,
                     Price = item.Price,
                     Title = item.Title
                });
            }
        }
    }
}
