using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DataTtansferObjects.ProductModuleDto;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var brands = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(brands);
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Specifications = new ProductWithBrandAndTypeSpecification(queryParams);
            var repo = _unitOfWork.GetRepository<Product, int>();
            var AllProducts = await repo.GetAllAsync(Specifications);
            var Data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(AllProducts);
            var countSpecifications = new CountSpecifications(queryParams);
            var totalCount = await repo.CountAsync(countSpecifications);
            return new PaginatedResult<ProductDto>(queryParams.PageIndex, AllProducts.Count(), totalCount, Data);

        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductType, int>();
            var types = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);

        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var Specifications = new ProductWithBrandAndTypeSpecification(id);
            var repo = _unitOfWork.GetRepository<Product, int>();
            var product = await repo.GetByIdAsync(Specifications);
            if(product is null)
                throw new ProductNotFoundException(id);

            return _mapper.Map<Product, ProductDto>(product);
        }
    }
}
