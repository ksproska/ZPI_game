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

namespace Challenges
{
    public class RunGAChallenge: MonoBehaviour
    {
        [SerializeField] private InputField generationSize;
        [SerializeField] private Dropdown selection, crossover, mutation;
        [SerializeField] private Slider crossoverProbability, mutationProbability;
        [SerializeField] private Text crossoverLabel, mutationLabel;
        [SerializeField] Text currentDetails;
        [SerializeField] Text history;
        [SerializeField] private DrawGraph _graph;
        [SerializeField] private Image mapImage;
        [SerializeField] private int maxIterations;
        
        [SerializeField]
        private Sprite play, pause;

        [SerializeField] private Image image;

        private float bestAllTime;

        public void changeImage()
        {
            var current = image.sprite;
            if (current == pause)
            {
                image.sprite = play;
            }
            else
            {
                image.sprite = pause;
            }
        }
        
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
            // // Filter all the selectors that have been completed by the player
            // var allSelectors = TypeToNameMappers.GetSelectionDescriptionMapper()
            //     .Select(pair => (pair.Key, pair.Value))
            //     .ToList();
            // var selectorLevelNames = allSelectors
            //     .Select(s => (s.Key, s.Value, RunGAOptionsFilter.SelectionLevelNames[s.Value]))
            //     .ToList();
            // var completedSelectors = selectorLevelNames
            //     .Where(s => LevelMap.Instance.IsLevelDone(LevelMap.GetClearMapName(s.Item3), CurrentGameState.Instance.CurrentSlot))
            //     .ToList();
            // selection.AddOptions(completedSelectors.Select(tuple => tuple.Key).ToList());

            // // Filter all the crosseres that have been completed by the player
            // var allCrossers = TypeToNameMappers.GetCrossoverDescriptionMapper()
            //     .Select(pair => (pair.Key, pair.Value))
            //     .ToList();
            // var crossersLevelNames = allCrossers
            //     .Select(c => (c.Key, c.Value, RunGAOptionsFilter.CrossoverLevelNames[c.Value]))
            //     .ToList();
            // var completedCrossers = crossersLevelNames
            //     .Where(c => LevelMap.Instance.IsLevelDone(LevelMap.GetClearMapName(c.Item3), CurrentGameState.Instance.CurrentSlot))
            //     .ToList();
            // crossover.AddOptions(completedCrossers.Select(tuple => tuple.Key).ToList());

            // // Filter all the mutators that have been completed by the player
            // var allMutators = TypeToNameMappers.GetMutationDescriptionMapper()
            //     .Select(pair => (pair.Key, pair.Value))
            //     .ToList();
            // var mutatorsLevelNames = allMutators
            //     .Select(m => (m.Key, m.Value, RunGAOptionsFilter.MutationLevelNames[m.Value]))
            //     .ToList();
            // var completedMutators = mutatorsLevelNames
            //     .Where(m => LevelMap.Instance.IsLevelDone(LevelMap.GetClearMapName(m.Item3), CurrentGameState.Instance.CurrentSlot))
            //     .ToList();
            // mutation.AddOptions(completedMutators.Select(tuple => tuple.Key).ToList());

            var slotInfo = LoadSaveHelper.Instance.GetSlot(CurrentGameState.Instance.CurrentSlot);
            bestAllTime = slotInfo.BestScores[CurrentGameState.Instance.CurrentMapId];
            if (bestAllTime == -1) bestAllTime = int.MaxValue;
            
            selection.AddOptions(TypeToNameMappers.GetSelectionDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
            crossover.AddOptions(TypeToNameMappers.GetCrossoverDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
            mutation.AddOptions(TypeToNameMappers.GetMutationDescriptionMapper().Keys.Select(k => k.ToString()).ToList());
            
            mapImage.sprite = Resources.Load<Sprite>($"Challenges/challenge{CurrentGameState.Instance.CurrentMapId}");
            crossoverLabel.text = $"Crossover probability: {crossoverProbability.value * 100:0.00}%";
            mutationLabel.text = $"Mutation probability: {mutationProbability.value * 100:0.00}%";
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
                    if (_ga.GetIterationNumber() > maxIterations)
                    {
                        _isRunning = false;
                        OnEndRun();
                        changeImage();
                    }
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
            else
            {
                OnEndRun();
            }
            _isRunning = !_isRunning;
        }

        public void Terminate()
        {
            _isRunning = false;
            changeImage();
        }

        public async void OnEndRun()
        {
            var currentBest = (float)_ga.GetBestForIterationScore();
            if(currentBest < bestAllTime)
            {
                var slotInfo = LoadSaveHelper.Instance.GetSlot(CurrentGameState.Instance.CurrentSlot);
                slotInfo.BestScores[CurrentGameState.Instance.CurrentMapId] = currentBest;
                LoadSaveHelper.Instance.SaveGameState();
                if(CurrentGameState.Instance.CurrentUserId != -1)
                {
                    var userId = CurrentGameState.Instance.CurrentUserId;
                    var mapId = CurrentGameState.Instance.CurrentMapId;
                    var score = new Webserver.Score(userId, mapId, currentBest);
                    var (result, info) = await Webserver.ScoreSynchro.PutNewScore(score);
                }
            }
        }

        public void SetupCrossoverProbability(float value)
        {
            crossoverLabel.text = $"Crossover probability: {value * 100:0.00}%";
        }
        
        public void SetupMutationProbability(float value)
        {
            mutationLabel.text = $"Mutation probability: {value * 100:0.00}%";
        }
    }
}