import random
from DistancesGrid import DistancesGrid
from Circuit import Circuit


class GeneticAlgorithm:

    def __init__(self, weights_grid: DistancesGrid, generation_size: int, selector,
                 mutator, mutation_probability: float,
                 crosser, crossover_probability: float):

        self.weights_grid = weights_grid
        self.selector = selector
        self.mutator = mutator
        self.mutation_probability = mutation_probability
        self.crosser = crosser
        self.crossover_probability = crossover_probability

        self.generation = [Circuit.get_random(weights_grid) for i in range(generation_size)]  # get_random weights_grid
        self.best = Circuit.get_best(self.generation)  # get_best
        self.best_for_iteration = self.best
        self.iteration = 0

    def run_iteration(self):
        next_generation = list()
        while len(next_generation) < len(self.generation):
            parent1 = self.selector.get(self.generation)  # selector generation
            parent2 = self.selector.get(self.generation)  # selector generation

            child_genotype = parent1.city_ids  # city_ids
            if self.crossover_probability < random.random():
                child_genotype = self.crosser.get(parent1.city_ids, parent2.city_ids)  # crosser

            if self.mutation_probability < random.random():
                child_genotype = self.mutator.get(child_genotype)  # mutator

            child = Circuit(child_genotype, self.weights_grid)  # Circuit child_genotype weights_grid
            next_generation.append(child)  # append
        self.generation = next_generation  # next_generation
        self.best_for_iteration = Circuit.get_best(self.generation)  # get_best
        self.iteration += 1  # +=

        if self.best.score > self.best_for_iteration.score:  # score score
            self.best = self.best_for_iteration
