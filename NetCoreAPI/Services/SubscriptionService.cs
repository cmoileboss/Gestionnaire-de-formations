using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;

namespace NetCoreAPI.Services;

/// <summary>
/// Service métier pour la gestion des abonnements.
/// </summary>
public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initialise une nouvelle instance du service abonnement.
    /// </summary>
    /// <param name="subscriptionRepository">Repository abonnement injecté.</param>
    /// <param name="mapper">Instance d'AutoMapper injectée.</param>
    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SubscriptionDto>> GetAllAsync()
    {
        var subscriptions = await _subscriptionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions);
    }

    public async Task<SubscriptionDto?> GetByIdAsync(int id)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id);
        if (subscription == null)
            throw new KeyNotFoundException($"Abonnement avec l'ID {id} non trouvé.");
        return _mapper.Map<SubscriptionDto>(subscription);
    }

    public async Task<SubscriptionDto> CreateAsync(SubscriptionDto dto)
    {
        var subscription = _mapper.Map<Subscription>(dto);
        await _subscriptionRepository.AddAsync(subscription);
        return _mapper.Map<SubscriptionDto>(subscription);
    }

    public async Task<SubscriptionDto> UpdateAsync(int id, SubscriptionDto dto)
    {
        var existing = await _subscriptionRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Abonnement avec l'ID {id} non trouvé.");

        _mapper.Map(dto, existing);
        await _subscriptionRepository.UpdateAsync(existing);
        return _mapper.Map<SubscriptionDto>(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _subscriptionRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Abonnement avec l'ID {id} non trouvé.");

        await _subscriptionRepository.DeleteAsync(id);
        return true;
    }
}
