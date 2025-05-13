using AspNetCore_gRPC.Protos;
using Grpc.Core;
using static AspNetCore_gRPC.Protos.ProductService;

namespace AspNetCore_gRPC.Services
{
    public class ProductServiceGRPC : ProductServiceBase
    {
        public override Task CreateProduct(IAsyncStreamReader<CreateProductRequest> requestStream, IServerStreamWriter<CreateProductReply> responseStream, ServerCallContext context)
        {
            return base.CreateProduct(requestStream, responseStream, context);
        }

        public override Task<UpdateProductReply> UpdateProduct(UpdateProductReply request, ServerCallContext context)
        {
            return base.UpdateProduct(request, context);
        }

        public override Task<GetProductByIDReply> GetProductByID(GetProductByIDRequset request, ServerCallContext context)
        {
            return base.GetProductByID(request, context);
        }

        public override Task<RemoveProductByIDReply> RemoveProduct(IAsyncStreamReader<RemoveProductByIDRequset> requestStream, ServerCallContext context)
        {
            return base.RemoveProduct(requestStream, context);
        }
        public override Task GetAllProducts(GetAllProductsRequset request, IServerStreamWriter<GetAllProductsReply> responseStream, ServerCallContext context)
        {
            return base.GetAllProducts(request, responseStream, context);
        }
    }
}
