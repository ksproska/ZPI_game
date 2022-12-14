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
    [SerializeField] private Slider crossoverProbability, mutationProbability, genSizeSlider;
    [SerializeField] Text currentDetails;
    [SerializeField] Text history;
    [SerializeField] private DrawGraph _graph;
    [SerializeField] private GameObject citiesContainer;
    [SerializeField] private City mockup;
    
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
        LoadSavedSandboxState();
        SetGa();
    }

    private void SetGa()
    {
        // For debug purpouse
        if (selection.options.Count == 0)
            selection.AddOptions(TypeToNameMappers.GetSelectionDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        if (mutation.options.Count == 0)
            mutation.AddOptions(TypeToNameMappers.GetMutationDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        if (crossover.options.Count == 0)
            crossover.AddOptions(TypeToNameMappers.GetCrossoverDescriptionMapper().Keys.Select(k => k.ToString()).ToList());

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
            if (newCities.Count < 3) return;
            if (newCities.Count != _allCities.Count)
            {
                _graph.ClearBest();
            }
            _allCities = newCities;
            SetGa();
        }
        _isRunning = !_isRunning;
    }

    public void SaveMapState()
    {
        _allCities = CityHandler.GetAllCities();
        var map = CityHandler.CitiesToMap(_allCities);
        var slotNum = CurrentGameState.Instance.CurrentSlot;
        var slot = LoadSaveHelper.Instance.GetSlot(slotNum);

        var (selectionEnum, mutEnum, crossEnum) = MapDropdownState();

        slot.Sandbox.Crosser = crossEnum;
        slot.Sandbox.Mutator = mutEnum;
        slot.Sandbox.Selector = selectionEnum;
        slot.Sandbox.MutationProb = mutationProbability.value;
        slot.Sandbox.CrossoverProbab = crossoverProbability.value;
        slot.Sandbox.PopulationSize = int.Parse(generationSize.text);
        slot.Sandbox.UserMap = map;
        LoadSaveHelper.Instance.SaveGameState();
    }

    private (Selector, Mutator, Crosser) MapDropdownState()
    {
        Crosser crossEnum = TypeToNameMappers.GetCrossoverDescriptionMapper()[crossover.options[crossover.value].text] switch
        {
            CrossoverType.Cycle => Crosser.CX,
            CrossoverType.Order => Crosser.OX,
            CrossoverType.PartiallyMatched => Crosser.PMX,
            _ => Crosser.OX,
        };

        Mutator mutEnum = TypeToNameMappers.GetMutationDescriptionMapper()[mutation.options[mutation.value].text] switch
        {
            MutationType.RSM => Mutator.RSM,
            MutationType.Thrors => Mutator.Thrors,
            _ => Mutator.RSM,
        };

        Selector selectionEnum = TypeToNameMappers.GetSelectionDescriptionMapper()[selection.options[selection.value].text] switch
        {
            SelectionType.Roulette => Selector.Roulette,
            SelectionType.Tournament => Selector.Tournament,
            _ => Selector.Roulette,
        };
        return (selectionEnum, mutEnum, crossEnum);
    }

    private (SelectionType, MutationType, CrossoverType) MapToDropdownState(SavedSlotInfo slot)
    {
        SelectionType selectionType = slot.Sandbox.Selector switch
        {
            Selector.Roulette => SelectionType.Roulette,
            Selector.Tournament => SelectionType.Tournament,
            _ => SelectionType.Roulette
        };
        MutationType mutationType = slot.Sandbox.Mutator switch
        {
            Mutator.RSM => MutationType.RSM,
            Mutator.Thrors => MutationType.Thrors,
            _ => MutationType.RSM,
        };
        CrossoverType crossoverType = slot.Sandbox.Crosser switch
        {
            Crosser.CX => CrossoverType.Cycle,
            Crosser.OX => CrossoverType.Order,
            Crosser.PMX => CrossoverType.PartiallyMatched,
            _ => CrossoverType.Order,
        };
        return (selectionType, mutationType, crossoverType);
    }

    public void LoadSavedSandboxState()
    {
        var slotNum = CurrentGameState.Instance.CurrentSlot;
        var slot = LoadSaveHelper.Instance.GetSlot(slotNum);
        if(slot.Sandbox.UserMap == null)
        {
            InstantiateDummyCities();
            return;
        }
        _allCities = CityHandler.MapToCity(slot.Sandbox.UserMap, citiesContainer, mockup);
        var (sel, mut, cross) = MapToDropdownState(slot);

        selection.value = (int)sel;
        mutation.value = (int)mut;
        crossover.value = (int)cross;

        crossoverProbability.value = slot.Sandbox.CrossoverProbab;
        mutationProbability.value = slot.Sandbox.MutationProb;
        generationSize.text = slot.Sandbox.PopulationSize.ToString();
        genSizeSlider.value = slot.Sandbox.PopulationSize;
    }

    private void InstantiateDummyCities()
    {
        var coords = new List<(int, int)> { (195, 205),  (159, 112), (159, 432), (273, 121) };
        var parentRectTransform = citiesContainer.gameObject.GetComponent<RectTransform>();
        foreach (var (x, y) in coords)
        {
            var city = Instantiate(mockup, citiesContainer.transform);
            var pointTransform = city.GetComponent<RectTransform>();
            pointTransform.SetParent(parentRectTransform);
            pointTransform.anchoredPosition = new Vector3(x, y, 0);
            pointTransform.anchorMax = new(0, 0);
            pointTransform.anchorMin = new(0, 0);
            pointTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        
    }

    public void ClearMap()
    {
        _allCities = CityHandler.GetAllCities();
        _allCities.Count.Debug();
        _allCities.ForEach(c => Destroy(c.gameObject));
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
