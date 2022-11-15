using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperUtils;
using GA;
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
        _updateDeltaTime = speed;
    }
    
    void Start()
    {
        selection.AddOptions(TypeToNameMappers.GetSelectionDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        crossover.AddOptions(TypeToNameMappers.GetCrossoverDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        mutation.AddOptions(TypeToNameMappers.GetMutationDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
        
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
