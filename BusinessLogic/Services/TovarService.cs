using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class TovarService : ITovarService
    {
        private IRepositoryWrapper _repositoryWrapper;
        public TovarService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<List<Tovar>> GetAll()
        {
            return await _repositoryWrapper.Tovar.FindAll();
        }
        public async Task<Tovar> GetById(int id, int id1)
        {
            var user = await _repositoryWrapper.Tovar
            .FindByCondition(x => x.IdTovara == id && x.IdKategorii == id1);
            return user.First();
        }
        public async Task Create(Tovar model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException(nameof(model.Name));
            }

            if (model.Kolichestvo == 0)
            {
                throw new ArgumentException(nameof(model.Kolichestvo));
            }

            if (model.Price == 0)
            {
                throw new ArgumentException(nameof(model.Price));
            }

            if (string.IsNullOrEmpty(model.OpisanieTovara))
            {
                throw new ArgumentException(nameof(model.OpisanieTovara));
            }

            await _repositoryWrapper.Tovar.Create(model);
            await _repositoryWrapper.Save();
        }
        public async Task Update(Tovar model)
        {
            await _repositoryWrapper.Tovar.Update(model);
            await _repositoryWrapper.Save();
        }
        public async Task Delete(int id)
        {
            var user = await _repositoryWrapper.Tovar
            .FindByCondition(x => x.IdTovara == id);
            await _repositoryWrapper.Tovar.Delete(user.First());
            await _repositoryWrapper.Save();
        }
    }
}
