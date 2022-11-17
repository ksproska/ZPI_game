import random
from DistancesGrid import DistancesGrid
from Circuit import Circuit


class GeneticAlgorithm:

    def __init__(self, weights_grid: DistancesGrid, generation_size: int, selector):  # selector

        self.weights_grid = weights_grid  # weights_grid
        self.selector = selector  # selector

        self.generation = [Circuit.get_random(weights_grid) for _ in range(generation_size)]  # get_random weights_grid
        self.best = Circuit.get_best(self.generation)  # get_best
        self.best_for_iteration = self.best
        self.iteration = 0  # 0

    def run_iteration(self):
        next_generation = list()
        while len(next_generation) < len(self.generation):
            parent1 = self.selector.get(self.generation)  # selector generation
            parent2 = self.selector.get(self.generation)  # selector generation
            ...
