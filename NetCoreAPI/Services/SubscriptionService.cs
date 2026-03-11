using AutoMapper;
using NetCoreAPI.DTOs;
using NetCoreAPI.Models;
using NetCoreAPI.Repositories;
using NetCoreAPI.Utils;

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

    public async Task<Result<IEnumerable<SubscriptionDto>>> GetAllAsync()
    {
        try
        {
            var subscriptions = await _subscriptionRepository.GetAllAsync();
            return Result<IEnumerable<SubscriptionDto>>.Success(_mapper.Map<IEnumerable<SubscriptionDto>>(subscriptions));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving subscriptions.", ex);
        }
    }

    public async Task<Result<SubscriptionDto>> GetByIdAsync(int id)
    {
        try
        {
            var subscription = await _subscriptionRepository.GetByIdAsync(id);
            if (subscription == null)
                return Result<SubscriptionDto>.Failure($"Abonnement avec l'ID {id} non trouvé.");
            return Result<SubscriptionDto>.Success(_mapper.Map<SubscriptionDto>(subscription));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while retrieving subscription {id}.", ex);
        }
    }

    public async Task<Result<SubscriptionDto>> CreateAsync(SubscriptionDto dto)
    {
        try
        {
            var subscription = _mapper.Map<Subscription>(dto);
            await _subscriptionRepository.AddAsync(subscription);
            return Result<SubscriptionDto>.Success(_mapper.Map<SubscriptionDto>(subscription));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while creating subscription.", ex);
        }
    }

    public async Task<Result<SubscriptionDto>> UpdateAsync(int id, SubscriptionDto dto)
    {
        try
        {
            var existing = await _subscriptionRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<SubscriptionDto>.Failure($"Abonnement avec l'ID {id} non trouvé.");

            _mapper.Map(dto, existing);
            await _subscriptionRepository.UpdateAsync(existing);
            return Result<SubscriptionDto>.Success(_mapper.Map<SubscriptionDto>(existing));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating subscription {id}.", ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var existing = await _subscriptionRepository.GetByIdAsync(id);
            if (existing == null)
                return Result<bool>.Failure($"Abonnement avec l'ID {id} non trouvé.");

            await _subscriptionRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting subscription {id}.", ex);
        }
    }
}
