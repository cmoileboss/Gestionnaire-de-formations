using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

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

    public async Task<IEnumerable<ModuleDto>> GetAllAsync()
    {
        var modules = await _moduleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ModuleDto>>(modules);
    }

    public async Task<ModuleDto?> GetByIdAsync(int id)
    {
        var module = await _moduleRepository.GetByIdAsync(id);
        if (module == null)
            throw new KeyNotFoundException($"Module avec l'ID {id} non trouvé.");
        return _mapper.Map<ModuleDto>(module);
    }

    public async Task<ModuleDto> CreateAsync(ModuleDto dto)
    {
        var module = _mapper.Map<Module>(dto);
        await _moduleRepository.AddAsync(module);
        return _mapper.Map<ModuleDto>(module);
    }

    public async Task<ModuleDto> UpdateAsync(int id, ModuleDto dto)
    {
        var existing = await _moduleRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Module avec l'ID {id} non trouvé.");

        _mapper.Map(dto, existing);
        existing.ModuleId = id;
        await _moduleRepository.UpdateAsync(existing);
        return _mapper.Map<ModuleDto>(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _moduleRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Module avec l'ID {id} non trouvé.");

        await _moduleRepository.DeleteAsync(id);
        return true;
    }
}
