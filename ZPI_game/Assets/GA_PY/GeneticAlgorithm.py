import random
from DistancesGrid import DistancesGrid
from Circuit import Circuit


class GeneticAlgorithm:

    def __init__(self, weights_grid: DistancesGrid, generation_size: int, selector,
                 mutator, mut_prob: float,
                 crosser, cross_prob: float):

        self.weights_grid = weights_grid
        self.selector = selector
        self.mutator = mutator  # mutator
        self.mut_prob = mut_prob  # mut_prob
        self.crosser = crosser
        self.cross_prob = cross_prob

        self.generation = [Circuit.get_random(
            weights_grid) for _ in range(generation_size)]
        self.best = Circuit.get_best(self.generation)
        self.best_for_iter = self.best
        self.iteration = 0

    def run_iteration(self):
        """Method runs one iteration for genetic algorithm.
        It is split into a couple of steps:
        1. Select two parents
        2. Cross two parents resulting in a new child (which might get "the best genes" from both parents)
        3. Mutate a child (causing some randomization in procedure)
        3. Repeat steps 1-3 until the new generation is the same size as old generation.
        4. Find the best child in new generation and save it.
        """
        next_generation = list()
        while len(next_generation) < len(self.generation):
            parent1 = self.selector.get(self.generation)
            parent2 = self.selector.get(self.generation)

            child_genotype = parent1.city_ids
            if self.cross_prob < random.random():
                child_genotype = self.crosser.get(
                    parent1.city_ids, parent2.city_ids)

            if self.mut_prob < random.random():  # mut_prob
                child_genotype = self.mutator.get(
                    child_genotype)  # mutator child_genotype

            child = Circuit(child_genotype, self.weights_grid)
            next_generation.append(child)
        self.generation = next_generation
        self.best_for_iter = Circuit.get_best(self.generation)
        self.iteration += 1

        if self.best.score > self.best_for_iter.score:
            self.best = self.best_for_iter
