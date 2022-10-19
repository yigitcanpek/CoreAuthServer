using CoreAuthServer.Core.Repositories;
using CoreAuthServer.Core.Services;
using CoreAuthServer.Data.DesignPattern;
using CoreAuthServer.Service.Mapper;
using CoreSharedLibary.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreAuthServer.Service.Services
{
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public ServiceGeneric(UnitOfWork unitOfWork,IGenericRepository<TEntity>genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }


        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            TEntity newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            await _genericRepository.AddAsync(newEntity);

            await _unitOfWork.CommitAsync();

            TDto newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(newDto, 200);
            
        }

       

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            List<TDto> products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());

            return Response<IEnumerable<TDto>>.Success(products, 200);
        }



        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);
            if (product==null)
            {
                return Response<TDto>.Fail("Id not found", 404, true);
            }
            return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product),200);
        }




        public async Task<Response<NoDataDto>> Remove(int id)
        {
            TEntity isExistEntity = await _genericRepository.GetByIdAsync(id);
            if (isExistEntity ==null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404,true);
            }

            _genericRepository.Remove(isExistEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }





        public async Task<Response<NoDataDto>> Update(TDto entity, int id)
        {
            TEntity isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            TEntity updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

            _genericRepository.Update(updateEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> list = _genericRepository.Where(predicate);

            //list.Skip(4).Take(5); IEnumarable yaparsan hala sorgu gönderebilirsin , IQuarayable da bunu yapamazsın..

            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(list.ToList()), 200);
        }
    }
}
