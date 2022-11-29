using System;
using System.Collections.Generic;
using System.Linq;
using CurrentState;
using DeveloperUtils;
using GA;
using LevelUtils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RunGa : MonoBehaviour
{
    [SerializeField] private InputField generationSize;
    [SerializeField] private Dropdown selection, crossover, mutation;
    [SerializeField] private Slider crossoverProbability, mutationProbability;
    [SerializeField] Text currentDetails;
    [SerializeField] Text history;
    [SerializeField] private DrawGraph _graph;
    
    private List<City> _allCities;

    IGeneticAlgorithm<int> _ga;
    IGaPrettifier _gaPrettifier;
    
    private float _lastUpdate = 0;
    private bool _isRunning = false;
    private float _updateDeltaTime = 0.4f;

    public void SetSpeed(float speed)
    {
        _updateDeltaTime = 1 - speed;
    }
    
    void Start()
    {
        // Filter all the selectors that have been completed by the player
        var allSelectors = TypeToNameMappers.GetSelectionDescriptionMapper()
            .Select(pair => (pair.Key, pair.Value))
            .ToList();
        var selectorLevelNames = allSelectors
            .Select(s => (s.Key, s.Value, RunGAOptionsFilter.SelectionLevelNames[s.Value]))
            .ToList();
        var completedSelectors = selectorLevelNames
            .Where(s => LevelMap.Instance.IsLevelDone(LevelMap.GetClearMapName(s.Item3), CurrentGameState.Instance.CurrentSlot))
            .ToList();
        selection.AddOptions(completedSelectors.Select(tuple => tuple.Key).ToList());

        // Filter all the crosseres that have been completed by the player
        var allCrossers = TypeToNameMappers.GetCrossoverDescriptionMapper()
            .Select(pair => (pair.Key, pair.Value))
            .ToList();
        var crossersLevelNames = allCrossers
            .Select(c => (c.Key, c.Value, RunGAOptionsFilter.CrossoverLevelNames[c.Value]))
            .ToList();
        var completedCrossers = crossersLevelNames
            .Where(c => LevelMap.Instance.IsLevelDone(LevelMap.GetClearMapName(c.Item3), CurrentGameState.Instance.CurrentSlot))
            .ToList();
        crossover.AddOptions(completedCrossers.Select(tuple => tuple.Key).ToList());

        // Filter all the mutators that have been completed by the player
        var allMutators = TypeToNameMappers.GetMutationDescriptionMapper()
            .Select(pair => (pair.Key, pair.Value))
            .ToList();
        var mutatorsLevelNames = allMutators
            .Select(m => (m.Key, m.Value, RunGAOptionsFilter.MutationLevelNames[m.Value]))
            .ToList();
        var completedMutators = mutatorsLevelNames
            .Where(m => LevelMap.Instance.IsLevelDone(LevelMap.GetClearMapName(m.Item3), CurrentGameState.Instance.CurrentSlot))
            .ToList();
        mutation.AddOptions(completedMutators.Select(tuple => tuple.Key).ToList());

        //selection.AddOptions(TypeToNameMappers.GetSelectionDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        //crossover.AddOptions(TypeToNameMappers.GetCrossoverDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        //mutation.AddOptions(TypeToNameMappers.GetMutationDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        
        SetGa();
    }

    private void SetGa()
    {
        _allCities = CityHandler.GetAllCities();
        var selectionType = TypeToNameMappers.GetSelectionDescriptionMapper()[selection.options[selection.value].text];
        var crossoverType = TypeToNameMappers.GetCrossoverDescriptionMapper()[crossover.options[crossover.value].text];
        var mutationType = TypeToNameMappers.GetMutationDescriptionMapper()[mutation.options[mutation.value].text];

        _ga = GeneticAlgorithmProxy.Get(
            CityCalculations.GetDistances(_allCities),
            Int32.Parse(generationSize.text),
            mutationType,
            mutationProbability.value,
            crossoverType,
            crossoverProbability.value,
            selectionType,
            0.5);
        _gaPrettifier = new GaPrettifier<int>(_ga);
        history.text = _gaPrettifier.GetIterationLogHeader();
    }

    void FixedUpdate()
    {
        if (_isRunning)
        {
            _lastUpdate += Time.deltaTime;
            if(_lastUpdate > _updateDeltaTime)
            {
                _lastUpdate = 0;
                currentDetails.text = _gaPrettifier.GetCurrentIterationLog();
                history.text += _gaPrettifier.GetCurrentIterationLogIfNewBestFound();
                _graph.AddPointAndUpdate(_ga.GetIterationNumber(), (float)_ga.GetBestScore());
                var bestGenome = _ga.GetBestGenotype();
                CityHandler.DrawLines(bestGenome, _allCities);
                _ga.RunIteration();
            }
        }
    }

    public void StartStop()
    {
        if (!_isRunning)
        {
            _graph.RemoveAllPoints();
            var newCities = CityHandler.GetAllCities();
            if (newCities.Count != _allCities.Count)
            {
                _graph.ClearBest();
            }
            _allCities = newCities;
            SetGa();
        }
        _isRunning = !_isRunning;
    }
}

class RunGAOptionsFilter
{
    public static Dictionary<SelectionType, string> SelectionLevelNames => new Dictionary<SelectionType, string>
    {
        { SelectionType.Roulette, "map_4_RouletteImpl_3" },
        { SelectionType.Tournament, "map_6_TournamentImpl_5" }
    };

    public static Dictionary<CrossoverType, string> CrossoverLevelNames => new Dictionary<CrossoverType, string>
    {
        { CrossoverType.Cycle, "map_17_cxImpl2_16" },
        { CrossoverType.Order, "map_13_oxImpl_12" },
        { CrossoverType.PartiallyMatched, "map_10_pmxImpl_9" },
    };

    public static Dictionary<MutationType, string> MutationLevelNames => new Dictionary<MutationType, string>
    {
        { MutationType.Thrors, "map_24_throrsImpl_23" },
        { MutationType.RSM, "map_21_rsmImpl_20" },
    };
}
