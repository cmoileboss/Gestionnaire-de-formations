using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;
using NetCoreAPI.Utils;

namespace NetCoreAPI.Services;

/// <summary>
/// Service métier pour la gestion des modules.
/// </summary>
public class ModuleService : IModuleService
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service module.
    /// </summary>
    /// <param name="moduleRepository">Repository module injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public ModuleService(IModuleRepository moduleRepository, IMapper mapper)
    {
        _moduleRepository = moduleRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ModuleDto>>> GetAllAsync()
    {
        try
        {
            var modules = await _moduleRepository.GetAllAsync();
            return Result<IEnumerable<ModuleDto>>.Success(_mapper.Map<IEnumerable<ModuleDto>>(modules));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving modules.", ex);
        }
    }

    public async Task<Result<ModuleDto>> GetByIdAsync(int id)
    {
        try
        {
            var module = await _moduleRepository.GetByIdAsync(id);
            if (module == null)
                return Result<ModuleDto>.Failure($"Module avec l'ID {id} non trouvé.");
            return Result<ModuleDto>.Success(_mapper.Map<ModuleDto>(module));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving module {id}.", ex);
        }
    }

    public async Task<Result<ModuleDto>> CreateAsync(ModuleDto dto)
    {
        try
        {
            var module = _mapper.Map<Module>(dto);
            await _moduleRepository.AddAsync(module);
            return Result<ModuleDto>.Success(_mapper.Map<ModuleDto>(module));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating module.", ex);
        }
    }

    public async Task<Result<ModuleDto>> UpdateAsync(int id, ModuleDto dto)
    {
        try
        {
            var existing = await _moduleRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<ModuleDto>.Failure($"Module avec l'ID {id} non trouvé.");

            _mapper.Map(dto, existing);
            existing.ModuleId = id;
            await _moduleRepository.UpdateAsync(existing);
            return Result<ModuleDto>.Success(_mapper.Map<ModuleDto>(existing));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating module {id}.", ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var existing = await _moduleRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<bool>.Failure($"Module avec l'ID {id} non trouvé.");

            await _moduleRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting module {id}.", ex);
        }
    }
}
