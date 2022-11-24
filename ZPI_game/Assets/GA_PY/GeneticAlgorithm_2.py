import random
from DistancesGrid import DistancesGrid
from Circuit import Circuit


class GeneticAlgorithm:
    def __init__(self, weights_grid: DistancesGrid, generation_size: int, selector,
                 crosser, cross_prob: float):
        ...
        self.crosser = crosser  # crosser
        self.cross_prob = cross_prob  # cross_prob
        ...

    def run_iteration(self):
        """Method runs one iteration for genetic algorithm.
        It is split into a couple of steps:
        1. Select two parents
        2. Cross two parents resulting in a new child (which might get "the best genes" from both parents)
        ...
        """
        next_gen = list()
        while len(next_gen) < len(self.generation):
            parent1 = self.selector.get(self.generation)
            parent2 = self.selector.get(self.generation)

            child_genotype = parent1.city_ids  # city_ids
            if self.cross_prob < random.random():  # cross_prob
                child_genotype = self.crosser.get(
                    parent1.city_ids, parent2.city_ids)  # crosser city_ids
            ...

            child = Circuit(child_genotype, self.weights_grid)  # Circuit child_genotype
            next_gen.append(child)  # append
        self.generation = next_gen  # next_gen
        self.best_for_iter = Circuit.get_best(self.generation)
        self.iteration += 1  # +=

        if self.best.score > self.best_for_iter.score:  # score
            self.best = self.best_for_iter  # best_for_iter
